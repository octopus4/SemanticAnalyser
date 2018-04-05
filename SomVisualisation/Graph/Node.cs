using System.Collections.Generic;

namespace Visualisation.Graph
{
    internal class Node
    {
        private static readonly double Diminisher = 2;
        public static readonly double Radius = 6;

        private double Scaler { get; set; }

        public double X { get; private set; }
        public double Y { get; private set; }

        public string Word { get; }
#warning switch list to array
        public List<Node> ConnectedTo { get; private set; }
        public ColorAdapter Color { get; set; }

        public Node(string word, double x, double y)
        {
            X = x;
            Y = y;
            Word = word;
            Scaler = 0.25;
            ConnectedTo = new List<Node>();
        }

        public void ConnectWith(Node neighbor)
        {
            if (!ConnectedTo.Contains(neighbor))
            {
                ConnectedTo.Add(neighbor);
                MoveToward(neighbor);
                Scaler /= Diminisher;
            }
        }

        public void MoveToward(Node neighbor)
        {
            X += -(X - neighbor.X) * Scaler;
            Y += -(Y - neighbor.Y) * Scaler;
        }
    }
}
