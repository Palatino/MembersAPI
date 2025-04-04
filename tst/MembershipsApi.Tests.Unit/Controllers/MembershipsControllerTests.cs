using Application.Common.Interfaces;
using Application.Logging;
using Contracts;
using ErrorOr;
using FluentAssertions;
using MembershipsApi.Controllers;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;

namespace MembershipsApi.Tests.Unit.Controllers
{
    public class MembershipsControllerTests
    {
        private readonly MembershipsController _sut;
        private readonly ILoggerAdapter<MembershipsController> _logger = Substitute.For<ILoggerAdapter<MembershipsController>>();
        private readonly IMembershipsService _membershipsService = Substitute.For<IMembershipsService>();

        public MembershipsControllerTests()
        {
            _sut = new MembershipsController(_logger, _membershipsService);
        }

        [Fact]
        public async Task GetMemberships_ShouldReturnMembers_WhenInvoked()
        {
            //Arrange
            MembershipDto[] expectedMemberships = [
                new MembershipDto(){
                    Name = "Juan",
                    Surname = "Perez",
                    Country = Contracts.Enums.CountryDto.ATG,
                    SubscriptionType = Contracts.Enums.SubscriptionTypeDto.Premium,
                    Id = Guid.NewGuid(),
                    Email = "juan.perez@gmail.com",
                    PhoneNumber = 73759034
                },
                new MembershipDto(){
                    Name = "Ana",
                    Surname = "Fernandez",
                    Country = Contracts.Enums.CountryDto.BHR,
                    SubscriptionType = Contracts.Enums.SubscriptionTypeDto.Deluxe,
                    Id = Guid.NewGuid(),
                    Email = "ana.fernandez@gmail.com",
                    PhoneNumber = 73759034
                }
                ];
            _membershipsService.GetMembershipsAsync().Returns(expectedMemberships);

            //Act
            var actionResult = await _sut.GetMemberships();

            //Assert
            var okObjectResult = (ObjectResult)actionResult;
            var memberships = (MembershipDto[])okObjectResult.Value;
            memberships.Should().BeEquivalentTo(expectedMemberships);

            _logger.Received(2).LogInformation(Arg.Any<string>());

        }
        [Fact]
        public async Task GetMemberships_ShouldReturnUnexpectedError_WhenExceptionHappens()
        {
            //Arrange
            var errorDescription = "Some error";
            _membershipsService.GetMembershipsAsync().Returns(Error.Unexpected(description: errorDescription));

            //Act
            var actionResult = await _sut.GetMemberships();

            //Assert
            var errorActionResult = (ObjectResult)actionResult;
            errorActionResult.StatusCode.Should().Be(500);
            var errorMessage = (ErrorResponse)errorActionResult.Value;
            errorMessage.ErrorCode.Should().Be(500);
            errorMessage.ErrorMessage.Should().Be(errorDescription);

            _logger.Received(2).LogInformation(Arg.Any<string>());

        }

        [Fact]
        public async Task GetMembership_ShouldReturnMember_WhenInvoked()
        {
            //Arrange
            MembershipDto expectedMembership =
                new MembershipDto()
                {
                    Name = "Juan",
                    Surname = "Perez",
                    Country = Contracts.Enums.CountryDto.ATG,
                    SubscriptionType = Contracts.Enums.SubscriptionTypeDto.Premium,
                    Id = Guid.NewGuid(),
                    Email = "juan.perez@gmail.com",
                    PhoneNumber = 73759034
                };
            _membershipsService.GetMembershipByIdAsync(Arg.Is(expectedMembership.Id.Value)).Returns(expectedMembership);

            //Act
            var actionResult = await _sut.GetMembership(expectedMembership.Id.Value);

            //Assert
            var okObjectResult = (ObjectResult)actionResult;
            var memberships = (MembershipDto)okObjectResult.Value;
            memberships.Should().BeEquivalentTo(expectedMembership);

            _logger.Received(2).LogInformation(Arg.Any<string>());

        }
        [Fact]
        public async Task GetMembership_ShouldReturnNotFound_WhenMembershipNotInDbInvoked()
        {
            //Arrange

            var error = Error.NotFound(description: "Not found description");
            _membershipsService.GetMembershipByIdAsync(Arg.Any<Guid>()).Returns(error);

            //Act

            var actionResult = await _sut.GetMembership(Guid.NewGuid());

            //Assert
            var notFoundResult = (NotFoundObjectResult)actionResult;
            var errorResponse = (ErrorResponse)notFoundResult.Value;
            errorResponse.ErrorMessage.Should().Be(error.Description);
            errorResponse.ErrorCode.Should().Be(404);

            _logger.Received(2).LogInformation(Arg.Any<string>());

        }
        [Fact]
        public async Task GetMembership_ShouldReturnUnexpectedError_WhenExceptionHappens()
        {
            //Arrange
            var errorDescription = "Some error";
            _membershipsService.GetMembershipByIdAsync(Arg.Any<Guid>()).Returns(Error.Unexpected(description: errorDescription));

            //Act
            var actionResult = await _sut.GetMembership(Guid.NewGuid());

            //Assert
            var errorActionResult = (ObjectResult)actionResult;
            errorActionResult.StatusCode.Should().Be(500);
            var errorMessage = (ErrorResponse)errorActionResult.Value;
            errorMessage.ErrorCode.Should().Be(500);
            errorMessage.ErrorMessage.Should().Be(errorDescription);

            _logger.Received(2).LogInformation(Arg.Any<string>());

        }

        [Fact]
        public async Task DeleteMembership_ShouldReturnNoContent_WhenSuccessfulDeletion()
        {
            //Arrange
            var idToDelete = Guid.NewGuid();
            _membershipsService.DeleteMembershipByIdAsync(Arg.Is(idToDelete)).Returns(Result.Deleted);

            //Act
            var actionResult = await _sut.DeleteMembership(idToDelete);

            //Assert
            var noContentResult = (NoContentResult)actionResult;
            noContentResult.Should().NotBeNull();
            _logger.Received(2).LogInformation(Arg.Any<string>());

        }
        [Fact]
        public async Task DeleteMembership_ShouldReturnNotFound_WhenIdNotFound()
        {
            //Arrange
            var idToDelete = Guid.NewGuid();
            var error = Error.NotFound(description: "Not found error description");
            _membershipsService.DeleteMembershipByIdAsync(Arg.Is(idToDelete)).Returns(error);

            //Act
            var actionResult = await _sut.DeleteMembership(idToDelete);

            //Assert
            var notFoundResult = (NotFoundObjectResult)actionResult;
            var errorResponse = (ErrorResponse)notFoundResult.Value;
            errorResponse.ErrorMessage.Should().Be(error.Description);
            errorResponse.ErrorCode.Should().Be(404);

            _logger.Received(2).LogInformation(Arg.Any<string>());

        }
        [Fact]
        public async Task DeleteMembership_ShouldReturnUnexpectedError_WhenExceptionHappens()
        {
            //Arrange
            var errorDescription = "Some error";
            var idToDelete = Guid.NewGuid();
            _membershipsService.DeleteMembershipByIdAsync(Arg.Is(idToDelete))
                .Returns(Error.Unexpected(description: errorDescription));

            //Act
            var actionResult = await _sut.DeleteMembership(idToDelete);

            //Assert
            var errorActionResult = (ObjectResult)actionResult;
            errorActionResult.StatusCode.Should().Be(500);
            var errorMessage = (ErrorResponse)errorActionResult.Value;
            errorMessage.ErrorCode.Should().Be(500);
            errorMessage.ErrorMessage.Should().Be(errorDescription);

            _logger.Received(2).LogInformation(Arg.Any<string>());

        }

        [Fact]
        public async Task CreateNewMembership_ShouldReturnMembership_WhenSuccessfulCreation()
        {
            //Arrange
            var membership = new MembershipDto()
            {
                Name = "Juan",
                Surname = "Perez",
                Country = Contracts.Enums.CountryDto.ATG,
                SubscriptionType = Contracts.Enums.SubscriptionTypeDto.Premium,
                Id = Guid.NewGuid(),
                Email = "juan.perez@gmail.com",
                PhoneNumber = 73759034
            };
            _membershipsService.CreateNewMembershipAsync(Arg.Is(membership)).Returns(membership);

            //Act
            var actionResult = await _sut.CreateMembership(membership);

            //Assert
            var createdResult = (CreatedAtActionResult)actionResult;
            createdResult.Should().NotBeNull();
            createdResult.Value.Should().BeEquivalentTo(membership);
            _logger.Received(2).LogInformation(Arg.Any<string>());

        }
        [Fact]
        public async Task CreateNewMembership_ShouldReturnUnexpectedError_WhenExceptionHappens()
        {
            //Arrange
            var errorDescription = "Some error";
            _membershipsService.CreateNewMembershipAsync(Arg.Any<MembershipDto>())
                .Returns(Error.Unexpected(description: errorDescription));
            var membership = new MembershipDto()
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
            var actionResult = await _sut.CreateMembership(membership);

            //Assert
            var errorActionResult = (ObjectResult)actionResult;
            errorActionResult.StatusCode.Should().Be(500);
            var errorMessage = (ErrorResponse)errorActionResult.Value;
            errorMessage.ErrorCode.Should().Be(500);
            errorMessage.ErrorMessage.Should().Be(errorDescription);

            _logger.Received(2).LogInformation(Arg.Any<string>());

        }

        [Fact]
        public async Task UpdateMembership_ShouldReturnUpdatedMembership_WhenSuccessfulUpdate()
        {
            //Arrange
            var idToUpdate = Guid.NewGuid();
            var membership = new MembershipDto()
            {
                Name = "Juan",
                Surname = "Perez",
                Country = Contracts.Enums.CountryDto.ATG,
                SubscriptionType = Contracts.Enums.SubscriptionTypeDto.Premium,
                Id = Guid.NewGuid(),
                Email = "juan.perez@gmail.com",
                PhoneNumber = 73759034
            };
            _membershipsService.UpdateMembershipAsync(Arg.Is(idToUpdate), Arg.Is(membership)).Returns(membership);

            //Act
            var actionResult = await _sut.UpdateMembership(idToUpdate, membership);

            //Assert
            var okObjectResult = (ObjectResult)actionResult;
            var memberships = (MembershipDto)okObjectResult.Value;
            memberships.Should().BeEquivalentTo(membership);

            _logger.Received(2).LogInformation(Arg.Any<string>());

        }
        [Fact]
        public async Task UpdateMembership_ShouldReturnNotFound_WhenMembershipNotFound()
        {
            //Arrange
            var idToUpdate = Guid.NewGuid();
            var membership = new MembershipDto()
            {
                Name = "Juan",
                Surname = "Perez",
                Country = Contracts.Enums.CountryDto.ATG,
                SubscriptionType = Contracts.Enums.SubscriptionTypeDto.Premium,
                Id = Guid.NewGuid(),
                Email = "juan.perez@gmail.com",
                PhoneNumber = 73759034
            };
            var error = Error.NotFound(description: "Error description");
            _membershipsService.UpdateMembershipAsync(Arg.Is(idToUpdate), Arg.Is(membership)).Returns(error);

            //Act
            var actionResult = await _sut.UpdateMembership(idToUpdate, membership);

            //Assert
            var notFoundResult = (NotFoundObjectResult)actionResult;
            var errorResponse = (ErrorResponse)notFoundResult.Value;
            errorResponse.ErrorMessage.Should().Be(error.Description);
            errorResponse.ErrorCode.Should().Be(404);

            _logger.Received(2).LogInformation(Arg.Any<string>());

        }
        [Fact]
        public async Task UpdateMembership_ShouldReturnUnexpectedError_WhenExceptionHappens()
        {
            //Arrange
            var idToUpdate = Guid.NewGuid();
            var membership = new MembershipDto()
            {
                Name = "Juan",
                Surname = "Perez",
                Country = Contracts.Enums.CountryDto.ATG,
                SubscriptionType = Contracts.Enums.SubscriptionTypeDto.Premium,
                Id = Guid.NewGuid(),
                Email = "juan.perez@gmail.com",
                PhoneNumber = 73759034
            };
            var error = Error.Unexpected(description: "Error description");
            _membershipsService.UpdateMembershipAsync(Arg.Is(idToUpdate), Arg.Is(membership)).Returns(error);

            //Act
            var actionResult = await _sut.UpdateMembership(idToUpdate, membership);

            //Assert
            var errorActionResult = (ObjectResult)actionResult;
            errorActionResult.StatusCode.Should().Be(500);
            var errorMessage = (ErrorResponse)errorActionResult.Value;
            errorMessage.ErrorCode.Should().Be(500);
            errorMessage.ErrorMessage.Should().Be(error.Description);

            _logger.Received(2).LogInformation(Arg.Any<string>());

        }


    }
}
