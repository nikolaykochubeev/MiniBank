using System;
using System.Runtime.Serialization;

namespace Minibank.Core.Exceptions
{
    public class MinibankException : Exception
    {
        public MinibankException()
        {
        }

        public MinibankException(string message)
            : base(message)
        {
        }

        public MinibankException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        protected MinibankException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}