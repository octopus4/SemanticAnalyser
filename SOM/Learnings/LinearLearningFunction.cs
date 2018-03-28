namespace SOM.Learnings
{
    internal class LinearLearningFunction : LearningFunction
    {
        private static readonly double Multiplier = 0.9;

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
