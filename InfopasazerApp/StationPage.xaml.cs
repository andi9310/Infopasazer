using System;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using InfopasazerApp.StationsServiceReference;
using InfopasazerApp.TrainsServiceReference;


namespace InfopasazerApp
{
    public sealed partial class StationPage
    {
        private Station _station;
        private TrainGroup _trains;
        public StationPage()
        {
            InitializeComponent();
        }

        private async Task RefreshTrains()
        {
            var client = new TrainsClient();
            _trains = await client.GetAsync(_station.Id);
            arrivals.ItemsSource = _trains.Arrivals;
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.Parameter == null) return;
            _station = (Station) e.Parameter;
            stationNameTextBlock.Text = _station.Name;
            var url = await BingApi.GetImageUrl(_station.Name);

            var refreshTask = RefreshTrains();

            Background = new ImageBrush
            {
                Stretch = Stretch.UniformToFill,
                ImageSource = new BitmapImage {UriSource = new Uri(url)}
            };
            await refreshTask;
        }
    }
}
