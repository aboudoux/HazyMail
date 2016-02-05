using System;

namespace Hazymail.Exceptions
{
    [Serializable]
    public class EmailNotSpecifiedException : Exception
    {
        public EmailNotSpecifiedException()
            : base("Please specify the email address")
        {
            
        }
    }
}