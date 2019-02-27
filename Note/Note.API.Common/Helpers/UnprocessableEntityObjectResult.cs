namespace Note.API.Common.Helpers
{
    using Microsoft.AspNetCore.Mvc.ModelBinding;
    using System.Collections.Generic;
    using Microsoft.AspNetCore.Mvc;
    using System;

    public class UnprocessableEntityObjectResult : ObjectResult
    {
        public UnprocessableEntityObjectResult(ModelStateDictionary modelState) : base(new
        {
            Success = false,
            Payload = new SerializableError(modelState),
            Status = new
            {
                Code = "422",
                Message = "Unprocessable Entity"
            }
        })
        {
            if (modelState == null)
            {
                throw new ArgumentNullException(nameof(modelState));
            }

            StatusCode = 422;
        }
    }
}
