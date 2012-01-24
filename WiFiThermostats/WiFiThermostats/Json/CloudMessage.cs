using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace WiFiThermostats.Json
{
    [DataContract]
    public class CloudMessage
    {
        [DataMember(Name = "authkey", EmitDefaultValue = false)]
        public string AuthenticationKey { get; set; }

        [DataMember(Name = "enabled", EmitDefaultValue = false)]
        public bool? Enabled { get; set; }

        [DataMember(Name = "interval", EmitDefaultValue = false)]
        public int? Interval { get; set; }

        [DataMember(Name = "status", EmitDefaultValue = false)]
        public bool? Status { get; set; }

        [DataMember(Name = "url", EmitDefaultValue = false)]
        public string Url { get; set; }
    }
}
