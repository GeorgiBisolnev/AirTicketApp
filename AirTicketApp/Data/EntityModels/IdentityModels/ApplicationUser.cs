using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static AirTicketApp.Data.Common.UserModelConstants;

namespace AirTicketApp.Data.EntityModels.IdentityModels
{
    public class ApplicationUser : IdentityUser
    {
        [StringLength(MaxPassportNumLength)]
        public string? PassportNum { get; set; } = null!;

        public int? CountryId { get; set; }

        [ForeignKey(nameof(CountryId))]
        public Country? Country { get; set; }

        [StringLength(MaxFirstNameStringLenght)]
        public string? FirstName { get; set; }

        [StringLength(MaxLastNameStringLenght)]
        public string? LastName { get; set; }
    }
}
