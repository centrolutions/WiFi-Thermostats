using System.Runtime.Serialization;

namespace WiFiThermostats.Json
{
    [DataContract]
    public class ModelMessage
    {
        [DataMember(Name = "model")]
        public string Model { get; set; }
    }
}
