using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AirTicketApp.Data.Configuration
{
        internal class RoleConfiguration : IEntityTypeConfiguration<IdentityRole>
        {
            public void Configure(EntityTypeBuilder<IdentityRole> builder)
            {
                builder.HasData(
                    new IdentityRole
                    {
                        Id = "f4e6d3d7-2c27-4e98-ac4c-aae04be8411b",
                        Name = "Administrator",
                        NormalizedName = "ADMINISTRATOR",
                        ConcurrencyStamp = "0c8835d0-fe93-4abc-9893-1a3a4667d41e"
                    }
                );
            }
        }
}
