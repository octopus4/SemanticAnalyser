namespace SomVisualisation
{
    public class ColorAdapter
    {
        public static readonly ColorAdapter Default = new ColorAdapter(0, 0, 0);

        public byte A { get; }
        public byte R { get; }
        public byte G { get; }
        public byte B { get; }

        public ColorAdapter(byte a, byte r, byte g, byte b)
        {
            A = a;
            R = r;
            G = g;
            B = b;
        }

        public ColorAdapter(byte r, byte g, byte b) : this((byte)255, r, g, b)
        {
        }

        public ColorAdapter(int r, int g, int b) : this((byte)r, (byte)g, (byte)b)
        {
        }

        public ColorAdapter(int a, int r, int g, int b) : this((byte)a, (byte)b, (byte)g, (byte)b)
        {
        }
    }
}
