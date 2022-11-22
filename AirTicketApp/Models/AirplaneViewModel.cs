using AirTicketApp.Data.EntityModels;

namespace AirTicketApp.Models
{
    public class AirplaneViewModel
    {
        public int Id { get; set; }
        public string Model { get; set; }

        public Manufacture Manufacture { get; set; }

        public int Capacity { get; set; }
    }
}
