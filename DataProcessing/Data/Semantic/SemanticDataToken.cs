using System;
using System.Collections.Generic;
using System.Linq;

namespace DataPreprocessing.Data.Semantic
{
    /// <summary>
    /// Wrap of the semantic data vector
    /// </summary>
    public class SemanticDataToken : DataToken
    {
        /// <summary>
        /// String that divides context in one distance from the context in another distance
        /// </summary>
        private static readonly string DistantSeparator = ";";
        /// <summary>
        /// String that divides left context from the right context
        /// </summary>
        private static readonly string ContextSeparator = "|";
        /// <summary>
        /// Size of context, e.g. number of words that are encounted in the learning process
        /// </summary>
        public static int ContextSize { get; set; }

        public SemanticDataToken()
            : base()
        {
        }

        private static T[] FillArray<T>(int length, T initialValue, T otherValue)
        {
            List<T> types = new List<T>(length) { initialValue };
            for (int i = 1; i < length; i++)
            {
                types.Add(otherValue);
            }

            return types.ToArray();
        }

        /// <summary>
        /// Initializes values of token weights vector
        /// </summary>
        /// <param name="values"></param>
        /// <param name="typeMask"></param>
        /// <param name="flowMask"></param>
        public override void InitValues(string[] values, DataType[] typeMask, DataFlow[] flowMask)
        {
            List<object> convertedValues = new List<object>() { values[0] };
            int[] distantSeparatorIndexes = GetDistantSeparatorIndexes(values);
            int previousIndex = 1;
            int distance = 1;
            foreach (int index in distantSeparatorIndexes)
            {
#pragma warning disable CS1030 // #warning: "parameter of distance?'
#warning parameter of distance?
                string[] context = FromRange(values, previousIndex, index);
#pragma warning restore CS1030 // #warning: "parameter of distance?'
                int separatorIndex = Array.IndexOf(context, ContextSeparator);

                string[] rightValues = FromRange(context, 0, separatorIndex);
                string[] leftValues = FromRange(context, separatorIndex + 1, context.Length);

                SemanticPair[] rightContext = (SemanticPair[])CalculateValue(DataType.Semantic, DataFlow.Input, string.Join(ContextSeparator, rightValues));
                SemanticPair[] leftContext = (SemanticPair[])CalculateValue(DataType.Semantic, DataFlow.Input, string.Join(ContextSeparator, leftValues));

                for (int i = 0; i < rightContext.Length; i++)
                {
                    rightContext[i].Frequence /= distance;
                    leftContext[i].Frequence /= distance;
                }

                convertedValues.Add(rightContext);
                convertedValues.Add(leftContext);

                previousIndex = index + 1;
                distance++;
            }

            Values = convertedValues.ToArray();
            Types = FillArray(Values.Length, DataType.Categorial, DataType.Semantic);
            Flows = FillArray(Values.Length, DataFlow.NonUsed, DataFlow.Input);
        }

        private int[] GetDistantSeparatorIndexes(string[] values)
        {
            int[] distantSeparatorIndexes = new int[values.Count(v => v == ";")];
            int i = 0;
            int j = 0;
            foreach (string value in values)
            {
                if (value == DistantSeparator)
                {
                    distantSeparatorIndexes[i] = j;
                    i++;
                }
                j++;
            }

            return distantSeparatorIndexes;
        }

        /// <summary>
        /// Returns all items from values before the separator
        /// </summary>
        /// <param name="values">Values that are to be converted to the token data</param>
        /// <param name="startIndex">Index of the first item in the array</param>
        /// <param name="lastIndex">Index of the last item in the array</param>
        /// <returns></returns>
        private string[] FromRange(string[] values, int startIndex, int lastIndex)
        {
            List<string> result = new List<string>(lastIndex - startIndex);
            for (int i = startIndex; i < lastIndex; i++)
            {
                result.Add(values[i]);
            }

            return result.ToArray();
        }

        /// <summary>
        /// Transferes data from string to semantic view
        /// </summary>
        /// <param name="dataType">Type of the vector component</param>
        /// <param name="dataFlow">Flow of the vector component</param>
        /// <param name="value">Value that is to be converted</param>
        /// <returns></returns>
        protected override object CalculateValue(DataType dataType, DataFlow dataFlow, string value)
        {
            if (dataType != DataType.Semantic || dataFlow != DataFlow.Input)
            {
                return null;
            }

            string[] values = value.Split('|');
            SemanticPair[] context = new SemanticPair[values.Length];
            for (int i = 0; i < values.Length; i++)
            {
                int id = i;
                double frequence = double.Parse(values[i]);
                context[i] = new SemanticPair(id, frequence);
            }

            Array.Sort(context, SemanticPair.Comparison);
            SemanticPair[] result = new SemanticPair[ContextSize];
            Array.Copy(context, result, result.Length);

            return result;
        }
    }
}
