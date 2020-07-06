using System;
using System.Threading.Tasks;
using Metars.Models;
using Prism.Navigation;

namespace Metars.Services.Interfaces
{
    public interface IAppNavigationService
    {
        INavigationService PrismNavigationService { get; set; }
        Task StartNavigationStack();
        Task NavigateToAirportDetails(Airport airport);
    }
}
