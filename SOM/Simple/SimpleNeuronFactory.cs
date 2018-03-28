using DataProcessing.Data;

namespace SOM.Simple
{
    public class SimpleNeuronFactory : INeuronFactory
    {
        public Neuron Produce(DataToken token, int x, int y)
        {
            return new SimpleNeuron(token, x, y);
        }
    }
}
