using System.Collections.Generic;
using DataProcessing.Data;
using SOM;

namespace Visualisation
{
    public abstract class DataPresenter
    {
        protected bool IsDragging { get; set; }
        protected double Scale { get; set; }
        protected double X { get; set; }
        protected double Y { get; set; }
        protected Rectangle CaptureArea { get; set; }
        protected IView View { get; private set; }
        protected CanvasCreator ComponentCreator { get; set; }

        protected DataSource Source { get; }
        protected List<DataToken> SourceTokens { get; set; }
        protected Dictionary<Neuron, List<DataToken>> ClusterResults { get; set; }


        public Canvas Image { get; protected set; }
        public int Width { get; }
        public int Height { get; }

        protected DataPresenter(int width, int height, DataSource source, CanvasCreator componentCreator, IView view)
        {
            Width = width;
            Height = height;
            Source = source;
            ComponentCreator = componentCreator;
            SourceTokens = new List<DataToken>(Source);
            View = view;
        }

        protected double ToScreen(double coordinate, double transformer)
        {
            return (transformer * coordinate) * Scale;
        }

        protected double FromScreen(double coordinate, double transformer)
        {
            return coordinate / (Scale * transformer);
        }

        public abstract void Init(Dictionary<Neuron, List<DataToken>> clusterResults);

        public void MouseDown(double x, double y)
        {
            X = FromScreen(x, Width);
            Y = FromScreen(y, Height);
            IsDragging = true;
            MouseClick(x, y);
        }

        public void MouseUp(double x, double y)
        {
            IsDragging = false;
        }

        protected abstract void MouseClick(double x, double y);

        public void MouseWheel(double x, double y, int delta)
        {
            x = FromScreen(x, Width) + CaptureArea.X;
            y = FromScreen(y, Height) + CaptureArea.Y;
            Scale = delta > 0 ? Scale * 1.125 : Scale / 1.125;
            ScaleArea(x, y);
            Invalidate();
        }

        private void ScaleArea(double x, double y)
        {
            CaptureArea.Width = 1 / Scale;
            CaptureArea.Height = 1 / Scale;

            CaptureArea.X = x - CaptureArea.Width / 3;
            CaptureArea.Y = y - CaptureArea.Height / 3;
        }

        public void MouseMove(double x, double y)
        {
            x = FromScreen(x, Width);
            y = FromScreen(y, Height);
            if (IsDragging)
            {
                double dx = X - x;
                double dy = Y - y;
                CaptureArea.X += dx;
                CaptureArea.Y += dy;
                Invalidate();
            }
            X = x;
            Y = y;
        }

        protected void Invalidate()
        {
            OnInvalidate();
            View.Update(Image);
        }

        protected abstract void OnInvalidate();
    }
}
