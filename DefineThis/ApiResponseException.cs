using System;
using System.Runtime.Serialization;

namespace DefineThis
{
    public class ApiResponseException : Exception
    {
        public ApiResponseException() : base()
        {
        }

        public ApiResponseException(string message) : base(message)
        {
        }

        public ApiResponseException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected ApiResponseException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
