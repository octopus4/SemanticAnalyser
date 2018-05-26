using System;
using System.Collections.Generic;
using System.Linq;
using DataPreprocessing.Data;
using SOM;

namespace Visualisation.SOM.Drawers.Numerical
{
    public class NumericalMapDrawer : MapDrawer<double>
    {
        private IEnumerable<double> Values { get; }
        private IEnumerable<double> IntervalValues { get; }
        private Dictionary<double, ColorAdapter> Palette { get; }

        public NumericalMapDrawer(Dictionary<Neuron, List<DataToken>> clusterResult, int index, double cellWidth, double cellHeight, IEnumerable<double> values)
            : base(clusterResult, index, cellWidth, cellHeight)
        {
            Values = values;
            double min = Values.Min();
            double max = Values.Max();
            double step = (max - min) / 512;

            IntervalValues = SetIntervals(min, max, step);
            Palette = DetectColors(IntervalValues);
        }

        internal override void Draw(PaintTool paintTool, Cluster cluster)
        {
            ColorAdapter color = ColorAdapter.Default;
            if (cluster.Size > 0)
            {
                double pairValue = cluster.Select(t => double.Parse(t.Values[Index].ToString())).Min();
                double minimalPairValue = IntervalValues
                         .Select(d => Math.Abs(d - pairValue))
                         .Min();
                double value = IntervalValues
                    .First(v => Math.Abs(v - pairValue) == minimalPairValue);
                color = Palette[value];
            }

            int x = (int)(cluster.RelativeX * CellWidth);
            int y = (int)(cluster.RelativeY * CellHeight);

            paintTool.DrawArea(x, y, CellWidth, CellHeight, color);
        }

        private IEnumerable<double> SetIntervals(double min, double max, double step)
        {
            double[] result = new double[(int)((max - min) / step)];
            for (int i = 0; i < result.Length; i++)
            {
                result[i] = min + step * i;
            }
            return result;
        }
    }
}
