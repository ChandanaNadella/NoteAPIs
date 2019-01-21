namespace Note.API.DataContracts.Requests
{
    using System.ComponentModel.DataAnnotations;
    public class SubscriberCreationRequest
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(200)]
        public string FullName { get; set; }

        [Required]
        [MaxLength(50)]
        public string Email { get; set; }

        public string ContactNumber { get; set; }

        [Required]
        [MaxLength(250)]
        public string Organization_Clinic_Name { get; set; }

        [MaxLength(50)]
        public string Comments { get; set; }
    }
}
