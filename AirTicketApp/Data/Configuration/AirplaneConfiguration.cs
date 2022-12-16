using AirTicketApp.Data.EntityModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AirTicketApp.Data.Configuration
{
    internal class AirplaneConfiguration : IEntityTypeConfiguration<Airplane>
    {
        public void Configure(EntityTypeBuilder<Airplane> builder)
        {
            builder.HasData(CreateAirplanes());
        }

        private List<Airplane> CreateAirplanes()
        {
            var airplanes = new List<Airplane>()
            {
                new Airplane()
                {
                    Id = 1,
                    ManufactureId=1,
                    Model="737",
                    Capacity=166
                },
                new Airplane()
                {
                    Id = 2,
                    ManufactureId=2,
                    Model="Skyhawk 172",
                    Capacity=4
                },
                new Airplane()
                {
                    Id=3,
                    ManufactureId=3,
                    Model="А318",
                    Capacity=136
                },
                new Airplane()
                {
                    Id=4,
                    ManufactureId=4,
                    Model="Superjet 100",
                    Capacity=108
                }
            };
            return airplanes;
        }
    }
}
