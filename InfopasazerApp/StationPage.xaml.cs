﻿using System;
using System.Linq;
using System.Threading.Tasks;
using Windows.System;
using Windows.UI.Xaml;
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
            var dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Tick += dispatcherTimer_Tick;
            dispatcherTimer.Interval = new TimeSpan(0, 1, 0);
            dispatcherTimer.Start();

            InitializeComponent();
        }

        private async void dispatcherTimer_Tick(object sender, object e)
        {
            await RefreshTrains();
        }

        private async Task RefreshTrains()
        {
            var client = new TrainsClient();
            _trains = await client.GetAsync(_station.Id);
            trainsList.ItemsSource = _showDepartures ? _trains.Departures.Select(x => new AppTrain(x)) : _trains.Arrivals.Select(x => new AppTrain(x));
        }

        private async Task SetBackground()
        {
            var url = await BingApi.GetStationImage(_station.Name);

            if (url != null)
            {
                Background = new ImageBrush
                {
                    Stretch = Stretch.UniformToFill,
                    ImageSource = new BitmapImage {UriSource = new Uri(url)}
                };
            }
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.Parameter == null) return;
            _station = (Station)e.Parameter;
            if (_station.Id == -1)
            {
                return;
            }
            var refreshTask = RefreshTrains();
            var backgroundTask = SetBackground();
            stationNameTextBlock.Text = _station.Name;
            await refreshTask;
            await backgroundTask;
        }

        private async void Source_OnTapped(object sender, TappedRoutedEventArgs e)
        {
            var train = (((e.OriginalSource as TextBlock)?.Parent as Border)?.Parent as Grid)?.DataContext as AppTrain;
            if (train == null)
            {
                return;
            }
            var client = new StationsClient();
            var station = await client.GetStationForNameAsync(train.Source);
            Frame.Navigate(typeof(StationPage), station);
        }

        private async void Mode_Click(object sender, RoutedEventArgs e)
        {
            ((Button) e.OriginalSource).Content = _showDepartures ? "Przyjazdy" : "Odjazdy";
            _showDepartures = !_showDepartures;
            await RefreshTrains();
        }

        private async void Destination_OnTapped(object sender, TappedRoutedEventArgs e)
        {
            var train = (((e.OriginalSource as TextBlock)?.Parent as Border)?.Parent as Grid)?.DataContext as AppTrain;
            if (train == null)
            {
                return;
            }
            var client = new StationsClient();
            var station = await client.GetStationForNameAsync(train.Destination);
            Frame.Navigate(typeof(StationPage), station);
        }

        private void Name_OnTapped(object sender, TappedRoutedEventArgs e)
        {
            var train = (((e.OriginalSource as TextBlock)?.Parent as Border)?.Parent as Grid)?.DataContext as AppTrain;
            if (train == null)
            {
                return;
            }
            Frame.Navigate(typeof(TrainPage), train);
        }

        private async void Company_OnTapped(object sender, TappedRoutedEventArgs e)
        {
            var train = (((e.OriginalSource as TextBlock)?.Parent as Border)?.Parent as Grid)?.DataContext as AppTrain;
            if (train == null)
            {
                return;
            }
            await Launcher.LaunchUriAsync(new Uri(train.CompanyUrl));
        }

        private void mainPageButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(MainPage));
        }
    }
}
