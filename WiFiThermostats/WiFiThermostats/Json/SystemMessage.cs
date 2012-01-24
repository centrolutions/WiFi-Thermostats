using System.Runtime.Serialization;

namespace WiFiThermostats.Json
{
    [DataContract]
    public class SystemMessage
    {
        [DataMember(Name = "api_version")]
        public int ApiVersion { get; set; }

        [DataMember(Name = "fw_version")]
        public string FirmwareVersion { get; set; }

        [DataMember(Name = "uuid")]
        public string UniqueIdentifier { get; set; }

        [DataMember(Name = "wlan_fw_version")]
        public string WirelessFirmwareVersion { get; set; }
    }
}
