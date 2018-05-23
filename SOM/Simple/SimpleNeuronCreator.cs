using DataProcessing.Data;

namespace SOM.Simple
{
    public class SimpleNeuronCreator : INeuronCreator
    {
        public Neuron Produce(int x, int y)
        {
            return new SimpleNeuron(x, y);
        }
    }
}
