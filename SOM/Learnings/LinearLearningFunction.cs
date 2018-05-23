namespace SOM.Learnings
{
    internal class LinearLearningFunction : ILearningFunction
    {
        private static readonly double Multiplier = 0.5;

        private int EpochCount { get; }

        public LinearLearningFunction(int epochCount)
        {
            EpochCount = epochCount;
        }

        public double Calculate(int epochNumber)
        {
            return Multiplier * (1 - epochNumber / EpochCount);
        }
    }
}
