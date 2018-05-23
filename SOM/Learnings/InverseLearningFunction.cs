namespace SOM.Learnings
{
    public class InverseLearningFunction : ILearningFunction
    {
        private static readonly double StartValue = 1;

        private double Coefficient { get; }

        public InverseLearningFunction(int sourceLength)
        {
            Coefficient = sourceLength / 100.0;
        }

        public double Calculate(int epochNumber)
        {
            return Coefficient * StartValue / (StartValue + epochNumber);
        }
    }
}
