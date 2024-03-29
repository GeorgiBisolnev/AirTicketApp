﻿using AirTicketApp.Data.EntityModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AirTicketApp.Data.Configuration
{
    internal class AirportConfiguration : IEntityTypeConfiguration<Airport>
    {
        public void Configure(EntityTypeBuilder<Airport> builder)
        {
            builder.HasData(CreateAirports());
        }

        private List<Airport> CreateAirports()
        {
            var airports = new List<Airport>();

            airports.Add(new Airport()
            {
                Id=1,
                Name = "Sofia Airport",
                IATACode="SOF",
                CityId=1

            });
            airports.Add(new Airport()
            {
                Id = 2,
                Name = "Sheremetyevo Airport",
                IATACode = "SVO",
                CityId = 2

            });
            airports.Add(new Airport()
            {
                Id = 3,
                Name = "Istanbul Airport",
                IATACode = "IST",
                CityId = 3

            });
            airports.Add(new Airport()
            {
                Id = 4,
                Name = "Burgas Airport",
                IATACode = "BOJ",
                CityId = 4

            });
            airports.Add(new Airport()
            {
                Id = 5,
                Name = "Barajas Airport",
                IATACode = "MAD",
                CityId = 4
            });

            return airports;
        }
    }
}
