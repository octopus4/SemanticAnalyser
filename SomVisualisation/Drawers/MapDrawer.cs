using System.Collections.Generic;
using System.Linq;
using DataProcessing.Data;
using SOM;

namespace SomVisualisation.Drawers
{
    public abstract class MapDrawer<T>
    {
        protected int CellWidth { get; }
        protected int CellHeight { get; }

        protected CanvasCreator CanvasCreator { get; }
        protected Dictionary<Neuron, List<DataToken>> ClusterResult { get; }
        protected int Index { get; }

        public MapDrawer(Dictionary<Neuron, List<DataToken>> clusterResult,
            int index, int cellWidth, int cellHeight, CanvasCreator canvasCreator)
        {
            CanvasCreator = canvasCreator;
            ClusterResult = clusterResult;
            Index = index;

            CellWidth = cellWidth;
            CellHeight = cellHeight;
        }

        internal abstract Canvas Draw(IEnumerable<T> data, int width, int height);

        protected Dictionary<T, ColorAdapter> DetectColors(IEnumerable<T> data)
        {
            Dictionary<T, ColorAdapter> result = new Dictionary<T, ColorAdapter>();
            var distinctData = data.Distinct().ToList();
            ColorAdapter[] colors = Colorizer.CreatePalette(distinctData.Count);

            for (int i = 0; i < colors.Length; i++)
            {
                result.Add(distinctData[i], colors[i]);
            }

            return result;
        }
    }
}
