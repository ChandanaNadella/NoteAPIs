namespace Note.API.DataContracts
{
    using Note.API.Common.Messages;
    using System;
    using System.ComponentModel.DataAnnotations;

    public class OperatoryNotesUpdateDto
    {
          [Key]
         [Required(ErrorMessage = AlertMessages.noteId_Required)]
         [RegularExpression("^([0-9]+)", ErrorMessage = AlertMessages.noteId_Invalid)]
         [Range(0, int.MaxValue, ErrorMessage = AlertMessages.noteId_Range)]
         public string note_id { set; get; }     


       
        [RegularExpression("^([1-9]+)", ErrorMessage = AlertMessages.patientId_Invalid)]
        [Required(ErrorMessage = AlertMessages.patientId_Required)]
        public string patient_id { set; get; }

        [Required(ErrorMessage = AlertMessages.providerId_Required)]
        [RegularExpression("^([a-zA-Z0-9]{1,3})", ErrorMessage = AlertMessages.providerId_Invalid)]
        public string user_id { set; get; }


        [RegularExpression("^([a-zA-Z])", ErrorMessage = AlertMessages.noteclass_Invalid)]
        public string note_class { set; get; }

        [Required(ErrorMessage = AlertMessages.noteType_Required)]
        [RegularExpression("^([a-zA-Z0-9]{1})", ErrorMessage = AlertMessages.noteType_Invalid)]
        public string note_type { set; get; }

        public string note { set; get; }

        [Required(ErrorMessage = AlertMessages.color_Required)]
        [RegularExpression("^([0-9]+)", ErrorMessage = AlertMessages.color_Invalid)]        
        [Range(0, int.MaxValue, ErrorMessage = AlertMessages.color_Range)]
        public string color { set; get; }

        [RegularExpression("^([a-zA-Z]{0,1})", ErrorMessage = AlertMessages.post_proc_status_Invalid)]
        public string post_proc_status { set; get; }

       
        [Required(ErrorMessage = AlertMessages.locked_Eod_Required)]
        [RegularExpression("^([0-9]+)", ErrorMessage = AlertMessages.locked_Eod_Invalid)]
        [Range(0, int.MaxValue, ErrorMessage = AlertMessages.locked_Eod_Invalid)]
        public string locked_eod { set; get; }

        [RegularExpression("^([a-zA-Z]{0,1})", ErrorMessage = AlertMessages.status_Invalid)]
        public string status { set; get; }

        [RegularExpression("^([a-zA-Z]{0,55})", ErrorMessage = AlertMessages.tooth_Data_Invalid)]
        public string tooth_data { set; get; }

        //DEFAULT Value = -1
        [Required(ErrorMessage = AlertMessages.claimId_Required)]
        [RegularExpression("^-?([0-9]{1,2147483647})", ErrorMessage = AlertMessages.claimId_Invalid)]       
        [Range(int.MinValue, int.MaxValue, ErrorMessage = AlertMessages.claimId_Range)]
        public string claim_id { set; get; }

        [RegularExpression("^([a-zA-Z]{0,1})", ErrorMessage = AlertMessages.statement_Yn_Invalid)]
        //DEFAULT Value = N
        public string statement_yn { set; get; }

        [RegularExpression("^([0-9]{0,5})", ErrorMessage = AlertMessages.resp_Party_Id_Invalid)]
        [Required(ErrorMessage = AlertMessages.resp_Party_Id_Required)]
        public string resp_party_id { set; get; }
    
        [RegularExpression("^([a-zA-Z0-9]{0,10})", ErrorMessage = AlertMessages.tooth_Invalid)]        
        public string tooth { set; get; }

        [RegularExpression("^([0-9]+)", ErrorMessage = AlertMessages.tranNum_Invalid)]
        [Range(0, int.MaxValue, ErrorMessage = AlertMessages.tranNum_Invalid)]
        public string tran_num { set; get; }

        [RegularExpression("^([a-zA-Z0-9]{0,40})", ErrorMessage = AlertMessages.archive_Name_Invalid)]
        public string archive_name { set; get; }

        [RegularExpression("^([a-zA-Z0-9]{0,4000})", ErrorMessage = AlertMessages.archive_Path_Invalid)]
        public string archive_path { set; get; }

        [RegularExpression("^([a-zA-Z0-9]{0,5})", ErrorMessage = AlertMessages.service_Code_Invalid)]
        public string service_code { set; get; }

        
        [Range(1, short.MaxValue, ErrorMessage = AlertMessages.clinicId_Range)]
        [RegularExpression("^([1-9]+)", ErrorMessage = AlertMessages.clinicId_Invalid)]
        [Required(ErrorMessage = AlertMessages.clinicId_Required)]
        public string practice_id { set; get; }


        [RegularExpression("^([a-zA-Z]{0,23})", ErrorMessage = AlertMessages.surface_Detail_Invalid)]
        public string surface_detail { set; get; }

        [RegularExpression("^([a-zA-Z0-9]{0,8})", ErrorMessage = AlertMessages.surface_Invalid)]
        public string surface { set; get; }
    }
}
