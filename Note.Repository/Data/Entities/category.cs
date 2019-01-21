using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace Note.Repository.Data.Entities
{
  public class category
    {
        [Key]
        public string category_id { get; set; }

        public string description { get; set; }
        
        public int color { get; set; }
        
        public bool may_delete { get; set; }
    }
}
