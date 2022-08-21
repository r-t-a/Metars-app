using Metars.Database;
using Metars.Repositories;
using Metars.Repositories.Interfaces;
using Metars.Services;
using Metars.Services.Interfaces;
using Metars.ViewModels;
using Metars.Views;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Crashes;
using Prism;
using Prism.Ioc;
using Prism.Unity;
using Xamarin.Forms.Xaml;

namespace Metars
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class App : PrismApplication
    {
        public App()
        {
            InitializeComponent();
        }

        public App(IPlatformInitializer initializer = null) : base(initializer) { }

        protected override async void OnInitialized()
        {
            var navigationService = Container.Resolve<IAppNavigationService>();
            navigationService.PrismNavigationService = NavigationService;

            var handleAppLifeCycleService = new HandleAppLifeCycleService(NavigationService,
                Container.Resolve<ILocalDatabase>(),
                navigationService);

            await handleAppLifeCycleService.OnAppLaunch();

            AppCenter.Start("ios=" + Constants.AppCenterAPIKeys.IOSAppCenterKey +
                            ";android=" + Constants.AppCenterAPIKeys.AndroidAppCenterKey,
                            typeof(Crashes));
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            //Services
            containerRegistry.Register<IBaseService, BaseService>();
            containerRegistry.Register<IHandleAppLifeCycleService, HandleAppLifeCycleService>();
            containerRegistry.Register<IAppNavigationService, AppNavigationService>();
            containerRegistry.Register<IAirportService, AirportService>();
            containerRegistry.Register<IRestService, RestService>();
            containerRegistry.Register<IStationService, StationService>();

            //Repositories
            containerRegistry.Register<IAirportRepository, AirportRepository>();
            containerRegistry.Register<IStationRepository, StationRepository>();

            //Navigation Pages
            containerRegistry.RegisterForNavigation<RootNavigation>();
            containerRegistry.RegisterForNavigation<AirportPage, AirportPageViewModel>();
            containerRegistry.RegisterForNavigation<AirportDetailPage, AirportDetailPageViewModel>();

            //Other
            containerRegistry.RegisterSingleton<ILocalDatabase, LocalDatabase>();
        }
    }
}
