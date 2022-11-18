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
                }
            };



            return airplanes;
        }
    }
}
