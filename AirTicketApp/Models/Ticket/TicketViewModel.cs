using AirTicketApp.Data.EntityModels;
using AirTicketApp.Models.ApplicationUser;
using AirTicketApp.Models.FlightModels;
using System.ComponentModel.DataAnnotations;
using static AirTicketApp.Data.Common.UserModelConstants;

namespace AirTicketApp.Models.Ticket
{
    public class TicketViewModel
    {
        public FlightViewModelDetails? FlightModel { get; set; }

        [StringLength(MaxPassportNumLength)]
        [RegularExpression(@"^[a-zA-Z1-9]{5,20}$", ErrorMessage ="Passport number must be 5 to 20 symbols")]
        public string PassportNum { get; set; } = null!;

        public int? CountryId { get; set; }

        public Country? Country { get; set; }

        [StringLength(MaxFirstNameStringLenght)]
        public string FirstName { get; set; }

        [StringLength(MaxLastNameStringLenght)]
        public string LastName { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        public string? UserName { get; set; }

        [Phone]
        [Required(ErrorMessage = "Required")]
        [RegularExpression(@"^[\+]?[(]?[0-9]{3}[)]?[-\s\.]?[0-9]{3}[-\s\.]?[0-9]{4,6}$", ErrorMessage = "Wrong phone number")]
        public string PhoneNumber { get; set; }

    }
}
