namespace Note.API.Controllers
{
    using AutoMapper;
    using Note.API.DataContracts.Responses;
    using Note.Services.Contracts;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using System.Collections.Generic;    

    [ApiVersion("1.0")]
    [Route("api/newpatients")]//required for default versioning
    [Route("api/v{version:apiVersion}/newpatients")]
    [Authorize]       
    public class NewPatientController : Controller
    {
        private readonly INewPatientService _service;
        private readonly IMapper _mapper;
        private ILogger<NewPatientController> _logger;

        public NewPatientController(INewPatientService service, IMapper mapper, ILogger<NewPatientController> logger)
        {
            _service = service;
            _mapper = mapper;
            _logger = logger;
        }

        #region GET
        [HttpGet]
        //[HttpGet("{clinicId}")]
        [Route("getbyclinicid/{clinicId}")]
        public IActionResult GetByClinicId(string clinicId)
        {
            var data = _service.GetNewPatientDetailByClinicIdAsync(clinicId);

            if (data != null && data.Item2)
            {
                _logger.LogInformation(200, data.Item3);
                return Ok(new ApiSuccessResponseData(data.Item2, data.Item1, new KeyValuePair<string, string>("200", data.Item3)));
            }
            else
            {
                _logger.LogInformation(500, data.Item3);
                return NotFound(new ApiErrorResponseData(false, null, new KeyValuePair<string, string>("404", data.Item3)));
            }
        }

        [HttpGet]
        [Route("getdashboarddetails")]
        public IActionResult GetNewPatientDetailByCurrentMonthAsync()
        {
            var data = _service.GetNewPatientDetailByCurrentMonthAsync();

            if (data != null && data.Item2)
            {
                _logger.LogInformation(200, data.Item3);
                return Ok(new ApiSuccessResponseData(data.Item2, data.Item1, new KeyValuePair<string, string>("200", data.Item3)));
            }
            else
            {
                _logger.LogInformation(500, data.Item3);
                return NotFound(new ApiErrorResponseData(false, null, new KeyValuePair<string, string>("404", data.Item3)));
            }
        }
        #endregion
    }
}
