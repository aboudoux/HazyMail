using System;
using System.Net;
using System.Net.Mail;
using Hazymail.DNS;
using Hazymail.Helpers;

namespace Hazymail
{
    public class MailSender : IMailSender
    {
        private readonly IDnsService _dnsService;

        private readonly SmtpClient _client = new SmtpClient();

        public MailSender(IDnsService dnsService, IParameters parameters)
        {
            if (dnsService == null) throw new ArgumentNullException("dnsService");
            _dnsService = dnsService;

            if (!string.IsNullOrEmpty(parameters.SmtpServer)) {

                if (parameters.EnableSsl) {
                    _client.EnableSsl = true;
                    ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
                    ServicePointManager.CheckCertificateRevocationList = false;
                }

                _client.Host = parameters.SmtpServer;
                _client.Credentials = new NetworkCredential(parameters.SmtpLogin, parameters.SmtpPassword);                
            }
        }

        public void SendMail(MailMessage message)
        {
            if (string.IsNullOrEmpty(_client.Host)) {
                var domain = MessageHelper.ExtractDomainFromEmail(message.To[0].Address);
                var mx = _dnsService.GetMxRecords(domain);
                if (mx.Length == 0)
                    throw new Exception("no mx record has been found for " + domain);
                _client.Host = mx[0];
            }
            message.Body += "\r\n\r\n"+
                            "_________________________________________________________________\r\n" +
                            "This mail was sent with Hazymail.NET - http://hazymail.boudoux.fr";
            _client.Send(message);
        }
    }
}