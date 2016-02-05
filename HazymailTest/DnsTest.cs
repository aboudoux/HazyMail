using System;
using System.Net;
using Hazymail.DNS;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HazymailTest
{
    [TestClass]
    public class DnsTest
    {
        [TestMethod]
        public void TestGetGmailDns()
        {
            var service = new CrossPlatformDnsService();
            var result = service.GetMxRecords("gmail.com");
            Assert.AreEqual(5, result.Length);
        }       
    }
}
