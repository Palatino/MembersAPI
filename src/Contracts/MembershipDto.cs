using Contracts.Enums;
using System;


namespace Contracts
{
    public class MembershipDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Surname { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public ulong? PhoneNumber { get; set; }
        public SubscriptionTypeDto SubscriptionType { get; set; } = SubscriptionTypeDto.Free;
        public CountryDto Country { get; set; }
    }
}
