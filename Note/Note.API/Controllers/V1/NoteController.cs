namespace Note.API.Controllers
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
    using Note.Repository.Data;   
    using RC=Note.Repository.Data.Entities;
    using Note.Repository.Data.Entities;

    [ApiVersion("1.0")]
    //[Route("api/users")]//required for default versioning
    [Route("api/v{version:apiVersion}/notes")]    
    [ApiController]
    [Authorize]
    public class NoteController : Controller
    {
        private IUserService _userService;
        private AppSettings _appSettings;
        private IUrlHelper _urlHelper;
        private IPropertyMappingService _propertyMappingService;
        private ITypeHelperService _typeHelperService;
        NoteDataContext objcat = new NoteDataContext();

        static List<Notes> notes = new List<Notes>() {
        new Notes(){ Id=1,Name="Note-1"},
        new Notes(){ Id=2,Name="Note-2"},
        new Notes(){ Id=3,Name="Note-3"},
        };


        public NoteController(
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
        [HttpGet(Name = "getAllNotes")]
        [AllowAnonymous]
        //[Route("")]

        public IActionResult Get()
        {
            NoteDataContext objnote = new NoteDataContext();
            var lstNote = objnote.GetOperatoryNotes();
            return Ok(lstNote);

        }

        [HttpGet("{id}")]
        public IActionResult Detail(string id)
        {
            NoteDataContext objnote = new NoteDataContext();
            var lstNote = objnote.GetOperatoryNotesByPatientId(id);
            return Ok(lstNote);

        }

        #endregion
        #endregion Methods
    }
    public class Notes
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
