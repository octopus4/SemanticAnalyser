using System.Collections.Generic;

namespace Visualisation.Graph
{
    internal class Node
    {
        private static readonly double Diminisher = 2;
        public static readonly double Radius = 6;

        private double Prescaler { get; set; }

        public double X { get; set; }
        public double Y { get; set; }

        public double RelativeX { get; private set; }
        public double RelativeY { get; private set; }

        public string Word { get; }
        public List<Node> ConnectedTo { get; private set; }
        public ColorAdapter Color { get; set; }

        public Node(string word, double x, double y)
        {
            X = x;
            Y = y;
            Word = word;
            Prescaler = 2;
            ConnectedTo = new List<Node>();
        }

        public void ConnectWith(Node neighbor)
        {
            if (!ConnectedTo.Contains(neighbor))
            {
                ConnectedTo.Add(neighbor);
            }
        }

        public void MoveToward(Node neighbor)
        {
            X += -(X - neighbor.X) / Prescaler;
            Y += -(Y - neighbor.Y) / Prescaler;
            Prescaler *= Diminisher;
        }

        public void ToRelative(Rectangle captureArea)
        {
            RelativeX = X - captureArea.X;
            RelativeY = Y - captureArea.Y;
        }
    }
}
