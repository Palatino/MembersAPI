using Contracts;
using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Mapping
{

    public static class MembershipExtensions
    {
        public static MembershipDto ToDto(this Membership entity)
        {
            return new MembershipDto
            {
                Id = entity.Id,
                Name = entity.Name,
                Surname = entity.Surname,
                Email = entity.Email,
                PhoneNumber = entity.PhoneNumber,
                SubscriptionType = entity.SubscriptionType.ToDto(),
                Country = entity.Country.ToDto() 
            };
        }
    }
}
