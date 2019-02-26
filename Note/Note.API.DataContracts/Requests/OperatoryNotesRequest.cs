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
        [Required(ErrorMessage = AlertMessages.ClinicId_Required)]
        public string ClinicId { get; set; }

        [Required(ErrorMessage = AlertMessages.PatientId_Required)]
        [MaxLength(5, ErrorMessage = AlertMessages.PatientId_Invalid)]
        public string PatientId { get; set; }

        [Required(ErrorMessage = AlertMessages.ProviderId_Required)]
        [MaxLength(3, ErrorMessage = AlertMessages.ProviderId_Invalid)]
        public string UserId { get; set; }
    }
}
