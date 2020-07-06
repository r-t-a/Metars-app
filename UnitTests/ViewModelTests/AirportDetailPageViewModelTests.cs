using System;
using Metars;
using Metars.Models;
using Metars.Services.Interfaces;
using Metars.ViewModels;
using NSubstitute;
using NUnit.Framework;
using Prism.Events;
using Prism.Navigation;

namespace UnitTests.ViewModelTests
{
    [TestFixture]
    public class AirportDetailPageViewModelTests
    {
        private AirportDetailPageViewModel ViewModel;

        [SetUp]
        public void SetupVM()
        {
            ViewModel = new AirportDetailPageViewModel(Substitute.For<INavigationService>(), Substitute.For<IAppNavigationService>(), Substitute.For<IEventAggregator>());
        }

        [Test]
        public void TestSelectedAirportInNavParams()
        {
            var airport = new Airport() { Name = "Test" };
            var navParams = new NavigationParameters()
            {
                {Constants.NavParams.SelectedAirport, airport }
            };
            ViewModel.Initialize(navParams);

            Assert.AreEqual(ViewModel.SelectedAirport.Name, airport.Name);
        }
    }
}
