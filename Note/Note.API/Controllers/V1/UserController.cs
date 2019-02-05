﻿namespace Note.API.Controllers
{
    using AutoMapper;
    using API.Common.Extensions;
    using API.Common.Helpers;
    using API.Common.Messages;
    using API.Common.Settings;
    using API.DataContracts;
    using API.DataContracts.Responses;
    using Note.Services.Contracts;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Options;
    using Microsoft.IdentityModel.Tokens;
    using System;
    using System.Collections.Generic;
    using System.IdentityModel.Tokens.Jwt;
    using System.Security.Claims;
    using System.Text;
    using DC = DataContracts;
    using RP = Repository.Data.Entities;

    [ApiVersion("1.0")]
    //[Route("api/users")]//required for default versioning
    [Route("api/v{version:apiVersion}/users")]
    [Authorize]
    [ApiController]
    public class UserController : Controller
    {
        private IUserService _userService;
        private AppSettings _appSettings;
        private IUrlHelper _urlHelper;
        private IPropertyMappingService _propertyMappingService;
        private ITypeHelperService _typeHelperService;

        public UserController(
            IUserService userService,
            IOptions<AppSettings> appSettings, IUrlHelper urlHelper, IPropertyMappingService propertyMappingService,
            ITypeHelperService typeHelperService)
        {
            _userService = userService;
            _appSettings = appSettings.Value;
            _urlHelper = urlHelper;
            _propertyMappingService = propertyMappingService;
            _typeHelperService = typeHelperService;
        }


        #region Utilities
        private string CreateUserResourceUri(UserResourceParameters paginationResourceParameters, ResourceUriType type)
        {
            switch (type)
            {
                case ResourceUriType.PreviousPage:
                    return _urlHelper.Link("getAllUsers",
                      new
                      {
                          fields = paginationResourceParameters.Fields,
                          orderBy = paginationResourceParameters.OrderBy,
                          searchQuery = paginationResourceParameters.SearchQuery,
                          genre = paginationResourceParameters.Clinic,
                          pageNumber = paginationResourceParameters.PageNumber - 1,
                          pageSize = paginationResourceParameters.PageSize
                      });
                case ResourceUriType.NextPage:
                    return _urlHelper.Link("getAllUsers",
                      new
                      {
                          fields = paginationResourceParameters.Fields,
                          orderBy = paginationResourceParameters.OrderBy,
                          searchQuery = paginationResourceParameters.SearchQuery,
                          genre = paginationResourceParameters.Clinic,
                          pageNumber = paginationResourceParameters.PageNumber + 1,
                          pageSize = paginationResourceParameters.PageSize
                      });

                default:
                    return _urlHelper.Link("getAllUsers",
                    new
                    {
                        fields = paginationResourceParameters.Fields,
                        orderBy = paginationResourceParameters.OrderBy,
                        searchQuery = paginationResourceParameters.SearchQuery,
                        genre = paginationResourceParameters.Clinic,
                        pageNumber = paginationResourceParameters.PageNumber,
                        pageSize = paginationResourceParameters.PageSize
                    });
            }
        }

        #endregion Utilities

        #region Methods

        #region POST
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userDto"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost("authenticate")]
        public IActionResult Authenticate([FromBody]UserCredentials userDto)
        {
            var user = Mapper.Map<Tuple<RP.User, bool, string>, Tuple<DC.User, bool, string>>(_userService.Authenticate(userDto.Username, userDto.Password));

            if (!user.Item2)
            {
                switch (user.Item3)
                {
                    case AlertMessages.User_Doesnot_Exits:
                        return NotFound(new ApiErrorResponseData(user.Item2, null, new KeyValuePair<string, string>("404", user.Item3)));
                    case AlertMessages.Username_Password_Empty:
                        return BadRequest(new ApiErrorResponseData(user.Item2, null, new KeyValuePair<string, string>("400", user.Item3)));
                    case AlertMessages.Password_Incorrect:
                        return BadRequest(new ApiErrorResponseData(user.Item2, null, new KeyValuePair<string, string>("400", user.Item3)));
                }
            }

            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            byte[] key = Encoding.ASCII.GetBytes(_appSettings.Secret);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Item1.Id.ToString())
                }),
                Expires = DateTime.UtcNow.AddHours(10),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);
            user.Item1.Token = tokenString;

            // return basic user info (without password) and token to store client side
            return Ok(new ApiSuccessResponseData(user.Item2, user.Item1, new KeyValuePair<string, string>("200", user.Item3)));
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public IActionResult Register([FromBody]DC.Requests.UserCreationRequest user)
        {
            try
            {
                // save 
                Tuple<RP.User, bool, string> userData = _userService.Create(user);

                if (!userData.Item2) return NotFound(new ApiErrorResponseData(userData.Item2, null, new KeyValuePair<string, string>("404", userData.Item3)));

                return Ok(new ApiSuccessResponseData(userData.Item2, userData.Item1, new KeyValuePair<string, string>("200", userData.Item3)));
            }
            catch (Exception ex)
            {
                // return error message if there was an exception
                return NotFound();
            }
        }
        #endregion

        #region GET
        [HttpGet(Name = "getAllUsers")]
        public IActionResult GetAll([FromQuery]UserResourceParameters pageparams)
        {
            if (!_propertyMappingService.ValidMappingExistsFor<UserCreationResponse, RP.User>
               (pageparams.OrderBy))
            {
                return BadRequest(new ApiErrorResponseData(false, null, new KeyValuePair<string, string>("400", "Bad Request")));
            }


            if (!_typeHelperService.TypeHasProperties<UserCreationResponse>
                (pageparams.Fields))
            {
                return BadRequest(new ApiErrorResponseData(false, null, new KeyValuePair<string, string>("400", "Bad Request")));
            }

            var users = _userService.GetAll(pageparams);

            var prevPageLink = users.Item1.HasPrevious ?
                CreateUserResourceUri(pageparams,
                ResourceUriType.PreviousPage) : null;

            var nxtPageLink = users.Item1.HasNext ?
                CreateUserResourceUri(pageparams,
                ResourceUriType.NextPage) : null;

            var paginationMetadata = new
            {
                totalCount = users.Item1.TotalCount,
                pageSize = users.Item1.PageSize,
                currentPage = users.Item1.CurrentPage,
                totalPages = users.Item1.TotalPages,
                previousPageLink = prevPageLink,
                nextPageLink = nxtPageLink
            };

            Response.Headers.Add("X-Pagination",
                Newtonsoft.Json.JsonConvert.SerializeObject(paginationMetadata));

            var userMappedList = Mapper.Map<IEnumerable<UserCreationResponse>>(users.Item1);

            return Ok(new ApiSuccessResponseData(users.Item2, userMappedList.ShapeData(pageparams.Fields), new KeyValuePair<string, string>("200", users.Item3)));
        }


        [HttpGet("{id}", Name = "GetUser")]
        public IActionResult GetById(string id, [FromQuery] string fields)
        {
            if (!_typeHelperService.TypeHasProperties<UserCreationResponse>
              (fields))
            {
                return BadRequest();
            }

            var userFromRepo = _userService.GetById(id);

            if (!userFromRepo.Item2)
            {
                return NotFound();
            }

            var user = Mapper.Map<UserCreationResponse>(userFromRepo.Item1);
            return Ok(user.ShapeData(fields));
        }
        #endregion

        //#region PUT
        //[HttpPut]
        //[Route("Update")]
        //public Tuple<User, bool, string> Update(string id, [FromBody]User userDto)
        //{
        //    // map dto to entity and set id
        //    var user = _mapper.Map<Note.Services.Model.User>(userDto);
        //    user.Id = id;

        //    try
        //    {
        //        // save 
        //        var userDtata = _userService.Update(user, userDto.Password);
        //        return _mapper.Map<Tuple<User, bool, string>>(userDtata);
        //    }
        //    catch (Exception ex)
        //    {
        //        // return error message if there was an exception
        //        return _mapper.Map<Tuple<User, bool, string>>(Tuple.Create(new User(), ex.Message));
        //    }
        //}

        //[HttpPut]
        //[Route("UserActivateDeactivate")]
        //public Tuple<bool, string> UserActivateDeactivate(string id)
        //{
        //    try
        //    {
        //        var userDtata = _userService.UserActivateDeactivate(id);
        //        return _mapper.Map<Tuple<bool, string>>(userDtata);
        //    }
        //    catch (Exception ex)
        //    {
        //        // return error message if there was an exception
        //        return _mapper.Map<Tuple<bool, string>>(Tuple.Create(new User(), ex.Message));
        //    }
        //}
        //#endregion

        //#region DELETE
        //[HttpDelete]
        //[Route("Delete")]
        //public Tuple<bool, string> Delete(string id)
        //{
        //    try
        //    {
        //        var userDtata = _userService.Delete(id);
        //        return _mapper.Map<Tuple<bool, string>>(userDtata);
        //    }
        //    catch (Exception ex)
        //    {
        //        // return error message if there was an exception
        //        return _mapper.Map<Tuple<bool, string>>(Tuple.Create(new User(), ex.Message));
        //    }
        //}
        //#endregion

        #endregion Methods
    }
}