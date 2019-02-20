namespace Note.Repository.Data.Entities
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class operatory_notes
    {
       [Key]
        [Required]
        public int note_id { set; get; }

        /// <summary>
        ///Patient table data.
        /// </summary>
        #region Patient's Details

        [Required]
      [MaxLength(5)]
        //Value is from the patient_id column on the patient table.  
        //Foreign key fk_patient exists.
        public string patient_id { set; get; }
    
       [MaxLength(40)]
        public string patientFirstName { set; get; }

        [MaxLength(40)]
        public string patientLastName { set; get; }

        #endregion Patient's Details

        /// <summary>
        ///Provider table data.
        /// </summary>
        #region Provider's Details

        [Required]
        [MaxLength(3)]
        public string provider_id { set; get; }

         [MaxLength(40)]
        public string first_name { set; get; }

        [MaxLength(40)]
        public string last_name { set; get; }

        #endregion Provider's Details
        [MaxLength(3)]
        public string user_id { set; get; }


        [Required]
        public DateTime date_entered { set; get; }


       [MaxLength(1)]
        public char note_class { set; get; }

        [MaxLength(1)]
        public string note_type { set; get; }

        public int note_type_id { set; get; }

        [MaxLength(40)]
        public string description { set; get; }

        [MaxLength(4000)]
        public string note { set; get; }

        public int color { set; get; }

        [MaxLength(1)]
        public char post_proc_status { set; get; }

        public String date_modified { set; get; }

        [MaxLength(3)]
        public string modified_by { set; get; }

        public int locked_eod { set; get; }

        [MaxLength(1)]
        public char status { set; get; }

        [MaxLength(55)]
        public string tooth_data { set; get; }

        //DEFAULT Value = -1
        public int claim_id { set; get; }

        [MaxLength(1)]
        //DEFAULT Value = N
        public char statement_yn { set; get; }

        [MaxLength(5)]
        public string resp_party_id { set; get; }

        [MaxLength(10)]
        public string tooth { set; get; }

        public int tran_num { set; get; }

        [MaxLength(40)]
        public string archive_name { set; get; }

        [MaxLength(4000)]
        public string archive_path { set; get; }

        [MaxLength(5)]
        public string service_code { set; get; }

        public short practice_id { set; get; }

        public DateTime freshness { set; get; }

        [MaxLength(23)]
        public string surface_detail { set; get; }

        [MaxLength(8)]
        public string surface { set; get; }
    }

}
