using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.UI.Xaml.Navigation;
using InfopasazerApp.TrainsServiceReference;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace InfopasazerApp
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class TrainPage
    {
        private Train _train;
        private IEnumerable<TrainStation> _trainDetails;
        public TrainPage()
        {
            InitializeComponent();
        }

        private async Task RefreshTrain()
        {
            var client = new TrainsClient();
            _trainDetails = await client.GetTrainAsync(_train.Id);
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.Parameter == null) return;
            _train = (Train)e.Parameter;
            await RefreshTrain();
        }
    }
}
