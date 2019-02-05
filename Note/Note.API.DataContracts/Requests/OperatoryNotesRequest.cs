using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Text;

namespace Note.API.DataContracts.Requests
{
   public class  OperatoryNotesRequest
    {
        [CustomError(ErrorMessage = "Clinic Id is Required Field")]
        public string  ClinicId {get; set; }
        [CustomError(ErrorMessage = "Patient Id is Required Field")]
        
        public string PatientId { get; set; }
        [CustomError(ErrorMessage = "Provider Id is Required Field")]  
        public string ProviderId { get; set; }
    }
}
