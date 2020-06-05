using System;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace GuardameLugar.Common.Exceptions
{
    [Serializable]
    public class NotFoundException : BaseException
    {
        public NotFoundException()
        {
        }

        public NotFoundException(string message) : base(message)
        {
        }

        public NotFoundException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        public NotFoundException(string message, int errorCode) : base(message, errorCode)
        {
        }

        [SecurityPermissionAttribute(SecurityAction.Demand, SerializationFormatter = true)]
        protected NotFoundException(SerializationInfo info, StreamingContext context)
           : base(info, context)
        {
        }
    }
}