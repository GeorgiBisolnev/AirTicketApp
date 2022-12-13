using AirTicketApp.Data.EntityModels;
using System.ComponentModel.DataAnnotations;
using static AirTicketApp.Data.Common.UserModelConstants;

namespace AirTicketApp.Models.ApplicationUser
{
    public class ApplicationUserViewModel
    {
        [StringLength(MaxPassportNumLength)]
        public string PassportNum { get; set; } = null!;

        public int? CountryId { get; set; }

        public Country? Country { get; set; }

        [StringLength(MaxFirstNameStringLenght)]
        public string FirstName { get; set; }

        [StringLength(MaxLastNameStringLenght)]
        public string LastName { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        public string UserName { get; set; }

        [Phone]
        public string PhoneNumber { get; set; }
    }
}
