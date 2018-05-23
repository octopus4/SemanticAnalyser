namespace Visualisation
{
    public abstract class Canvas
    {
        public int Width { get; private set; }
        public int Height { get; private set; }

        public PaintTool Tool { get; private set; }

        protected Canvas(int width, int height)
        {
            Width = width;
            Height = height;
        }

        public void Init()
        {
            Tool = CreateTool();
        }

        protected abstract PaintTool CreateTool();

        public abstract object Render();
    }
}
