using System.Collections.Generic;
using System.Linq;
using DataPreprocessing.Data;
using DataPreprocessing.Data.Semantic;
using Visualisation.SOM.Drawers;
using Visualisation.SOM.Drawers.Numerical;
using Visualisation.SOM.Drawers.Categorial;
using Visualisation.SOM.Drawers.Semantic;
using SOM;
using System;

namespace Visualisation.SOM
{
    public class MapPresenter : DataPresenter
    {
        private double MapWidth { get; set; }
        private double MapHeight { get; set; }
        private double NeuronWidth { get; set; }
        private double NeuronHeight { get; set; }
        private List<Cluster> Clusters { get; set; }
        private Dictionary<string, Canvas> Maps { get; set; }

        public int Index { get; set; }
        public string[] Headers { get; private set; }
        public List<string> SelectedNeuronRecords { get; private set; }

        public MapPresenter(int width, int height, DataSource source, CanvasCreator componentCreator, string[] headers, IView view)
            : base(width, height, source, componentCreator, view)
        {
            CaptureArea = new Rectangle(0, 0, 1, 1);
            Headers = headers;
        }

        public override void Init(Dictionary<Neuron, List<DataToken>> clusterResults)
        {
            ClusterResults = clusterResults;
            Clusters = clusterResults.Select(pair => new Cluster(pair.Key, pair.Value)).ToList();

            MapWidth = ClusterResults.Keys.Select(neuron => neuron.Position.X).Max() + 1;
            MapHeight = ClusterResults.Keys.Select(neuron => neuron.Position.Y).Max() + 1;

            SetCellSize();
            DrawMaps();
            Invalidate();
        }

        private void SetCellSize()
        {
            NeuronWidth = ToScreen(1 / MapWidth, Width);
            NeuronHeight = ToScreen(1 / MapHeight, Height);
        }

        private void DrawMaps()
        {
            Maps = new Dictionary<string, Canvas>();
            for (int i = 0; i < Source.DataTypes.Length; i++)
            {
                Maps.Add(Headers[i], DrawMap(i));
            }
        }

        private Canvas DrawMap(int index)
        {
            Action<PaintTool, Cluster> action = GetDrawingAction(index, Source.DataTypes[index]);
            Canvas canvas = ComponentCreator.CreateCanvas(Width, Height);
            PaintTool tool = canvas.Tool;
            tool.StartRendering();
            foreach (Cluster cluster in Clusters)
            {
                cluster.ToRelative(CaptureArea);
                action(tool, cluster);
            }
            tool.Dispose();

            return canvas;
        }

        private Action<PaintTool, Cluster> GetDrawingAction(int index, DataType dataType)
        {
            SetCellSize();
            switch (dataType)
            {
                case DataType.Numerical:
                    var numericalData = SourceTokens.Select(token => double.Parse(token.Values[index].ToString()));
                    MapDrawer<double> numericalDrawer = new NumericalMapDrawer(ClusterResults, index, NeuronWidth, NeuronHeight, numericalData);
                    return numericalDrawer.Draw;

                case DataType.Categorial:
                    var categorialData = SourceTokens.Select(token => token.Values[index].ToString());
                    MapDrawer<string> categorialDrawer = new CategorialMapDrawer(ClusterResults, index, NeuronWidth, NeuronHeight, categorialData);
                    return categorialDrawer.Draw;

                case DataType.Semantic:
                    MapDrawer<SemanticPair[]> semanticDrawer = new SemanticMapDrawer(ClusterResults, index, NeuronWidth, NeuronHeight);
                    return semanticDrawer.Draw;

                default:
                    throw new ArgumentException("Provided data type is not valid", nameof(dataType));
            }
        }

        protected override void MouseClick(double x, double y)
        {
            return;
        }

        protected override void OnInvalidate()
        {
            List<string> keys = Maps.Keys.ToList();
            string attribute = keys[Index];
            Maps[attribute] = DrawMap(Index);
            Image = Maps[attribute];
        }
    }
}
