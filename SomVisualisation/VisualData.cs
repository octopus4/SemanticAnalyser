using System.Collections.Generic;
using DataProcessing.Data;
using SOM;

namespace Visualisation
{
    public abstract class VisualData
    {
        protected CanvasCreator ComponentFactory { get; set; }

        protected DataSource Source { get; }
        protected List<DataToken> SourceTokens { get; set; }
        protected Dictionary<Neuron, List<DataToken>> ClusterResults { get; set; }

        public int Width { get; }
        public int Height { get; }

        public VisualData(int width, int height, DataSource source, CanvasCreator canvasCreator)
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

        public abstract void Init(Dictionary<Neuron, List<DataToken>> clusterResults);
    }
}
