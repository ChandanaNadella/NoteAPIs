using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Note.API.DataContracts
{
    public class TokenDetails
    {
        [Required]
        public string ProductName { set; get; }

        [Required]
        public string Token { set; get; }
    }
}
