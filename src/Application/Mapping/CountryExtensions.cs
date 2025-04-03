using Contracts.Enums;
using Domain.Enums;

namespace Application.Mapping
{

    public static class CountryExtensions
    {
        public static CountryDto ToDto(this Country country)
        {
            return country switch
            {
                Country.AFG => CountryDto.AFG,
                Country.ALB => CountryDto.ALB,
                Country.DZA => CountryDto.DZA,
                Country.AND => CountryDto.AND,
                Country.AGO => CountryDto.AGO,
                Country.ATG => CountryDto.ATG,
                Country.ARG => CountryDto.ARG,
                Country.ARM => CountryDto.ARM,
                Country.AUS => CountryDto.AUS,
                Country.AUT => CountryDto.AUT,
                Country.AZE => CountryDto.AZE,
                Country.BHS => CountryDto.BHS,
                Country.BHR => CountryDto.BHR,
                Country.BGD => CountryDto.BGD,
                Country.BRB => CountryDto.BRB,
                Country.BLR => CountryDto.BLR,
                Country.BEL => CountryDto.BEL,
                Country.BLZ => CountryDto.BLZ,
                _ => throw new ArgumentException($"No mapping defined for {country}")
            };
        }
    }

}
