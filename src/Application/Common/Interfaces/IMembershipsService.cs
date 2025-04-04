using Contracts;
using Contracts.Enums;
using ErrorOr;

namespace Application.Common.Interfaces
{
    public interface IMembershipsService
    {
        Task<ErrorOr<MembershipDto>> CreateNewMembershipAsync(MembershipDto membership);
        Task<ErrorOr<Deleted>> DeleteMembershipByIdAsync(Guid membershipId);
        Task<ErrorOr<MembershipDto>> GetMembershipByIdAsync(Guid membershipId);
        Task<ErrorOr<IEnumerable<MembershipDto>>> GetMembershipsAsync(CountryDto? countryDto = null, SubscriptionTypeDto? subscriptionTypeDto = null);
        Task<ErrorOr<MembershipDto>> UpdateMembershipAsync(Guid membershipId, MembershipDto membershipDto);
    }
}