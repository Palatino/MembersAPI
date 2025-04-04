using Application.Mapping;
using Contracts.Enums;
using Domain.Enums;

namespace Application.Tests.Unit.Mapping
{


    public class CountryExtensionsTests
    {
        [Theory]
        [InlineData(Country.AFG, CountryDto.AFG)]
        [InlineData(Country.ALB, CountryDto.ALB)]
        [InlineData(Country.DZA, CountryDto.DZA)]
        [InlineData(Country.AND, CountryDto.AND)]
        [InlineData(Country.AGO, CountryDto.AGO)]
        [InlineData(Country.ATG, CountryDto.ATG)]
        [InlineData(Country.ARG, CountryDto.ARG)]
        [InlineData(Country.ARM, CountryDto.ARM)]
        [InlineData(Country.AUS, CountryDto.AUS)]
        [InlineData(Country.AUT, CountryDto.AUT)]
        [InlineData(Country.AZE, CountryDto.AZE)]
        [InlineData(Country.BHS, CountryDto.BHS)]
        [InlineData(Country.BHR, CountryDto.BHR)]
        [InlineData(Country.BGD, CountryDto.BGD)]
        [InlineData(Country.BRB, CountryDto.BRB)]
        [InlineData(Country.BLR, CountryDto.BLR)]
        [InlineData(Country.BEL, CountryDto.BEL)]
        [InlineData(Country.BLZ, CountryDto.BLZ)]
        public void ToDto_ShouldReturnCorrectCountryDto(Country country, CountryDto expectedDto)
        {
            // Act
            var result = country.ToDto();

            // Assert
            Assert.Equal(expectedDto, result);
        }

        [Fact]
        public void ToDto_ShouldThrowArgumentException_WhenNoMappingIsDefined()
        {
            // Arrange
            var invalidCountry = (Country)999;

            // Act
            var exception = Assert.Throws<ArgumentException>(() => invalidCountry.ToDto());

            // Assert
            Assert.Equal("No mapping defined for 999", exception.Message);
        }
    }
}
