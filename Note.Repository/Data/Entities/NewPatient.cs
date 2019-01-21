using System.ComponentModel.DataAnnotations;

namespace Note.Repository.Data.Entities
{
    public class NewPatient
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int ClinicId { get; set; }

        [Required]
        public System.DateTime Date { get; set; }

        [Required]
        public int Year { get; set; }

        [Required]
        public int Month { get; set; }

        [Required]
        public int Week { get; set; }

        [Required]
        public int Count { get; set; }

        [Required]
        public decimal Amount { get; set; }

        [Required]
        public int ProviderType { get; set; }

        [Required]
        public bool IsCompleted { get; set; }

        [Required]
        public System.DateTime CreatedDateUTC { get; set; }

        [Required]
        public System.DateTime UpdatedDateUTC { get; set; }
    }
}
