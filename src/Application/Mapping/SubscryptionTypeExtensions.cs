using Contracts.Enums;
using Domain.Enums;

namespace Application.Mapping
{

    public static class SubscriptionTypeExtensions
    {
        public static SubscriptionTypeDto ToDto(this SubscriptionType subscriptionType)
        {
            return subscriptionType switch
            {
                SubscriptionType.Free => SubscriptionTypeDto.Free,
                SubscriptionType.Premium => SubscriptionTypeDto.Premium,
                SubscriptionType.Deluxe => SubscriptionTypeDto.Deluxe,
                _ => throw new ArgumentException($"No mapping defined for {subscriptionType}")
            };
        }
    }
}
