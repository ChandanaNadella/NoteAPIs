namespace Note.API.DataContracts
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.ComponentModel.DataAnnotations;
    public class Provider
    {
        [Required]
        [MaxLength(3)]
        public string Id { set; get; }

        [MaxLength(40)]
        public string Name { set; get; }
    }
}
