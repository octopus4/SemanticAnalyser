using System.Globalization;
using System.Windows;
using System.Windows.Media;
using Visualisation.Graph;

namespace Visualisation.WPF
{
    internal class WPFPaintTool : PaintTool
    {
        private Color Color { get; set; }
        private DrawingContext Context { get; set; }
        internal DrawingVisual Visual { get; }

        internal WPFPaintTool() : base()
        {
            Visual = new DrawingVisual();
        }

        internal override void DrawCluster(float x, float y, float width, float height)
        {
            Context.DrawRectangle(Brushes.White, new Pen(Brushes.Black, 2), new Rect(x, y, width, height));
        }

        internal override void DrawCluster(float x, float y, float width, float height, ColorAdapter color)
        {
            Color brushColor = Color.FromArgb(color.A, color.R, color.G, color.B);
            Brush brush = new SolidColorBrush(brushColor);
            Context.DrawRectangle(brush, new Pen(Brushes.Black, 0.7), new Rect(x, y, width, height));
        }

        internal override void DrawText(string text, float x, float y)
        {
            byte alpha = Opacity.HasValue ? (byte)(Opacity.Value) : (byte)255;
            Brush brush = new SolidColorBrush(Color.FromArgb(alpha, 0, 0, 0));
            CultureInfo culture = CultureInfo.CurrentCulture;
            FlowDirection flow = FlowDirection.LeftToRight;
            Typeface typeface = new Typeface(new FontFamily("Segoe UI"), FontStyles.Normal, FontWeights.SemiBold, FontStretches.SemiCondensed);
            Context.DrawText(new FormattedText(text, culture, flow, typeface, FontSize, brush), new Point(x, y));
        }

        internal override void SetColor(ColorAdapter color)
        {
            Color = Color.FromArgb(color.A, color.R, color.G, color.B);
        }

        internal override void StartRendering()
        {
            if (Context == null)
            {
                Context = Visual.RenderOpen();
            }
        }

        public override void Dispose()
        {
            Context.Close();
        }

        internal override void DrawNode(float x, float y, Node node)
        {
            byte alpha = Opacity.HasValue ? (byte)Opacity : node.Color.A;
            Color brushColor = Color.FromArgb(alpha, node.Color.R, node.Color.G, node.Color.B);
            Brush brush = new SolidColorBrush(brushColor);
            Context.DrawEllipse(brush, new Pen(brush, 2), new Point(x, y), Node.Radius, Node.Radius);
            DrawText(node.Word, x + 2, y - 2);
        }

        internal override void DrawLine(float x1, float y1, float x2, float y2, ColorAdapter color)
        {
            byte alpha = Opacity.HasValue ? (byte)Opacity : color.A;
            Color brushColor = Color.FromArgb(alpha, color.R, color.G, color.B);
            Brush brush = new SolidColorBrush(brushColor);
            Context.DrawLine(new Pen(brush, 2), new Point(x1, y1), new Point(x2, y2));
        }
    }
}
