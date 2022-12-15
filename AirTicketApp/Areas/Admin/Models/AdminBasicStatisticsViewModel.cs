namespace AirTicketApp.Areas.Admin.Models
{
    public class AdminBasicStatisticsViewModel
    {
        public AdminBasicStatisticsViewModel()
        {
            NumberOfUsers = 0;
            TotalTickets = 0;
            MostPopularAirport = "";
        }
        public int NumberOfUsers { get; set; }
        public int TotalTickets { get; set; }
        public string MostPopularAirport { get; set; }
    }
}
