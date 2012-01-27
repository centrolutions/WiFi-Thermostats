using System;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading;
using System.Collections.Generic;

namespace WiFiThermostats.Upnp
{
    internal class Ssdp : ISsdp
    {
        const string SSDPAddress = "239.255.255.250";
        const int SSDPPort = 1900;

        IPAddress ssdpIPAddress = IPAddress.Parse(SSDPAddress);
        string query;
        byte[] searchMessage;
        readonly int searchRetrySeconds;
        readonly int searchTimeoutSeconds;
        IPEndPoint listenEndpoint;
        IPEndPoint multicastEndpoint;

        SearchState state;
        Socket searchSocket;
        SocketAsyncEventArgs socketArgs;

        Timer retryTimer;
        Timer timeoutTimer;

        public Ssdp(string searchString)
        {
            query = searchString;
            searchRetrySeconds = 2;
            searchTimeoutSeconds = 10;

            searchMessage = Encoding.UTF8.GetBytes(query);
            multicastEndpoint = new IPEndPoint(ssdpIPAddress, SSDPPort);
            listenEndpoint = new IPEndPoint(IPAddress.Any, SSDPPort);
        }

        public void Cleanup()
        {
            if (searchSocket == null)
                return;

            try
            {
                searchSocket.Close();
            }
            catch { }
        }

        public void StartSearch()
        {
            if (state == SearchState.Searching)
                this.StopSearch(SearchStoppedReason.Aborted);

            searchSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            var args = CreateSearchEventArgs();
            searchSocket.SendToAsync(args);

            retryTimer = new Timer(new TimerCallback(SearchRetry), null, TimeSpan.FromSeconds(searchRetrySeconds), TimeSpan.FromSeconds(searchRetrySeconds));
            timeoutTimer = new Timer(new TimerCallback(SearchTimeout), null, TimeSpan.FromSeconds(searchTimeoutSeconds), TimeSpan.FromMilliseconds(-1));
            state = SearchState.Searching;
        }

        SocketAsyncEventArgs CreateSearchEventArgs()
        {
            if (socketArgs == null)
            {
                socketArgs = new SocketAsyncEventArgs();
                socketArgs.Completed += new EventHandler<SocketAsyncEventArgs>(ListenCompleted);
                socketArgs.SetBuffer(searchMessage, 0, searchMessage.Length);
                socketArgs.RemoteEndPoint = multicastEndpoint;
            }
            return socketArgs;
        }

        void SearchRetry(object state)
        {
            try
            {
                if (searchSocket != null)
                    searchSocket.SendToAsync(CreateSearchEventArgs());
            }
            catch { }
        }

        void SearchTimeout(object state)
        {
            StopSearch(SearchStoppedReason.TimedOut);
        }

        public void StopSearch(SearchStoppedReason reason)
        {
            if (searchSocket != null)
                Cleanup();

            if (retryTimer != null)
                retryTimer.Dispose();

            if (timeoutTimer != null)
                timeoutTimer.Dispose();

            if (SearchStopped != null)
                SearchStopped(this, new SearchStoppedEventArgs(reason));

            state = SearchState.SearchingCompleted;
        }

        public event EventHandler<SearchStoppedEventArgs> SearchStopped;

        public event EventHandler<EventArgs> SearchStarted;

        public event EventHandler<DeviceFoundEventArgs> DeviceFound;


        void ListenCompleted(object sender, SocketAsyncEventArgs e)
        {
            if (e.SocketError == SocketError.Success)
            {
                if (e.LastOperation == SocketAsyncOperation.SendTo)
                {
                    try
                    {
                        //sent multicast, now get ready for unicast
                        e.RemoteEndPoint = listenEndpoint;
                        searchSocket.ReceiveBufferSize = 8000;
                        byte[] receiveBuffer = new byte[8000];
                        e.SetBuffer(receiveBuffer, 0, 8000);
                        searchSocket.ReceiveFromAsync(e);
                        if (SearchStarted != null)
                            SearchStarted(this, new EventArgs());
                    }
                    catch { }
                    return;
                }
                else if (e.LastOperation == SocketAsyncOperation.ReceiveFrom)
                {
                    try
                    {
                        //got a response
                        string result = Encoding.UTF8.GetString(e.Buffer, e.Offset, e.BytesTransferred);
                        var resultDictionary = ParseResults(result);
                        if (DeviceFound != null)
                            DeviceFound(this, new DeviceFoundEventArgs(e.RemoteEndPoint, resultDictionary));

                        searchSocket.ReceiveFromAsync(e);
                    }
                    catch { }
                    return;
                }
            }
            else
            {
                if (e.SocketError != SocketError.OperationAborted)
                {
                    StopSearch(SearchStoppedReason.Error);
                    state = SearchState.NotSearching;
                }
            }

            this.StopSearch(SearchStoppedReason.Error);
            return;
        }

        Dictionary<string, string> ParseResults(string result)
        {
            var dict = new Dictionary<string, string>();
            if (string.IsNullOrWhiteSpace(result))
                return dict;

            var lines = result.Split(new char[] { '\n' });
            foreach (var line in lines)
            {
                var segments = line.Split(':');
                if (segments.Length > 1)
                    dict.Add(segments[0].Trim().ToUpper(), string.Join(":", segments, 1, segments.Length - 1).Trim());
            }

            return dict;
        }


        protected enum SearchState
        {
            NotSearching,
            Searching,
            SearchingCompleted
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
                //free managed resources
                if (socketArgs != null)
                {
                    socketArgs.Dispose();
                    socketArgs = null;
                }

                if (searchSocket != null)
                {
                    searchSocket.Dispose();
                    searchSocket = null;
                }

                if (timeoutTimer != null)
                {
                    timeoutTimer.Dispose();
                    timeoutTimer = null;
                }

                if (retryTimer != null)
                {
                    retryTimer.Dispose();
                    retryTimer = null;
                }
            }
        }
    }
}
