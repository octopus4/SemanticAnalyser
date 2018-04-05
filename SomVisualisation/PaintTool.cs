using System;
using Visualisation.Graph;

namespace Visualisation
{
    public abstract class PaintTool : IDisposable
    {
        public float FontSize { get; }
        public int? Opacity { get; set; }

        internal PaintTool()
        {
#warning magic number => put into ctor
            FontSize = 24.0f;
        }

        internal abstract void StartRendering();
        internal abstract void SetColor(ColorAdapter color);
        internal abstract void DrawNode(float x, float y, Node node);
        internal abstract void DrawLine(float x1, float y1, float x2, float y2, ColorAdapter color);
        internal abstract void DrawCluster(float x, float y, float width, float height);
        internal abstract void DrawCluster(float x, float y, float width, float height, ColorAdapter color);
        internal abstract void DrawText(string text, float x, float y);
        public abstract void Dispose();
    }
}
