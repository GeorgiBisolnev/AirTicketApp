using AirTicketApp.Data.EntityModels.IdentityModels;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AirTicketApp.Data.EntityModels
{
    public class Ticket
    {
        [Key]
        public Guid guid { get; set; }

        [Required]
        public int FlightId { get; set; }

        [ForeignKey(nameof(FlightId))]
        public Flight Flight { get; set; } = null!;

        [Required]
        public string UserId { get; set; } = null!;

        [ForeignKey(nameof(UserId))]
        public ApplicationUser User { get; set; } = null!;

    }
}
