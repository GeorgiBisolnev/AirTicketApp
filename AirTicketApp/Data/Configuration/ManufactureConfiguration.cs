using AirTicketApp.Data.EntityModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AirTicketApp.Data.Configuration
{
    internal class ManufactureConfiguration : IEntityTypeConfiguration<Manufacture>
    {
        public void Configure(EntityTypeBuilder<Manufacture> builder)
        {
            builder.HasData(CreateManufacture());
        }

        private List<Manufacture> CreateManufacture()
        {

            var manufactures = new List<Manufacture>()
            {
                new Manufacture()
                {
                    Id=1,
                    Name="Boeing"
                }
            };

            return manufactures;

        }
    }
}
