using System;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using Pamella;

public class View : Pamella.View
{
    private bool _help = false;
    private bool _update = true;
    private bool _toColor = false;
    private string _algorithm;
    private Bitmap originalImage = null;
    private Bitmap colorImage = null;

    protected override void OnStart(IGraphics g)
    {
        originalImage = (Bitmap)Bitmap.FromFile("./grayscale.jpg");
        g.SubscribeKeyDownEvent(key =>
        {
            switch (key)
            {
                case Input.Escape:
                    App.Close();
                    break;

                case Input.Space:
                    _toColor = !_toColor;
                    _update = true;
                    break;

                case Input.H:
                    _help = !_help;
                    _update = true;
                    break;

                case Input.Left:
                case Input.A:
                    Requester.Option--;
                    _update = true;
                    break;

                case Input.Right:
                case Input.D:
                    Requester.Option++;
                    _update = true;
                    break;
            }
        });
    }

    protected override void OnRender(IGraphics g)
    {
        g.Clear(Color.Black);

        var scale = Math.Min(g.Width / originalImage.Width, g.Height / originalImage.Height);
        var newWidth = originalImage.Width * scale;
        var newHeight = originalImage.Height * scale;
        var rect = new RectangleF(
            (g.Width - newWidth) / 2,
            (g.Height - newHeight) / 2,
            newWidth,
            newHeight
        );

        if (!_toColor)
            g.DrawImage(rect, originalImage);
        else
        {
            g.DrawImage(rect, colorImage);
            g.DrawText(
                new RectangleF(0, 40, 100, 40),
                Brushes.Red,
                Requester.Algorithm
            );
        }
    }

    protected override async void OnFrame(IGraphics g)
    {
        if (_update)
        {
            _update = false;
            if (_toColor)
                await Colorir();
            Invalidate();
        }
    }

    private async Task Colorir()
    {
        if (_algorithm != Requester.Algorithm)
        {
            _algorithm = Requester.Algorithm;
            colorImage = await originalImage.toColorAsync();
        }
    }
}