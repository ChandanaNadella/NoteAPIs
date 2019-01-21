namespace Note.API.DataContracts.Responses
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class UserCreationResponse
    {
        [DataType(DataType.Text)]
        public string Id { get; set; }

        [Required]
        [DataType(DataType.Text)]
        public string Name { get; set; }

        [Required]
        [DataType(DataType.Text)]
        public string Username { get; set; }

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
