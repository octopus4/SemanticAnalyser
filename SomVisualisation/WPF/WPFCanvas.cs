using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Visualisation.WPF
{
    public class WpfCanvas : Canvas
    {
        private Visual Visual { get; set; }

        public WpfCanvas(int width, int height) : base(width, height)
        {
        }

        protected override PaintTool CreateTool()
        {
            WpfPaintTool tool = new WpfPaintTool(24.0f, 1);
            Visual = tool.Visual;

            return tool;
        }

        public override object Render()
        {
            RenderTargetBitmap bmp = new RenderTargetBitmap(Width, Height, 96, 96, PixelFormats.Default);
            bmp.Render(Visual);

            return bmp;
        }
    }
}
