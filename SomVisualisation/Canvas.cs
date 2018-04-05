namespace Visualisation
{
    public abstract class Canvas
    {
        public int Width { get; private set; }
        public int Height { get; private set; }

        public PaintTool Tool { get; }

        public Canvas(int width, int height)
        {
            Width = width;
            Height = height;

            Tool = CreateTool();
        }

        protected abstract PaintTool CreateTool();
    }
}
