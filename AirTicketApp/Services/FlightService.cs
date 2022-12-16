using AirTicketApp.Data.Common.Repository;
using AirTicketApp.Data.EntityModels;
using AirTicketApp.Services.Contracts;
using Microsoft.EntityFrameworkCore;
using AirTicketApp.Models.FlightModels;

namespace AirTicketApp.Services
{
    public class FlightService : IFlightService
    {
        private readonly IRepository repo;

        public FlightService(IRepository _repo)
        {
            this.repo = _repo;
        }
        /// <summary>
        /// Процедурата връща модел за всички полети с дата на заминаване -най-малко днешна дата
        /// </summary>
        /// <returns>Всички бъдещо полети</returns>
        public async Task<IEnumerable<FlightViewModel>> AllFlights()
        {
            var flights =  await repo.AllReadonly<Flight>()
                .AsNoTracking()
                .Include(m=>m.Airplane.Manufacture)
                .Include(c=>c.ArrivalAirport.City)
                .Where(d=>d.DepartureDate>=DateTime.Today)
                .Select(f=> new FlightViewModel()
                {
                    Id=f.Id,
                    ArrivalAirport = f.ArrivalAirport,
                    DepartureAirport = f.DepartureAirport,
                    ArrivalDate = f.ArrivalDate,
                    DepartureDate = f.DepartureDate,
                    Duration = f.Duration,
                    Company = f.Company,
                    Airplane = f.Airplane,
                    Price = f.Price,
                    Snack = f.Snack,
                    Food = f.Food,
                    Luggage = f.Luggage,
                })
                .ToListAsync();
            
            return flights;
        }
        /// <summary>
        /// Процедурата създава полет по всички входни параметри от модела
        /// </summary>
        /// <param name="model"></param>
        /// <returns>Връща Id на полета при успешно създаване на полет</returns>
        public async Task<int> Create(FlightViewModel model)
        {
            var flight = new Flight()
            {
                ArrivalId = model.ArrivalId,
                DepartureId = model.DepartureId,
                ArrivalDate = model.ArrivalDate,
                DepartureDate = model.DepartureDate,
                CompanyId = model.CompanyId,
                Price = model.Price,
                AirplaneId = model.AirplaneId,
                Snack = model.Snack,
                Food = model.Food,
                Luggage = model.Luggage,
                Duration = model.Duration
            };

            await repo.AddAsync(flight);
            await repo.SaveChangesAsync();

            return flight.Id;
        }
        /// <summary>
        /// finds detiled information about flights
        /// </summary>
        /// <param name="flightId"></param>
        /// <returns>Flight model</returns>
        /// <exception cref="Exception"></exception>
        public async Task<FlightViewModelDetails> Details(int Id)
        {

            var f = await repo.AllReadonly<Flight>()
                            .AsNoTracking()
                            .Include(m => m.Airplane.Manufacture)
                            .Include(c => c.ArrivalAirport.City.Country)
                            .Include(c => c.DepartureAirport.City.Country)
                            .Include(c=>c.Company)
                            .FirstOrDefaultAsync(f => f.Id == Id);

            int buyedFlights = await repo.AllReadonly<Ticket>()
                .AsNoTracking()
                .CountAsync(t => t.FlightId == Id);

            if (f==null)
            {
                throw new Exception ("We cant find flight information!");
            }

            
            var flightModel = new FlightViewModelDetails()
            {
                Id = f.Id,
                ArrivalAirport = f.ArrivalAirport,
                DepartureAirport = f.DepartureAirport,
                ArrivalDate = f.ArrivalDate,
                DepartureDate = f.DepartureDate,
                Duration = f.Duration,
                Company = f.Company,
                Airplane = f.Airplane,
                Price = f.Price,
                Snack = f.Snack,
                Food = f.Food,
                Luggage = f.Luggage,
                AvailablePlaces = f.Airplane.Capacity - buyedFlights,
            };

            return flightModel;
        }
        /// <summary>
        /// Процедурата връща модел за полет, като потребителя предварително подава като параметър дата на полет, начин на сортиране
        /// летище на излитане и кацане
        /// </summary>
        /// <param name="sorting"></param>
        /// <param name="searchDate"></param>
        /// <param name="ArrivalAirportId"></param>
        /// <param name="DepartureAirportId"></param>
        /// <returns>Връща списъчен модел с всички полети по зададените критерии</returns>
        /// <exception cref="ArgumentException"></exception>
        public async Task<IEnumerable<FlightViewModel>> AllFlightsFilter(
            FlightSorting? sorting = 0,
            DateTime? searchDate = null,
            int? ArrivalAirportId = null,
            int? DepartureAirportId = null)
        {
            if (searchDate==null || ArrivalAirportId == null || DepartureAirportId == null ||
                ArrivalAirportId == 0 || DepartureAirportId==0)
            {
                throw new ArgumentException("Wrong input parameters"); 
            }
            var result = await AllFlights();

            result = result.Where(f => f.ArrivalAirport.Id == ArrivalAirportId);

            result = result.Where(f => f.DepartureAirport.Id == DepartureAirportId);

            result = result.Where(f => f.DepartureDate.Date == searchDate);

            result = sorting switch
            {
                FlightSorting.Price => result
                    .OrderBy(f=>f.Price).ToList(),
                FlightSorting.Company=>result
                    .OrderByDescending(f=>f.Company.Name).ToList(),
                FlightSorting.DepartureDate => result
                    .OrderBy(f=>f.DepartureDate).ToList(),
                FlightSorting.ArrivalDate=> result
                    .OrderBy(f => f.ArrivalDate).ToList(),
                _ => result.OrderBy(f=>f.Id).ToList()
            };

            return result;
        }
        /// <summary>
        /// Процедурата редактира полет по подаденият модел и ID на полет
        /// 
        /// </summary>
        /// <param name="flightId"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public async Task Edit(int flightId, FlightViewModel model)
        {
            var flight = await repo.All<Flight>()
                .Include(f=>f.ArrivalAirport)
                .Include(f=>f.DepartureAirport)
                .Include(c=>c.Company)
                .FirstOrDefaultAsync(f=>f.Id==flightId);

            if (flight == null)
            {
                throw new ArgumentException("There is no flightwith such id");
            }
            // ако няма закупени билети по този полет, то може да променим летище за кацане и пристигане, компанията и самолета            
            if (await BuyedFlightsByGivenFlightId(flightId)==false) 
            {
                flight.ArrivalId = model.ArrivalId;
                flight.DepartureId = model.DepartureId;
                flight.CompanyId = model.CompanyId;
                flight.AirplaneId = model.AirplaneId;
            } else if(model.ArrivalId!=flight.ArrivalId  
                || flight.DepartureId != model.DepartureId 
                || flight.CompanyId != model.CompanyId 
                || flight.AirplaneId != model.AirplaneId
                // ако датата на излитане и кацане е коригирана с разлика повече от 24 то тази проверка няма да позволи това
                || model.ArrivalDate > flight.ArrivalDate.AddDays(1)
                || model.ArrivalDate < flight.ArrivalDate.AddDays(-1)
                || model.DepartureDate > flight.DepartureDate.AddDays(1)
                || model.DepartureDate < flight.DepartureDate.AddDays(-1))
            {
                throw new ArgumentException("Company, Airplane, airports and date's more that 24 hours cant be chaged due to avalible tickets for this flight!");
            }
            //параметрите които можем да редактираме без значение дали има закупени билети
            flight.Snack = model.Snack;
            flight.Food = model.Food;
            flight.Luggage = model.Luggage;
            flight.Price = model.Price;
            flight.Duration = model.Duration;
            flight.ArrivalDate = model.ArrivalDate;
            flight.DepartureDate = model.DepartureDate;

            await repo.SaveChangesAsync();
        }
        /// <summary>
        /// Процедурата проверява дали има закупени билети по подадено Id на полет
        /// Трябва да се помисли дали процедурата не трябва да се премести в Ticket сървиса
        /// </summary>
        /// <param name="flightId"></param>
        /// <returns>Връща бълева стойност, дали има закупени билети за подаденият полет</returns>
        public async Task<bool> BuyedFlightsByGivenFlightId(int flightId)
        {
            var result = await repo.AllReadonly<Ticket>()
                .AsNoTracking()
                .FirstOrDefaultAsync(t => t.FlightId == flightId);

            if (result==null)
            {
                return false;
            }

            return true;
        }
        /// <summary>
        /// Процедурата връща полетен модел при подаден Id на полет
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public async Task<FlightViewModel> GetFlightById(int Id)
        {
            var model = await repo.AllReadonly<Flight>()
                .AsNoTracking()
                .Where(f=>f.Id==Id)
                .Include(f=>f.ArrivalAirport)
                .Include(f=>f.DepartureAirport)
                .Include(c=>c.Company)
                .Include(c=>c.ArrivalAirport.City.Country)
                .Include(c=>c.DepartureAirport.City.Country)
                .Include(a=>a.Airplane.Manufacture)
                .Select(f => new FlightViewModel()
                {
                    Id = f.Id,
                    ArrivalAirport = f.ArrivalAirport,
                    DepartureAirport = f.DepartureAirport,
                    ArrivalDate = f.ArrivalDate,
                    DepartureDate = f.DepartureDate,
                    Duration = f.Duration,
                    Company = f.Company,
                    CompanyId=f.CompanyId,
                    Airplane = f.Airplane,
                    Price = f.Price,
                    Snack = f.Snack,
                    Food = f.Food,
                    Luggage = f.Luggage,
                })
                .FirstOrDefaultAsync();

            if (model == null)
            {
                throw new ArgumentException("There is no flight with such Id!");
            }

            return model;
        }
        /// <summary>
        /// Процедурата проверява дали в БД съществува полет с подаден параметър Id на полет
        /// </summary>
        /// <param name="flightId"></param>
        /// <returns>Връща булева стойност дали съществува такъв полет</returns>
        public async Task<bool> FlightExists(int flightId)
        {
            var result = await repo.AllReadonly<Flight>()
                .AsNoTracking()
                .ToListAsync();

            if (result.FirstOrDefault(f => f.Id == flightId) == null)
            {
                return false;
            }

            return true;
        }
        /// <summary>
        /// Процедурата въща най-евтините 3 полета в БД
        /// Полети с по-стара дата от днешната не се взимат в изчислението на процедурата
        /// Процедурата се използва за реклама на евтините билети в началната страница на приложението
        /// </summary>
        /// <returns>Връща 3 най-евтини полети като модел</returns>
        public async Task<IEnumerable<FlightViewCarouselModel>> GetMostCheapThreeFlights()
        {
            var result = await repo.AllReadonly<Flight>()
                .Include(f=>f.ArrivalAirport)
                .Include(f=>f.DepartureAirport.City)
                .Include(f=>f.DepartureAirport.City)
                .AsNoTracking()
                .Where(f=>f.DepartureDate>=DateTime.Now)
                .OrderBy(f => f.Price)
                .Take(3)
                .Select(f => new FlightViewCarouselModel()
                {
                    FlightId = f.Id,
                    ArrivalAirport = f.ArrivalAirport.Name,
                    DepartureAirport = f.DepartureAirport.Name,
                    ArrivalCity = f.ArrivalAirport.City.Name,
                    DepartureCity = f.DepartureAirport.City.Name,
                    Price = f.Price,
                    Company = f.Company.Name,
                    ImgUrlC = f.Company.ImgUrlCarousel
                })
                .ToListAsync();

            return result;
        }
        /// <summary>
        /// Процедурата връща информация за най-скъпият полет
        /// Полети с по-стара дата от днешната не се взимат в предвид в изчислението
        /// </summary>
        /// <returns>Връща име на летището и града плюс цена за най-скъпият полет</returns>
        public async Task<string> MostExpensiveDestination()
        {
            var result = await repo.AllReadonly<Flight>()
                .AsNoTracking()
                .Where(f=>f.DepartureDate>= DateTime.Now)
                .Include(f=>f.ArrivalAirport.City)
                .OrderByDescending(t => t.Price)
                .Select(t => new
                {
                    ArrivalAirport = t.ArrivalAirport.Name,
                    ArrivalCity = t.ArrivalAirport.City.Name,
                    Price = t.Price
                })
                .FirstOrDefaultAsync();
            if (result==null)
            {
                return  "";
            }

            return $"{result.ArrivalCity}, {result.ArrivalAirport} for ${result.Price.ToString("F2")}"; 
        }
    }
}
