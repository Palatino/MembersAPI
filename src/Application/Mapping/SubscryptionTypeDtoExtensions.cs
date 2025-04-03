using Contracts.Enums;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
