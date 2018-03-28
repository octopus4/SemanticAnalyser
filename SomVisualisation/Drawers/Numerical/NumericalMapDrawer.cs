using System;
using System.Collections.Generic;
using System.Linq;
using DataProcessing.Data;
using SOM;

namespace SomVisualisation.Drawers.Numerical
{
    public class NumericalMapDrawer : MapDrawer<double>
    {
        public NumericalMapDrawer(Dictionary<Neuron, List<DataToken>> clusterResult,
            int index, int cellWidth, int cellHeight, CanvasCreator canvasCreator)
            : base(clusterResult, index, cellWidth, cellHeight, canvasCreator)
        {
        }

        internal override Canvas Draw(IEnumerable<double> data, int width, int height)
        {
            Canvas result = CanvasCreator.CreateCanvas(width, height);
            PaintTool mapGraphics = result.Tool;

            double min = data.Min();
            double max = data.Max();
            double step = (max - min) / 512;

            IEnumerable<double> intervalValues = SetIntervals(min, max, step);

            Dictionary<double, ColorAdapter> palette = DetectColors(intervalValues);

            foreach (var pair in ClusterResult)
            {
                ColorAdapter color = ColorAdapter.Default;
                if (pair.Value.Count > 0)
                {
                    double pairValue = pair.Value.Select(t => double.Parse(t.Values[Index].ToString())).Min();
                    double value = intervalValues.First(v => Math.Abs(v - pairValue) == intervalValues.Select(d => Math.Abs(d - pairValue)).Min());
                    color = palette[value];
                }

                int x = (int)(pair.Key.Position.X * CellWidth);
                int y = (int)(pair.Key.Position.Y * CellHeight);

                mapGraphics.DrawCluster(x, y, width, height, color);
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
