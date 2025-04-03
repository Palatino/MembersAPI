using Contracts.Enums;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Contracts
{

    public class MembershipDto
    {

        [SwaggerSchema("The unique identifier of the membership.")]
        public Guid? Id { get; set; }
        [Required]
        [MaxLength(100)]
        [SwaggerSchema("Name of the membership owner")]
        public string Name { get; set; } = string.Empty;
        [Required]
        [MaxLength(100)]
        [SwaggerSchema("Surname of the membership owner")]
        public string Surname { get; set; } = string.Empty;
        [Required]
        [MaxLength(150)]
        [EmailAddress]
        [SwaggerSchema("Email of the membership owner")]
        public string Email { get; set; } = string.Empty;
        [Column(TypeName = "varchar(15)")]
        [SwaggerSchema("Phone number of the membership owner")]
        public ulong? PhoneNumber { get; set; }
        [Required]
        [SwaggerSchema("Subscription type of the membership type")]
        public SubscriptionTypeDto SubscriptionType { get; set; } = SubscriptionTypeDto.Free;
        [Required]
        [SwaggerSchema("Membership country")]
        public CountryDto Country { get; set; }
    }
}
