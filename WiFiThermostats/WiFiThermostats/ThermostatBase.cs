using System;
using System.Text;
using System.IO;
using System.Net;
using WiFiThermostats.Json;
using System.Runtime.Serialization.Json;

namespace WiFiThermostats
{
    /* Path                         GET     POST    GET IMPLEMENTED     POST IMPLEMENTED
     * /cloud                       X       X       X                   X
     * /sys                         X               X
     * /sys/command                         X                           X
     * /sys/diag/log                X               ? submitted to forum
     * /sys/diag/stats/history      X               ? submitted to forum
     * /sys/diag/stats/live         X               ? submitted to forum
     * /sys/filesystem                      X                           scary; don't want to implement, yet
     * /sys/firmware                X       X       X                   scary; don't want to implement, yet
     * /sys/fs-image                        X                           scary; don't want to implement, yet
     * /sys/fw-image                        X                           scary; don't want to implement, yet
     * /sys/mode                    X       X       
     * /sys/name                    X       X       X                   X
     * /sys/network                 X       X
     * /sys/services                X
     * /sys/updater                         X
     * /sys/watchdog                X       X
     * /tstat                       X       X       X                   X
     * /tstat/version               X 
     * /tstat/program/heat          X       X       X
     * /tstat/program/cool          X       X       X
     * /tstat/program/heat/<day>    X       X
     * /tstat/program/cool/<day>    X       X
     * /tstat/model                 X               X
     * /tstat/led                           X
     * /tstat/pma                           X
     * /tstat/uma                           X
     * /tstat/help                  X
     * /tstat/datalog               X
     */
    public class ThermostatBase
    {
        public Uri BaseUri { get; private set; }

        IClient _JsonClient;

        #region Service URLs
        string CloudUrl
        {
            get
            {
                return Path.Combine(BaseUri.ToString(), "cloud");
            }
        }

        string SysUrl
        {
            get
            {
                return Path.Combine(BaseUri.ToString(), "sys");
            }
        }

        string SysNameUrl
        {
            get
            {
                return Path.Combine(BaseUri.ToString(), "sys/name");
            }
        }

        string SysCommandUrl
        {
            get
            {
                return Path.Combine(BaseUri.ToString(), "sys/command");
            }
        }

        string SysFirmwareUrl
        {
            get
            {
                return Path.Combine(BaseUri.ToString(), "sys/firmware");
            }
        }

        string SysModeUrl
        {
            get
            {
                return Path.Combine(BaseUri.ToString(), "sys/mode");
            }
        }

        string TstatModelUrl
        {
            get
            {
                return Path.Combine(BaseUri.ToString(), "tstat/model");
            }
        }

        string TstatUrl
        {
            get
            {
                return Path.Combine(BaseUri.ToString(), "tstat");
            }
        }

        string TstatHeatProgramUrl
        {
            get
            {
                return Path.Combine(BaseUri.ToString(), "tstat/program/heat");
            }
        }

        string TstatCoolProgramUrl
        {
            get
            {
                return Path.Combine(BaseUri.ToString(), "tstat/program/cool");
            }
        }
        #endregion
        
        public ThermostatBase(string location) : this(location, new Client())
        {
        }
        public ThermostatBase(string location, IClient jsonClient)
        {
            if (!location.StartsWith("http://")) //handle IP addresses that aren't formatted as URIs
                location = "http://" + location;

            var locationUri = new Uri(location);
            BaseUri = new Uri("http://" + new Uri(location).Host);
            _JsonClient = jsonClient;
        }

        public event EventHandler<ThermostatResultEventArgs<CloudMessage>> GetCloudSettingsCompleted;
        public void GetCloudSettingsAsync()
        {
            _JsonClient.GetData<CloudMessage>(CloudUrl, (result) =>
            {
                if (GetCloudSettingsCompleted != null)
                    GetCloudSettingsCompleted(this, new ThermostatResultEventArgs<CloudMessage>(result));
            });
        }

        public event EventHandler<ThermostatResultEventArgs<PostResult>> PostCloudSettingsCompleted;
        public void PostCloudSettingsAsync(CloudMessage message)
        {
            _JsonClient.PostData(CloudUrl, message, (result) =>
            {
                if (PostCloudSettingsCompleted != null)
                    PostCloudSettingsCompleted(this, new ThermostatResultEventArgs<PostResult>(result));
            });
        }

        public event EventHandler<ThermostatResultEventArgs<SystemMessage>> GetSystemInfoCompleted;
        public void GetSystemInfoAsync()
        {
            _JsonClient.GetData<SystemMessage>(SysUrl, (result) =>
            {
                if (GetSystemInfoCompleted != null)
                    GetSystemInfoCompleted(this, new ThermostatResultEventArgs<SystemMessage>(result));
            });
        }

        public event EventHandler<ThermostatResultEventArgs<NameMessage>> GetNameCompleted;
        public void GetNameAsync()
        {
            _JsonClient.GetData<NameMessage>(SysNameUrl, (result) =>
            {
                if (GetNameCompleted != null)
                    GetNameCompleted(this, new ThermostatResultEventArgs<NameMessage>(result));
            });
        }

        public event EventHandler<ThermostatResultEventArgs<PostResult>> PostNameCompleted;
        public void PostNameAsync(NameMessage message)
        {
            _JsonClient.PostData(SysNameUrl, message, (result) =>
            {
                if (PostNameCompleted != null)
                    PostNameCompleted(this, new ThermostatResultEventArgs<PostResult>(result));
            });
        }

        public event EventHandler<ThermostatResultEventArgs<ModelMessage>> GetModelCompleted;
        public void GetModelAsync()
        {
            _JsonClient.GetData<ModelMessage>(TstatModelUrl, (result) =>
            {
                if (GetModelCompleted != null)
                    GetModelCompleted(this, new ThermostatResultEventArgs<ModelMessage>(result));
            });
        }

        public event EventHandler<ThermostatResultEventArgs<TstatMessage>> GetStatusCompleted;
        public void GetStatusAsync()
        {
            _JsonClient.GetData<TstatMessage>(TstatUrl, (result) =>
            {
                if (GetStatusCompleted != null)
                    GetStatusCompleted(this, new ThermostatResultEventArgs<TstatMessage>(result));
            });
        }

        public event EventHandler<ThermostatResultEventArgs<PostResult>> PostStatusCompleted;
        public void PostStatusAsync(TstatMessage message)
        {
            _JsonClient.PostData(TstatUrl, message, (result) =>
            {
                if (PostStatusCompleted != null)
                    PostStatusCompleted(this, new ThermostatResultEventArgs<PostResult>(result));
            });
        }

        public event EventHandler<ThermostatResultEventArgs<PostResult>> PostCommandCompleted;
        public void PostCommandAsync(string command)
        {
            _JsonClient.PostData(SysCommandUrl, command, (result) =>
            {
                if (PostCommandCompleted != null)
                    PostCommandCompleted(this, new ThermostatResultEventArgs<PostResult>(result));
            });
        }

        public event EventHandler<ThermostatResultEventArgs<SystemFirmwareMessage>> GetSystemFirmwareCompleted;
        public void GetSystemFirmwareAsync()
        {
            _JsonClient.GetData<SystemFirmwareMessage>(SysFirmwareUrl, (result) =>
            {
                if (GetSystemFirmwareCompleted != null)
                    GetSystemFirmwareCompleted(this, new ThermostatResultEventArgs<SystemFirmwareMessage>(result));
            });
        }

        public event EventHandler<ThermostatResultEventArgs<SystemModeMessage>> GetSystemModeCompleted;
        public void GetSystemModeAsync()
        {
            _JsonClient.GetData<SystemModeMessage>(SysModeUrl, (result) =>
            {
                if (GetSystemModeCompleted != null)
                    GetSystemModeCompleted(this, new ThermostatResultEventArgs<SystemModeMessage>(result));
            });
        }

        public event EventHandler<ThermostatResultEventArgs<ProgramMessage>> GetHeatProgramCompleted;
        public void GetHeatProgramAsync()
        {
            _JsonClient.GetData<ProgramMessage>(TstatHeatProgramUrl, (result) =>
                {
                    if (GetHeatProgramCompleted != null)
                        GetHeatProgramCompleted(this, new ThermostatResultEventArgs<ProgramMessage>(result));
                });
        }

        public event EventHandler<ThermostatResultEventArgs<ProgramMessage>> GetCoolProgramCompleted;
        public void GetCoolProgramAsync()
        {
            _JsonClient.GetData<ProgramMessage>(TstatHeatProgramUrl, (result) =>
            {
                if (GetCoolProgramCompleted != null)
                    GetCoolProgramCompleted(this, new ThermostatResultEventArgs<ProgramMessage>(result));
            });
        }

        public override bool Equals(object obj)
        {
            var tstat = obj as ThermostatBase;
            if (tstat == null)
                return false;

            return this.BaseUri.Equals(tstat.BaseUri);
        }

        public override int GetHashCode()
        {
            return this.BaseUri.GetHashCode();
        }
    }

}
