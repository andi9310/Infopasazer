using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using InfopasazerMiner;

namespace InfopasazerService
{
    [ServiceContract]
    public interface ITrains
    {
        [OperationContract]
        TrainGroup Get(int stationId);
    }
}
