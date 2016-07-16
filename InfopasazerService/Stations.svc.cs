using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using InfopasazerMiner;

namespace InfopasazerService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Stations" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Stations.svc or Stations.svc.cs at the Solution Explorer and start debugging.
    public class Stations : IStations
    {
        public List<Station> Get(string pattern)
        {
            return InfopasazerMiner.InfopasazerMiner.GetStations(pattern).ToList();

        }
    }
}
