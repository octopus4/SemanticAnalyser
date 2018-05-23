using System.Globalization;
using System.Windows;
using System.Windows.Media;
using Visualisation.Graph;

namespace Visualisation.WPF
{
    internal class WpfPaintTool : PaintTool
    {
        private Color Color { get; set; }
        private DrawingContext Context { get; set; }
        internal DrawingVisual Visual { get; }

        internal WpfPaintTool(float fontSize, double scale) : base(fontSize, scale)
        {
            Visual = new DrawingVisual();
        }

        internal override void DrawArea(double x, double y, double width, double height)
        {
            Context.DrawRectangle(Brushes.White, new Pen(Brushes.Black, 2), new Rect(x, y, width, height));
        }

        internal override void DrawArea(double x, double y, double width, double height, ColorAdapter color)
        {
            Color brushColor = Color.FromArgb(color.A, color.R, color.G, color.B);
            Brush brush = new SolidColorBrush(brushColor);
            Context.DrawRectangle(brush, new Pen(Brushes.Black, 0.7), new Rect(x, y, width, height));
        }

        internal override void DrawText(string text, double x, double y)
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

        internal override void DrawNode(double x, double y, Node node)
        {
            byte alpha = Opacity.HasValue ? (byte)Opacity : node.Color.A;
            Color brushColor = Color.FromArgb(alpha, node.Color.R, node.Color.G, node.Color.B);
            Brush brush = new SolidColorBrush(brushColor);
            double radius = Scale > 1.75 ? Node.Radius * NodeRadiusScale : Node.Radius * Scale;
            Context.DrawEllipse(brush, new Pen(brush, 2), new Point(x, y), radius, radius);
            if (Scale > 0.325)
            {
                DrawText(node.Word, x + 2, y - 2);
            }
        }

        internal override void DrawLine(double x1, double y1, double x2, double y2, ColorAdapter color)
        {
            byte alpha = Opacity.HasValue ? (byte)Opacity : color.A;
            Color brushColor = Color.FromArgb(alpha, color.R, color.G, color.B);
            Brush brush = new SolidColorBrush(brushColor);
            Context.DrawLine(new Pen(brush, 1), new Point(x1, y1), new Point(x2, y2));
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                Context.Close();
            }
        }
    }
}
