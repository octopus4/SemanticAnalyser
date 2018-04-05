namespace Visualisation
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

        public static bool operator == (ColorAdapter left, ColorAdapter right)
        {
            if (ReferenceEquals(left, right))
            {
                return true;
            }
            if (!ReferenceEquals(left, null) || !ReferenceEquals(right, null))
            {
                return false;
            }

            return left.Equals(right);
        }

        public static bool operator !=(ColorAdapter left, ColorAdapter right)
        {
            return !(left == right);
        }

        public override bool Equals(object obj)
        {
            var adapter = obj as ColorAdapter;
            return adapter != null &&
                A == adapter.A &&
                R == adapter.R &&
                G == adapter.G &&
                B == adapter.B;
        }

        public override int GetHashCode()
        {
            var hashCode = -1749689076;
            hashCode = hashCode * -1521134295 + A.GetHashCode();
            hashCode = hashCode * -1521134295 + R.GetHashCode();
            hashCode = hashCode * -1521134295 + G.GetHashCode();
            hashCode = hashCode * -1521134295 + B.GetHashCode();
            return hashCode;
        }
    }
}
