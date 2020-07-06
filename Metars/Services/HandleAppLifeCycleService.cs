using System.Threading.Tasks;
using Metars.Services.Interfaces;
using Prism.Navigation;

namespace Metars.Services
{
    public class HandleAppLifeCycleService : IHandleAppLifeCycleService
    {
        private readonly IAppNavigationService _appNavigationService;
        private readonly ILocalDatabase _localDatabase;

        public HandleAppLifeCycleService(INavigationService navigationService, ILocalDatabase localDatabase, IAppNavigationService appNavigationService)
        {
            _localDatabase = localDatabase;
            _appNavigationService = appNavigationService;
            _appNavigationService.PrismNavigationService = navigationService;
        }

        public async Task OnAppLaunch()
        {
            _localDatabase.InitializeDatabase();
            await _appNavigationService.StartNavigationStack();
        }
    }
}
