using System;
using System.Drawing;

public static class Utils
{
    public static double[] Luminosity(this Bitmap bitmap)
    {
        double[] luminosity = new double[bitmap.Width * bitmap.Height];

        for (int x = 0; x < bitmap.Width; x++)
            for (int y = 0; y < bitmap.Height; y++)
            {
                Color pixel = bitmap.GetPixel(x, y);

                double L = 0.299 * pixel.R + 0.587 * pixel.G + 0.114 * pixel.B;

                luminosity[bitmap.Width * y + x] = L;
            }

        return luminosity;
    }

    public static Color LabToRgb(double L, double a, double b)
    {
        double y = (L + 16.0) / 116.0;
        double x = a / 500.0 + y;
        double z = y - b / 200.0;

        double x3 = Math.Pow(x, 3);
        double y3 = Math.Pow(y, 3);
        double z3 = Math.Pow(z, 3);

        x = (x3 > 0.008856) ? x3 : ((x - 16.0 / 116.0) / 7.787);
        y = (y3 > 0.008856) ? y3 : ((y - 16.0 / 116.0) / 7.787);
        z = (z3 > 0.008856) ? z3 : ((z - 16.0 / 116.0) / 7.787);

        double refX = 95.047;
        double refY = 100.000;
        double refZ = 108.883;

        double xR = x * refX;
        double yR = y * refY;
        double zR = z * refZ;

        xR = (xR > 0.008856) ? Math.Pow(xR, 1.0 / 3.0) : (xR * 7.787 + 16.0 / 116.0);
        yR = (yR > 0.008856) ? Math.Pow(yR, 1.0 / 3.0) : (yR * 7.787 + 16.0 / 116.0);
        zR = (zR > 0.008856) ? Math.Pow(zR, 1.0 / 3.0) : (zR * 7.787 + 16.0 / 116.0);

        double cr = xR * 3.2406 + yR * -1.5372 + zR * -0.4986;
        double cg = xR * -0.9689 + yR * 1.8758 + zR * 0.0415;
        double cb = xR * 0.0557 + yR * -0.2040 + zR * 1.0570;

        cr = (cr > 0.0031308) ? (1.055 * Math.Pow(cr, (1.0 / 2.4)) - 0.055) : 12.92 * cr;
        cg = (cg > 0.0031308) ? (1.055 * Math.Pow(cg, (1.0 / 2.4)) - 0.055) : 12.92 * cg;
        cb = (cb > 0.0031308) ? (1.055 * Math.Pow(cb, (1.0 / 2.4)) - 0.055) : 12.92 * cb;

        cr = Math.Max(0, Math.Min(1, cr));
        cg = Math.Max(0, Math.Min(1, cg));
        cb = Math.Max(0, Math.Min(1, cb));

        return Color.FromArgb((int)(cr * 255), (int)(cg * 255), (int)(cb * 255));
    }
}