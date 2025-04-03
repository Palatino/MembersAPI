using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.EntityConfigurations
{
    class MembershipConfiguration : IEntityTypeConfiguration<Membership>
    {
        public void Configure(EntityTypeBuilder<Membership> builder)
        {
            var settings = new JsonSerializerSettings
            {
                Converters = new List<JsonConverter>
                {
                    new StringEnumConverter(new CamelCaseNamingStrategy(), false),
                },
                NullValueHandling = NullValueHandling.Ignore,
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };

            //Save country and subscription type as string rather than its int value
            //Makes DB more readble and avoiding problems if enum is extended
            builder.Property(e => e.Country).HasConversion(
                v => JsonConvert.SerializeObject(v, settings),
                v => JsonConvert.DeserializeObject<Domain.Enums.Country>(v, settings));

            builder.Property(e => e.SubscriptionType).HasConversion(
                v => JsonConvert.SerializeObject(v, settings),
                v => JsonConvert.DeserializeObject<Domain.Enums.SubscriptionType>(v, settings));
        }
    }
}
