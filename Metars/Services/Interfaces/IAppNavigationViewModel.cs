using System;
using System.ComponentModel;
using Prism.Navigation;

namespace Metars.Services.Interfaces
{
    public interface IAppNavigationViewModel : INavigationAware, IDestructible, INotifyPropertyChanged, IInitialize, IEventSubscriber
    {
        INavigationService PrismNavigationService { get; set; }
    }
}
