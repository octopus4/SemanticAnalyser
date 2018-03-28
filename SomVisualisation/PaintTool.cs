using System;

namespace SomVisualisation
{
    public abstract class PaintTool : IDisposable
    {
        public float FontSize { get; }

        internal PaintTool()
        {
            FontSize = 24.0f;
        }

        internal abstract void StartRendering();
        internal abstract void SetColor(ColorAdapter color);
        internal abstract void DrawCluster(float x, float y, float width, float height);
        internal abstract void DrawCluster(float x, float y, float width, float height, ColorAdapter color);
        internal abstract void DrawText(string text, float x, float y);
        public abstract void Dispose();
    }
}
