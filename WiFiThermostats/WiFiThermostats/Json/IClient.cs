using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WiFiThermostats.Json
{
    public interface IClient
    {
        void GetData<T>(string url, Action<T> callback);
        void PostData<T>(string url, T message, Action<PostResult> callback);
    }
}
