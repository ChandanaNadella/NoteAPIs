using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Note.Repository.Data.Entities
{
    public class Clinic
    {
        [Key]
        public int Id { set; get; }

        [Required]
        public int OrganizationId { set; get; }

        [Required]
        public string Name { set; get; }

        public string AddressLine1 { set; get; }

        public string AddressLine2 { set; get; }

        public string Email { set; get; }

        public long PhoneNumber { set; get; }

        public string City { set; get; }

        public string State { set; get; }

        public string Zipcode { set; get; }

        [Required]
        public DateTime CreatedDateUTC { set; get; }

        [Required]
        public DateTime UpdatedDateUTC { set; get; }

        [Required]
        public string TimeZone { set; get; }

    }
}
