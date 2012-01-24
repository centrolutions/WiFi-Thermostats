using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace WiFiThermostats.Json
{
    [DataContract]
    public class SystemFirmwareMessage
    {
        [DataMember(Name = "fw_version", EmitDefaultValue = false)]
        public Version FirmwareVersion { get; set; }

        [DataMember(Name = "api_version", EmitDefaultValue = false)]
        public int? ApiVersion { get; set; }
    }
}
