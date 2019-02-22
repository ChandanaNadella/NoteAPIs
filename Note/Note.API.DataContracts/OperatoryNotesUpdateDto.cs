using Note.API.Common.Messages;
using Note.API.DataContracts.Requests;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Note.API.DataContracts
{
    public class OperatoryNotesUpdateDto
    {
        [Key]
        [Required (ErrorMessage = AlertMessages.NoteId_Required)]
    //    [CustomError(ErrorMessage = AlertMessages.NoteId_Required)]
        public int note_id { set; get; }

     
        [Required(ErrorMessage = AlertMessages.PatientId_Required)]
      //  [CustomError.logger(ErrorMessage = AlertMessages.PatientId_Required)]
        [MaxLength(5,ErrorMessage = AlertMessages.PatientId_Required)]
        public string patient_id { set; get; }

        public DateTime date_entered { set; get; }

        [Required(ErrorMessage = AlertMessages.ProviderId_Required)]
        [MaxLength(3, ErrorMessage = AlertMessages.ProviderId_Invalid)]
        public string user_id { set; get; }


        //[MaxLength(1, ErrorMessage = "This Feild Exceeds the limit.")]
        public char note_class { set; get; }
        [Required(ErrorMessage = AlertMessages.NoteType_Required)]
        [MaxLength(1, ErrorMessage = AlertMessages.NoteType_Invalid)]
        public string note_type { set; get; }

        public int note_type_id { set; get; }

        //[MaxLength(40, ErrorMessage = "This Feild Exceeds the limit.")]
        public string description { set; get; }

        //[MaxLength(4000, ErrorMessage = "This Feild Exceeds the limit.")]
        public string note { set; get; }

        public int color { set; get; }

       // [MaxLength(1, ErrorMessage = "This Feild Exceeds the limit.")]
        public char post_proc_status { set; get; }

        public String date_modified { set; get; }

       // [MaxLength(3)]
        public string modified_by { set; get; }

        public int locked_eod { set; get; }

        //[MaxLength(1)]
        public char status { set; get; }

        //[MaxLength(55)]
        public string tooth_data { set; get; }

        //DEFAULT Value = -1
        public int claim_id { set; get; }

        //[MaxLength(1)]
        //DEFAULT Value = N
        public char statement_yn { set; get; }

        //[MaxLength(5)]
        public string resp_party_id { set; get; }

        //[MaxLength(10)]
        public string tooth { set; get; }

        public int tran_num { set; get; }

        //[MaxLength(40)]
        public string archive_name { set; get; }

        //[MaxLength(4000)]
        public string archive_path { set; get; }

        //[MaxLength(5)]
        public string service_code { set; get; }

        [Required(ErrorMessage = AlertMessages.ClinicId_Required)]
        public short practice_id { set; get; }

        public DateTime freshness { set; get; }

        //[MaxLength(23)]
        public string surface_detail { set; get; }

        //[MaxLength(8)]
        public string surface { set; get; }
    }
}
