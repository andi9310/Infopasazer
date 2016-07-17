using System;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using InfopasazerApp.InfopasazerServiceReference;


namespace InfopasazerApp
{
    public sealed partial class StationPage
    {
        private Station _station;
        public StationPage()
        {
            InitializeComponent();
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.Parameter == null) return;
            _station = (Station) e.Parameter;
            stationNameTextBlock.Text = _station.Name;
            var url = await BingApi.GetImageUrl(_station.Name);
            Background = new ImageBrush
            {
                Stretch = Stretch.UniformToFill,
                ImageSource = new BitmapImage {UriSource = new Uri(url)}
            };
        }
    }
}
