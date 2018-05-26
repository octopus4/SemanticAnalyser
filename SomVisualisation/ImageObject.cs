namespace Visualisation
{
    internal abstract class ImageObject
    {
        public double X { get; protected set; }
        public double Y { get; protected set; }

        public double RelativeX { get; private set; }
        public double RelativeY { get; private set; }

        protected ImageObject(double x, double y)
        {
            X = x;
            Y = y;
        }
        
        public void ToRelative(Rectangle captureArea)
        {
            RelativeX = X - captureArea.X;
            RelativeY = Y - captureArea.Y;
        }
    }
}
