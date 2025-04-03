using Contracts;
using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Mapping
{
    public static class MembershipDtoExtensions
    {
        public static Membership ToDomainEntity(this MembershipDto dto)
        {
            return new Membership
            {
                Id = dto.Id,
                Name = dto.Name,
                Surname = dto.Surname,
                Email = dto.Email,
                PhoneNumber = dto.PhoneNumber,
                SubscriptionType = dto.SubscriptionType.ToDomainEntity(),
                Country = dto.Country.ToDomainEntity(),
                DateCreated = DateTime.UtcNow,
                LastModified = DateTime.UtcNow
            };
        }

        public static Membership ToDomainEntity(this MembershipDto dto, Membership existingMembership)
        {
            existingMembership.SubscriptionType = dto.SubscriptionType.ToDomainEntity();
            existingMembership.Country = dto.Country.ToDomainEntity();
            existingMembership.Name = dto.Name;
            existingMembership.Surname = dto.Surname;
            existingMembership.PhoneNumber = dto.PhoneNumber;
            existingMembership.Email = dto.Email;
            existingMembership.LastModified = DateTime.UtcNow;

            return existingMembership;


        }
    }
}
