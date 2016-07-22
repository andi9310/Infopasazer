using System.Collections.Generic;
using InfopasazerMiner;

namespace InfopasazerService
{
    public class Trains : ITrains
    {
        public TrainGroup Get(int stationId)
        {
            return InfopasazerMiner.InfopasazerMiner.GetTrainsForStation(stationId);
        }
        public IEnumerable<TrainStation> GetTrain(int trainId)
        {
            return InfopasazerMiner.InfopasazerMiner.GetTrainDetails(trainId);
        }
    }
}
