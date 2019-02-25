using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Text;

namespace Note.API.Common.Helpers
{
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
