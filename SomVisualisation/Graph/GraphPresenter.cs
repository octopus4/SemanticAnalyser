using System;
using System.Collections.Generic;
using System.Linq;
using DataProcessing.Data;
using DataProcessing.Data.Semantic;
using SOM;

namespace Visualisation.Graph
{
    public class GraphPresenter : DataPresenter
    {
        private List<Node> Nodes { get; set; }
        private SemanticMatrix Matrix { get; set; }
        private ColorAdapter SelectedColor { get; set; }
        private PaintTool Tool { get; set; }
        public int RelativeWidth { get; private set; }
        public int RelativeHeight { get; private set; }

        public GraphPresenter(int width, int height, DataSource source, CanvasCreator componentCreator, IView view)
            : base(width, height, source, componentCreator, view)
        {
            Matrix = SemanticDataSource.Matrix;
            InitNodes();
            Scale = 1;
            CaptureArea = new Rectangle(-0.5, -0.5, 1, 1);
            RelativeHeight = Height;
            RelativeWidth = Width;
        }

        private void InitNodes()
        {
            Nodes = new List<Node>(Matrix.Length);
            double angle = 0;
            for (int i = 0; i < Matrix.Length; i++)
            {
                double x = 0.8 * Math.Cos(angle);
                double y = 0.8 * Math.Sin(angle);
                Nodes.Add(new Node(Matrix[i], x, y));
                angle += 2 * Math.PI / Matrix.Length;
            }
        }

        public override void Init(Dictionary<Neuron, List<DataToken>> clusterResults)
        {
            ClusterResults = clusterResults;
            ColorizeNodes();
            MakeGraph();
            Invalidate();
        }

        private void MakeGraph()
        {
            for (int i = 0; i < Matrix.Length; i++)
            {
                double[] vector = Matrix.GetVector(1, i);
                double[] sortedVector = vector.OrderBy(value => value).ToArray();

                int index = (int)(sortedVector.Length / 2.0);
                double stepValue = sortedVector[index];
                for (int j = 0; j < Matrix.Length; j++)
                {
                    Node firstNode = Nodes.First(node => node.Word == Matrix[i]);
                    Node secondNode = Nodes.First(node => node.Word == Matrix[j]);
                    double frequence = Matrix.GetVector(1, i)[j];
                    if (frequence > stepValue)
                    {
                        for (int k = 0; k < Matrix[i, j]; k++)
                        {
                            firstNode.ConnectWith(secondNode);
                            secondNode.ConnectWith(firstNode);
                        }
                    }
                }
            }
        }

        private void ColorizeNodes()
        {
            List<Neuron> neurons = ClusterResults.Keys.Where(key => ClusterResults[key].Count > 0).ToList();
            ColorAdapter[] palette = Colorizer.CreatePalette(neurons.Count);
            int colorIndex = 0;
            foreach (Neuron neuron in neurons)
            {
                List<string> words = ClusterResults[neuron].Select(token => token.Values[0].ToString()).ToList();
                List<Node> nodes = Nodes.Where(node => words.Contains(node.Word)).ToList();
                foreach (Node node in nodes)
                {
                    node.Color = palette[colorIndex];
                    foreach (Node neighbor in nodes)
                    {
                        if (node != neighbor)
                        {
                            node.MoveToward(neighbor);
                        }
                    }
                }
                colorIndex++;
            }
        }

        private void DrawGraph()
        {
            Image = ComponentCreator.CreateCanvas(Width, Height);
            Image.Init();

            using (Tool = Image.Tool)
            {
                Tool.Scale = Scale;
                Tool.StartRendering();
                Tool.DrawArea(0, 0, Width, Height);
                Nodes.ForEach(node => node.ToRelative(CaptureArea));
                foreach (Node node in Nodes)
                {
                    Show(node);
                }
            }
        }

        private void Show(Node node)
        {
            if (node.ConnectedTo.Count == 0)
            {
                return;
            }

            double x = ToScreen(node.RelativeX, Width);
            double y = ToScreen(node.RelativeY, Height);

            if (SelectedColor != null)
            {
                Tool.Opacity = SelectedColor != node.Color ? 32 : (int?)null;
            }
            Tool.DrawNode(x, y, node);
            ShowNeighbors(node, x, y);
        }

        private void ShowNeighbors(Node node, double x, double y)
        {
            foreach (Node neighbor in node.ConnectedTo)
            {
                if (SelectedColor != null)
                {
                    Tool.Opacity = SelectedColor != neighbor.Color ? 32 : 64;
                }
                else
                {
                    Tool.Opacity = 192;
                }
                double neighborX = ToScreen(neighbor.RelativeX, Width);
                double neighborY = ToScreen(neighbor.RelativeY, Height);
                Tool.DrawLine(x, y, neighborX, neighborY, node.Color);
            }
            Tool.Opacity = null;
        }

        protected sealed override void MouseClick(double x, double y)
        {
            double mouseX = FromScreen(x, Width);
            double mouseY = FromScreen(y, Height);

            Node selectedNode = Nodes.FirstOrDefault(node => IsChoosen(node, mouseX, mouseY));
            SelectedColor = selectedNode?.Color;
            Invalidate();
        }

        private bool IsChoosen(Node node, double mouseX, double mouseY)
        {
            return Math.Abs(node.RelativeX - mouseX) <= Node.Radius / (Scale * Width)
                && Math.Abs(node.RelativeY - mouseY) <= Node.Radius / (Scale * Height);
        }

        protected sealed override void OnInvalidate()
        {
            DrawGraph();
        }
    }
}
