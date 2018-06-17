using System.Runtime.Serialization;

namespace Service.Model.Exception
{
    public class UnSupportedException : System.Exception
    {
        public UnSupportedException()
        {
        }

        protected UnSupportedException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public UnSupportedException(string message) : base(message)
        {
        }

        public UnSupportedException(string message, System.Exception innerException) : base(message, innerException)
        {
        }
    }
}