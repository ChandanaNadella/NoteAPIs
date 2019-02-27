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
        public int Id { set; get; }

        [Required(ErrorMessage = AlertMessages.patientId_Required)]
        [RegularExpression("^([0-9]{1,5})", ErrorMessage = AlertMessages.patientId_Invalid)]
        public string PatientId { set; get; }

        [Required(ErrorMessage = AlertMessages.providerId_Required)]
        [RegularExpression("^([a-zA-Z0-9]{1,3})", ErrorMessage = AlertMessages.providerId_Invalid)]
        public string UserId { set; get; }

        [RegularExpression("^([a-zA-Z])", ErrorMessage = AlertMessages.noteclass_Invalid)]
        public string NoteClass { set; get; }

        [Required(ErrorMessage = AlertMessages.noteType_Required)]
        [RegularExpression("^([a-zA-Z0-9])", ErrorMessage = AlertMessages.noteType_Invalid)]
        public string NoteType { set; get; }

        public string Note { set; get; }

        [RegularExpression("^[0-9]\\d*$", ErrorMessage = AlertMessages.color_Invalid)]
        public int Color { set; get; }

        [RegularExpression("^([a-zA-Z])", ErrorMessage = AlertMessages.post_proc_status_Invalid)]
        public string PostProcStatus { set; get; }

        [RegularExpression("^[1-9]\\d*$", ErrorMessage = AlertMessages.locked_Eod_Invalid)]
        public int LockedEod { set; get; }

        [RegularExpression("^([a-zA-Z])", ErrorMessage = AlertMessages.status_Invalid)]
        public string Status { set; get; }

        [RegularExpression("^([a-zA-Z]{1,55})", ErrorMessage = AlertMessages.tooth_data_Invalid)]
        public string ToothData { set; get; }

        //DEFAULT Value = -1
        [RegularExpression("^-?[0-9]\\d*$", ErrorMessage = AlertMessages.claimId_Invalid)]
        public int ClaimID { set; get; }

        [RegularExpression("^([a-zA-Z])", ErrorMessage = AlertMessages.statement_yn_Invalid)]
        //DEFAULT Value = N
        public string StatementYn { set; get; }

        [RegularExpression("^([0-9]{1,5})", ErrorMessage = AlertMessages.resp_party_id_Invalid)]
        public string RespPartyId { set; get; }

        [RegularExpression("^([a-zA-Z0-9]{1,10})", ErrorMessage = AlertMessages.tooth_Invalid)]
        public string Tooth { set; get; }

        [RegularExpression("^[1-9]\\d*$", ErrorMessage = AlertMessages.tranNum_Invalid)]
        public int TranNum { set; get; }

        [RegularExpression("^([a-zA-Z0-9]{1,40})", ErrorMessage = AlertMessages.archive_name_Invalid)]
        public string ArchiveName { set; get; }

        [RegularExpression("^([a-zA-Z0-9]{1,4000})", ErrorMessage = AlertMessages.archive_path_Invalid)]
        public string ArchivePath { set; get; }

        [RegularExpression("^([a-zA-Z0-9]{1,5})", ErrorMessage = AlertMessages.service_code_Invalid)]
        public string ServiceCode { set; get; }

        [RegularExpression("^[1-9]\\d*$", ErrorMessage = AlertMessages.clinicId_Invalid)]
        public short ClinicID { set; get; }


        [RegularExpression("^([a-zA-Z]{1,23})", ErrorMessage = AlertMessages.surface_detail_Invalid)]
        public string SurfaceDetail { set; get; }

        [RegularExpression("^([a-zA-Z0-9]{1,8})", ErrorMessage = AlertMessages.surface_Invalid)]
        public string Surface { set; get; }
    }
}
