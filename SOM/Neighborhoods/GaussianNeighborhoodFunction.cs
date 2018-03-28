using System;

namespace SOM.Neighborhoods
{
    internal class GaussianNeighborhoodFunction : INeighborhoodFunction
    {
        public double Calculate(double distance)
        {
            return Math.Exp(-(distance * distance) / 2);
        }
    }
}
