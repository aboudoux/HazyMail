using System;
using System.Collections.Generic;
using System.IO;
using Hazymail.Exceptions;

namespace Hazymail
{
    public class Parameters : IParameters
    {
        private readonly string _senderName = "HazyMail";
        private readonly string _senderAddress = "do-not-reply@Hazymail.com";
        private readonly string _encoding = "ISO-8859-1";        
        private readonly string _subject = "";
        private readonly List<IRecipientAddress> _recipientAddresses = new List<IRecipientAddress>();
        private readonly List<string> _attachements = new List<string>();       
 
        private readonly string _body = "";
        private readonly string _smtpServer = "";
        private readonly string _smtpLogin = "";
        private readonly string _smtpPassword = "";
        private bool _enableSsl = false;

        public Parameters(IList<string> args)
        {
            if( args.Count == 0 )
                PrintUsage();

            int argCount = 0;
            foreach (var argument in args) {
                switch (argument) {
                    case "-sn": _senderName = GetArg(args, argCount); break;
                    case "-sa": _senderAddress = GetArg(args, argCount); break;
                    case "-ra": ParseRecipients(GetArg(args,argCount)); break;
                    case "-s": _subject = GetArg(args,argCount); break;                    
                    case "-b": _body = GetBody(GetArg(args, argCount)); break;
                    case "-a": ParseAttachement(GetArg(args, argCount)); break;
                    case "-ce": _encoding = GetArg(args,argCount); break;
                    case "-ss": _smtpServer = GetArg(args, argCount); break;
                    case "-sl": _smtpLogin = GetArg(args, argCount); break;
                    case "-sp": _smtpPassword = GetArg(args, argCount); break;
                    case "-ssl": _enableSsl = true; break;                        
                    case "-h": PrintUsage(); break;
                }
                argCount++;
            }
        }                      

        public string Encoding { get { return _encoding; } }
        public string SmtpServer { get { return _smtpServer; } }
        public string SmtpLogin { get { return _smtpLogin; } }
        public string SmtpPassword { get { return _smtpPassword; } }
        public bool EnableSsl { get { return _enableSsl; } }
        public string SenderName { get { return _senderName; } }
        public string SenderAddress { get { return _senderAddress; } }
        public IEnumerable<IRecipientAddress> Recipients { get { return _recipientAddresses; } }
        public string Subject { get { return _subject; } }        
        public string Body { get { return _body; }             }
        public IEnumerable<string> Attachements { get { return _attachements; } }    
    

        private static void PrintUsage()
        {
            Console.WriteLine(                
                "Usage : HazyMail.exe [options]" + Environment.NewLine + Environment.NewLine +
                "Options are : " + Environment.NewLine +
                " -ra [Recipient address]   Requiered - See examples for multiple addresses" + Environment.NewLine +
                " -s  [Subject]             Optional" + Environment.NewLine + 
                " -sn [Sender name]         Optional  - HazyMail by default" + Environment.NewLine +
                " -sa [Sender address]      Optional  - do-not-reply@Hazymail.com by default" + Environment.NewLine +
                " -b  [Body]                Optional  - Can be a string or a file path" + Environment.NewLine +
                " -a  [Attachements]        Optional  - Separate paths by ;" + Environment.NewLine +
                " -ce [Characters encoding] Optional  - ISO-8859-1 by default" + Environment.NewLine +
                " -ss [Smtp Server]         Optional  - Force hazymail to pass by a smtp" + Environment.NewLine +
                " -sl [Smtp Login]          Optional" + Environment.NewLine +
                " -sp [Smtp password]       Optional" + Environment.NewLine +
                " -ssl                      Optional  - Enable SSL for SMTP authentication" + Environment.NewLine +
                " -h  print this help" + Environment.NewLine + Environment.NewLine +
                "Examples : " + Environment.NewLine + Environment.NewLine +
                "Send simple mail" + Environment.NewLine +
                "hazymail -ra recipient@test.com -s TEST -b \"This is a test\"" + Environment.NewLine + Environment.NewLine +
                "Send mail to multiple adresses with attachements" + Environment.NewLine +
                "hazymail -ra \"test@test.com;root@test.com\" -s TEST -b \"This is a test\" -a \"c:\\report.txt;d:\\data.xls\""+ Environment.NewLine + Environment.NewLine +
                "Send mail to multiples adresses with their name" + Environment.NewLine +
                "hazymail -ra \"Test account<test@test.com>;ROOT<root@test.com>\" -s TEST -b \"This is a test\""+ Environment.NewLine + Environment.NewLine +
                "Go to http://hazymail.boudoux.fr for more examples and informations."
                );
            Environment.Exit(0);
        }

        private void ParseRecipients(string line)
        {
            if (string.IsNullOrEmpty(line)) return;
            var splitted = line.Split(';');
            foreach (var s in splitted)
                _recipientAddresses.Add(new RecipientAddress(s));
        }

        private void ParseAttachement(string line)
        {
            if (string.IsNullOrEmpty(line)) return;
            var splitted = line.Split(';');
            foreach (var fileName in splitted) {
                if(!File.Exists(fileName))
                    throw new AttachementFileNotFoundException(fileName);
                _attachements.Add(fileName);
            }
        }

        private string GetArg(IList<string> args, int currentArgCount)
        {
            if (currentArgCount + 1 >= args.Count)
                return string.Empty;
            if (args[currentArgCount + 1].StartsWith("-"))
                return string.Empty;
            return args[currentArgCount + 1];
        }

        private static string GetBody(string body)
        {
            return File.Exists(body) ? File.ReadAllText(body) : body;
        }        
    }
}