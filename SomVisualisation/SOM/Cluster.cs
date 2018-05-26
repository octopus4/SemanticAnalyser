using System.Collections;
using System.Collections.Generic;
using DataPreprocessing.Data;
using SOM;

namespace Visualisation.SOM
{
    class Cluster : ImageObject, IEnumerable<DataToken>
    {
        private List<DataToken> Data { get; }
        public Neuron Neuron { get; }

        public DataToken this[int index]
        {
            get { return Data[index]; }
        }

        public int Size { get { return Data.Count; } }

        public Cluster(Neuron neuron, List<DataToken> data) : base(neuron.Position.X, neuron.Position.Y)
        {
            Neuron = neuron;
            Data = data;
        }

        public IEnumerator<DataToken> GetEnumerator()
        {
            return Data.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
