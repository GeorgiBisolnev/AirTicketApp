using AirTicketApp.Data.EntityModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AirTicketApp.Data.Configuration
{
    internal class CountryConfiguration : IEntityTypeConfiguration<Country>
    {
        public void Configure(EntityTypeBuilder<Country> builder)
        {
            builder.HasData(CreateCountries());
        }

        private List<Country> CreateCountries()
        {
            var countries   = new List<Country>()
            {
                new Country()
                {
                    Id=1,
                    Name="Bulgaria"
                },
                new Country()
                {
                    Id=2,
                    Name = "Russia"
                },
                new Country()
                {
                    Id=3,
                    Name="Turkey"
                },
                new Country()
                {
                    Id=4,
                    Name="Spain"
                }
            };
            return countries;
        }

    }
}
