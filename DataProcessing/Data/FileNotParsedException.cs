using System;
using System.Runtime.Serialization;

namespace DataProcessing.Data
{
    [Serializable]
    public class FileNotParsedException : Exception
    {
        public FileNotParsedException()
        {
        }

        public FileNotParsedException(string message) : base(message)
        {
        }

        public FileNotParsedException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected FileNotParsedException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}