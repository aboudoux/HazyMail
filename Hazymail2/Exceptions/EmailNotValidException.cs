using System;

namespace Hazymail.Exceptions
{
    [Serializable]
    public class EmailNotValidException : Exception
    {             
        public EmailNotValidException(string email) : base(email + " is not a valid email address")
        {
        }       
    }
}