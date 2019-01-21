namespace Note.API.Controllers.V1
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
    [Route("api/v{version:apiVersion}/subscriber")]
    //[Authorize]
    [ApiController]
    public class SubscriberController : Controller
    {
        private ISubscriberService _subscriberService;
        private AppSettings _appSettings;
        private IUrlHelper _urlHelper;
        private IPropertyMappingService _propertyMappingService;
        private ITypeHelperService _typeHelperService;

        public SubscriberController(ISubscriberService subscriberService, IOptions<AppSettings> appSettings, IUrlHelper urlHelper, IPropertyMappingService propertyMappingService,
            ITypeHelperService typeHelperService)
        {
            _subscriberService = subscriberService;
            _appSettings = appSettings.Value;
            _urlHelper = urlHelper;
            _propertyMappingService = propertyMappingService;
            _typeHelperService = typeHelperService;
        }

        #region Utilities
        //private string CreateSubscriberResourceUri(SubscriberResourceParameters paginationResourceParameters, ResourceUriType type)
        //{
        //    switch (type)
        //    {
        //        case ResourceUriType.PreviousPage:
        //            return _urlHelper.Link("getAllSubscribers",
        //              new
        //              {
        //                  fields = paginationResourceParameters.Fields,
        //                  orderBy = paginationResourceParameters.OrderBy,
        //                  searchQuery = paginationResourceParameters.SearchQuery,
        //                  pageNumber = paginationResourceParameters.PageNumber - 1,
        //                  pageSize = paginationResourceParameters.PageSize
        //              });
        //        case ResourceUriType.NextPage:
        //            return _urlHelper.Link("getAllSubscribers",
        //              new
        //              {
        //                  fields = paginationResourceParameters.Fields,
        //                  orderBy = paginationResourceParameters.OrderBy,
        //                  searchQuery = paginationResourceParameters.SearchQuery,
        //                  pageNumber = paginationResourceParameters.PageNumber + 1,
        //                  pageSize = paginationResourceParameters.PageSize
        //              });

        //        default:
        //            return _urlHelper.Link("getAllSubscribers",
        //            new
        //            {
        //                fields = paginationResourceParameters.Fields,
        //                orderBy = paginationResourceParameters.OrderBy,
        //                searchQuery = paginationResourceParameters.SearchQuery,
        //                pageNumber = paginationResourceParameters.PageNumber,
        //                pageSize = paginationResourceParameters.PageSize
        //            });
        //    }
        //}

        #endregion Utilities

        [HttpPost("register")]
        public IActionResult Register([FromBody]DC.Requests.SubscriberCreationRequest subscriber)
        {
            try
            {
                // save 
                Tuple<bool, string> data = _subscriberService.Create(subscriber);

                if (!data.Item1)
                    return NotFound(new ApiErrorResponseData(data.Item1, null, new KeyValuePair<string, string>("404", data.Item2)));

                return Ok(new ApiSuccessResponseData(data.Item1, new { message = data.Item2 }, new KeyValuePair<string, string>("200", data.Item2)));
            }
            catch (Exception ex)
            {
                // return error message if there was an exception
                return NotFound(new ApiErrorResponseData(false, null, new KeyValuePair<string, string>("404", ex.Message)));
            }
        }

        [HttpPost("getbyid")]
        public IActionResult GetById(int id)
        {
            try
            {
                // save 
                Tuple<RP.Subscriber, bool, string> data = _subscriberService.GetById(id);

                if (!data.Item2) return NotFound(new ApiErrorResponseData(data.Item2, null, new KeyValuePair<string, string>("404", data.Item3)));

                return Ok(new ApiSuccessResponseData(data.Item2, data.Item2, new KeyValuePair<string, string>("200", data.Item3)));
            }
            catch (Exception ex)
            {
                // return error message if there was an exception
                return NotFound(new ApiErrorResponseData(false, null, new KeyValuePair<string, string>("404", ex.Message)));
            }
        }

        //[HttpGet(Name = "getAllSubscribers")]
        //public IActionResult GetAll([FromQuery]SubscriberResourceParameters pageparams)
        //{
        //    if (!_propertyMappingService.ValidMappingExistsFor<SubscriberResourceParameters, RP.Subscriber>
        //       (pageparams.OrderBy))
        //    {
        //        return BadRequest();
        //    }


        //    if (!_typeHelperService.TypeHasProperties<SubscriberCreationResponse>
        //        (pageparams.Fields))
        //    {
        //        return BadRequest();
        //    }

        //    var subscriber = _subscriberService.GetAll(pageparams);

        //    var prevPageLink = subscriber.Item1.HasPrevious ?
        //      CreateSubscriberResourceUri(pageparams,
        //        ResourceUriType.PreviousPage) : null;

        //    var nxtPageLink = subscriber.Item1.HasNext ?
        //        CreateSubscriberResourceUri(pageparams,
        //        ResourceUriType.NextPage) : null;

        //    var paginationMetadata = new
        //    {
        //        totalCount = subscriber.Item1.TotalCount,
        //        pageSize = subscriber.Item1.PageSize,
        //        currentPage = subscriber.Item1.CurrentPage,
        //        totalPages = subscriber.Item1.TotalPages,
        //        previousPageLink = prevPageLink,
        //        nextPageLink = nxtPageLink
        //    };

        //    Response.Headers.Add("X-Pagination",
        //        Newtonsoft.Json.JsonConvert.SerializeObject(paginationMetadata));

        //    var userMappedList = Mapper.Map<IEnumerable<SubscriberCreationResponse>>(subscriber.Item1);

        //    var result = new
        //    {
        //        data = userMappedList.ShapeData(pageparams.Fields),
        //        success = subscriber.Item2,
        //        message = subscriber.Item3
        //    };

        //    return Ok(result);
        //}

    }
}
