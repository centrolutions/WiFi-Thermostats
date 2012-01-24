using System.Runtime.Serialization;

namespace WiFiThermostats.Json
{
    [DataContract]
    public class TstatTime
    {
        [DataMember(Name = "day")]
        public TstatDayOfWeek DayOfWeek { get; set; }

        [DataMember(Name = "hour")]
        public int Hour { get; set; }

        [DataMember(Name = "minute")]
        public int Minute { get; set; }
    }
}
