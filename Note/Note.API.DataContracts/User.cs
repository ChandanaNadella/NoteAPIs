namespace Note.API.DataContracts
{
    using System;
    using System.ComponentModel.DataAnnotations;
    public class User
    {
        [DataType(DataType.Text)]
        public string Id { get; set; }

        [Required]
        [DataType(DataType.Text)]
        public string Firstname { get; set; }

        [Required]
        [DataType(DataType.Text)]
        public string Lastname { get; set; }

        [Required]
        [DataType(DataType.Text)]
        public string Username { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required]
        public bool EmailConfirmed { get; set; }

        [DataType(DataType.PhoneNumber)]
        public string Phone { get; set; }
        public string Token { get; set; }
        public bool IsActive { get; set; }
        public DateTime LockoutEndDateUtc { get; set; }

        [Required]
        public bool LockoutEnabled { get; set; }

        [Required]
        public int AccessFailedCount { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime LastLoginDateUTC { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime CreatedDateUTC { get; set; }
    }
}
