using AirTicketApp.Data.EntityModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AirTicketApp.Data.Configuration
{
    public class CompaniesConfiguration : IEntityTypeConfiguration<Company>
    {
        public void Configure(EntityTypeBuilder<Company> builder)
        {
            builder.HasData(CreateCompanies());
        }

        private List<Company> CreateCompanies()
        {
            var companies = new List<Company>()
            {
                new Company()
                {
                    Id = 1,
                    Name = "Bulgaria Air"
                },
                new Company()
                {
                    Id = 2,
                    Name = "Aeroflot"
                },
                new Company()
                {
                    Id = 3,
                    Name = "Turkish Airlines"
                },
            };



            return companies;
        }
    }
}
