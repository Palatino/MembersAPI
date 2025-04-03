using Contracts.Enums;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Contracts
{
    public class MembershipDto
    {
        public Guid? Id { get; set; }
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
        [Column(TypeName = "varchar(15)")]
        public ulong? PhoneNumber { get; set; }
        [Required]
        public SubscriptionTypeDto SubscriptionType { get; set; } = SubscriptionTypeDto.Free;
        [Required]
        public CountryDto Country { get; set; }
    }
}
