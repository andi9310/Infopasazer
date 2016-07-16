using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Devices.Geolocation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Services.Maps;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace InfopasazerApp
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage
    {
        private GeolocationAccessStatus _accessStatus = GeolocationAccessStatus.Denied;
        public string City { get; set; } = "";
        public MainPage()
        {
            InitializeComponent();
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
            textBlock.Text = City;
            var x = new InfopasazerServiceReference.StationsClient();

            var f = await x.GetAsync(City);
            var s = f.Aggregate("", (current, ss) => current + ss.Name);
            listView.ItemsSource = f.Select(elem => elem.Name);

        }
        private async void button_Click(object sender, RoutedEventArgs e)
        {
            var x = new InfopasazerServiceReference.StationsClient();

            var f = await x.GetAsync(textBox.Text);
            var s = f.Aggregate("", (current, ss) => current + ss.Name);
            listView.ItemsSource = f.Select(elem => elem.Name);
        }
    }
}
