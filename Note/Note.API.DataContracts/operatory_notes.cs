using System;
using System.Collections.Generic;
using System.Text;

namespace Note.API.DataContracts
{
    public class operatory_notes
    {
        public string note_id { set; get; }

        public string patient_id { set; get; }
        public string note_type { set; get; }

        public string description { set; get; }
        public DateTime date_modified { set; get; }
        public string practice_id { set; get; }
    }
}
