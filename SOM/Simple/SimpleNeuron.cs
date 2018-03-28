using DataProcessing.Data;

namespace SOM.Simple
{
    public class SimpleNeuron : Neuron
    {
        public SimpleNeuron(DataToken token, int x, int y)
            : base(token, x, y)
        {
        }

        protected override void InitWeights(DataToken token)
        {
            Weights = new object[token.Length];
            for (int i = 0; i < token.Length; i++)
            {
                if (token.Types[i] == DataType.Numerical &&
                    token.Flows[i] == DataFlow.Input)
                {
                    Weights[i] = (double)token.Values[i];
                }
            }
        }

        public override void CorrectWeights(DataToken token, double diminishingFactor)
        {
            for (int i = 0; i < Weights.Length; i++)
            {
                if (token.Types[i] == DataType.Numerical &&
                    token.Flows[i] == DataFlow.Input)
                {
                    double weight = (double)Weights[i];
                    weight += diminishingFactor * ((double)token.Values[i] - (double)Weights[i]);
                    Weights[i] = weight;
                }
            }
        }
    }
}
