using System.Collections.Generic;
using System.Linq;
using Visualisation.Drawers;
using Visualisation.Drawers.Numerical;
using Visualisation.Drawers.Categorial;
using Visualisation.Drawers.Semantic;
using DataProcessing.Data;
using DataProcessing.Data.Semantic;
using SOM;

namespace Visualisation
{
    public class VisualMap : VisualData
    {
        private DataType MapType { get; set; }

        private int NeuronWidth { get; set; }
        private int NeuronHeight { get; set; }

        private Dictionary<string, Canvas> Maps { get; set; }

        public int Index { get; set; }
        public string[] Headers { get; private set; }

        public List<string> SelectedNeuronRecords { get; private set; }

        public VisualMap(int width, int height, DataSource source, CanvasCreator canvasCreator, string[] headers)
            : base(width, height, source, canvasCreator)
        {
            Headers = headers;
        }

        public override void Init(Dictionary<Neuron, List<DataToken>> clusterResults)
        {
            ClusterResults = clusterResults;

            SetCellSize(ClusterResults.Keys);
            DrawMaps();
        }

        private void SetCellSize(IEnumerable<Neuron> neurons)
        {
            double mapWidth = neurons.Select(neuron => neuron.Position.X).Max() + 1;
            double mapHeight = neurons.Select(neuron => neuron.Position.Y).Max() + 1;

            NeuronWidth = (int)(Width / mapWidth);
            NeuronHeight = (int)(Height / mapHeight);
        }

        private void DrawMaps()
        {
            Maps = new Dictionary<string, Canvas>();
            for (int i = 0; i < Source.DataTypes.Length; i++)
            {
                Maps.Add(Headers[i], DrawMap(i, Source.DataTypes[i]));
            }
        }

        private Canvas DrawMap(int index, DataType dataType)
        {
            switch (dataType)
            {
                case DataType.Numerical:
                    var numericalData = SourceTokens.Select(token => double.Parse(token.Values[index].ToString()));
                    MapDrawer<double> numericalDrawer = new NumericalMapDrawer(ClusterResults, index, NeuronWidth, NeuronHeight, ComponentFactory, numericalData);
                    return numericalDrawer.Draw(Width, Height);

                case DataType.Categorial:
                    var categorialData = SourceTokens.Select(token => token.Values[index].ToString());
                    MapDrawer<string> categorialDrawer = new CategorialMapDrawer(ClusterResults, index, NeuronWidth, NeuronHeight, ComponentFactory, categorialData);
                    return categorialDrawer.Draw(Width, Height);

                case DataType.Semantic:
                    MapDrawer<SemanticPair[]> semanticDrawer = new SemanticMapDrawer(ClusterResults, index, NeuronWidth, NeuronHeight, ComponentFactory);
                    return semanticDrawer.Draw(Width, Height);
                default:
                    return null;
            }
        }

        public Canvas Show()
        {
            List<string> keys = Maps.Keys.ToList();
            string attribute = keys[Index];
            return Maps[attribute];
        }

        public void MouseClick(object sender, int x, int y)
        {
            int index = (int)sender;

            int i = x / NeuronWidth;
            int j = y / NeuronHeight;

            Neuron neuron = ClusterResults.Keys.First(n => n.Position.X == i && n.Position.Y == j);
            SelectedNeuronRecords = ClusterResults[neuron].Select(token => token.Values[index].ToString()).ToList();
        }
    }
#warning Add statistics to the map output
}
