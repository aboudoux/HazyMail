using System;
using System.Net;
using System.Net.Mail;
using System.Reflection;
using Hazymail.DNS;
using Hazymail.Helpers;

namespace Hazymail
{
    class Program
    {
        static void Main(string[] args)
        {            
            try {
                Console.WriteLine(
                    "HazyMail.NET, version {0} by Aurelien BOUDOUX." + Environment.NewLine +
                    "http://hazymail.boudoux.fr" + Environment.NewLine + Environment.NewLine, Assembly.GetEntryAssembly().GetName().Version.ToString(3));

                var p = new Parameters(args);
                var service = new CrossPlatformDnsService();

                foreach (var recipient in p.Recipients) {
                    try {
                        Console.Write("Sending email to {0}...", recipient.Email);
                        var message = MessageHelper.CreateMessage(p, recipient);                        
                        
                        var sender = new MailSender(service, p);
                        sender.SendMail(message);
                        
                        Console.WriteLine("OK");
                    }
                    catch (Exception ex) {
                        Console.WriteLine("Error : " + ex.Message);                        
                    }                    
                } 
            }
            catch (Exception ex) {
                Console.WriteLine(ex.Message);
                Environment.Exit(1);
            }            
        }
    }
}
