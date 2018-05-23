using System.Text;

namespace DataProcessing.Data
{
    /// <summary>
    /// Wrap on the classical vector representation of data
    /// </summary>
    public abstract class DataToken
    {
        /// <summary>
        /// Length of the data vector
        /// </summary>
        public int Length { get { return Values.Length; } }
        /// <summary>
        /// Types of the vector's units
        /// </summary>
        public DataType[] Types { get; protected set; }
        /// <summary>
        /// Flows of the vector's units
        /// </summary>
        public DataFlow[] Flows { get; protected set; }
        /// <summary>
        /// Actual values vector contains
        /// </summary>
        public object[] Values { get; protected set; }

        /// <summary>
        /// Class of the data vector
        /// </summary>
        public string Class
        {
            get
            {
                for (int i = 0; i < Flows.Length; i++)
                {
                    if (Flows[i] == DataFlow.Output)
                    {
                        return Values[i].ToString();
                    }
                }
                return null;
            }
        }

        /// <summary>
        /// Initializes vector of values in the data token
        /// </summary>
        /// <param name="values">String representation of vector's values</param>
        /// <param name="typeMask">Types of vector's values</param>
        /// <param name="flowMask">Flows of vector's values</param>
        public abstract void InitValues(string[] values, DataType[] typeMask, DataFlow[] flowMask);
        protected abstract object CalculateValue(DataType dataType, DataFlow dataFlow, string value);

        /// <summary>
        /// Concats vector's values into a string
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            StringBuilder result = new StringBuilder();
            result.Append("[");
            foreach (var value in Values)
            {
                if (value != null)
                {
                    result.Append($"{value.ToString()};");
                }
            }
            result.Append("]");
            return result.ToString();
        }
    }
}
