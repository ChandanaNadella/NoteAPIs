using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Text;
using Note.API.Common;
using Note.API.Common.Messages;

namespace Note.API.DataContracts.Requests
{
    public class OperatoryNotesRequest
    {

        [Required(ErrorMessage = AlertMessages.clinicId_Required)]
        [Range(1, short.MaxValue, ErrorMessage = AlertMessages.clinicId_Range)]
        [RegularExpression("^([1-9]+)", ErrorMessage = AlertMessages.clinicId_Invalid)]

        public string ClinicId { get; set; }


        [Required(ErrorMessage = AlertMessages.patientId_Required)]
        [RegularExpression("^([0-9]{1,5})", ErrorMessage = AlertMessages.patientId_Invalid)]
        public string PatientId { get; set; }

        [Required(ErrorMessage = AlertMessages.providerId_Required)]
        [RegularExpression("^([a-zA-Z0-9]{1,3})", ErrorMessage = AlertMessages.providerId_Invalid)]
        public string UserId { get; set; }
    }
}
