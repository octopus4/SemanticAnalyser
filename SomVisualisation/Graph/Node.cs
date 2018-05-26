using System.Collections.Generic;

namespace Visualisation.Graph
{
    internal class Node : ImageObject
    {
        private static readonly double Diminisher = 2;
        public static readonly double Radius = 6;

        private double Prescaler { get; set; }

        public string Word { get; }
        public List<Node> ConnectedTo { get; private set; }
        public ColorAdapter Color { get; set; }

        public Node(string word, double x, double y) : base(x, y)
        {
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
    }
}
