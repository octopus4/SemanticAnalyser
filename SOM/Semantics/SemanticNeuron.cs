using System;
using System.Collections.Generic;
using System.Linq;

using DataProcessing.Data;
using DataProcessing.Data.Semantic;

namespace SOM.Semantics
{
    public class SemanticNeuron : Neuron
    {
        private static readonly int StartIndex = 1;

        public SemanticNeuron(DataToken token, int x, int y) : base(token, x, y)
        {
        }

        protected override void InitWeights(DataToken token)
        {
            Weights = new object[token.Length];
            Weights[0] = token.Values[0];
            for (int index = StartIndex; index < token.Length; index++)
            {
                SemanticPair[] weights = new SemanticPair[((SemanticPair[])token.Values[index]).Length];
                for (int i = 0; i < weights.Length; i++)
                {
                    weights[i] = ((SemanticPair[])token.Values[1])[i];
                }
                Weights[index] = weights;
            }
        }

        public SemanticPair[] GetSemanticPairs()
        {
            return (SemanticPair[])Weights[0];
        }

        public override void CorrectWeights(DataToken token, double diminishingFactor)
        {
            for (int index = StartIndex; index < token.Length; index++)
            {
                if (token.Types[index] != DataType.Semantic)
                {
                    throw new ArgumentException();
                }

                IEnumerable<SemanticPair> tokenPairs = (SemanticPair[])token.Values[index];
                IEnumerable<SemanticPair> neuronPairs = (SemanticPair[])Weights[index];

                IEnumerable<int> tokenIdList = tokenPairs.Select(pair => pair.WordId);
                IEnumerable<int> neuronIdList = neuronPairs.Select(pair => pair.WordId);

                List<int> idList = tokenIdList.Union(neuronIdList).ToList();

                List<SemanticPair> neuronWeights = CreateUniversalSet(neuronPairs, idList);
                List<SemanticPair> tokenWeights = CreateUniversalSet(tokenPairs, idList);

                for (int i = 0; i < tokenWeights.Count; i++)
                {
                    neuronWeights[i].Frequence += diminishingFactor * (tokenWeights[i].Frequence - neuronWeights[i].Frequence);
                }
                neuronWeights.Sort(SemanticPair.Comparison);
                neuronWeights = neuronWeights.Take(SemanticDataToken.ContextSize).ToList();
                Weights[index] = neuronWeights.ToArray();
            }
        }

        private List<SemanticPair> CreateUniversalSet(IEnumerable<SemanticPair> pairsList, IEnumerable<int> idList)
        {
            List<SemanticPair> universalSet = new List<SemanticPair>(idList.Count());
            foreach (int id in idList)
            {
                universalSet.Add(pairsList.FirstOrDefault(p => p.WordId == id) ?? new SemanticPair(id, 0));
            }

            return universalSet;
        }
    }
}
