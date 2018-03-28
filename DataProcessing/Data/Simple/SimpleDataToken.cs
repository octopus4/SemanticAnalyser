namespace DataProcessing.Data.Simple
{
    public class SimpleDataToken : DataToken
    {
        public SimpleDataToken(string[] values, DataType[] typeMask, DataFlow[] flowMask)
            : base(values, typeMask, flowMask)
        {
        }

        protected override void InitValues(string[] values, DataType[] typeMask, DataFlow[] flowMask)
        {
            Values = new object[values.Length];

            if (typeMask != null && flowMask != null)
            {
                Types = new DataType[typeMask.Length];
                Flows = new DataFlow[flowMask.Length];
            }

            for (int i = 0; i < values.Length; i++)
            {
                Values[i] = CalculateValue(typeMask[i], flowMask[i], values[i]);
                Types[i] = typeMask[i];
                Flows[i] = flowMask[i];
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