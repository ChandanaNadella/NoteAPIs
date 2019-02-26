using Microsoft.AspNetCore.Mvc;
namespace Note.API.Common.Helpers
{
    using Microsoft.AspNetCore.Mvc.ModelBinding;
    using System;

    public class UnprocessableEntityObjectResult : ObjectResult
    {
        public UnprocessableEntityObjectResult(ModelStateDictionary modelState) : base(new SerializableError(modelState))
        {
            if (modelState == null)
            {
                //_logger.LogError(string.Format("Date : {0}, Error Status Code: 400, Error Status Message: Bad Request", DateTime.Now.ToString()));
                //_logger.LogInformation("-----------------------------------------------------------------------------");
                throw new ArgumentNullException(nameof(modelState));
            }
            StatusCode = 422;
        }
    }
}
