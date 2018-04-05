namespace Visualisation.WPF
{
    public class WPFCanvasCreator : CanvasCreator
    {
        internal override Canvas CreateCanvas(int width, int height)
        {
            return new WPFCanvas(width, height);
        }
    }
}
