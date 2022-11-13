using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AirTicketApp.Data.EntityModels
{
    public class User : IdentityUser
    {
        [Required]
        public string PassportNum { get; set; } = null!;

        public int? CountryId { get; set; }

        [ForeignKey(nameof(CountryId))]
        public Country Country { get; set; } 
    }
}
