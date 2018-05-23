using DataPreprocessing.Data;

namespace SOM.Semantics
{
    public class SemanticNeuronCreator : INeuronCreator
    {
        public Neuron Produce(int x, int y)
        {
            return new SemanticNeuron(x, y);
        }
    }
}
