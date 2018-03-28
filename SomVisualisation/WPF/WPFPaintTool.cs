using System.Globalization;
using System.Windows;
using System.Windows.Media;

namespace SomVisualisation.WPF
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
            CultureInfo culture = CultureInfo.CurrentCulture;
            FlowDirection flow = FlowDirection.LeftToRight;
            Typeface typeface = new Typeface(new FontFamily("Segoe UI"), FontStyles.Normal, FontWeights.SemiBold, FontStretches.SemiCondensed);
            Context.DrawText(new FormattedText(text, culture, flow, typeface, FontSize, Brushes.Black), new Point(x, y));
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
    }
}
