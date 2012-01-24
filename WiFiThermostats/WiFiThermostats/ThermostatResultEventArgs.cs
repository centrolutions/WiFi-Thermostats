using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WiFiThermostats
{
    public class ThermostatResultEventArgs<T> : EventArgs
    {
        public ThermostatResultEventArgs(T result)
        {
            Result = result;
        }

        public T Result { get; private set; }
    }
}
