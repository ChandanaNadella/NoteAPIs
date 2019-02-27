using System;
using System.Collections.Generic;
using System.Text;

namespace Note.API.DataContracts.Responses
{
    public class ApiErrorResponseData
    {
        public ApiErrorResponseData(bool isSucess, object data, KeyValuePair<string, string> statusCode)
        {
            if (isSucess)
            {
                Success = isSucess;
                Payload = data;
              //  Error = null;
            }
            else
            {
                Success = isSucess;
                Payload = data;
                Status = new
                {
                    Code = statusCode.Key,
                    Message = statusCode.Value
                };
            }
        }

        public bool Success { get; set; }

        public object Payload { get; set; }
        public object Status { get; set; }
    }
}
