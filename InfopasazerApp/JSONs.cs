using System.Runtime.Serialization;

namespace InfopasazerApp
{
    [DataContract]
    public class Response
    {
        [DataMember(Name = "value")]
        public Value[] Value;
    }

    [DataContract(Name = "value")]
    public class Value
    {
        [DataMember(Name = "contentUrl")]
        public string ContentUrl;
    }
}