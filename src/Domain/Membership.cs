using Domain.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain
{
    public class Membership
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;
        [Required]
        [MaxLength(100)]
        public string Surname { get; set; } = string.Empty;
        [Required]
        [MaxLength(150)]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
        [Phone]
        [Column(TypeName = "varchar(15)")]
        public ulong? PhoneNumber { get; set; }
        [Required]
        public DateTime DateCreated { get; set; } = DateTime.UtcNow;
        [Required]
        public DateTime LastModified { get; set; } = DateTime.UtcNow;
        [Required]
        public SubscriptionType SubscriptionType { get; set; } = SubscriptionType.Free;
        [Required]
        public Country Country { get; set; }

    }
}
