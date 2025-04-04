using Application.Common.Interfaces;
using Application.Logging;
using Application.Services;
using Contracts;
using Domain;
using ErrorOr;
using FluentAssertions;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Tests.Unit.Services
{
    public class MembershipServiceTests : IDisposable
    {
        private readonly MembershipsService _sut;
        private readonly ILoggerAdapter<MembershipsService> _logger = Substitute.For<ILoggerAdapter<MembershipsService>>();
        private readonly IMembershipsDbContext _context;

        private SqliteConnection _connection = new SqliteConnection("Filename=:memory:");

        public MembershipServiceTests()
        {
            _context = DbHelper.CreateSqlLiteIDbContext(_connection);

            _sut = new MembershipsService(_context, _logger);
        }

        public void Dispose()
        {
            _connection.Dispose();
        }

        [Fact]
        public async Task CreateNewMembershipAsync_ShouldCreateNewMemberships_WhenInputIsCorrect()
        {
            //Arrange
            var newMembership = new MembershipDto()
            {
                Name = "Juan",
                Surname = "Perez",
                Country = Contracts.Enums.CountryDto.ATG,
                SubscriptionType = Contracts.Enums.SubscriptionTypeDto.Premium,
                Id = Guid.NewGuid(),
                Email = "juan.perez@gmail.com",
                PhoneNumber = 73759034
            };

            //Act
            var result = await _sut.CreateNewMembershipAsync(newMembership);

            //Assert
            //Check response
            result.IsError.Should().BeFalse();
            var membership = (MembershipDto)result.Value;
            membership.Should().BeEquivalentTo(newMembership, options => options.Excluding(m => m.Id));

            //Check DB
            var membershipInDb = await _context.Memberships.SingleOrDefaultAsync(m => m.Id == membership.Id);
            membershipInDb.Should().BeEquivalentTo(membership);
            membershipInDb.DateCreated.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(10));
            membershipInDb.LastModified.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(10));

            _logger.Received(1).LogInformation(Arg.Any<string>(), Arg.Is<object[]>(p => p.Contains(membership.Id)));

        }
        [Fact]
        public async Task CreateNewMembershipAsync_ShouldReturnUnexpectedError_WhenExceptionHappens()
        {
            //Arrange
            var newMembership = new MembershipDto()
            {
                Name = "Juan",
                Surname = "Perez",
                Country = Contracts.Enums.CountryDto.ATG,
                SubscriptionType = Contracts.Enums.SubscriptionTypeDto.Premium,
                Id = Guid.NewGuid(),
                Email = "juan.perez@gmail.com",
                PhoneNumber = 73759034
            };
            var exception = new Exception("Some exception");
            _logger.WhenForAnyArgs(x => x.LogInformation(Arg.Any<string>(), Arg.Any<object[]>())).Do(x => { throw exception; });


            //Act
            var result = await _sut.CreateNewMembershipAsync(newMembership);

            //Assert
            result.IsError.Should().BeTrue();
            var error = result.FirstError;
            error.Type.Should().Be(ErrorType.Unexpected);
            _logger.Received(1).LogError(exception, Arg.Any<string>());
        }

        [Fact]
        public async Task DeleteMembershipByIdAsync_ShouldReturnDeleted_WhenDeletedSuccessfully()
        {
            //Arrange
            var newMembership = new Membership()
            {
                Name = "Juan",
                Surname = "Perez",
                Country = Domain.Enums.Country.ATG,
                SubscriptionType = Domain.Enums.SubscriptionType.Premium,
                Id = Guid.NewGuid(),
                Email = "juan.perez@gmail.com",
                PhoneNumber = 73759034,
                DateCreated = DateTime.UtcNow,
                LastModified = DateTime.UtcNow
            };

            _context.Memberships.Add(newMembership);
            await _context.SaveChangesAsync();
            //Act

            var result = await _sut.DeleteMembershipByIdAsync(newMembership.Id);

            //Assert
            result.IsError.Should().BeFalse();
            result.Value.Should().BeOfType<Deleted>();
            var membershipInDb = await _context.Memberships.SingleOrDefaultAsync(m => m.Id == newMembership.Id);
            membershipInDb.Should().BeNull();
            _logger.Received(1).LogInformation(Arg.Any<string>(), Arg.Is<object[]>(p => p.Contains(newMembership.Id)));

        }
        [Fact]
        public async Task DeleteMembershipByIdAsync_ShouldReturnNotFoundError_WhenMembershipNotInDb()
        {
            //Arrange
            //Act
            var guid = Guid.NewGuid();
            var result = await _sut.DeleteMembershipByIdAsync(guid);

            //Assert
            result.IsError.Should().BeTrue();
            var error = result.FirstError;
            error.Type.Should().Be(ErrorType.NotFound);
            error.Description.Should().Contain("Could not find membership with Id");
            _logger.Received(1).LogInformation(Arg.Any<string>(), Arg.Is<object[]>(p => p.Contains(guid)));

        }
        [Fact]
        public async Task DeleteMembershipByIdAsync_ShouldReturnUnexpectedError_WhenExceptionHappens()
        {
            var exception = new Exception("Fake exception");
            _logger.WhenForAnyArgs(x => x.LogInformation(Arg.Any<string>(), Arg.Any<object[]>())).Do(x => { throw exception; });

            //Act
            var membershipId = Guid.NewGuid();
            var result = await _sut.DeleteMembershipByIdAsync(membershipId);
            //Assert
            result.IsError.Should().BeTrue();
            var error = result.FirstError;
            error.Type.Should().Be(ErrorType.Unexpected);
            error.Description.Should().Contain("Unexpected error while deleting membership");
            _logger.Received(1).LogError(exception, Arg.Any<string>(), Arg.Is<object[]>(p => p.Contains(membershipId)));
        }

        [Fact]
        public async Task GetMembershipsAsync_ShouldReturnMemberships_WhenInvoked()
        {
            //Arrange
            Membership[] memberships = [
                new Membership()
            {
                Name = "Juan",
                Surname = "Perez",
                Country = Domain.Enums.Country.ATG,
                SubscriptionType = Domain.Enums.SubscriptionType.Premium,
                Id = Guid.NewGuid(),
                Email = "juan.perez@gmail.com",
                PhoneNumber = 73759034,
                DateCreated = DateTime.UtcNow,
                LastModified = DateTime.UtcNow
            },
            new Membership()
            {
                Name = "Elena",
                Surname = "Martinez",
                Country = Domain.Enums.Country.BLZ,
                SubscriptionType = Domain.Enums.SubscriptionType.Premium,
                Id = Guid.NewGuid(),
                Email = "elena.martinez@gmail.com",
                PhoneNumber = 56799766,
                DateCreated = DateTime.UtcNow,
                LastModified = DateTime.UtcNow
            }];

            _context.Memberships.AddRange(memberships);
            await _context.SaveChangesAsync();

            //Act
            var result = await _sut.GetMembershipsAsync();

            //Assert
            result.IsError.Should().BeFalse();
            result.Value.Should().BeEquivalentTo(memberships, options => options
            .Excluding(m=>m.DateCreated)
            .Excluding(m=>m.LastModified)
            );
            _logger.Received(1).LogInformation(Arg.Any<string>());

        }
        [Fact]
        public async Task GetMembershipsAsync_ShouldReturnFilteredMemberships_WhenInvokedWithFilter()
        {
            //Arrange
            Membership[] memberships = [
                new Membership()
            {
                Name = "Juan",
                Surname = "Perez",
                Country = Domain.Enums.Country.ATG,
                SubscriptionType = Domain.Enums.SubscriptionType.Premium,
                Id = Guid.NewGuid(),
                Email = "juan.perez@gmail.com",
                PhoneNumber = 73759034,
                DateCreated = DateTime.UtcNow,
                LastModified = DateTime.UtcNow
            },
            new Membership()
            {
                Name = "Elena",
                Surname = "Martinez",
                Country = Domain.Enums.Country.BLZ,
                SubscriptionType = Domain.Enums.SubscriptionType.Premium,
                Id = Guid.NewGuid(),
                Email = "elena.martinez@gmail.com",
                PhoneNumber = 56799766,
                DateCreated = DateTime.UtcNow,
                LastModified = DateTime.UtcNow
            },
            new Membership()
            {
                Name = "Marta",
                Surname = "Hernandez",
                Country = Domain.Enums.Country.BLZ,
                SubscriptionType = Domain.Enums.SubscriptionType.Free,
                Id = Guid.NewGuid(),
                Email = "marta.hernandez@gmail.com",
                PhoneNumber = 56799766,
                DateCreated = DateTime.UtcNow,
                LastModified = DateTime.UtcNow
            }
            ];

            _context.Memberships.AddRange(memberships);
            await _context.SaveChangesAsync();

            //Act
            var result = await _sut.GetMembershipsAsync(Contracts.Enums.CountryDto.BLZ, Contracts.Enums.SubscriptionTypeDto.Free);

            //Assert

            Membership[] expectedFilteredResult = [memberships[2]];

            result.IsError.Should().BeFalse();
            result.Value.Should().BeEquivalentTo(expectedFilteredResult, options => options
            .Excluding(m => m.DateCreated)
            .Excluding(m => m.LastModified)
            );
            _logger.Received(1).LogInformation(Arg.Any<string>());

        }
        [Fact]
        public async Task GetMembershipsAsync_ShouldReturnUnexpectedError_WhenExceptionHappens()
        {
            //Arrange

            var exception = new Exception("exception message");
            _logger.WhenForAnyArgs(x => x.LogInformation(Arg.Any<string>(), Arg.Any<object[]>())).Do(x => { throw exception; });
            //Act
            var result = await _sut.GetMembershipsAsync();

            //Assert
            result.IsError.Should().BeTrue();
            var error = result.FirstError;
            error.Type.Should().Be(ErrorType.Unexpected);
            error.Description.Should().Contain("Unexpected error while retrieving membership");
            _logger.Received(1).LogError(exception, Arg.Any<string>());

        }

        [Fact]
        public async Task GetMembershipsByIdAsync_ShouldReturnMemberships_WhenInvoked()
        {
            //Arrange
            var membership = new Membership()
            {
                Name = "Juan",
                Surname = "Perez",
                Country = Domain.Enums.Country.ATG,
                SubscriptionType = Domain.Enums.SubscriptionType.Premium,
                Id = Guid.NewGuid(),
                Email = "juan.perez@gmail.com",
                PhoneNumber = 73759034,
                DateCreated = DateTime.UtcNow,
                LastModified = DateTime.UtcNow
            };

            _context.Memberships.Add(membership);
            await _context.SaveChangesAsync();

            //Act
            var result = await _sut.GetMembershipByIdAsync(membership.Id);

            //Assert
            result.IsError.Should().BeFalse();
            result.Value.Should().BeEquivalentTo(membership, options => options
            .Excluding(m => m.DateCreated)
            .Excluding(m => m.LastModified)
            );
            _logger.Received(1).LogInformation(Arg.Any<string>());

        }
        [Fact]
        public async Task GetMembershipsByIdAsync_ShouldReturnNotFound_WhenMembershipNotInDb()
        {
            //Arrange


            //Act
            var id = Guid.NewGuid();
            var result = await _sut.GetMembershipByIdAsync(id);

            //Assert
            result.IsError.Should().BeTrue();
            result.FirstError.Type.Should().Be(ErrorType.NotFound);
            _logger.Received(1).LogInformation(Arg.Any<string>(), Arg.Is<object[]>(c => c.Contains(id)));

        }
        [Fact]
        public async Task GetMembershipsByIdAsync_ShouldReturnUnexpectedError_WhenExceptionHappens()
        {
            //Arrange

            var exception = new Exception("exception message");
            _logger.WhenForAnyArgs(x => x.LogInformation(Arg.Any<string>(), Arg.Any<object[]>())).Do(x => { throw exception; });
            //Act
            var id = Guid.NewGuid();
            var result = await _sut.GetMembershipByIdAsync(id);

            //Assert
            result.IsError.Should().BeTrue();
            var error = result.FirstError;
            error.Type.Should().Be(ErrorType.Unexpected);
            error.Description.Should().Contain("Unexpected error while retrieving memberships");
            _logger.Received(1).LogError(exception, Arg.Any<string>());

        }

        [Fact]
        public async Task UpdateMembershipAsync_ShouldUpdateMembership_WhenInvoked()
        {
            //Arrange
            DateTime dateCreated;

            DateTime.TryParseExact(
                "03/04/2025", 
                "dd/MM/yyyy", 
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out dateCreated);

            var membership = new Membership()
            {
                Name = "Juan",
                Surname = "Perez",
                Country = Domain.Enums.Country.ATG,
                SubscriptionType = Domain.Enums.SubscriptionType.Premium,
                Id = Guid.NewGuid(),
                Email = "juan.perez@gmail.com",
                PhoneNumber = 73759034,
                DateCreated = dateCreated,
                LastModified = dateCreated,
            };

            _context.Memberships.Add(membership);
            await _context.SaveChangesAsync();

            //Act
            var updateRequest = new MembershipDto()
            {
                Name = "David",
                Surname = "Gonzalez",
                Country = Contracts.Enums.CountryDto.BEL,
                SubscriptionType = Contracts.Enums.SubscriptionTypeDto.Deluxe,
                Email = "david.gonzalez@gmail.com",
                PhoneNumber = 73659034,
            };
            var result = await _sut.UpdateMembershipAsync(membership.Id, updateRequest);

            //Assert
            result.IsError.Should().BeFalse();
            result.Value.Should().BeEquivalentTo(updateRequest, options => options.Excluding(m=>m.Id));
            result.Value.Id.Should().Be(membership.Id);

            //Check dates in DB
            var updatedMembershipInDb = await _context.Memberships.SingleOrDefaultAsync(m => m.Id==membership.Id);
            updatedMembershipInDb.Should().NotBeNull();
            updatedMembershipInDb.DateCreated.Should().Be(dateCreated);
            updatedMembershipInDb.LastModified.Should().BeAfter(dateCreated);

        }
        [Fact]
        public async Task UpdateMembershipAsync_ShouldReturnNotFound_WhenMembershipNotInDb()
        {
            //Arrange


            //Act
            var updateRequest = new MembershipDto()
            {
                Name = "David",
                Surname = "Gonzalez",
                Country = Contracts.Enums.CountryDto.BEL,
                SubscriptionType = Contracts.Enums.SubscriptionTypeDto.Deluxe,
                Email = "david.gonzalez@gmail.com",
                PhoneNumber = 73659034,
            };
            var id = Guid.NewGuid();
            var result = await _sut.UpdateMembershipAsync(id, updateRequest);

            //Assert
            result.IsError.Should().BeTrue();
            result.FirstError.Type.Should().Be(ErrorType.NotFound);
            _logger.Received(1).LogInformation(Arg.Any<string>(), Arg.Is<object[]>(c => c.Contains(id)));

        }
        [Fact]
        public async Task UpdateMembershipAsync_ShouldReturnUnexpectedError_WhenExceptionHappens()
        {
            //Arrange
            var exception = new Exception("exception message");
            _logger.WhenForAnyArgs(x => x.LogInformation(Arg.Any<string>(), Arg.Any<object[]>())).Do(x => { throw exception; });

            //Act
            var updateRequest = new MembershipDto()
            {
                Name = "David",
                Surname = "Gonzalez",
                Country = Contracts.Enums.CountryDto.BEL,
                SubscriptionType = Contracts.Enums.SubscriptionTypeDto.Deluxe,
                Email = "david.gonzalez@gmail.com",
                PhoneNumber = 73659034,
            };
            var id = Guid.NewGuid();
            var result = await _sut.UpdateMembershipAsync(id, updateRequest);

            //Assert
            result.IsError.Should().BeTrue();
            var error = result.FirstError;
            error.Type.Should().Be(ErrorType.Unexpected);
            error.Description.Should().Contain("Unexpected error while updating membership");
            _logger.Received(1).LogError(exception, Arg.Any<string>());

        }
    }
}
