using System.Windows.Media;

namespace SomVisualisation.WPF
{
    public class WPFCanvas : Canvas
    {
        public Visual Visual { get; }

        public WPFCanvas(int width, int height) : base(width, height)
        {
            Visual = ((WPFPaintTool)Tool).Visual;
        }

        protected override PaintTool CreateTool()
        {
            return new WPFPaintTool();
        }
    }
}
