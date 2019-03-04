namespace Note.API.Controllers
{
    using API.Common.Extensions;
    using API.Common.Helpers;
    using API.Common.Messages;
    using API.Common.Settings;
    using API.DataContracts;
    using API.DataContracts.Responses;
    using AutoMapper;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using Note.API.DataContracts.Requests;
    using Note.Services.Contracts;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using DC = DataContracts;
    using RP = Repository.Data.Entities;

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

        /// <summary>
        /// To Add Sorting,Paging,Data Shaping for the GetBy method.
        /// </summary>
        /// <param name="paginationResourceParameters"></param>
        /// <param name="type"></param>
        /// <returns>pageNumber,pageSize,orderBy,fields</returns>

        #region Utilities
        private string CreateNoteResourceUri(NoteResourceParameter paginationResourceParameters, ResourceUriType type)
        {
            switch (type)
            {
                case ResourceUriType.PreviousPage:
                   return _urlHelper.Link("GetPatientNotes",
                      new
                      {
                          fields = paginationResourceParameters.Fields,
                          orderBy = paginationResourceParameters.OrderBy,
                          pageNumber = paginationResourceParameters.PageNumber - 1,
                          pageSize = paginationResourceParameters.PageSize
                      });
             

                case ResourceUriType.NextPage:
                    return _urlHelper.Link("GetPatientNotes",
                      new
                      {
                          fields = paginationResourceParameters.Fields,
                          orderBy = paginationResourceParameters.OrderBy,
                          pageNumber = paginationResourceParameters.PageNumber + 1,
                          pageSize = paginationResourceParameters.PageSize
                      });


                default:

                    return _urlHelper.Link("GetPatientNotes",
                    new
                    {
                        fields = paginationResourceParameters.Fields,
                        orderBy = paginationResourceParameters.OrderBy,
                        pageNumber = paginationResourceParameters.PageNumber,
                        pageSize = paginationResourceParameters.PageSize
                    });
            }
        }

        #endregion Utilities


        #region Methods


        #region GET
        /// <summary>
        /// To get data based on ClinicId,PatientId,UserId from the operatorynotes table.
        /// </summary>
        /// <param name="notesData"></param>
        /// <returns>operatorynotes table filtered by ClinicId,PatientId,UserId </returns>

        [HttpGet(Name = "GetPatientNotes")]
        public IActionResult GetPatientNotes([FromQuery]NoteResourceParameter notesData)

        {
            try
            {
                
                if (notesData.OperatoryNoteRequest.ClinicId == null && notesData.OperatoryNoteRequest.PatientId == null && notesData.OperatoryNoteRequest.UserId == null)
                {
                    _logger.LogInformation("-----------------------------------------------------------------------------");
                    _logger.LogError(string.Format("Date : {0},Request object can not be NULL, Error Status Code: {1}, Error Status Message: {2}", DateTime.Now.ToString(), BadRequestResponse.Code, BadRequestResponse.Message));
                    _logger.LogInformation("-----------------------------------------------------------------------------");
                    return BadRequest(new ApiErrorResponseData(false, "Request object can not be NULL.", new KeyValuePair<string, string>(BadRequestResponse.Code, BadRequestResponse.Message)));


                }

                else
                {
                    if (!ModelState.IsValid)
                    {
                        if (!_typeHelperService.TypeHasProperties<DC.OperatoryNotes>(notesData.Fields))
                        {
                            _logger.LogInformation("-----------------------------------------------------------------------------");
                            _logger.LogError(string.Format("Date : {0}, Error Status Code: {1}, Error Status Message: {2}", DateTime.Now.ToString(), BadRequestResponse.Code, BadRequestResponse.Message));
                            _logger.LogInformation("-----------------------------------------------------------------------------");

                            return BadRequest(new ApiErrorResponseData(false, "Request field(s) not found.", new KeyValuePair<string, string>(BadRequestResponse.Code, BadRequestResponse.Message)));


                        }
                        else
                        {
                            var message = string.Join(" | ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
                            _logger.LogInformation("-----------------------------------------------------------------------------");
                            _logger.LogError(string.Format("Date : {0}, Error Status Code: {1}, Error Status Message: {2}", DateTime.Now.ToString(), UnprocessableEntityResponse.Code, UnprocessableEntityResponse.Message));
                            _logger.LogInformation(message);
                            _logger.LogInformation("-----------------------------------------------------------------------------");

                            return new Common.Helpers.UnprocessableEntityObjectResult(ModelState);

                        }

                    }

                    else
                    {
                        var result = _noteService.getNotes(notesData);

                        // To get the page links of next page and previous page.

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
                        if ((result == null) || (result.Count() == 0))
                        {
                            if (SettingsExtensions.IsDBConnected)
                            {
                                _logger.LogInformation("-----------------------------------------------------------------------------");
                                _logger.LogError(string.Format("Date : {0}, Error Status Code: {1}, Error Status Message:{2}", DateTime.Now.ToString(), NoContentResponse.Code, NoContentResponse.DBConFailedMessage));
                                _logger.LogInformation("-----------------------------------------------------------------------------");


                                return StatusCode(NoContentRespons
                                    e.Code, new ApiErrorResponseData(false, null, new KeyValuePair<string, string>(NoContentResponse.CodeString, NoContentResponse.DBConFailedMessage)));
                            }
                            else
                            {
                                _logger.LogInformation("-----------------------------------------------------------------------------");
                                _logger.LogError(string.Format("Date : {0}, Error Status Code: {1}, Error Status Message:{2}", DateTime.Now.ToString(), InternalServerError.Code, InternalServerError.DBConFailedMessage));
                                _logger.LogInformation("-----------------------------------------------------------------------------");


                               return StatusCode(InternalServerError.Code, new ApiErrorResponseData(false, null, new KeyValuePair<string, string>(InternalServerError.CodeString, InternalServerError.DBConFailedMessage)));
                             
                            }
                        }
                        else
                        {
                            var noteMappedList = Mapper.Map<IEnumerable<OperatoryNotes>>(result);

                            return Ok(new ApiSuccessResponseData(true, noteMappedList.ShapeData(notesData.Fields), new KeyValuePair<string, string>(SuccessResponse.Code, SuccessResponse.Message)));

                        }



                    }

                }

               

            }
            
            // Exception handling
            catch (Exception ex)
            {
            }

            return null; 

        }


        #endregion

        /// <summary>
        /// To Update and Insert a row into operatorynotes tables.
        /// </summary>
        /// <param name="opNotesDto"></param>
        /// <param name="autoNoteId"></param>
        /// <returns>Payload and the row inserted into the operatorynotes table.</returns>
        
        #region POST

        [HttpPost(Name = "PostInsertUpdateOperatoryNotes")]
        public IActionResult PostInsertUpdateOperatoryNotes(OperatoryNotesUpdateDto opNotesDto, int? autoNoteId)
        {
            try
            {
                if ( !SettingsExtensions.IsDBConnected)
                {
                    _logger.LogInformation("-----------------------------------------------------------------------------");
                    _logger.LogError(string.Format("Date : {0}, Error Status Code: {1}, Error Status Message:{2}", DateTime.Now.ToString(), InternalServerError.Code, InternalServerError.DBConFailedMessage));
                    _logger.LogInformation("-----------------------------------------------------------------------------");

                    return StatusCode(InternalServerError.Code, new ApiErrorResponseData(false, null, new KeyValuePair<string, string>(InternalServerError.CodeString, InternalServerError.DBConFailedMessage)));
                }
                 else if (opNotesDto == null)
                {
                    _logger.LogInformation("-----------------------------------------------------------------------------");
                    _logger.LogError(string.Format("Date : {0},Request object can not be NULL, Error Status Code: {1}, Error Status Message: {2}", DateTime.Now.ToString(), BadRequestResponse.Code, BadRequestResponse.Message));
                    _logger.LogInformation("-----------------------------------------------------------------------------");
                    return BadRequest(new ApiErrorResponseData(false, "Request object can not be NULL.", new KeyValuePair<string, string>(BadRequestResponse.Code, BadRequestResponse.Message)));
                }
                else
                {

                    if (!ModelState.IsValid)
                    {


                        var message = string.Join(" | ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));

                        _logger.LogInformation("-----------------------------------------------------------------------------");
                        _logger.LogError(string.Format("Date : {0}, Error Status Code: {1}, Error Status Message: {2}", DateTime.Now.ToString(), UnprocessableEntityResponse.Code, UnprocessableEntityResponse.Message));
                        _logger.LogInformation(message);
                        _logger.LogInformation("-----------------------------------------------------------------------------");


                        return new Common.Helpers.UnprocessableEntityObjectResult(ModelState);

                    }
                    else
                    {
                        var OperatoryNotesUpdateDto = Mapper.Map<RP.operatory_notes>(opNotesDto);
                        var isAffected = _noteService.InsertOrUpdateNotes(OperatoryNotesUpdateDto, autoNoteId, opNotesDto.note_class);

                        if (isAffected == true)
                        {
                            return Ok(new ApiSuccessResponseData(true, opNotesDto, new KeyValuePair<string, string>(SuccessResponse.Code, SuccessResponse.Message)));
                        }
                        else
                        {
                            _logger.LogInformation("-----------------------------------------------------------------------------");
                            _logger.LogError(string.Format("Date : {0}, Error Status Code: {1}, Error Status Message: {2}", DateTime.Now.ToString(), NoContentResponse.Code, NoContentResponse.Message));
                            _logger.LogInformation("-----------------------------------------------------------------------------");

                            return StatusCode(NoContentResponse.Code, new ApiErrorResponseData(false, null, new KeyValuePair<string, string>(NoContentResponse.CodeString, NoContentResponse.DBConFailedMessage)));

                        }

                    }


                }

            }

            // Exception handling
            catch (Exception ex)
            {

            }
            return null;

        }
        #endregion
        #endregion Methods



    }


}
