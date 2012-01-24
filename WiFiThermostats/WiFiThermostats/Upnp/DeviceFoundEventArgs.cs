using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;

namespace WiFiThermostats.Upnp
{
    public class DeviceFoundEventArgs : EventArgs
    {
        public EndPoint RemoteEndpoint { get; private set; }
        public Dictionary<string, string> Results { get; private set; }

        public DeviceFoundEventArgs(EndPoint remoteEndpoint, Dictionary<string, string> results)
        {
            RemoteEndpoint = remoteEndpoint;
            Results = results;
        }
    }
}
