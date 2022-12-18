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

        [Required(ErrorMessage = "Company name is required")]
        [StringLength(MaxCompanyNameLength)]
        public string Name { get; set; } = null!;
        [Required(ErrorMessage = "Required")]
        [RegularExpression(@"^.*\.(jpg|JPG|gif|GIF|png|PNG)$", ErrorMessage = "Invalid img adress")]
        public string ImgUrl { get; set; }
        [Required(ErrorMessage = "Required")]
        [RegularExpression(@"^.*\.(jpg|JPG|gif|GIF|png|PNG)$",ErrorMessage ="Invalid img adress")]
        public string ImgUrlCarousel { get; set; }        
    }
}
