using System.Runtime.Serialization;

namespace WiFiThermostats.Json
{
    [DataContract]
    public class TstatMessage
    {
        [DataMember(Name = "fmode", EmitDefaultValue = false)]
        public FanOperatingMode FanMode { get; set; }

        [DataMember(Name = "fstate", EmitDefaultValue = false)]
        public FanOperatingState FanState { get; set; }

        [DataMember(Name = "hold", EmitDefaultValue = false)]
        public TargetTempHoldStatus HoldStatus { get; set; }

        [DataMember(Name = "override", EmitDefaultValue = false)]
        public TargetTempOverrideStatus? OverrideStatus { get; set; }

        [DataMember(Name = "t_type_post", EmitDefaultValue = false)]
        public TargetTempPostType? PostType { get; set; }

        [DataMember(Name = "temp", EmitDefaultValue = false)]
        public float? CurrentTemperature { get; set; }

        [DataMember(Name = "time", EmitDefaultValue = false)]
        public TstatTime Time { get; set; }

        [DataMember(Name = "tmode", EmitDefaultValue = false)]
        public ThermostatOperatingMode OperatingMode { get; set; }

        [DataMember(Name = "tstate", EmitDefaultValue = false)]
        public HvacOperatingState? OperatingState { get; set; }

        [DataMember(Name = "t_heat", EmitDefaultValue = false)]
        public float? TemporaryTargetHeatSetpoint { get; set; }

        [DataMember(Name = "t_cool", EmitDefaultValue = false)]
        public float? TemporaryTargetCoolSetpoint { get; set; }

        [DataMember(Name = "a_heat", EmitDefaultValue = false)]
        public float? AbsoluteTargetHeatSetpoint { get; set; }

        [DataMember(Name = "a_cool", EmitDefaultValue=false)]
        public float? AbsoluteTargetCoolSetpoint { get; set; }

        [DataMember(Name = "a_mode", EmitDefaultValue = false)]
        public AbsoluteTargetTemperatureMode? AbsoluteTemperatureMode { get; set; }

        public float GetTargetTemperature()
        {
            if (this.AbsoluteTargetHeatSetpoint.HasValue)
                return this.AbsoluteTargetHeatSetpoint.Value;
            else if (this.AbsoluteTargetCoolSetpoint.HasValue)
                return this.AbsoluteTargetCoolSetpoint.Value;
            else if (this.TemporaryTargetHeatSetpoint.HasValue)
                return this.TemporaryTargetHeatSetpoint.Value;
            else if (this.TemporaryTargetCoolSetpoint.HasValue)
                return this.TemporaryTargetCoolSetpoint.Value;
            else
                return -1; //not in absolute or temporary mode
        }
    }

    
}
