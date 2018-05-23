namespace Visualisation.WPF
{
    public class WpfCanvasCreator : CanvasCreator
    {
        internal override Canvas CreateCanvas(int width, int height)
        {
            Canvas canvas = new WpfCanvas(width, height);
            canvas.Init();
            return canvas;
        }
    }
}
