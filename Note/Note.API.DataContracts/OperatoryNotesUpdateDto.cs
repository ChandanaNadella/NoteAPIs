namespace Note.API.DataContracts
{
    using Note.API.Common.Messages;
    using System;
    using System.ComponentModel.DataAnnotations;

    public class OperatoryNotesUpdateDto
    {
        [Key]
        [Required (ErrorMessage = AlertMessages.noteId_Required)]
        [RegularExpression("^[0-9]\\d*$", ErrorMessage = AlertMessages.noteId_Invalid)]
        public int note_id { set; get; }

 
        [Required(ErrorMessage = AlertMessages.patientId_Required)]
        [RegularExpression("^([0-9]{1,5})", ErrorMessage = AlertMessages.patientId_Invalid)]
        public string patient_id { set; get; }

        [Required(ErrorMessage = AlertMessages.providerId_Required)]
        [RegularExpression("^([a-zA-Z0-9]{1,3})", ErrorMessage = AlertMessages.providerId_Invalid)]
        public string user_id { set; get; }


        [RegularExpression("^([a-zA-Z])", ErrorMessage = AlertMessages.noteclass_Invalid)]
        public string note_class { set; get; }

        [Required(ErrorMessage = AlertMessages.noteType_Required)]
        [RegularExpression("^([a-zA-Z0-9])", ErrorMessage = AlertMessages.noteType_Invalid)]
        public string note_type { set; get; }

        public string note { set; get; }

        [RegularExpression("^[0-9]\\d*$", ErrorMessage = AlertMessages.color_Invalid)]
        public int color { set; get; }

        [RegularExpression("^([a-zA-Z])", ErrorMessage = AlertMessages.post_proc_status_Invalid)]
        public string post_proc_status { set; get; }

        [RegularExpression("^[1-9]\\d*$", ErrorMessage = AlertMessages.locked_Eod_Invalid)]
        public int locked_eod { set; get; }

        [RegularExpression("^([a-zA-Z])", ErrorMessage = AlertMessages.status_Invalid)]
        public string status { set; get; }

        [RegularExpression("^([a-zA-Z]{1,55})", ErrorMessage = AlertMessages.tooth_data_Invalid)]
        public string tooth_data { set; get; }

        //DEFAULT Value = -1
        [RegularExpression("^-?[0-9]\\d*$", ErrorMessage = AlertMessages.claimId_Invalid)]
        public int claim_id { set; get; }

        [RegularExpression("^([a-zA-Z])", ErrorMessage = AlertMessages.statement_yn_Invalid)]
        //DEFAULT Value = N
        public string statement_yn { set; get; }

        [RegularExpression("^([0-9]{1,5})", ErrorMessage = AlertMessages.resp_party_id_Invalid)]
        public string resp_party_id { set; get; }

        [RegularExpression("^([a-zA-Z0-9]{1,10})", ErrorMessage = AlertMessages.tooth_Invalid)]
        public string tooth { set; get; }

        [RegularExpression("^[1-9]\\d*$", ErrorMessage = AlertMessages.tranNum_Invalid)]
        public int tran_num { set; get; }

        [RegularExpression("^([a-zA-Z0-9]{1,40})", ErrorMessage = AlertMessages.archive_name_Invalid)]
        public string archive_name { set; get; }

        [RegularExpression("^([a-zA-Z0-9]{1,4000})", ErrorMessage = AlertMessages.archive_path_Invalid)]
        public string archive_path { set; get; }

        [RegularExpression("^([a-zA-Z0-9]{1,5})", ErrorMessage = AlertMessages.service_code_Invalid)]
        public string service_code { set; get; }

        [RegularExpression("^[1-9]\\d*$", ErrorMessage = AlertMessages.clinicId_Invalid)]
        public short practice_id { set; get; }


        [RegularExpression("^([a-zA-Z]{1,23})", ErrorMessage = AlertMessages.surface_detail_Invalid)]
        public string surface_detail { set; get; }

        [RegularExpression("^([a-zA-Z0-9]{1,8})", ErrorMessage = AlertMessages.surface_Invalid)]
        public string surface { set; get; }
    }
}
