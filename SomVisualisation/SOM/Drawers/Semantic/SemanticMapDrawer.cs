using DataPreprocessing.Data;
using DataPreprocessing.Data.Semantic;
using SOM;
using System.Collections.Generic;

namespace Visualisation.SOM.Drawers.Semantic
{
    public class SemanticMapDrawer : MapDrawer<SemanticPair[]>
    {
        public SemanticMapDrawer(Dictionary<Neuron, List<DataToken>> clusterResult, int index, double cellWidth, double cellHeight)
            : base(clusterResult, index, cellWidth, cellHeight)
        {
        }

        internal override void Draw(PaintTool paintTool, Cluster cluster)
        {
            int x = (int)(cluster.RelativeX * CellWidth);
            int y = (int)(cluster.RelativeY * CellHeight);

            paintTool.DrawArea(x, y, CellWidth, CellHeight);
            for (int i = 0; i < CellHeight / (12 * 1.125f) && i < cluster.Size; i++)
            {
                string word = cluster[i].Values[0].ToString();
                paintTool.DrawText(word, x + 2, y + (paintTool.FontSize * 1.125f) * i);
            }
        }
    }
}
