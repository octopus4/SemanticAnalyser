using System.Collections.Generic;
using System.Linq;
using DataPreprocessing.Data;
using SOM;

namespace Visualisation.SOM.Drawers.Categorial
{
    public class CategorialMapDrawer : MapDrawer<string>
    {
        private IEnumerable<string> Values { get; }
        private Dictionary<string, ColorAdapter> Palette { get; }

        public CategorialMapDrawer(Dictionary<Neuron, List<DataToken>> clusterResult, int index, double cellWidth, double cellHeight, IEnumerable<string> values)
            : base(clusterResult, index, cellWidth, cellHeight)
        {
            Values = values;
            Palette = DetectColors(Values);
        }

        internal override void Draw(PaintTool paintTool, Cluster cluster)
        {
            ColorAdapter color = ColorAdapter.Default;
            if (cluster.Size > 0)
            {
                List<string> values = cluster.Select(t => t.Values[Index].ToString()).ToList();
                string value = CalculateValue(values);
                color = Palette[value];
            }

            int x = (int)(cluster.RelativeX * CellWidth);
            int y = (int)(cluster.RelativeY * CellHeight);

            paintTool.DrawArea(x, y, CellWidth, CellHeight, color);
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
