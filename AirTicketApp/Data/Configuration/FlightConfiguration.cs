using AirTicketApp.Data.EntityModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AirTicketApp.Data.Configuration
{
    public class FlightConfiguration : IEntityTypeConfiguration<Flight>
    {
        public void Configure(EntityTypeBuilder<Flight> builder)
        {
            DateTime date = DateTime.Parse("2022-11-19 16:30:00");
            builder.HasData(new Flight()
            {
                Id = 1,
                DepartureId = 1,
                ArrivalId = 2,
                ArrivalDate = date.AddHours(2),
                DepartureDate = date,
                CompanyId = 1,
                Price = 100,
                AirplaneId = 1,
                Drinks = true,
                Food = true,
                Luggage = true,
                Duration = 120
            }); 
        }
    }
}
