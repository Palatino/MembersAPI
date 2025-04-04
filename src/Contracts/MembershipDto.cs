using Contracts.Enums;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Contracts
{
    /// <summary>
    /// Membership information
    /// </summary>
    public class MembershipDto
    {
        /// <summary>
        /// Unique identifier of the membership
        /// </summary>
        /// <example>A26CC87F-6A2C-473F-8F3B-24C5BF723ADF</example>
        public Guid? Id { get; set; }

        /// <summary>
        /// Name of the membership owner
        /// </summary>
        /// <example>John</example>
        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Surname of the membership owner
        /// </summary>
        /// <example>Doe</example>
        [Required]
        [MaxLength(100)]
        public string Surname { get; set; } = string.Empty;

        /// <summary>
        /// Email of the membership owner
        /// </summary>
        /// <example>john.doe@autodesk.com</example>
        [Required]
        [MaxLength(150)]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// Phone number of the membership owner, only numeric value
        /// </summary>
        /// <example>447645305259</example>
        [Column(TypeName = "varchar(15)")]
        public ulong? PhoneNumber { get; set; }

        /// <summary>
        /// Subscription type of the membership type
        /// </summary>
        /// <example>Free</example>
        [Required]
        public SubscriptionTypeDto SubscriptionType { get; set; } = SubscriptionTypeDto.Free;

        /// <summary>
        /// Membership country, three letter code ISO 3166-1 alpha-3 
        /// </summary>
        /// <example>AFG</example>
        [Required]
        public CountryDto Country { get; set; }
    }
}
