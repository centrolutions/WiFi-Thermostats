using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using System.Runtime.Serialization.Json;

namespace WiFiThermostats.Json
{
    public class Client : IClient
    {
        public void GetData<T>(string url, Action<T> callback)
        {
            WebClient web = new WebClient();
            web.DownloadStringCompleted += (s, e) =>
            {
                if (e.Error != null || e.Result == null)
                    return;
                var json = e.Result;
                T result = default(T);
                GetObject(json, out result);
                callback(result);
            };
            web.DownloadStringAsync(new Uri(url));
        }

        public void PostData<T>(string url, T message, Action<PostResult> callback)
        {
            WebClient web = new WebClient();
            web.UploadStringCompleted += (s, e) =>
            {
                if (e.Error != null || e.Result == null)
                    return;
                var json = e.Result;
                PostResult result = null;
                GetObject(json, out result);
                callback(result);
            };
            string messageString = (typeof(T) == typeof(string)) ? message as string : GetJsonString(message);
            web.UploadStringAsync(new Uri(url), messageString);
        }

        void GetObject<T>(string json, out T dataContract)
        {
            using (var ms = new MemoryStream(Encoding.UTF8.GetBytes(json)))
            {
                var dcs = new DataContractJsonSerializer(typeof(T));
                dataContract = (T)dcs.ReadObject(ms);
                ms.Close();
            }
        }

        string GetJsonString<T>(T dataContract)
        {
            string result = string.Empty;
            var dcs = new DataContractJsonSerializer(typeof(T));
            using (var ms = new MemoryStream())
            {
                dcs.WriteObject(ms, dataContract);
                ms.Flush();
                ms.Position = 0;
                var bytes = ms.ToArray();
                result = Encoding.UTF8.GetString(bytes, 0, bytes.Length);
                ms.Close();
            }

            return result;
        }
    }
}
