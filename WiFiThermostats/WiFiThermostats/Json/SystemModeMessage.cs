using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace WiFiThermostats.Json
{
    [DataContract]
    public class SystemModeMessage
    {
        [DataMember(Name = "mode")]
        public SystemMode Mode { get; set; }
    }

    public enum SystemMode
    {
        Provisioning = 0,
        Normal = 1,
    }
}
