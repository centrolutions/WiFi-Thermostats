using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WiFiThermostats.Upnp
{
    public class SearchStoppedEventArgs : EventArgs
    {
        public SearchStoppedReason Reason { get; private set; }

        public SearchStoppedEventArgs(SearchStoppedReason reason)
        {
            Reason = reason;
        }
    }
}
