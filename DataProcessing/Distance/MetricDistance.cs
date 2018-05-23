using System;

namespace DataPreprocessing.Distance
{
    internal static class MetricDistance
    {

        public static double Distance(double[] differences, double norm)
        {
            double result = 0;
            for (int i = 0; i < differences.Length; i++)
            {
                result += Math.Abs(Math.Pow(differences[i], norm));
            }
            return Math.Pow(result, 1 / norm);
        }
    }
}
