using Contracts.Enums;
using Domain.Enums;

namespace Application.Mapping
{

    public static class SubscriptionTypeDtoExtensions
    {
        public static SubscriptionType ToDomainEntity(this SubscriptionTypeDto subscriptionTypeDto)
        {
            return subscriptionTypeDto switch
            {
                SubscriptionTypeDto.Free => SubscriptionType.Free,
                SubscriptionTypeDto.Premium => SubscriptionType.Premium,
                SubscriptionTypeDto.Deluxe => SubscriptionType.Deluxe,
                _ => throw new ArgumentException($"No mapping defined for {subscriptionTypeDto}")
            };
        }
    }
}
