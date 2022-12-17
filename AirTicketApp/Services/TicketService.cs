using AirTicketApp.Data.Common.Repository;
using AirTicketApp.Data.EntityModels;
using AirTicketApp.Data.EntityModels.IdentityModels;
using AirTicketApp.Models.Ticket;
using AirTicketApp.Services.Contracts;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace AirTicketApp.Services
{
    public class TicketService : ITicketService
    {

        private readonly IRepository repo;

        private readonly IFlightService flightService;

        public TicketService(
            IRepository _repo,
            IFlightService _flightService)
        {
            this.repo = _repo;
            this.flightService = _flightService;
        }
        /// <summary>
        /// Връща всички билети по дадено Id на потребител от таблицата Users
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>Връща модел за преглед на билети</returns>
        public async Task<List<TicketAllViewModel>> AllTicketsByUser(string userId)
        {

            var result = await repo.AllReadonly<Ticket>()
                .AsNoTracking()
                .Where(x => x.UserId == userId)
                .Select(t => new TicketAllViewModel()
                {
                    TicketId = t.guid.ToString(),
                    FlightId = t.FlightId
                })
                .ToListAsync();

            foreach (var ticket in result)
            {
                var flightModel = await flightService.GetFlightById(ticket.FlightId);
                ticket.ArrivalDate = flightModel.ArrivalDate;
                ticket.DepartureDate = flightModel.DepartureDate;
                ticket.ArrivalAirportName = flightModel.ArrivalAirport.Name;
                ticket.ArrivalAirportCode = flightModel.ArrivalAirport.IATACode;
                ticket.ArrivalCity = flightModel.ArrivalAirport.City.Name;
                ticket.ArrivalCountry= flightModel.ArrivalAirport.City.Country.Name;
                ticket.DepartureAirportName = flightModel.DepartureAirport.Name;
                ticket.DepartureAirportCode = flightModel.DepartureAirport.IATACode;
                ticket.DepartureCity = flightModel.DepartureAirport.City.Name;
                ticket.DepartureCountry = flightModel.DepartureAirport.City.Country.Name;
                ticket.Price = flightModel.Price;
                ticket.CompanyName = flightModel.Company.Name;
            }

            return result.OrderByDescending(t => t.ArrivalDate).ToList();
        }
        /// <summary>
        /// Тази процедура приверява дали има свободни билети за даденият полет и участващият в него самолет
        /// </summary>
        /// <param name="flightId"></param>
        /// <param name="capacity"></param>
        /// <returns>Връща даали има или няма свободни билети за този полет</returns>
        public async Task<bool> AvailableTickets(int flightId, int capacity)
        {
            var result = await repo.AllReadonly<Ticket>()
                .AsNoTracking()
                .CountAsync(t=>t.FlightId==flightId);

            if (result< capacity)
            {
                return true;
            }

            return false;
        }
        /// <summary>
        /// Процедурата записва нов билет за даденият потребител и ID на полет.
        /// За в бъеще трябва да се премахне подаването на капацитета на самолета участващ в полета
        /// Мисля че е излишно да се подава като параметър
        /// </summary>
        /// <param name="flightId"></param>
        /// <param name="userId"></param>
        /// <param name="capacity"></param>
        /// <returns>Връща номера на билета при успешно купуване на билет</returns>
        /// <exception cref="ArgumentException"></exception>
        public async Task<string> BuyTicket(int flightId, string userId, int capacity)
        {
            bool flightExsists = await flightService.FlightExists(flightId);
            Ticket ticket = new Ticket();
            var flight = await flightService.GetFlightById(flightId);
            DateTime departureDate = flight.DepartureDate;

            if (departureDate<DateTime.Now)
            {
                throw new ArgumentException("This flight is no longer avaliable!");
            }
                
            var ticketsAvalibale = await AvailableTickets(flightId, capacity);
            if (flightExsists && ticketsAvalibale)
            {
                ticket.FlightId = flightId;
                ticket.UserId = userId;

                await repo.AddAsync(ticket);
                await repo.SaveChangesAsync();

                return ticket.guid.ToString();
            }
            else
            {
                throw new ArgumentException("There is no avalible tickets for this flight!");
            }

        }
        /// <summary>
        /// Процедурата групира всички купени билети по летище на пристигане и връща летището за което са купени най-много билети
        /// </summary>
        /// <returns>Връща името на лтището с най-много закупени билети</returns>
        public async Task<string> MostPopularAirport()
        {
            var list = await repo.AllReadonly<Ticket>()
                .Include(t => t.Flight.ArrivalAirport)
                .GroupBy(t => t.Flight.ArrivalAirport.Name)
                .Select(g => new
                {
                    Name = g.Key,
                    Count = g.Count()
                })
                .OrderByDescending(t=>t.Count)
                .ToListAsync();
            
            if (list.Count() == 0)
            {
                return "";
            }

            string result = list.FirstOrDefault().Name;
            return result;
        }
        /// <summary>
        /// Процедурата връща брой на всички закупени билети
        /// </summary>
        /// <returns>бройка на всички закупени билети</returns>
        public async Task<int> TotalTickets()
        {
            return await repo.AllReadonly<Ticket>()
                .AsNoTracking()
                .CountAsync();
        }
    }
}
