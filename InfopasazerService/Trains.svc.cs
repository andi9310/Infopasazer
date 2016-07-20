using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using InfopasazerMiner;

namespace InfopasazerService
{
    public class Trains : ITrains
    {
        public TrainGroup Get(int stationId)
        {
            return InfopasazerMiner.InfopasazerMiner.GetTrainsForStation(stationId);
        }
    }
}
