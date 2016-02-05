using System;

namespace Hazymail.Exceptions
{
    [Serializable]
    public class AttachementFileNotFoundException : Exception
    {
        public AttachementFileNotFoundException(string fileName)
            : base("the file '"+ fileName + "' cannot be attached to the message because there is no.")
        {
        }
    }
}