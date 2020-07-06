using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Metars.Models;

namespace Metars.Repositories.Interfaces
{
    public interface IAirportRepository
    {
        Task<IEnumerable<Airport>> ReadAllAirports(CancellationToken token);
        Task UpsertNewAirport(Airport airport, CancellationToken token);
        Task RemoveAirport(long airportId, CancellationToken token);
    }
}
