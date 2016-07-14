using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace InfopasazerService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "GetStations" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select GetStations.svc or GetStations.svc.cs at the Solution Explorer and start debugging.
    public class GetStations : IGetStations
    {
        public List<string> DoWork(string pattern)
        {
            var x = new InfopasazerMiner.Class1();

            return new List<string>() { "aaa", "bbb", pattern, x.X };
        }
    }
}
