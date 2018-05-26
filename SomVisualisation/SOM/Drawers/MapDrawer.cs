using System.Collections.Generic;
using System.Linq;
using DataPreprocessing.Data;
using SOM;

namespace Visualisation.SOM.Drawers
{
    public abstract class MapDrawer<T>
    {
        protected double CellWidth { get; }
        protected double CellHeight { get; }

        protected Dictionary<Neuron, List<DataToken>> ClusterResult { get; }
        protected int Index { get; }

        internal int Scale { get; set; }
        internal Rectangle Capture { get; set; }

        protected MapDrawer(Dictionary<Neuron, List<DataToken>> clusterResult, int index, double cellWidth, double cellHeight)
        {
            ClusterResult = clusterResult;
            Index = index;
            CellWidth = cellWidth;
            CellHeight = cellHeight;
        }

        internal abstract void Draw(PaintTool paintTool, Cluster cluster);

        protected Dictionary<T, ColorAdapter> DetectColors(IEnumerable<T> data)
        {
            Dictionary<T, ColorAdapter> result = new Dictionary<T, ColorAdapter>();
            List<T> distinctData = data.Distinct().ToList();
            ColorAdapter[] colors = Colorizer.CreatePalette(distinctData.Count);

            for (int i = 0; i < colors.Length; i++)
            {
                result.Add(distinctData[i], colors[i]);
            }

            return result;
        }
    }
}
