using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using Hazymail.Exceptions;

namespace Hazymail.Helpers
{
    public static class MessageHelper
    {
        private const string EmailPattern = @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z";

        public static MailMessage CreateMessage(IParameters parameters, IRecipientAddress recipient)
        {            
            var message = new MailMessage();
            message.From = new MailAddress(parameters.SenderAddress, parameters.SenderName);            
            message.Subject = parameters.Subject;
            message.Body = parameters.Body;
            message.SubjectEncoding = Encoding.GetEncoding(parameters.Encoding);
            message.BodyEncoding = Encoding.GetEncoding(parameters.Encoding);
            ValidateEmail(recipient.Email);
            message.To.Add(new MailAddress(recipient.Email, recipient.Name));
            foreach (var attachement in parameters.Attachements) 
                message.Attachments.Add(new Attachment(attachement));            
            return message;
        }

        public static string ExtractDomainFromEmail(string email)
        {            
            ValidateEmail(email.Trim());

            var startIndex = email.IndexOf('@');            
            return email.Substring(startIndex + 1).Trim();
        }

        private static void ValidateEmail(string email)
        {
            if (string.IsNullOrEmpty(email))
                throw new EmailNotSpecifiedException();
            if (!Regex.IsMatch(email, EmailPattern, RegexOptions.IgnoreCase))
                throw new EmailNotValidException(email);
        }
    }
}
