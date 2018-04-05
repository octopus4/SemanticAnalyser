using System;
using System.Collections.Generic;
using System.Linq;
using DataProcessing.Data;
using DataProcessing.Data.Semantic;
using SOM;

namespace Visualisation.Graph
{
    public class VisualGraph : VisualData
    {
        private List<Node> Nodes { get; set; }
        private SemanticMatrix Matrix { get; set; }
        private ColorAdapter SelectedColor { get; set; }
        private PaintTool Tool { get; set; }

        public Canvas Image { get; private set; }

        public VisualGraph(int width, int height, DataSource source, CanvasCreator canvasCreator)
            : base(width, height, source, canvasCreator)
        {
            Matrix = SemanticDataSource.Matrix;
            InitNodes();
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
            DrawGraph();
        }

        private void MakeGraph()
        {
            for (int i = 0; i < Matrix.Length; i++)
            {
                double[] vector = Matrix.GetVector(1, i);
                double[] sortedVector = new double[vector.Length];
                vector.CopyTo(sortedVector, 0);
                Array.Sort(sortedVector);
                int index = (int)(0.5 * sortedVector.Length);
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
            List<Neuron> neurons = ClusterResults.Keys.ToList();
            ColorAdapter[] palette = Colorizer.CreatePalette(neurons.Count);
            int colorIndex = 0;
            foreach (Neuron neuron in neurons)
            {
                List<string> words = ClusterResults[neuron].Select(token => token.Values[0].ToString()).ToList();
                List<Node> nodes = Nodes.Where(node => words.Contains(node.Word)).ToList();
                foreach (Node node in nodes)
                {
                    node.Color = palette[colorIndex];
                }
                colorIndex++;
            }
        }

        private void DrawGraph()
        {
            Image = ComponentFactory.CreateCanvas(Width, Height);

            using (Tool = Image.Tool)
            {
                Tool.StartRendering();
                Tool.DrawCluster(0, 0, Width, Height);
                foreach (Node node in Nodes)
                {
                    Show(node, Tool);
                }
            }
        }

        private void Show(Node node, PaintTool tool)
        {
            if (node.ConnectedTo.Count == 0)
            {
                return;
            }

            float x = ToScreen(node.X, Width);
            float y = ToScreen(node.Y, Height);

            if (SelectedColor != null)
            {
                Tool.Opacity = SelectedColor != node.Color ? 32 : (int?)null;
            }
            tool.DrawNode(x, y, node);
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
                float neighborX = ToScreen(neighbor.X, Width);
                float neighborY = ToScreen(neighbor.Y, Height);
                tool.DrawLine(x, y, neighborX, neighborY, node.Color);
            }
            Tool.Opacity = null;
        }

        private float ToScreen(double coordinate, double transformer)
        {
            return (float)transformer * (1 + (float)coordinate) / 2;
        }

        private double FromScreen(double coordinate, double transformer)
        {
            return 2 * coordinate / transformer - 1;
        }

        public void MouseClick(int x, int y)
        {
            double nodeX = FromScreen(x, Width);
            double nodeY = FromScreen(y, Height);

            Node selectedNode = Nodes.FirstOrDefault(node => Math.Abs(node.X - nodeX) <= Node.Radius / Width && Math.Abs(node.Y - nodeY) <= Node.Radius / Height);
            SelectedColor = selectedNode?.Color;
            Invalidate();
        }

        private void Invalidate()
        {
            DrawGraph();
        }
    }
}
