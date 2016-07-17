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

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.Parameter == null) return;
            _station = (Station) e.Parameter;
            stationNameTextBlock.Text = _station.Name;
        }
    }
}
