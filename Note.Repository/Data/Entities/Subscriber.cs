namespace Note.Repository.Data.Entities
{
    using System;
    using System.ComponentModel.DataAnnotations;
    public class Subscriber
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

        [Required]
        [MaxLength(50)]
        public string Comments { get; set; }

        [Required]
        public DateTime CreatedDateUTC { get; set; }

        [Required]
        public DateTime UpdatedDateUTC { get; set; }

        [MaxLength(200)]
        [Required]
        public string Timezone { get; set; }

        [Required]
        public bool IsActive { get; set; }
    }
}
