using SkiaSharp;
using Xamarin.Forms;

namespace Tategumi.TategumiViews
{
	public interface ITategumiViewController : IViewController
	{
		void SendDraw (SKCanvas canvas);
    //bool NextPage();
    //bool PrevPage();
  }
}

