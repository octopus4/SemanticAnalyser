using System.Collections.Generic;
using System.Linq;
using DataProcessing.Data;
using SOM;

namespace Visualisation.Drawers.Categorial
{
    public class CategorialMapDrawer : MapDrawer<string>
    {
        private IEnumerable<string> Values { get; }

        public CategorialMapDrawer(Dictionary<Neuron, List<DataToken>> clusterResult,
            int index, int cellWidth, int cellHeight, CanvasCreator componentCreator, IEnumerable<string> values)
            : base(clusterResult, index, cellWidth, cellHeight, componentCreator)
        {
            Values = values;
        }

        internal override Canvas Draw(int clusterWidth, int clusterHeight)
        {
            Canvas result = ComponentCreator.CreateCanvas(clusterWidth, clusterHeight);
            PaintTool mapGraphics = result.Tool;

            Dictionary<string, ColorAdapter> palette = DetectColors(Values);

            foreach (var pair in ClusterResult)
            {
                ColorAdapter color = ColorAdapter.Default;
                if (pair.Value.Count > 0)
                {
                    List<string> values = pair.Value.Select(t => t.Values[Index].ToString()).ToList();
                    string value = CalculateValue(values);
                    color = palette[value];
                }

                int x = (int)(pair.Key.Position.X * CellWidth);
                int y = (int)(pair.Key.Position.Y * CellHeight);

                mapGraphics.DrawArea(x, y, CellWidth, CellHeight, color);
            }

            return result;
        }

        private string CalculateValue(List<string> values)
        {
            List<string> distinctValues = values.Distinct().ToList();
            int indexOfMax = 0;
            int maxCount = 0;
            for (int i = 0; i < distinctValues.Count; i++)
            {
                int count = values.Count(v => v == distinctValues[i]);
                if (count > maxCount)
                {
                    indexOfMax = i;
                }
            }
            return distinctValues[indexOfMax];
        }
    }
}
