namespace Note.API.DataContracts.Requests
{
    using System.ComponentModel.DataAnnotations;

    public class UserCreationRequest
    {
        [DataType(DataType.Text)]
        public string Id { get; set; }

        [Required]
        [DataType(DataType.Text)]
        public string OrganizationId { get; set; }

        [Required]
        [DataType(DataType.Text)]
        public string FirstName { get; set; }

        [DataType(DataType.Text)]
        public string LastName { get; set; }

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
        public bool IsActive { get; set; }

        [Required]
        [DataType(DataType.Text)]
        public string Timezone { get; set; }
    }
}
