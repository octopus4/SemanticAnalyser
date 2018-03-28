namespace DataProcessing.Data
{
    /// <summary>
    /// Type of vector's unit data flow
    /// </summary>
    public enum DataFlow
    {
        /// <summary>
        /// This vector's unit is ignored during the learning process
        /// </summary>
        NonUsed,
        /// <summary>
        /// This vector's unit is meaningful in the learning process
        /// </summary>
        Input,
        /// <summary>
        /// This vector's unit is the required result of learning (Is used in classification models)
        /// </summary>
        Output
    }
}
