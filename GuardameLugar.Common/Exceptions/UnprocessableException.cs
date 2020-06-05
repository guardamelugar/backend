using System;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace GuardameLugar.Common.Exceptions
{
    [Serializable]
    public class UnprocessableException : BaseException
    {
        public UnprocessableException()
        {
        }

        public UnprocessableException(string message) : base(message)
        {
        }

        public UnprocessableException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        public UnprocessableException(string message, int errorCode) : base(message, errorCode)
        {
        }

        [SecurityPermissionAttribute(SecurityAction.Demand, SerializationFormatter = true)]
        protected UnprocessableException(SerializationInfo info, StreamingContext context)
           : base(info, context)
        {
        }
    }
}