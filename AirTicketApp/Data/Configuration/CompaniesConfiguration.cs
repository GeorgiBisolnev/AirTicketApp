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
                    Name = "Bulgaria Air",
                    ImgUrl="https://www.fim-musicians.org/wp-content/uploads/bulgaria-air.png",
                    ImgUrlCarousel="https://cdn.aiidatapro.net/media/3e/4e/b4/t780x490/3e4eb43084e6c2bffb1a75f14bdd696bdb4511b6.jpg"
                },
                new Company()
                {
                    Id = 2,
                    Name = "Aeroflot",
                    ImgUrl="https://s0.rbk.ru/emitent_pics/images/52/12/b2e6636f4cc75e7c8b61aecef55d6572.png",
                    ImgUrlCarousel="https://cdn.businesstraveller.com/wp-content/uploads/fly-images/945460/Aeroflot-B7378-916x516.jpg"
                },
                new Company()
                {
                    Id = 3,
                    Name = "Turkish Airlines",
                    ImgUrl="https://www.fim-musicians.org/wp-content/uploads/turkish-airlines.png",
                    ImgUrlCarousel="https://www.economic.bg/web/files/articles/61839/narrow_image/thumb_834x469_89.jpg"
                },
                new Company()
                {
                    Id = 4,
                    Name = "Emirates Air-lines",
                    ImgUrl="https://uniticket.co.uk/wp-content/uploads/2019/airlines_64/EK.png",
                    ImgUrlCarousel="https://c.ekstatic.net/ecl/aircraft-exterior/boeing-777/emirates-boeing-777-sun-setting-down-w768x480.jpg"
                },
                new Company(){
                    Id = 5,
                    Name = "Ryanair",
                    ImgUrl = "https://panorama.quicket.io/airlines/logo/logo-FR.png",
                    ImgUrlCarousel="https://upload.wikimedia.org/wikipedia/commons/thumb/0/01/Ryanair_Boeing_737-800_EI-EBX.jpg/800px-Ryanair_Boeing_737-800_EI-EBX.jpg"
                }
            };



            return companies;
        }
    }
}
