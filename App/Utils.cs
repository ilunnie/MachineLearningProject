using System;
using System.Drawing;
using ColorMine.ColorSpaces;

public static class Utils
{
    public static double[] Luminosity(this Bitmap bitmap)
    {
        double[] luminosity = new double[bitmap.Width * bitmap.Height];

        for (int x = 0; x < bitmap.Width; x++)
            for (int y = 0; y < bitmap.Height; y++)
            {
                Color pixel = bitmap.GetPixel(x, y);

                double L = (0.299 * pixel.R + 0.587 * pixel.G + 0.114 * pixel.B) * 100 / 255.0;

                luminosity[bitmap.Width * y + x] = L;
            }

        return luminosity;
    }

    public static Color LabToRgb(double L, double a, double b)
    {
        Lab lab = new Lab { L = L, A = a, B = b };
        Rgb rgb = lab.To<Rgb>();

        return Color.FromArgb((int)rgb.R, (int)rgb.G, (int)rgb.B);
    }
}