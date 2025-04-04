using Application.Common.Interfaces;
using Application.Logging;
using Application.Mapping;
using Contracts;
using Contracts.Enums;
using ErrorOr;
using Microsoft.EntityFrameworkCore;

namespace Application.Services
{
    public class MembershipsService : IMembershipsService
    {
        private readonly IMembershipsDbContext _context;
        private readonly ILoggerAdapter<MembershipsService> _logger;
        public MembershipsService(IMembershipsDbContext context, ILoggerAdapter<MembershipsService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<ErrorOr<MembershipDto>> GetMembershipByIdAsync(Guid membershipId)
        {
            try
            {
                var membership = await _context.Memberships.SingleOrDefaultAsync(m => m.Id == membershipId).ConfigureAwait(false);
                if(membership is null)
                {
                    _logger.LogInformation("Could not find membership with Id {membershipId}", membershipId);
                    return Error.NotFound(description: $"Could not find membership with Id {membershipId}");
                }
                var result = membership.ToDto();
                _logger.LogInformation("Memberships retrieved successfully");
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving memberships");
                return Error.Unexpected(description: "Unexpected error while retrieving memberships");
            }
        }
        public async Task<ErrorOr<IEnumerable<MembershipDto>>> GetMembershipsAsync(CountryDto? countryDto = null, SubscriptionTypeDto? subscriptionTypeDto = null)
        {
            try
            {
                var query = _context.Memberships
                                .AsNoTracking()
                                .AsQueryable();
                if (countryDto is not null)
                {
                    query = query.Where(s => s.Country == countryDto.Value.ToDomainEntity());
                }
                if (subscriptionTypeDto is not null)
                {
                    query = query.Where(w => w.SubscriptionType == subscriptionTypeDto.Value.ToDomainEntity());
                }

                var matchingMemberships = await query.ToArrayAsync().ConfigureAwait(false);
                var result = matchingMemberships.Select(m => m.ToDto()).ToArray();
                _logger.LogInformation("Memberships retrieved successfully");
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving memberships");
                return Error.Unexpected(description: "Unexpected error while retrieving memberships");
            }
        }
        public async Task<ErrorOr<Deleted>> DeleteMembershipByIdAsync(Guid membershipId)
        {
            try
            {
                var membershipToDelete = await _context.Memberships.SingleOrDefaultAsync(m => m.Id == membershipId).ConfigureAwait(false);
                if (membershipToDelete is null)
                {
                    _logger.LogInformation("Deletion of membership cancelled, " +
                        "Could not find membership with Id {membershipId}", membershipId);
                    return Error.NotFound(description: $"Could not find membership with Id {membershipId}");
                }

                _context.Memberships.Remove(membershipToDelete);
                await _context.SaveChangesAsync().ConfigureAwait(false);
                _logger.LogInformation("Deletion of membershipId {membershipId} successful", membershipId);
                return Result.Deleted;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting membership {membershipId}", membershipId);
                return Error.Unexpected(description: "Unexpected error while deleting membership");
            }
        }
        public async Task<ErrorOr<MembershipDto>> CreateNewMembershipAsync(MembershipDto membership)
        {
            try
            {

                //Ids to be assigned by DB
                membership.Id = null;

                var newMembership = membership.ToDomainEntity();
                _context.Memberships.Add(newMembership);
                await _context.SaveChangesAsync().ConfigureAwait(false);
                _logger.LogInformation("Creation of membership successful, new id {membershipId}", newMembership.Id);
                return newMembership.ToDto();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating membership");
                return Error.Unexpected(description: "Unexpected error while creating membership");
            }
        }
        public async Task<ErrorOr<MembershipDto>> UpdateMembershipAsync(Guid membershipId, MembershipDto membershipDto)
        {
            try
            {
                var membershipInDb = await _context.Memberships.SingleOrDefaultAsync(m => m.Id == membershipId).ConfigureAwait(false);
                if (membershipInDb is null)
                {
                    _logger.LogInformation("Update of membership cancelled, " +
                    "Could not find membership with Id {membershipId}", membershipId);
                    return Error.NotFound(description: $"Could not find membership with Id {membershipId}");
                }

                membershipInDb = membershipDto.ToDomainEntity(membershipInDb);
                await _context.SaveChangesAsync().ConfigureAwait(false);
                _logger.LogInformation("Update of membership {membershipId} successful ", membershipInDb.Id);
                return membershipInDb.ToDto();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating membership");
                return Error.Unexpected(description: "Unexpected error while updating membership");
            }
        }
    }
}
