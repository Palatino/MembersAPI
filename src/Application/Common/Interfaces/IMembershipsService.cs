using Contracts;
using Contracts.Enums;
using ErrorOr;

namespace Application.Common.Interfaces
{
    public interface IMembershipsService
    {
        Task<ErrorOr<MembershipDto>> CreateNewMembership(MembershipDto membership);
        Task<ErrorOr<Deleted>> DeleteMembershipById(Guid membershipId);
        Task<ErrorOr<MembershipDto>> GetMembership(Guid membershipId);
        Task<ErrorOr<IEnumerable<MembershipDto>>> GetMemberships(CountryDto? countryDto = null, SubscriptionTypeDto? subscriptionTypeDto = null);
        Task<ErrorOr<MembershipDto>> UpdateMembership(Guid membershipId, MembershipDto membershipDto);
    }
}