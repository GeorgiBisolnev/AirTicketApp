using AirTicketApp.Data.Configuration;
using AirTicketApp.Data.EntityModels;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AirTicketApp.Data
{
    public class AirTicketAppContext : IdentityDbContext
    {
        public AirTicketAppContext(DbContextOptions<AirTicketAppContext> options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {           
            builder.ApplyConfiguration(new UserConfiguration());
            builder.ApplyConfiguration(new AirplaneConfiguration());
            builder.ApplyConfiguration(new AirportConfiguration());
            builder.ApplyConfiguration(new CityConfiguration());
            builder.ApplyConfiguration(new CompaniesConfiguration());
            builder.ApplyConfiguration(new CountryConfiguration());
            builder.ApplyConfiguration(new ManufactureConfiguration());
            builder.ApplyConfiguration(new FlightConfiguration());

            base.OnModelCreating(builder);
        }

        public DbSet<Airplane> Airplanes { get; set; }
        public DbSet<Airport> Airports { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<Flight> Flights { get; set; }
        public DbSet<Manufacture> Manufactures { get; set; }
        public DbSet<Ticket> Tickets { get; set; }

    }
}