using System;

namespace DataPreprocessing.Data.Simple
{
    public class SimpleDataToken : DataToken
    {
        public override void InitValues(string[] values, DataType[] typeMask, DataFlow[] flowMask)
        {
            Check(nameof(typeMask), typeMask);
            Check(nameof(flowMask), flowMask);

            Values = new object[values.Length];

            Types = new DataType[typeMask.Length];
            Flows = new DataFlow[flowMask.Length];

            for (int i = 0; i < values.Length; i++)
            {
                Values[i] = CalculateValue(typeMask[i], flowMask[i], values[i]);
                Types[i] = typeMask[i];
                Flows[i] = flowMask[i];
            }
        }

        private void Check(string parameterName, object parameterValue)
        {
            if (parameterValue == null)
            {
                throw new ArgumentNullException(parameterName, $"{parameterName} shoul not be null");
            }
        }

        protected override object CalculateValue(DataType dataType, DataFlow dataFlow, string value)
        {
            switch (dataFlow)
            {
                case DataFlow.NonUsed:
                case DataFlow.Output:
                    return value;
                case DataFlow.Input:
                    switch (dataType)
                    {
                        case DataType.Numerical:
                            return double.Parse(value);
                        case DataType.Categorial:
                            return value;
                    }
                    break;
            }
            return null;
        }
    }
}