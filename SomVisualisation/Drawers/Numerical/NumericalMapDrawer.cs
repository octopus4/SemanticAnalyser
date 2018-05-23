using System;
using System.Collections.Generic;
using System.Linq;
using DataProcessing.Data;
using SOM;

namespace Visualisation.Drawers.Numerical
{
    public class NumericalMapDrawer : MapDrawer<double>
    {
        private IEnumerable<double> Values { get; }

        public NumericalMapDrawer(Dictionary<Neuron, List<DataToken>> clusterResult,
            int index, int cellWidth, int cellHeight, CanvasCreator componentCreator, IEnumerable<double> values)
            : base(clusterResult, index, cellWidth, cellHeight, componentCreator)
        {
            Values = values;
        }

        internal override Canvas Draw(int clusterWidth, int clusterHeight)
        {
            Canvas result = ComponentCreator.CreateCanvas(clusterWidth, clusterHeight);
            PaintTool mapGraphics = result.Tool;

            double min = Values.Min();
            double max = Values.Max();
            double step = (max - min) / 512;

            IEnumerable<double> intervalValues = SetIntervals(min, max, step);
            Dictionary<double, ColorAdapter> palette = DetectColors(intervalValues);

            foreach (var pair in ClusterResult)
            {
                ColorAdapter color = ColorAdapter.Default;
                if (pair.Value.Count > 0)
                {
                    double pairValue = pair.Value.Select(t => double.Parse(t.Values[Index].ToString())).Min();
#warning simplify
                    double value = intervalValues
                        .First(v => Math.Abs(v - pairValue) == intervalValues
                            .Select(d => Math.Abs(d - pairValue))
                            .Min());
                    color = palette[value];
                }

                int x = (int)(pair.Key.Position.X * CellWidth);
                int y = (int)(pair.Key.Position.Y * CellHeight);

                mapGraphics.DrawArea(x, y, clusterWidth, clusterHeight, color);
            }

            return result;
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
