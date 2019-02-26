namespace Note.API.DataContracts
{
    using Note.API.Common.Messages;
    using System;
    using System.ComponentModel.DataAnnotations;

    public class OperatoryNotesUpdateDto
    {
        [Key]
        [Required (ErrorMessage = AlertMessages.NoteId_Required)]
        public int note_id { set; get; }

     
        [Required(ErrorMessage = AlertMessages.PatientId_Required)]
        [MaxLength(5,ErrorMessage = AlertMessages.PatientId_Required)]
        public string patient_id { set; get; }

        public DateTime date_entered { set; get; }

        [Required(ErrorMessage = AlertMessages.ProviderId_Required)]
        [MaxLength(3, ErrorMessage = AlertMessages.ProviderId_Invalid)]
        public string user_id { set; get; }

        [Required(ErrorMessage = "Note Class should be T")]
        [MaxLength(1, ErrorMessage = "This Feild Exceeds the limit.")]
        public string note_class { set; get; }

        [Required(ErrorMessage = AlertMessages.NoteType_Required)]
        [MaxLength(1, ErrorMessage = AlertMessages.NoteType_Invalid)]
        public string note_type { set; get; }

        public int note_type_id { set; get; }

        [MaxLength(40, ErrorMessage = "This Feild Exceeds the limit.")]
        public string description { set; get; }

        [MaxLength(4000, ErrorMessage = "This Feild Exceeds the limit.")]
        public string note { set; get; }

        public int color { set; get; }

        [MaxLength(1, ErrorMessage = "This Feild Exceeds the limit.")]
        public string post_proc_status { set; get; }

        public string date_modified { set; get; }

        [MaxLength(3, ErrorMessage = "This Feild Exceeds the limit.")]
        public string modified_by { set; get; }

        public int locked_eod { set; get; }

        [MaxLength(1, ErrorMessage = "This Feild Exceeds the limit.")]
        public string status { set; get; }

        [MaxLength(55, ErrorMessage = "This Feild Exceeds the limit.")]
        public string tooth_data { set; get; }

        //DEFAULT Value = -1
        public int claim_id { set; get; }

        [MaxLength(1, ErrorMessage = "This Feild Exceeds the limit.")]
        //DEFAULT Value = N
        public string statement_yn { set; get; }

        [MaxLength(5, ErrorMessage = "This Feild Exceeds the limit.")]
        public string resp_party_id { set; get; }

        [MaxLength(10)]
        public string tooth { set; get; }

        public int tran_num { set; get; }

        [MaxLength(40, ErrorMessage = "This Feild Exceeds the limit.")]
        public string archive_name { set; get; }

        [MaxLength(4000, ErrorMessage = "This Feild Exceeds the limit.")]
        public string archive_path { set; get; }

        [MaxLength(5, ErrorMessage = "This Feild Exceeds the limit.")]
        public string service_code { set; get; }

        [Required(ErrorMessage = AlertMessages.ClinicId_Required)]
        public short practice_id { set; get; }

        public string freshness { set; get; }

        [MaxLength(23, ErrorMessage = "This Feild Exceeds the limit.")]
        public string surface_detail { set; get; }

        [MaxLength(8, ErrorMessage = "This Feild Exceeds the limit.")]
        public string surface { set; get; }
    }
}
