namespace WiFiThermostats.Json
{
    public enum TargetTempOverrideStatus
    {
        Disabled = 0,
        Enabled = 1
    }

    public enum TargetTempHoldStatus
    {
        Disabled = 0,
        Enabled = 1
    }

    public enum FanOperatingState
    {
        Off = 0,
        On = 1
    }

    public enum FanOperatingMode
    {
        Auto = 0,
        AutoCirculate = 1,
        On = 2
    }

    public enum TargetTempPostType
    {
        TemporaryTargetTemp = 0,
        AbsoluteTargetTemp = 1,
        Unknown = 2,
    }

    public enum TstatDayOfWeek
    {
        Monday = 0,
        Tuesday = 1,
        Wednesday = 2,
        Thursday = 3,
        Friday = 4,
        Saturday = 5,
        Sunday = 6
    }

    public enum ThermostatOperatingMode
    {
        Off = 0,
        Heat = 1,
        Cool = 2,
        Auto = 3,
    }

    public enum HvacOperatingState
    {
        Off = 0,
        Heat = 1,
        Cool = 2,
    }

    public enum AbsoluteTargetTemperatureMode
    {
        Disable = 0,
        Enable = 1,
    }
}