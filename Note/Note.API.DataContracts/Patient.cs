namespace Note.API.DataContracts
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Text;
     public  class Patient
    {
        [Required]
        [MaxLength(5)]
        //Value is from the patient_id column on the patient table.  
        //Foreign key fk_patient exists.
        public string Id { set; get; }

        [MaxLength(40)]
        public string Name { set; get; }
    }
}
