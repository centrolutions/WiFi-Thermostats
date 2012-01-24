using System;
using System.Runtime.Serialization;
using System.Collections.Generic;

namespace WiFiThermostats.Json
{
    [DataContract]
    public class ProgramMessage
    {
        [DataMember(Name = "0")]
        public int[] Monday
        {
            get;
            set;
        }

        [DataMember(Name = "1")]
        public int[] Tuesday
        {
            get;
            set;
        }

        [DataMember(Name = "2")]
        public int[] Wednesday
        {
            get;
            set;
        }

        [DataMember(Name = "3")]
        public int[] Thursday
        {
            get;
            set;
        }

        [DataMember(Name = "4")]
        public int[] Friday
        {
            get;
            set;
        }

        [DataMember(Name = "5")]
        public int[] Saturday
        {
            get;
            set;
        }

        [DataMember(Name = "6")]
        public int[] Sunday
        {
            get;
            set;
        }

        public Dictionary<TstatDayOfWeek, Dictionary<DateTime, float>> CreateProgramData()
        {
            Dictionary<TstatDayOfWeek, Dictionary<DateTime, float>> results = new Dictionary<TstatDayOfWeek,Dictionary<DateTime,float>>();

            results.Add(TstatDayOfWeek.Sunday, CreateTemperatureDictionary(Sunday));
            results.Add(TstatDayOfWeek.Monday, CreateTemperatureDictionary(Monday));
            results.Add(TstatDayOfWeek.Tuesday, CreateTemperatureDictionary(Tuesday));
            results.Add(TstatDayOfWeek.Wednesday, CreateTemperatureDictionary(Wednesday));
            results.Add(TstatDayOfWeek.Thursday, CreateTemperatureDictionary(Thursday));
            results.Add(TstatDayOfWeek.Friday, CreateTemperatureDictionary(Friday));
            results.Add(TstatDayOfWeek.Saturday, CreateTemperatureDictionary(Saturday));

            return results;
        }

        Dictionary<DateTime, float> CreateTemperatureDictionary(int[] raw)
        {
            Dictionary<DateTime, float> results = new Dictionary<DateTime,float>();
            DateTime min = DateTime.MinValue;
            for (int i = 0; i < raw.Length; i += 2)
            {
                results.Add(min.AddMinutes(raw[i]), raw[i + 1]);
            }

            return results;
        }
    }
}
