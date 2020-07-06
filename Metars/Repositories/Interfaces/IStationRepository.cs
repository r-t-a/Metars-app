using System;
using System.Threading;
using System.Threading.Tasks;
using Metars.Models;

namespace Metars.Repositories.Interfaces
{
    public interface IStationRepository
    {
        Task<Station> ReadCurrentStation(string identifier, CancellationToken token);
        Task UpsertNewStation(Station station, CancellationToken token);
        Task RemoveStation(string identifier, CancellationToken token);
    }
}
