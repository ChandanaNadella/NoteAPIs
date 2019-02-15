using Note.API.DataContracts.Requests;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Note.API.DataContracts
{
   public class OperatoryNotesUpdateDto
    {

        public int note_id { set; get; }

        [CustomError(ErrorMessage = "Patient Id is Required Field")]
        public string patient_id { set; get; }

        public DateTime Date_entered { set; get; }

        [CustomError(ErrorMessage = "Provider/user Id is Required Field")]
        public string user_id { set; get; }

        public char note_class { set; get; }

       // public string note_type { set; get; }

       // public int note_type_id { set; get; }

       // public string description { set; get; }

        public string note { set; get; }

        public int color { set; get; }

        public char post_proc_status { set; get; }

        public string date_modified { set; get; }

        public string modified_by { set; get; }

        public int locked_eod { set; get; }

        public char status { set; get; }

        public string tooth_data { set; get; }
        
        public int claim_id { set; get; }

        public char statement_yn { set; get; }

        public string resp_party_id { set; get; }

        public string tooth { set; get; }

        public int tran_num { set; get; }

        public string archive_name { set; get; }

        public string archive_path { set; get; }

        public string service_code { set; get; }

        [CustomError(ErrorMessage = "Practice Id is Required Field")]
        public short practice_id { set; get; }

        public DateTime freshness { set; get; }

        public string surface_detail { set; get; }

        public string surface { set; get; }

    }
}
