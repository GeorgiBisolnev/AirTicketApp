namespace AirTicketApp.Models.FlightModels
{
    public class FlightViewCarouselModel
    {
        public int FlightId { get; set; }
        public string ArrivalAirport { get; set; }
        public string DepartureAirport { get; set; }

        public decimal  Price { get; set; }
        public string Company { get; set; }

        public string DepartureCity { get; set; }
        public string ArrivalCity { get; set; }

        public string ImgUrlC { get; set; }
    }
}
