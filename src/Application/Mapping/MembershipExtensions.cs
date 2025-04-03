using Contracts;
using Domain;

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
