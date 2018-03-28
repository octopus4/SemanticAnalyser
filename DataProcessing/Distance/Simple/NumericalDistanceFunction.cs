using System;

namespace DataProcessing.Distance.Simple
{
    public class NumericalDistanceFunction : DistanceFunction
    {
        public NumericalDistanceFunction(Metric metric) : base(metric)
        {
        }

        public override double Calculate(object[] vector1, object[] vector2)
        {
            double[] differences = CalculateDifferences(vector1, vector2);
            return MetricDistance.Distance(differences, Norm);
        }

        public double Calculate(double?[] weights, object[] values)
        {
            object[] packedWeights = PackArray(weights);
            return Calculate(packedWeights, values);
        }

        private object[] PackArray(double?[] vector)
        {
            object[] result = new object[vector.Length];
            for (int i = 0; i < result.Length; i++)
            {
                if (vector[i].HasValue)
                {
                    result[i] = vector[i];
                }
            }
            return result;
        }

        private double[] CalculateDifferences(object[] vector1, object[] vector2)
        {
            double[] result = new double[vector1.Length];
            for (int i = 0; i < result.Length; i++)
            {
                if (vector1[i] != null && vector2[i] != null)
                {
                    result[i] = Convert.ToDouble(vector1[i]) - Convert.ToDouble(vector2[i]);
                }
            }
            return result;
        }
    }
}
