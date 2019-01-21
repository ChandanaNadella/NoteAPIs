
namespace Note.Repository.Data.Entities
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class User
    {
        [Key]
        public string Id { get; set; }

        [Required]
        public int OrganizationId { get; set; }

        [Required]
        [MaxLength(50)]
        public string FirstName { get; set; }
        [Required]
        [MaxLength(50)]
        public string LastName { get; set; }
        [Required]
        [MaxLength(50)]
        public string Username { get; set; }
      
        [MaxLength(50)]
        public string Email { get; set; }
        public bool EmailConfirmed { get; set; }
        
        [MaxLength(50)]
        public string Phone { get; set; }
        [Required]
        [MaxLength(50)]
        public byte[] PasswordHash { get; set; }
        [Required]
        [MaxLength(50)]
        public byte[] PasswordSalt { get; set; }
        [Required]
        public bool IsActive { get; set; }
        public DateTime LockoutEndDateUtc { get; set; }
        public bool LockoutEnabled { get; set; }
        public int AccessFailedCount { get; set; }
        [Required]
        public DateTime LastLoginDateUTC { get; set; }
        [Required]
        public DateTime CreatedDateUTC { get; set; }

        [MaxLength(200)]
        [Required]
        public string Timezone { get; set; }
    }
}
