using System;
using System.Runtime.Serialization;

namespace SOM
{
    [Serializable]
    internal class MapIsLearnedException : Exception
    {
        private int Epoch { get; }
        private int EpochCount { get; }

        public MapIsLearnedException()
        {
        }

        public MapIsLearnedException(string message) : base(message)
        {
        }

        public MapIsLearnedException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public MapIsLearnedException(string message, int epoch, int epochCount) : this(message)
        {
            Epoch = epoch;
            EpochCount = epochCount;
        }

        protected MapIsLearnedException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}