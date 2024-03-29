﻿using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Metars.Events;
using Metars.Helpers;
using Metars.Models;
using Metars.Resources;
using Metars.Services.Interfaces;
using Prism.Commands;
using Prism.Events;
using Prism.Navigation;
using Xamarin.Forms;

namespace Metars.ViewModels
{
    public class AirportPageViewModel : BaseNavigationViewModel<IAppNavigationService>
    {
        [Unity.Dependency]
        public IAirportService AirportService { get; set; }

        public ObservableCollection<Airport> Airports { get; set; } = new ObservableCollection<Airport>();

        public string AirportCode { get; set; }
        public string FilterText { get; set; }
        public bool IsBusy { get; set; }
        public bool HasSavedItems { get; set; }

        private DelegateCommand _getMetarCommand;
        public ICommand GetMetarCommand { get { return _getMetarCommand ?? (_getMetarCommand = new DelegateCommand(async () => await GetMetar().ConfigureAwait(false))); } }

        private DelegateCommand<Airport> _navigateToAirportDetailPageCommand;
        public ICommand NavigateToAirportDetailPageCommand { get { return _navigateToAirportDetailPageCommand ?? (_navigateToAirportDetailPageCommand = new DelegateCommand<Airport>(async (airport) => await NavigateToDetails(airport).ConfigureAwait(false))); } }

        private DelegateCommand<Airport> _refreshAirportCommand;
        public ICommand RefreshAirportCommand { get { return _refreshAirportCommand ?? (_refreshAirportCommand = new DelegateCommand<Airport>(async (airport) => await RefreshAirport(airport))); } }

        private DelegateCommand<Airport> _deleteAirportCommand;
        public ICommand DeleteAirportCommand { get { return _deleteAirportCommand ?? (_deleteAirportCommand = new DelegateCommand<Airport>(async (airport) => await DeleteAirport(airport))); } }

        private DelegateCommand _sortCommand;
        public ICommand SortCommand { get { return _sortCommand ?? (_sortCommand = new DelegateCommand(async () => await SortList())); } }

        private DelegateCommand _filterCommand;
        public ICommand FilterCommand { get { return _filterCommand ?? (_filterCommand = new DelegateCommand(async() => await FilterAirports())); } }

        public AirportPageViewModel(INavigationService navigationService, IAppNavigationService teamNavigationService, IEventAggregator eventAggregator)
            : base(navigationService, teamNavigationService, eventAggregator) => AddEventSubscriptions(this);

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
            LoadSavedAirports();
        }

        protected override void AddEventSubscriptions(IEventSubscriber subscriber)
        {
            base.AddEventSubscriptions(subscriber);
            subscriber.Subscribe<MetarResponseEvent, MetarResult>(async (response) => await UpdateMetarInfo(response));
        }

        private void LoadSavedAirports()
        {
            Device.BeginInvokeOnMainThread(async () =>
            {
                var airports = await AirportService.GetAllAirports(new System.Threading.CancellationToken()).ConfigureAwait(false);
                foreach (var airport in airports)
                {
                    var item = Airports.FirstOrDefault(x => x.Name == airport.Name);
                    if (item != null) continue;

                    Airports.Add(airport);
                }
                HasSavedItems = Airports != null && Airports.Any();
            });
        }

        private async Task GetMetar()
        {
            if (!HasNetworkAccess())
            {
                DisplayError(Localization.Error_Occured, Localization.Check_Connection);
                return;
            }

            if (string.IsNullOrWhiteSpace(AirportCode) || IsBusy) return;
            if (!AirportCode.ToUpper().StartsWith(Constants.IdentifierChar))
                AirportCode = AirportCode.Insert(0, Constants.IdentifierChar);

            IsBusy = true;
            await AirportService.GetAirportMetar(AirportCode.ToUpper(), new System.Threading.CancellationToken()).ConfigureAwait(false);
        }

        private async Task RefreshAirport(Airport airport)
        {
            if (IsBusy) return;

            IsBusy = true;
            await AirportService.GetAirportMetar(airport.Name.ToUpper(), new System.Threading.CancellationToken()).ConfigureAwait(false);
        }

        private async Task UpdateMetarInfo(MetarResult response)
        {
            IsBusy = false;
            AirportCode = string.Empty;
            if (response.StatusCode != System.Net.HttpStatusCode.OK)
            {
                DisplayError(Localization.Error_Occured, response.StatusCode.ToString());
                return;
            }

            var airport = Airport.MapFromResponse(response.AirportIdentifier, response.MetarResponse);
            var alreadyExists = Airports.FirstOrDefault(x => x.Name == airport.Name);
            if(alreadyExists == null)
            {
                Airports.Add(airport);
            }
            else
            {
                alreadyExists.Copy(airport);
                OnPropertyChanged(nameof(Airports));
            }
            HasSavedItems = Airports != null && Airports.Any();
            await AirportService.SaveAirport(airport, new System.Threading.CancellationToken()).ConfigureAwait(false);
        }

        private async Task DeleteAirport(Airport airport)
        {
            if (Airports.Any() && Airports.Contains(airport))
            {
                await AirportService.DeleteAirport(airport.Id, new System.Threading.CancellationToken()).ConfigureAwait(false);
                Airports.Remove(airport);
            }
            HasSavedItems = Airports != null && Airports.Any();
        }

        private async Task SortList()
        {
            var result = await SafeUserDialogs.Instance.ShowActionSheetAsync(null, null, new[] { "Default", "Name" });

            if (result == "Default")
                Airports = new ObservableCollection<Airport>(Airports.OrderBy(x => x.Id).ToList());

            if (result == "Name")
                Airports = new ObservableCollection<Airport>(Airports.OrderBy(x => x.Name).ToList());
        }

        private async Task FilterAirports()
        {
            var airports = await AirportService.GetAllAirports(new System.Threading.CancellationToken()).ConfigureAwait(false);
            Airports = !string.IsNullOrWhiteSpace(FilterText)
                ? new ObservableCollection<Airport>(airports.Where(x => x.Name.Trim().ToUpper().Contains(FilterText.Trim().ToUpper())).ToList())
                : new ObservableCollection<Airport>(airports);
        }

        private async Task NavigateToDetails(Airport airport)
        {
            if (airport == null) return;
            await AppNavigationService.NavigateToAirportDetails(airport).ConfigureAwait(false);
        }
    }
}
