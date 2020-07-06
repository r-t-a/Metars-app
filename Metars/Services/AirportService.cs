using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Metars.Events;
using Metars.Models;
using Metars.Models.Responses;
using Metars.Repositories.Interfaces;
using Metars.Services.Interfaces;
using Microsoft.AppCenter.Crashes;
using Prism.Events;
using Unity;

namespace Metars.Services
{
    public class AirportService : BaseService, IAirportService
    {
        [Dependency]
        public IEventAggregator EventAggregator { get; set; }
        [Dependency]
        public IRestService RestService { get; set; }
        [Dependency]
        public IAirportRepository AirportRepository { get; set; }

        public async Task<List<Airport>> GetAllAirports(CancellationToken token)
        {
            var airports = new List<Airport>();
            using (CancellationTokenSource linkedCts = CancellationTokenSource.CreateLinkedTokenSource(token, Token))
            {
                try
                {
                    var airportList = await AirportRepository.ReadAllAirports(linkedCts.Token).ConfigureAwait(false);
                    if (airportList == null || !airportList.Any()) return airports;
                    airports = airportList.ToList();
                }
                catch (OperationCanceledException)
                {
                    // TODO: Log the error to AppCenter
                }
            }
            return airports;
        }

        public async Task SaveAirport(Airport airport, CancellationToken token)
        {
            using (CancellationTokenSource linkedCts = CancellationTokenSource.CreateLinkedTokenSource(token, Token))
            {
                try
                {
                    await AirportRepository.UpsertNewAirport(airport, linkedCts.Token).ConfigureAwait(false);
                }
                catch (OperationCanceledException e)
                {
                    Crashes.TrackError(e);
                }
            }
        }

        public async Task DeleteAirport(long id, CancellationToken token)
        {
            using (CancellationTokenSource linkedCts = CancellationTokenSource.CreateLinkedTokenSource(token, Token))
            {
                try
                {
                    await AirportRepository.RemoveAirport(id, linkedCts.Token).ConfigureAwait(false);
                }
                catch (OperationCanceledException e)
                {
                    Crashes.TrackError(e);
                }
            }
        }

        public async Task GetAirportMetar(string airportCode, CancellationToken token)
        {
            using (CancellationTokenSource linkedCts = CancellationTokenSource.CreateLinkedTokenSource(token, Token))
            {
                try
                {
                    var request = $"{Constants.APIConstants.GetMetar}/{airportCode}";
                    var result = await RestService.Post<MetarResponse>(request, token).ConfigureAwait(false);
                    EventAggregator.GetEvent<MetarResponseEvent>().Publish(new MetarResult()
                    {
                        MetarResponse = result.Response,
                        StatusCode = result.StatusCode,
                        AirportIdentifier = airportCode
                    });
                }
                catch (OperationCanceledException e)
                {
                    Crashes.TrackError(e);
                }
            }
        }

        public async Task GetAirportDetails(string airportCode, CancellationToken token)
        {
            using (CancellationTokenSource linkedCts = CancellationTokenSource.CreateLinkedTokenSource(token, Token))
            {
                try
                {
                    var request = $"{Constants.APIConstants.GetStation}/{airportCode}";
                    var result = await RestService.Post<StationResponse>(request, token).ConfigureAwait(false);
                    EventAggregator.GetEvent<StationResponseEvent>().Publish(new StationResult()
                    {
                        StationResponse = result.Response,
                        StatusCode = result.StatusCode,
                    });
                }
                catch (OperationCanceledException e)
                {
                    Crashes.TrackError(e);
                }
            }
        }
    }
}
