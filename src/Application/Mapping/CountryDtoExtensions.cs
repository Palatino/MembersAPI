using Contracts.Enums;
using Domain.Enums;

namespace Application.Mapping
{
    public static class CountryDtoExtensions
    {
        public static Country ToDomainEntity(this CountryDto countryDto)
        {
            return countryDto switch
            {
                CountryDto.AFG => Country.AFG,
                CountryDto.ALB => Country.ALB,
                CountryDto.DZA => Country.DZA,
                CountryDto.AND => Country.AND,
                CountryDto.AGO => Country.AGO,
                CountryDto.ATG => Country.ATG,
                CountryDto.ARG => Country.ARG,
                CountryDto.ARM => Country.ARM,
                CountryDto.AUS => Country.AUS,
                CountryDto.AUT => Country.AUT,
                CountryDto.AZE => Country.AZE,
                CountryDto.BHS => Country.BHS,
                CountryDto.BHR => Country.BHR,
                CountryDto.BGD => Country.BGD,
                CountryDto.BRB => Country.BRB,
                CountryDto.BLR => Country.BLR,
                CountryDto.BEL => Country.BEL,
                CountryDto.BLZ => Country.BLZ,
                _ => throw new ArgumentException($"No mapping defined for {countryDto}")
            };
        }
    }
}

