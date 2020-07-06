using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Metars.Models;
using Metars.Repositories.Interfaces;
using Metars.Services.Interfaces;
using Microsoft.AppCenter.Crashes;
using Unity;

namespace Metars.Services
{
    public class StationService : BaseService, IStationService
    {
        [Dependency]
        public IStationRepository StationRepository { get; set; }

        public async Task SaveStation(Station station, CancellationToken token)
        {
            using (CancellationTokenSource linkedCts = CancellationTokenSource.CreateLinkedTokenSource(token, Token))
            {
                try
                {
                    await StationRepository.UpsertNewStation(station, linkedCts.Token).ConfigureAwait(false);
                }
                catch (OperationCanceledException e)
                {
                    Crashes.TrackError(e);
                }
            }
        }

        public async Task DeleteStation(string identifier, CancellationToken token)
        {
            using (CancellationTokenSource linkedCts = CancellationTokenSource.CreateLinkedTokenSource(token, Token))
            {
                try
                {
                    await StationRepository.RemoveStation(identifier, linkedCts.Token).ConfigureAwait(false);
                }
                catch (OperationCanceledException e)
                {
                    Crashes.TrackError(e);
                }
            }
        }

        public async Task<Station> GetStationInfo(string identifier, CancellationToken token)
        {
            using (CancellationTokenSource linkedCts = CancellationTokenSource.CreateLinkedTokenSource(token, Token))
            {
                try
                {
                    return await StationRepository.ReadCurrentStation(identifier, linkedCts.Token).ConfigureAwait(false);
                }
                catch (OperationCanceledException e)
                {
                    Crashes.TrackError(e);
                }
            }
            return null;
        }
    }
}
