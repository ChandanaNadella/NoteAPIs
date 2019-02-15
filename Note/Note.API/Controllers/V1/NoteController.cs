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
    using Note.Repository.Data;   
    using RC=Note.Repository.Data.Entities;
    using Note.Repository.Data.Entities;
    using Note.API.DataContracts.Requests;
    using Note.Services;
    using System.IO;
    using Microsoft.Extensions.Logging;
    using System.Linq;

    [ApiVersion("1.0")]
    //[Route("api/users")]//required for default versioning
    [Route("api/v{version:apiVersion}/notes")]    
    [ApiController]
   
    public class NoteController : Controller
    {
       
        private AppSettings _appSettings;
        private IUrlHelper _urlHelper;
        private IPropertyMappingService _propertyMappingService;
        private ITypeHelperService _typeHelperService;
        private INoteService _noteService;
        private ILogger _logger;
       
        public NoteController(
            INoteService noteService,
            ILogger<NoteController> logger,
            IOptions<AppSettings> appSettings, IUrlHelper urlHelper, IPropertyMappingService propertyMappingService,
            ITypeHelperService typeHelperService)
        {
            _noteService = noteService;
            _logger = logger;
            _appSettings = appSettings.Value;
            _urlHelper = urlHelper;
            _propertyMappingService = propertyMappingService;
            _typeHelperService = typeHelperService;
        }


        #region Utilities
        private string CreateNoteResourceUri(NoteResourceParameter paginationResourceParameters, ResourceUriType type)
        {
            switch (type)
            {
                case ResourceUriType.PreviousPage:



               var prevUrl=  _urlHelper.Link("GetPatientNotes",
                      new
                      {
                          fields = paginationResourceParameters.Fields,
                          orderBy = paginationResourceParameters.OrderBy,
                          //searchQuery = paginationResourceParameters.SearchQuery,
                          //genre = paginationResourceParameters.Clinic,
                          pageNumber = paginationResourceParameters.PageNumber - 1,
                          pageSize = paginationResourceParameters.PageSize
                      });
                    return prevUrl;
                case ResourceUriType.NextPage:

                    return _urlHelper.Link("GetPatientNotes",
                      new
                      {
                          fields = paginationResourceParameters.Fields,
                          orderBy = paginationResourceParameters.OrderBy,
                          //searchQuery = paginationResourceParameters.SearchQuery,
                          //genre = paginationResourceParameters.Clinic,
                          pageNumber = paginationResourceParameters.PageNumber + 1,
                          pageSize = paginationResourceParameters.PageSize
                      });

                default:
                    return _urlHelper.Link("GetPatientNotes",
                    new
                    {
                        fields = paginationResourceParameters.Fields,
                        orderBy = paginationResourceParameters.OrderBy,
                        //searchQuery = paginationResourceParameters.SearchQuery,
                        //genre = paginationResourceParameters.Clinic,
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
        //[AllowAnonymous]
        //[HttpPost("authenticate")]
        //public IActionResult Authenticate([FromBody]UserCredentials userDto)
        //{
        //    var user = Mapper.Map<Tuple<RP.User, bool, string>, Tuple<DC.User, bool, string>>(_userService.Authenticate(userDto.Username, userDto.Password));

        //    if (!user.Item2)
        //    {
        //        switch (user.Item3)
        //        {
        //            case AlertMessages.User_Doesnot_Exits:
        //                return NotFound(new ApiErrorResponseData(user.Item2, null, new KeyValuePair<string, string>("404", user.Item3)));
        //            case AlertMessages.Username_Password_Empty:
        //                return BadRequest(new ApiErrorResponseData(user.Item2, null, new KeyValuePair<string, string>("400", user.Item3)));
        //            case AlertMessages.Password_Incorrect:
        //                return BadRequest(new ApiErrorResponseData(user.Item2, null, new KeyValuePair<string, string>("400", user.Item3)));
        //        }
        //    }

        //    JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
        //    byte[] key = Encoding.ASCII.GetBytes(_appSettings.Secret);

        //    var tokenDescriptor = new SecurityTokenDescriptor
        //    {
        //        Subject = new ClaimsIdentity(new Claim[]
        //        {
        //            new Claim(ClaimTypes.Name, user.Item1.Id.ToString())
        //        }),
        //        Expires = DateTime.UtcNow.AddHours(10),
        //        SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        //    };

        //    var token = tokenHandler.CreateToken(tokenDescriptor);
        //    var tokenString = tokenHandler.WriteToken(token);
        //    user.Item1.Token = tokenString;

        //    // return basic user info (without password) and token to store client side
        //    return Ok(new ApiSuccessResponseData(user.Item2, user.Item1, new KeyValuePair<string, string>("200", user.Item3)));
        //}

        //[HttpPost("register")]
        //[AllowAnonymous]
        //public IActionResult Register([FromBody]DC.Requests.UserCreationRequest user)
        //{
        //    try
        //    {
        //        // save 
        //        Tuple<RP.User, bool, string> userData = _userService.Create(user);

        //        if (!userData.Item2) return NotFound(new ApiErrorResponseData(userData.Item2, null, new KeyValuePair<string, string>("404", userData.Item3)));

        //        return Ok(new ApiSuccessResponseData(userData.Item2, userData.Item1, new KeyValuePair<string, string>("200", userData.Item3)));
        //    }
        //    catch (Exception ex)
        //    {
        //        // return error message if there was an exception
        //        return NotFound();
        //    }
        //}
        #endregion

        #region GET
       
        [HttpGet(Name = "GetPatientNotes")]
        public IActionResult GetPatientNotes([FromQuery]NoteResourceParameter notesData)
        {

            try
            {

                if (notesData.OperatoryNoteRequest.ClinicId == null || notesData.OperatoryNoteRequest.PatientId == null || notesData.OperatoryNoteRequest.UserId == null)
                {

                    return BadRequest(new ApiErrorResponseData(false, null, new KeyValuePair<string, string>("400", "Bad Request")));

                }

                if (!_typeHelperService.TypeHasProperties<DC.OperatoryNotes>(notesData.Fields))
                {
                    return BadRequest(new ApiErrorResponseData(false, null, new KeyValuePair<string, string>("400", "Bad Request")));
                }

                var result = _noteService.getNotes(notesData);

                    


                    var prevPageLink = result.HasPrevious ?
                CreateNoteResourceUri(notesData,
                ResourceUriType.PreviousPage) : null;

                    var nxtPageLink = result.HasNext ?
                        CreateNoteResourceUri(notesData,
                        ResourceUriType.NextPage) : null;

                    var paginationMetadata = new
                    {
                        totalCount = result.TotalCount,
                        pageSize = result.PageSize,
                        currentPage = result.CurrentPage,
                        totalPages = result.TotalPages,
                        previousPageLink = prevPageLink,
                        nextPageLink = nxtPageLink
                    };

                    Response.Headers.Add("X-Pagination",
                        Newtonsoft.Json.JsonConvert.SerializeObject(paginationMetadata));


                    // var orderByresult = result.OrderBy(x=>x.Equals(notesData.OrderBy));





                    //var OperatoryRepo = _noteService.getNotes(notesData);
                    // var Operatory = Mapper.Map<IEnumerable<OperatoryNotes>>(OperatoryRepo);

                    if ((result == null) || (result.Count() == 0))
                    {
                        string filePath = @"..\Note.API.Common\ErrorLogs\Error.txt";
                        using (StreamWriter writer = new StreamWriter(filePath, true))
                        {
                            writer.WriteLine("Date : " + DateTime.Now.ToString());
                            writer.WriteLine("Error Status Code: 404, Error Status Message: Not Found");
                            writer.WriteLine("-----------------------------------------------------------------------------");
                        }

                        return NotFound(new ApiErrorResponseData(false, null, new KeyValuePair<string, string>("404", "Not Found")));


                    }
                var noteMappedList = Mapper.Map<IEnumerable<OperatoryNotes>>(result);

                return Ok(new ApiSuccessResponseData(true, noteMappedList.ShapeData(notesData.Fields), new KeyValuePair<string, string>("200","Success")));

               


            }
            catch (Exception ex)
            {
            }

            return null; 

        }
        #endregion
        #endregion Methods

        [HttpPost("postInsertOrUpdatePatientsNotes")]
        public IActionResult PostInsertUpdateOperatoryNotes(OperatoryNotesUpdateDto opNotesDto, int? autoNoteId, string noteType)
        {

            var OperatoryNotesUpdateDto = Mapper.Map<RP.operatory_notes>(opNotesDto);
            

            if (noteType != null || noteType =="")
            { 
                  _noteService.InsertOrUpdateNotes(OperatoryNotesUpdateDto, autoNoteId, noteType);
                return Ok(new ApiSuccessResponseData(true, opNotesDto, new KeyValuePair<string, string>("200", "Success")));
               
            }

          
           
            else
            {
                return BadRequest(new ApiErrorResponseData(false, null, new KeyValuePair<string, string>("400", "Bad Request, Note-Type is required")));
            }
           

        }

    }
   
    
}
