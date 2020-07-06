using System;
using System.Collections.Generic;
using System.ComponentModel;
using Metars.Resources;
using Metars.Services.Interfaces;
using Prism.Events;
using Prism.Navigation;
using Prism.Services;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Metars.ViewModels
{
    public abstract class BaseNavigationViewModel<TNavigation> : IAppNavigationViewModel where TNavigation : class, IAppNavigationService
    {
        [Unity.Dependency]
        public IPageDialogService PageDialogService { get; set; }

        public IEventAggregator EventAggregator { get; set; }

        public TNavigation AppNavigationService { get; }

        public INavigationService PrismNavigationService
        {
            get => AppNavigationService.PrismNavigationService;
            set => AppNavigationService.PrismNavigationService = value;
        }

        private readonly List<SubscriptionToken> _subscriptionTokens = new List<SubscriptionToken>();


        protected BaseNavigationViewModel(INavigationService navigationService, TNavigation teamNavigationService, IEventAggregator eventAggregator)
        {
            AppNavigationService = teamNavigationService;
            AppNavigationService.PrismNavigationService = navigationService;
            EventAggregator = eventAggregator;
        }

        public virtual void Initialize(INavigationParameters parameters)
        {
        }

        public virtual void OnNavigatedFrom(INavigationParameters parameters)
        {
        }

        public virtual void OnNavigatedTo(INavigationParameters parameters)
        {
        }

        protected virtual void AddEventSubscriptions(IEventSubscriber subscriber)
        {
        }

        protected bool IsBackNavigation(INavigationParameters navParams)
        {
            try
            {
                return navParams.GetNavigationMode() == Prism.Navigation.NavigationMode.Back;
            }
            catch (Exception)
            {
                return true;
            }
        }

        public bool HasNetworkAccess()
        {
            return Connectivity.NetworkAccess == NetworkAccess.Internet;
        }

        public void DisplayError(string title, string message)
        {
            Device.BeginInvokeOnMainThread(async () => {
                await PageDialogService.DisplayAlertAsync(title, message, Localization.Ok);
            });
        }

        public void Destroy()
        {
            _subscriptionTokens.ForEach(token => token.Dispose());
            _subscriptionTokens.Clear();
        }

        public void Subscribe<TEvent, TPayload>(Action<TPayload> action, ThreadOption? option = null) where TEvent : PubSubEvent<TPayload>, new()
        {
            var target = EventAggregator.GetEvent<TEvent>();
            var token = option.HasValue ? target?.Subscribe(action, option.Value) : target?.Subscribe(action);

            if (token != null)
                _subscriptionTokens.Add(token);
        }

        public void Subscribe<TEvent>(Action action, ThreadOption? option = null) where TEvent : PubSubEvent, new()
        {
            var target = EventAggregator.GetEvent<TEvent>();
            var token = option.HasValue ? target?.Subscribe(action, option.Value) : target?.Subscribe(action);

            if (token != null)
                _subscriptionTokens.Add(token);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}