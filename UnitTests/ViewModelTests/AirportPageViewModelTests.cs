using System;
using System.Threading.Tasks;
using Metars.Models;
using Metars.Services.Interfaces;
using Metars.ViewModels;
using NSubstitute;
using NUnit.Framework;
using Prism.Events;
using Prism.Navigation;
using Xamarin.Forms;

namespace UnitTests.ViewModelTests
{
    [TestFixture]
    public class AirportPageViewModelTests
    {
        private AirportPageViewModel ViewModel;

        [SetUp]
        public void SetupVM()
        {
            MockForms.Init();
            ViewModel = new AirportPageViewModel(Substitute.For<INavigationService>(), Substitute.For<IAppNavigationService>(), Substitute.For<IEventAggregator>());
        }

        [Test]
        public void GetMetarDoesntRunIfEmptyEntry()
        {
            ViewModel.AirportCode = string.Empty;
            ViewModel.GetMetarCommand.Execute(null);

            Assert.IsFalse(ViewModel.IsBusy);
            Assert.AreEqual(ViewModel.Airports.Count, 0);
        }

        [Test]
        public void GetMetarAddsToSavedList()
        {
            ViewModel.AirportService = Substitute.For<IAirportService>(); ;

            ViewModel.AirportCode = "KUAO";
            ViewModel.GetMetarCommand.Execute(null);

            Assert.IsTrue(ViewModel.IsBusy);
        }

        [Test]
        public async Task NavigateToDetailsPage()
        {
            var airport = new Airport() { Name = "test" };
            ViewModel.NavigateToAirportDetailPageCommand.Execute(airport);

            await ViewModel.AppNavigationService.Received(1).NavigateToAirportDetails(airport);
        }

        [Test]
        public void EmptyViewShowsIfNoSavedSearches()
        {
            ViewModel.Airports = null;
            ViewModel.OnNavigatedTo(null);
            Assert.IsFalse(ViewModel.HasSavedItems);
        }

        [Test]
        public void EmptyViewDoesntShowsIfSavedSearches()
        {
            var airport = new Airport() { Name = "test" };
            ViewModel.Airports = new System.Collections.ObjectModel.ObservableCollection<Airport>
            {
                airport
            };
            ViewModel.OnNavigatedTo(null);
            Assert.IsTrue(ViewModel.HasSavedItems);
        }
    }
}
