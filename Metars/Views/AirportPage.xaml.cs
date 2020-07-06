using System;
using Metars.Models;
using Metars.ViewModels;
using Xamarin.Forms;

namespace Metars.Views
{
    public partial class AirportPage : ContentPage
    {
        private AirportPageViewModel ViewModel => BindingContext as AirportPageViewModel;

        public AirportPage()
        {
            InitializeComponent();
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();

            if (AirportList.SelectedItem != null)
                AirportList.SelectedItem = null;
        }

        public void OnRefresh(object sender, EventArgs e)
        {
            var mi = ((MenuItem)sender);
            var airport = mi.CommandParameter as Airport;
            ViewModel.RefreshAirportCommand.Execute(airport);
        }

        public void OnDelete(object sender, EventArgs e)
        {
            var mi = ((MenuItem)sender);
            var airport = mi.CommandParameter as Airport;
            ViewModel.DeleteAirportCommand.Execute(airport);
        }

        private void AirportListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item is Airport selectedItem)
                ViewModel.NavigateToAirportDetailPageCommand.Execute(selectedItem);
        }
    }
}
