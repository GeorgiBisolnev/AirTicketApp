﻿using AirTicketApp.Models;
using AirTicketApp.Models.Flight;

namespace AirTicketApp.Services.Contracts
{
    public interface IFlightService
    {
        Task<IEnumerable<FlightViewModel>> AllFlights(); 
        Task<IEnumerable<FlightViewModel>> AllFlightsFilter(
            FlightSorting? sorting = FlightSorting.Price,
            DateTime? searchDate = null,
            int? ArrivalAirportId = null,
            int? DepartureAirportId = null
            ); 
        Task<IEnumerable<AirportViewModel>> GetAllAirports(); 
        Task<IEnumerable<CompanyViewModel>> GetAllCompanies(); 
        Task<IEnumerable<AirplaneViewModel>> GetAllAirplanes(); 
        Task<FlightViewModelDetails> Details(int id); 
        Task<int> Create(FlightViewModel model); 
    }
}
