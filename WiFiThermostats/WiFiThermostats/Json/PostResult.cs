using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace WiFiThermostats.Json
{
    [DataContract]
    public class PostResult
    {
        [DataMember(Name = "success")]
        public bool? Success { get; set; }
    }
}
