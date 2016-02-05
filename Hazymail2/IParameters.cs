using System.Collections.Generic;

namespace Hazymail
{
    public interface IParameters
    {        
        string SenderName { get;  }
        string SenderAddress { get;  }
        IEnumerable<IRecipientAddress> Recipients { get;  }
        string Subject { get;  }        
        string Body { get;  }
        IEnumerable<string> Attachements { get; }
        string Encoding { get; }
        string SmtpServer { get; }
        string SmtpLogin { get; }
        string SmtpPassword { get; }
        bool EnableSsl { get; }
    }
}