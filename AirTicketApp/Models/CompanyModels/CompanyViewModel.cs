using System.ComponentModel.DataAnnotations;
using static AirTicketApp.Data.Common.CompanyModelConstants;

namespace AirTicketApp.Models.CompanyModels
{
    public class CompanyViewModel
    {
        public CompanyViewModel()
        {
            Id = 0;
            Name = "";
            ImgUrl = "";
            ImgUrlCarousel = "";
        }

        [Required]
        public int Id { get; set; }

        [Required]
        [StringLength(MaxCompanyNameLength)]
        public string Name { get; set; } = null!;
        [Required]
        public string ImgUrl { get; set; }
        [Required]
        public string ImgUrlCarousel { get; set; }
    }
}
