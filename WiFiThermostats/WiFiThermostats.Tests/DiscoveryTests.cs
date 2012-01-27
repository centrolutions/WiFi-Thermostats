using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using WiFiThermostats.Upnp;
using System.Net;

namespace WiFiThermostats.Tests
{
    [TestClass]
    public class DiscoveryTests
    {
        [TestMethod]
        public void BeginSearch_CallsStartSearchOnSsdpOnce()
        {
            var mock = new Mock<ISsdp>();

            var disco = new Discovery(mock.Object);
            disco.BeginSearch();

            mock.Verify(ssdp => ssdp.StartSearch(), Times.Once());
        }

        [TestMethod]
        public void EndSearch_CallsStopSearchWithAbortedOnSsdpOnce()
        {
            var mock = new Mock<ISsdp>();

            var disco = new Discovery(mock.Object);
            disco.EndSearch();

            mock.Verify(ssdp => ssdp.StopSearch(SearchStoppedReason.Aborted), Times.Once());
        }

        [TestMethod]
        public void SsdpRaisesDeviceFound_ResponseIncludesServiceAndLocation_OneItemIsAddedToResults()
        {
            var mock = new Mock<ISsdp>();

            var disco = new Discovery(mock.Object);
            var ipEndpoint = new IPEndPoint(IPAddress.Parse("192.168.1.100"), 80);
            var resultDict = new Dictionary<string,string>()
                {
                    {"SERVICE", "com.marvell.wm.system:1.0"},
                    {"LOCATION", "http://192.168.1.204/sys/" }
                };
            mock.Raise(ssdp => ssdp.DeviceFound += null, new DeviceFoundEventArgs(ipEndpoint, resultDict));

            Assert.AreEqual(1, disco.Results.Count);
        }

        [TestMethod]
        public void SsdpRaisesDeviceFound_ResponseMissingService_NoItemsAddedToResults()
        {
            var mock = new Mock<ISsdp>();

            var disco = new Discovery(mock.Object);
            var ipEndpoint = new IPEndPoint(IPAddress.Parse("192.168.1.100"), 80);
            var resultDict = new Dictionary<string, string>()
                {
                    {"LOCATION", "http://192.168.1.204/sys/" }
                };
            mock.Raise(ssdp => ssdp.DeviceFound += null, new DeviceFoundEventArgs(ipEndpoint, resultDict));

            Assert.IsFalse(disco.Results.Any());
        }

        [TestMethod]
        public void SsdpRaisesDeviceFound_ResponseMissingLocation_NoItemsAddedToResults()
        {
            var mock = new Mock<ISsdp>();

            var disco = new Discovery(mock.Object);
            var ipEndpoint = new IPEndPoint(IPAddress.Parse("192.168.1.100"), 80);
            var resultDict = new Dictionary<string, string>()
                {
                    {"SERVICE", "com.marvell.wm.system:1.0"},
                };
            mock.Raise(ssdp => ssdp.DeviceFound += null, new DeviceFoundEventArgs(ipEndpoint, resultDict));

            Assert.IsFalse(disco.Results.Any());
        }
    }
}
