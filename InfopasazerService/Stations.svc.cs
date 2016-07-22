using System.Collections.Generic;
using System.Linq;
using InfopasazerMiner;

namespace InfopasazerService
{
    public class Stations : IStations
    {
        public List<Station> Get(string pattern)
        {
            return InfopasazerMiner.InfopasazerMiner.GetStations(pattern).ToList();

        }
        public Station GetStationForName(string name)
        {
            return InfopasazerMiner.InfopasazerMiner.GetStationForName(name);
        }


    }
}
