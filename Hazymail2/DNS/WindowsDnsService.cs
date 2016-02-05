using System;

namespace Hazymail.DNS
{
    public class WindowsDnsService : IDnsService
    {
        public string[] GetMxRecords(string domain)
        {
            return DnsMx.GetMXRecords(domain);
        }
    }
}