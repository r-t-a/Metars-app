using System.Threading.Tasks;
using Metars.Events;
using Metars.Models;
using Metars.Resources;
using Metars.Services.Interfaces;
using Prism.Events;
using Prism.Navigation;
using Prism.Services;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Metars.ViewModels
{
    public class AirportDetailPageViewModel : BaseNavigationViewModel<IAppNavigationService>
    {
        [Unity.Dependency]
        public IAirportService AirportService { get; set; }
        [Unity.Dependency]
        public IStationService StationService { get; set; }

        public Airport SelectedAirport { get; set; }
        public Station StationInfo { get; set; }

        public AirportDetailPageViewModel(INavigationService navigationService, IAppNavigationService teamNavigationService, IEventAggregator eventAggregator)
            : base(navigationService, teamNavigationService, eventAggregator) => AddEventSubscriptions(this);

        public override async void Initialize(INavigationParameters parameters)
        {
            base.Initialize(parameters);
            SelectedAirport = parameters.GetValue<Airport>(Constants.NavParams.SelectedAirport);
            StationInfo = await StationService.GetStationInfo(SelectedAirport.Name, new System.Threading.CancellationToken()).ConfigureAwait(false);
        }

        public override async void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
            if (!HasNetworkAccess())
            {
                DisplayError(Localization.Error_Occured, Localization.Check_Connection);
                return;
            }

            if (StationInfo == null)
                await AirportService.GetAirportDetails(SelectedAirport.Name, new System.Threading.CancellationToken()).ConfigureAwait(false);
        }

        protected override void AddEventSubscriptions(IEventSubscriber subscriber)
        {
            base.AddEventSubscriptions(subscriber);
            subscriber.Subscribe<StationResponseEvent, StationResult>(async (response) => await DislayMoreInfo(response));
        }

        private async Task DislayMoreInfo(StationResult stationResult)
        {
            if (stationResult.StatusCode != System.Net.HttpStatusCode.OK)
            {
                DisplayError(Localization.Error_Occured, stationResult.StatusCode.ToString());
                return;
            }

            StationInfo = Station.MapFromResponse(stationResult.StationResponse);
            if (StationInfo == null) return;
            await StationService.SaveStation(StationInfo, new System.Threading.CancellationToken()).ConfigureAwait(false);
        }
    }
}
