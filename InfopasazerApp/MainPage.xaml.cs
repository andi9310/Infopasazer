using System;
using System.Collections.Generic;
using System.Linq;
using Windows.Devices.Geolocation;
using Windows.Services.Maps;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using InfopasazerApp.InfopasazerServiceReference;

namespace InfopasazerApp
{
    public sealed partial class MainPage
    {
        private GeolocationAccessStatus _accessStatus = GeolocationAccessStatus.Denied;
        public string City { get; set; } = string.Empty;
        private List<Station> _stations; 
        public MainPage()
        {
            InitializeComponent();
            NavigationCacheMode = NavigationCacheMode.Required;
            RequestAccess();
            RefreshCity();
        }

        private async void RequestAccess()
        {
            _accessStatus = await Geolocator.RequestAccessAsync();
        }

        private async void RefreshCity()
        {
            if (_accessStatus != GeolocationAccessStatus.Allowed) return;
            var geolocator = new Geolocator();
            var position = await geolocator.GetGeopositionAsync();

            var myLocation = new BasicGeoposition
            {
                Longitude = position.Coordinate.Longitude,
                Latitude = position.Coordinate.Latitude
            };
            var pointToReverseGeocode = new Geopoint(myLocation);

            var result = await MapLocationFinder.FindLocationsAtAsync(pointToReverseGeocode);

            if (result?.Locations == null || result.Locations.Count < 1 || result.Locations[0].Address?.Town == null)
            {
                City = "";
                return;
            }
            City = result.Locations[0].Address.Town;
            currentCityTextBlock.Text = City;
            var x = new StationsClient();

            _stations = (await x.GetAsync(City)).ToList();
            
            stationsListView.ItemsSource = _stations.Select(elem => elem.Name);

        }
        private async void showStations_Click(object sender, RoutedEventArgs e)
        {
            var x = new StationsClient();
            _stations = (await x.GetAsync(textBox.Text)).ToList();
            stationsListView.ItemsSource = _stations.Select(elem => elem.Name);
        }

        private void refreshCity_Click(object sender, RoutedEventArgs e)
        {
            RefreshCity();
        }

        private void stationsListView_OnItemClick(object sender, ItemClickEventArgs e)
        {
            var clickedStation = _stations.Find(x => (string) e.ClickedItem == x.Name);
            Frame.Navigate(typeof(StationPage), clickedStation);
        }
    }
}
