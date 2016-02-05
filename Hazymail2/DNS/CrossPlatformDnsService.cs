using System;
using System.Collections.Generic;
using System.Net;
using System.Net.NetworkInformation;
using hazymail.DNS.DnsLookup;

namespace Hazymail.DNS
{
    public class CrossPlatformDnsService : IDnsService
    {
        private readonly IPAddress _dnsAddress;

        public CrossPlatformDnsService()
        {                       
            var adapters = NetworkInterface.GetAllNetworkInterfaces();
            foreach (var adapter in adapters) {                
                if (adapter.NetworkInterfaceType != NetworkInterfaceType.Loopback && adapter.OperationalStatus == OperationalStatus.Up) {                    
                    var adapterProperties = adapter.GetIPProperties();
                    var dnsServers = adapterProperties.DnsAddresses;
                    if (dnsServers.Count > 0) {
                        _dnsAddress = dnsServers[0];
                        break;
                    }
                }
            }
        }

        public string[] GetMxRecords(string domain)
        {
            var request = new Request();

            request.AddQuestion(new Question(domain, DnsType.MX, DnsClass.IN));
            var response = Resolver.Lookup(request, _dnsAddress);

            var result = new List<string>();
            
            if (response == null)
                return result.ToArray();
                      
            var mxRecords = new List<MXRecord>();
            
            foreach (var answer in response.Answers)
                mxRecords.Add(answer.Record as MXRecord);

            mxRecords.Sort((x, y) => x.Preference.CompareTo(y.Preference));

            foreach (var mxRecord in mxRecords)
                result.Add(mxRecord.DomainName);            

            return result.ToArray();
        }
    }
}