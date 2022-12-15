using System.ComponentModel.DataAnnotations;
using static AirTicketApp.Data.Common.CompanyModelConstants;

namespace AirTicketApp.Models.CompanyModels
{
    public class CompanyViewModel
    {
        public int Id { get; set; }

        [Required]
        [StringLength(MaxCompanyNameLength)]
        public string Name { get; set; } = null!;
        public string? ImgUrl { get; set; }
    }
}
