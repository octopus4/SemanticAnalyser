using System.Collections.Generic;
using System.Linq;
using DataProcessing.Data;
using DataProcessing.Data.Semantic;
using SOM;
using SomVisualisation.Drawers;
using SomVisualisation.Drawers.Numerical;
using SomVisualisation.Drawers.Categorial;
using SomVisualisation.Drawers.Semantic;

namespace SomVisualisation
{
    public class VisualMap
    {
        private DataType MapType { get; set; }

        private DataSource Source { get; }

        private int NeuronWidth { get; set; }
        private int NeuronHeight { get; set; }

        private List<DataToken> SourceTokens { get; set; }

        private CanvasCreator ComponentFactory { get; set; }
        private Dictionary<string, Canvas> Maps { get; set; }
        private Dictionary<Neuron, List<DataToken>> ClusterResults { get; set; }

        public int Index { get; set; }

        public int Width { get; }
        public int Height { get; }

        public string[] Headers { get; private set; }

        public List<string> SelectedNeuronRecords { get; private set; }

        public VisualMap(int width, int height, DataSource source, CanvasCreator canvasCreator)
        {
            Width = width;
            Height = height;
            Source = source;
            ComponentFactory = canvasCreator;

            InitSourceTokens();
        }

        private void InitSourceTokens()
        {
            SourceTokens = new List<DataToken>();
            foreach (DataToken token in Source)
            {
                SourceTokens.Add(token);
            }
        }

        public void Init(Dictionary<Neuron, List<DataToken>> clusterResults, string[] headers)
        {
            ClusterResults = clusterResults;

            SetCellSize(ClusterResults.Keys);
            CreateMaps(headers);
            Headers = headers;
        }

        private void CreateMaps(string[] headers)
        {
            Maps = new Dictionary<string, Canvas>();
            for (int i = 0; i < Source.DataTypes.Length; i++)
            {
                Maps.Add(headers[i], CreateMap(i, Source.DataTypes[i]));
            }
        }

        private Canvas CreateMap(int index, DataType dataType)
        {
            switch (dataType)
            {
                case DataType.Numerical:
                    var numericalData = SourceTokens.Select(token => double.Parse(token.Values[index].ToString()));
                    MapDrawer<double> numericalDrawer = new NumericalMapDrawer(ClusterResults, index, NeuronWidth, NeuronHeight, ComponentFactory);
                    return numericalDrawer.Draw(numericalData, Width, Height);

                case DataType.Categorial:
                    var categorialData = SourceTokens.Select(token => token.Values[index].ToString());
                    MapDrawer<string> categorialDrawer = new CategorialMapDrawer(ClusterResults, index, NeuronWidth, NeuronHeight, ComponentFactory);
                    return categorialDrawer.Draw(categorialData, Width, Height);

                case DataType.Semantic:
                    var semanticData = SourceTokens.Select(token => (SemanticPair[])token.Values[1]);
                    MapDrawer<SemanticPair[]> semanticDrawer = new SemanticMapDrawer(ClusterResults, index, NeuronWidth, NeuronHeight, ComponentFactory);
                    return semanticDrawer.Draw(semanticData, Width, Height);
                default:
                    return null;
            }
        }

        private void SetCellSize(IEnumerable<Neuron> neurons)
        {
            double MapWidth = neurons.Select(neuron => neuron.Position.X).Max() + 1;
            double MapHeight = neurons.Select(neuron => neuron.Position.Y).Max() + 1;

            NeuronWidth = (int)(Width / MapWidth);
            NeuronHeight = (int)(Height / MapHeight);
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
