using Application.Mapping;

namespace Application.Tests.Unit.Mapping
{
    using Contracts.Enums;
    using Domain.Enums;
    using Xunit;

    public class CountryDtoExtensionsTests
    {
        [Theory]
        [InlineData(CountryDto.AFG, Country.AFG)]
        [InlineData(CountryDto.ALB, Country.ALB)]
        [InlineData(CountryDto.DZA, Country.DZA)]
        [InlineData(CountryDto.AND, Country.AND)]
        [InlineData(CountryDto.AGO, Country.AGO)]
        [InlineData(CountryDto.ATG, Country.ATG)]
        [InlineData(CountryDto.ARG, Country.ARG)]
        [InlineData(CountryDto.ARM, Country.ARM)]
        [InlineData(CountryDto.AUS, Country.AUS)]
        [InlineData(CountryDto.AUT, Country.AUT)]
        [InlineData(CountryDto.AZE, Country.AZE)]
        [InlineData(CountryDto.BHS, Country.BHS)]
        [InlineData(CountryDto.BHR, Country.BHR)]
        [InlineData(CountryDto.BGD, Country.BGD)]
        [InlineData(CountryDto.BRB, Country.BRB)]
        [InlineData(CountryDto.BLR, Country.BLR)]
        [InlineData(CountryDto.BEL, Country.BEL)]
        [InlineData(CountryDto.BLZ, Country.BLZ)]
        public void ToDomainEntity_ShouldReturnCorrectCountry(CountryDto countryDto, Country expectedCountry)
        {
            // Act
            var result = countryDto.ToDomainEntity();

            // Assert
            Assert.Equal(expectedCountry, result);
        }

        [Fact]
        public void ToDomainEntity_ShouldThrowArgumentException_WhenNoMappingIsDefined()
        {
            // Arrange
            var invalidCountryDto = (CountryDto)999;  // Use a value that's not in the enum

            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() => invalidCountryDto.ToDomainEntity());
            Assert.Equal("No mapping defined for 999", exception.Message);
        }
    }
}
