using System;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace GuardameLugar.Common.Exceptions
{
    [Serializable]
    public class BaseException : Exception
    {
        public int ErrorCode { get; set; }

        public BaseException()
        {
        }

        public BaseException(string message) : base(message)
        {
        }

        public BaseException(string message, int errorCode) : base(message)
        {
            ErrorCode = errorCode;
        }

        public BaseException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        [SecurityPermissionAttribute(SecurityAction.Demand, SerializationFormatter = true)]
        protected BaseException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}