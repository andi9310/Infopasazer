using System.Collections.Generic;
using System.ServiceModel;
using InfopasazerMiner;

namespace InfopasazerService
{
    [ServiceContract]
    public interface ITrains
    {
        [OperationContract]
        TrainGroup Get(int stationId);

        [OperationContract]
        IEnumerable<TrainStation> GetTrain(int trainId);
    }
}
