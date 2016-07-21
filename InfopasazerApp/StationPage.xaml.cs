using System;
using System.Linq;
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
        private bool _showDepartures;
        public StationPage()
        {
            InitializeComponent();
        }

        private async Task RefreshTrains()
        {
            var client = new TrainsClient();
            _trains = await client.GetAsync(_station.Id);
            trainsList.ItemsSource = _showDepartures ? _trains.Departures.Select(x => new AppTrain(x)) : _trains.Arrivals.Select(x => new AppTrain(x));
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.Parameter == null) return;
            _station = (Station)e.Parameter;
            stationNameTextBlock.Text = _station.Name;
            var url = await BingApi.GetImageUrl(_station.Name);

            var refreshTask = RefreshTrains();

            Background = new ImageBrush
            {
                Stretch = Stretch.UniformToFill,
                ImageSource = new BitmapImage { UriSource = new Uri(url) }
            };
            await refreshTask;
        }

        private void Source_OnTapped(object sender, TappedRoutedEventArgs e)
        {
            var train = (((e.OriginalSource as TextBlock)?.Parent as Border)?.Parent as Grid)?.DataContext as AppTrain;
            if (train == null)
            {
                return;
            }
        }

        private async void Mode_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            ((Button) e.OriginalSource).Content = _showDepartures ? "Przyjazdy" : "Odjazdy";
            _showDepartures = !_showDepartures;
            await RefreshTrains();
        }
    }
}
