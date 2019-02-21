using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Text;
using Note.API.Common;
using Note.API.Common.Messages;

namespace Note.API.DataContracts.Requests
{
   public class  OperatoryNotesRequest
    {
        [CustomError(ErrorMessage = AlertMessages.ClinicId_Required)]
        public string  ClinicId {get; set; }
        [CustomError(ErrorMessage = AlertMessages.PatientId_Required)]
        
        public string PatientId { get; set; }
        [CustomError(ErrorMessage = AlertMessages.ProviderId_Required)]  
        public string UserId { get; set; }
    }
}
