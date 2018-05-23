using DataPreprocessing.Data;

namespace DataPreprocessing.Solutions
{
    /// <summary>
    /// Interface for solvers of "What-If" task
    /// </summary>
    public interface ISolver
    {
        /// <summary>
        /// Tries to classificate some input data token
        /// </summary>
        /// <param name="token">Instance of <see cref="DataToken"/></param>
        /// <returns>Name of the data token class, if it exists</returns>
        string Classify(DataToken token);
    }
}
