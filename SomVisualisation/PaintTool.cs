using System;
using Visualisation.Graph;

namespace Visualisation
{
    public abstract class PaintTool : IDisposable
    {
        protected static readonly double NodeRadiusScale = 1.5;

        public double Scale { get; set; }
        public double FontSize { get; }
        public int? Opacity { get; set; }

        protected PaintTool(float fontSize, double scale)
        {
            FontSize = fontSize;
            Scale = scale;
        }

        internal abstract void StartRendering();
        internal abstract void SetColor(ColorAdapter color);
        internal abstract void DrawNode(double x, double y, Node node);
        internal abstract void DrawLine(double x1, double y1, double x2, double y2, ColorAdapter color);
        internal abstract void DrawArea(double x, double y, double width, double height);
        internal abstract void DrawArea(double x, double y, double width, double height, ColorAdapter color);
        internal abstract void DrawText(string text, double x, double y);
        protected abstract void Dispose(bool disposing);

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
