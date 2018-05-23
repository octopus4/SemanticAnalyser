using DataPreprocessing.Data;

using SOM.Topologies;

namespace SOM
{
    public abstract class Neuron
    {
        public object[] Weights { get; protected set; }
        public Point Position { get; private set; }

        protected Neuron(int x, int y)
        {
            Position = new Point(x, y);
        }

        public abstract void InitWeights(DataToken token);

        public abstract void CorrectWeights(DataToken token, double diminishingFactor);
    }
}
