using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace WiFiThermostats.Tests
{
    [TestClass]
    public class ThermostatBaseTests
    {
        [TestMethod]
        public void Constructor_IPAddressStringPassed_BaseUriHasCorrectHost()
        {
            var ip = "192.168.1.101";
            var tstat = new ThermostatBase(ip);

            Assert.AreEqual(ip, tstat.BaseUri.Host);
        }

        [TestMethod]
        public void Constructor_HttpUrlStringPassed_BaseUriHasCorrectHost()
        {
            var ip = "192.168.1.102";
            var tstat = new ThermostatBase(string.Format("http://{0}", ip));

            Assert.AreEqual(ip, tstat.BaseUri.Host);
        }
    }
}
