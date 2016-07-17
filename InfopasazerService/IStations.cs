using System.Collections.Generic;
using System.ServiceModel;
using InfopasazerMiner;

namespace InfopasazerService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "Stations" in both code and config file together.
    [ServiceContract]
    public interface IStations
    {
        [OperationContract]
        List<Station> Get(string pattern);
    }
}
