using System;
using System.Collections.Generic;
using System.Globalization;
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
    public sealed partial class TrainPage
    {
        public Train MyTrain { get; set; }
        private IEnumerable<AppTrainStation> _trainDetails;
        public TrainPage()
        {
            var dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Tick += dispatcherTimer_Tick;
            dispatcherTimer.Interval = new TimeSpan(0, 1, 0);
            dispatcherTimer.Start();

            InitializeComponent();
        }
        private async void dispatcherTimer_Tick(object sender, object e)
        {
            await RefreshTrain();
        }

        private async Task RefreshTrain()
        {
            var client = new TrainsClient();
            var trainDetailsTask = client.GetTrainAsync(MyTrain.Id);

            trainNameTextBlock.Text = MyTrain.Name;
            companyTextBlock.Text = MyTrain.Company;
            dateTextBlock.Text = MyTrain.Date;
            relationTextBlock.Text = MyTrain.Source + " - " + MyTrain.Destination;

            var foundCurrent = false;
            var trainDetails = (await trainDetailsTask).Select(elem => new AppTrainStation(elem)).ToArray();
            for (var i = 0; i < trainDetails.Count(); i++)
            {
                if (!foundCurrent)
                {
                    string plannedTime;
                    string delay;
                    if (trainDetails[i].PlanedArrival == "")
                    {
                        plannedTime = trainDetails[i].PlannedDeparture;
                        delay = trainDetails[i].DepartureDelay;

                    }
                    else
                    {
                        plannedTime = trainDetails[i].PlanedArrival;
                        delay = trainDetails[i].ArrivalDelay;
                    }
                    var timeOnStation = DateTime.ParseExact(plannedTime, "HH:mm:ss", CultureInfo.InvariantCulture).TimeOfDay.Add(new TimeSpan(0, int.Parse(delay.Split(' ')[0]), 0));
                    var current = DateTime.Now.TimeOfDay;
                    if (current < timeOnStation)
                    {
                        trainDetails[i].BorderThickness = "5";
                        foundCurrent = true;
                    }
                }
                trainDetails[i].SetColor(foundCurrent);
            }
            _trainDetails = trainDetails;
            stationsList.ItemsSource = _trainDetails;
        }

        private async Task SetBackground()
        {
            var url = await BingApi.GetCompanyImage(MyTrain.Company);

            if (url != null)
            {
                Background = new ImageBrush
                {
                    Stretch = Stretch.UniformToFill,
                    ImageSource = new BitmapImage { UriSource = new Uri(url) }
                };
            }
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.Parameter == null) return;
            MyTrain = (Train)e.Parameter;

            var refreshTask = RefreshTrain();
            var backgroundTask = SetBackground();
            
            await refreshTask;
            await backgroundTask;
        }

        private async void StationName_OnTapped(object sender, TappedRoutedEventArgs e)
        {
            var station = (((e.OriginalSource as TextBlock)?.Parent as Border)?.Parent as Grid)?.DataContext as AppTrainStation;
            if (station == null)
            {
                return;
            }
            var client = new StationsClient();
            var stationToGo = await client.GetStationForNameAsync(station.Name);
            Frame.Navigate(typeof(StationPage), stationToGo);
        }

        private async void Company_OnTapped(object sender, TappedRoutedEventArgs e)
        {
            await Launcher.LaunchUriAsync(new Uri(MyTrain.CompanyUrl));
        }

        private void mainPageButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(MainPage));
        }
    }
}
