using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WiFiThermostats.Upnp
{
    public interface ISsdp : IDisposable
    {
        void Cleanup();
        void StartSearch();
        void StopSearch(SearchStoppedReason reason);
        event EventHandler<SearchStoppedEventArgs> SearchStopped;
        event EventHandler<EventArgs> SearchStarted;
        event EventHandler<DeviceFoundEventArgs> DeviceFound;
    }
}
