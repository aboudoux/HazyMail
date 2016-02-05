using System.Collections.Generic;
using System.Net.Mail;
using System.Text;

namespace Hazymail
{
    public interface IMailSender
    {
        void SendMail(MailMessage message);
    }
}
