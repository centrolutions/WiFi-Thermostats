using System;
using System.Linq;
using WiFiThermostats.Upnp;
using System.Collections.ObjectModel;

namespace WiFiThermostats
{
    public class Discovery : IDisposable
    {
        const string WM_DISCOVERMessage = "TYPE: WM-DISCOVER\r\nVERSION: 1.0\r\n\r\nservices: com.marvell.wm.system*\r\n\r\n";
        ISsdp _Ssdp;

        public ObservableCollection<ThermostatBase> Results { get; private set; }

        public Discovery() : this(new Ssdp(WM_DISCOVERMessage))
        {
        }

        public Discovery(ISsdp ssdp)
        {
            Results = new ObservableCollection<ThermostatBase>();
            _Ssdp = ssdp;
            _Ssdp.DeviceFound += new EventHandler<DeviceFoundEventArgs>(Disco_DeviceFound);
        }

        public void BeginSearch()
        {
            _Ssdp.StartSearch();
        }

        public void EndSearch()
        {
            _Ssdp.StopSearch(SearchStoppedReason.Aborted);
        }

        void Disco_DeviceFound(object sender, DeviceFoundEventArgs e)
        {
            string location = string.Empty;
            string service = string.Empty;

            if (!e.Results.TryGetValue("SERVICE", out service))
                return;
            if (service.StartsWith("com.marvell.wm") && e.Results.TryGetValue("LOCATION", out location))
            {
                var newTstat = new ThermostatBase(location);
                var existingTstat = Results.Where(t => t.Equals(newTstat)).SingleOrDefault();
                if (existingTstat == null)
                    Results.Add(newTstat);
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_Ssdp != null)
                {
                    _Ssdp.Dispose();
                    _Ssdp = null;
                }
            }
        }
    }
}
