using System.ComponentModel.DataAnnotations;

namespace AirTicketApp.Data.EntityModels
{
    public class Company
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = null!;
    }
}
