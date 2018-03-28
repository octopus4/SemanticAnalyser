using DataProcessing.Data;
using DataProcessing.Data.Semantic;
using SOM;
using System.Collections.Generic;

namespace SomVisualisation.Drawers.Semantic
{
    public class SemanticMapDrawer : MapDrawer<SemanticPair[]>
    {
        public SemanticMapDrawer(Dictionary<Neuron, List<DataToken>> clusterResult,
            int index, int cellWidth, int cellHeight, CanvasCreator canvasCreator)
            : base(clusterResult, index, cellWidth, cellHeight, canvasCreator)
        {
        }


        internal override Canvas Draw(IEnumerable<SemanticPair[]> data, int width, int height)
        {
            Canvas map = CanvasCreator.CreateCanvas(width, height);
            PaintTool drawer = map.Tool;

            drawer.StartRendering();
            foreach (var pair in ClusterResult)
            {
                int x = (int)(pair.Key.Position.X * CellWidth);
                int y = (int)(pair.Key.Position.Y * CellHeight);

                drawer.DrawCluster(x, y, CellWidth, CellHeight);
                for (int i = 0; i < CellHeight / (12 * 1.5) && i < pair.Value.Count; i++)
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
