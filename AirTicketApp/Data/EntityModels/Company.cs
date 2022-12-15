using System.ComponentModel.DataAnnotations;
using static AirTicketApp.Data.Common.CompanyModelConstants;

namespace AirTicketApp.Data.EntityModels
{
    public class Company
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(MaxCompanyNameLength)]
        public string Name { get; set; } = null!;

        public string? ImgUrl { get; set; }
        public string? ImgUrlCarousel { get; set; }
    }
}
