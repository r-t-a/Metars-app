using System;
using System.Threading.Tasks;
using Metars.Models;
using Metars.Services.Interfaces;
using Prism.Navigation;

namespace Metars.Services
{
    public class AppNavigationService : IAppNavigationService
    {
        public INavigationService PrismNavigationService { get; set; }

        public string GetNavigationPath()
        {
            return PrismNavigationService.GetNavigationUriPath();
        }

        public async Task Pop(bool modalNav = false)
        {
            await PrismNavigationService.GoBackAsync(useModalNavigation: modalNav);
        }

        public async Task StartNavigationStack()
        {
            await PrismNavigationService.NavigateAsync($"{Constants.NavigationPages.RootNavigation}/{Constants.NavigationPages.AirportPage}");
        }

        public async Task NavigateToAirportDetails(Airport airport)
        {
            var navParams = new NavigationParameters
            {
                { Constants.NavParams.SelectedAirport, airport }
            };

            var x =await PrismNavigationService.NavigateAsync(Constants.NavigationPages.AirportDetailPage, navParams);
        }
    }
}
