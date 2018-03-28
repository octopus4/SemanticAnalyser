using DataProcessing.Data;

namespace SOM.Semantics
{
    public class SemanticNeuronFactory : INeuronFactory
    {
        public Neuron Produce(DataToken token, int x, int y)
        {
            return new SemanticNeuron(token, x, y);
        }
    }
}
