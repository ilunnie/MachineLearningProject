using System.Drawing;
using Pamella;

public class View : Pamella.View
{
    private bool _help = false;
    private bool _update = false;
    private bool _toColor = false;
    private Bitmap originalImage = null;
    private Bitmap colorImage = null;

    protected override void OnStart(IGraphics g)
    {
        g.SubscribeKeyDownEvent(key => {
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
        
    }

    protected override void OnFrame(IGraphics g)
    {
        if (_update && _toColor)
            colorImage = originalImage.toColor();
    }
}