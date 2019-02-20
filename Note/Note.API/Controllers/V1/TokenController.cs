
namespace Note.API.Controllers.V1
{

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Options;
    using Note.API.Common.Settings;
    using Note.API.DataContracts.Responses;
    using Note.Services.Contracts;
    using System.Collections.Generic;
    using DC = Note.API.DataContracts;

    [ApiVersion("1.0")]
    //[Route("api/users")]//required for default versioning
    [Route("api/v{version:apiVersion}/token")]
    [ApiController]
    public class TokenController : Controller
    {
        private AppSettings _appSettings;
        private ITokenService _tokenService;
        public TokenController(
            ITokenService tokenService,
            IOptions<AppSettings> appSettings)
        {

            _tokenService = tokenService;
            _appSettings = appSettings.Value;
        }

        #region Methods

        /// <summary>
        ///To get the ProductName and Token and generate connection string.
        /// </summary>
        /// <param name="tokenDeatils"></param>
        /// <returns>connection string to connect to the database.</returns>

        #region Post

        [HttpPost("PostGenerateToken")]
        public IActionResult PostGenerateToken(DC.TokenDetails tokenDeatils)
        {

            var myToken = _tokenService.GetToken(tokenDeatils);

            return Ok(new ApiSuccessResponseData(true, myToken, new KeyValuePair<string, string>("200", "Success")));

        }

        #endregion Post

        #endregion Methods
    }
}