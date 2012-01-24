using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WiFiThermostats.Upnp
{
    public enum SearchStoppedReason
    {
        Aborted,
        Complete,
        Error,
        TimedOut
    }
}
