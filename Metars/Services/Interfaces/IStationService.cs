using System;
using System.Threading;
using System.Threading.Tasks;
using Metars.Models;

namespace Metars.Services.Interfaces
{
    public interface IStationService
    {
        Task SaveStation(Station station, CancellationToken token);
        Task DeleteStation(string identifier, CancellationToken token);
        Task<Station> GetStationInfo(string identifier, CancellationToken token);
    }
}
