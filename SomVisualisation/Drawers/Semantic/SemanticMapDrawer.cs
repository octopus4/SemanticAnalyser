using DataPreprocessing.Data;
using DataPreprocessing.Data.Semantic;
using SOM;
using System.Collections.Generic;

namespace Visualisation.Drawers.Semantic
{
    public class SemanticMapDrawer : MapDrawer<SemanticPair[]>
    {
        public SemanticMapDrawer(Dictionary<Neuron, List<DataToken>> clusterResult,
            int index, int cellWidth, int cellHeight, CanvasCreator componentCreator)
            : base(clusterResult, index, cellWidth, cellHeight, componentCreator)
        {
        }

        internal override Canvas Draw(int clusterWidth, int clusterHeight)
        {
            Canvas map = ComponentCreator.CreateCanvas(clusterWidth, clusterHeight);
            PaintTool drawer = map.Tool;

            drawer.StartRendering();
            foreach (var pair in ClusterResult)
            {
                int x = (int)(pair.Key.Position.X * CellWidth);
                int y = (int)(pair.Key.Position.Y * CellHeight);

                drawer.DrawArea(x, y, CellWidth, CellHeight);
                for (int i = 0; i < CellHeight / (12 * 1.125f) && i < pair.Value.Count; i++)
                {
                    string word = pair.Value[i].Values[0].ToString();
                    drawer.DrawText(word, x + 2, y + (drawer.FontSize * 1.125f) * i);
                }
            }
            drawer.Dispose();

            return map;
        }
    }
}
