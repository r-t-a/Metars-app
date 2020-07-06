using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Metars.Models;

namespace Metars.Services.Interfaces
{
    public interface IAirportService 
    {
        Task<List<Airport>> GetAllAirports(CancellationToken token);
        Task SaveAirport(Airport airport, CancellationToken token);
        Task DeleteAirport(long id, CancellationToken token);
        Task GetAirportMetar(string airportCode, CancellationToken token);
        Task GetAirportDetails(string airportCode, CancellationToken token);
    }
}
