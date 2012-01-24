using System.Runtime.Serialization;

namespace WiFiThermostats.Json
{
    [DataContract]
    public class NameMessage
    {
        [DataMember(Name = "name")]
        public string Name { get; set; }
    }
}
