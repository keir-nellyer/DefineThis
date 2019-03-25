using System;
using System.Runtime.Serialization;

namespace DefineThis.Exceptions
{
    /// <summary>
    /// An exception which is thrown when the Dictionary API returns an unexpected response code.
    /// </summary>
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
