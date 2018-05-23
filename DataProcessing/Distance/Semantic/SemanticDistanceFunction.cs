using System.Collections.Generic;
using System.Linq;
using System;
using DataPreprocessing.Data.Semantic;

namespace DataPreprocessing.Distance.Semantic
{
    public class SemanticDistanceFunction : DistanceFunction
    {
        public SemanticDistanceFunction(Metric metric) : base(metric)
        {
        }

        public override double Calculate(object[] vector1, object[] vector2)
        {
            double distance = 0;
            for (int index = 1; index < vector1.Length; index++)
            {
                SemanticPair[] left = (SemanticPair[])vector1[index];
                SemanticPair[] right = (SemanticPair[])vector2[index];

                IEnumerable<int> leftIdList = left.Select(pair => pair.WordId);
                IEnumerable<int> rightIdList = right.Select(pair => pair.WordId);

                List<int> unitedIdList = leftIdList.Union(rightIdList).ToList();
                SemanticPair[,] differences = new SemanticPair[unitedIdList.Count, 2];

                for (int i = 0; i < unitedIdList.Count; i++)
                {
                    double? leftFrequency = left.FirstOrDefault(pair => pair.WordId == unitedIdList[i])?.Frequence;
                    double? rightFrequency = right.FirstOrDefault(pair => pair.WordId == unitedIdList[i])?.Frequence;

                    differences[i, 0] = new SemanticPair(unitedIdList[i], leftFrequency ?? 0);
                    differences[i, 1] = new SemanticPair(unitedIdList[i], rightFrequency ?? 0);
                }

                distance += Calculate(differences);
            }
            distance /= (vector1.Length - 1);

            return distance;
        }

        private double Calculate(SemanticPair[,] differences)
        {
            double distance = 0;
            for (int i = 0; i < differences.GetLength(0); i++)
            {
                distance += Math.Abs(differences[i, 0].Frequence - differences[i, 1].Frequence);
            }
            distance /= differences.GetLength(0);

            return distance;
        }
    }
}
