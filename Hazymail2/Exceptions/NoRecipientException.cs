using System;
using System.Runtime.Serialization;

namespace Hazymail.Exceptions
{
    [Serializable]
    public class NoRecipientException : Exception
    {
        public NoRecipientException()
            : base("you must define at least one recipient")
        {
        }      
    }
}