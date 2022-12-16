using AirTicketApp.Data.EntityModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AirTicketApp.Data.Configuration
{
    internal class CityConfiguration : IEntityTypeConfiguration<City>
    {
        public void Configure(EntityTypeBuilder<City> builder)
        {
            builder.HasData(CreateCityes());
        }
            private List<City> CreateCityes()
            {
                    List<City> cities = new List<City>()
                    {
                        new City()
                        {
                            Id = 1,
                            Name = "Sofia",
                            CountryId = 1,
                        },

                        new City()
                        {
                            Id=2,
                            Name = "Moscow",
                            CountryId = 2
                        },

                        new City()
                        {
                            Id=3,
                            Name = "Istanbul",
                            CountryId = 3
                        },
                        new City()
                        {
                            Id = 4,
                            Name = "Burgas",
                            CountryId = 1,
                        },
                        new City()
                        {
                            Id = 5,
                            Name = "Madrid",
                            CountryId = 4,
                        },
                    };

                return cities;
            }
        } 
}
