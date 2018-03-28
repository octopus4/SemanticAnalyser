namespace SomVisualisation
{
    public static class Colorizer
    {
        public static ColorAdapter[] CreatePalette(int colors)
        {
            ColorAdapter[] palette = new ColorAdapter[colors];

            for (int i = 0; i < palette.Length; i++)
            {
                palette[i] = Colorize(i, colors);
            }

            return palette;
        }

        public static ColorAdapter Colorize(int index, int count)
        {
            if (count < 2)
            {
                count = 2;
            }

            int x = index * 1024 / (count - 1);
            if (x > 1024)
            {
                x -= 1024;
            }

            int r = R(x);
            int g = G(x);
            int b = B(x);

            return new ColorAdapter(r, g, b);
        }

        private static int R(int x)
        {
            if (x < 512)
            {
                return 0;
            }
            if (x < 767)
            {
                return x - 512;
            }

            return 255;
        }

        private static int G(int x)
        {
            if (x < 256)
            {
                return x;
            }
            if (x > 768)
            {
                return 1024 - x;
            }

            return 255;
        }

        private static int B(int x)
        {
            if (x > 511)
            {
                return 0;
            }
            if (x > 255)
            {
                return 511 - x;
            }

            return 255;
        }
    }
}
