using Android.Graphics;
using Android.Views;
using SkiaSharp;
using Tategumi.TategumiViews;

namespace Tategumi.Droid.Views
{
	public class NativeTategumiView : View
	{
    const string TAG = "SKVW";
		private Bitmap _bitmap;
		readonly TategumiView _tategumiView;
		readonly ITategumiViewController _tateView;

		public NativeTategumiView (Android.Content.Context context, TategumiView tategumiView) : base (context)
		{
			this._tategumiView = tategumiView;
			this._tateView = tategumiView;
      this.Touch += OnTouched;
    }

    protected override void OnDraw (Android.Graphics.Canvas canvas)
		{
      base.OnDraw (canvas);

			if (_bitmap == null || _bitmap.Width != canvas.Width || _bitmap.Height != canvas.Height) {
				if (_bitmap != null) 
					_bitmap.Dispose ();

				_bitmap = Bitmap.CreateBitmap (canvas.Width, canvas.Height, Bitmap.Config.Argb8888);
			}

      try
      {
				using (var surface = SKSurface.Create (canvas.Width, canvas.Height, SKColorType.Rgba8888/*N_32*/, SKAlphaType.Premul, _bitmap.LockPixels (), canvas.Width * 4)) {
					var skcanvas = surface.Canvas;
					skcanvas.Scale (((float)canvas.Width)/(float)_tategumiView.Width, ((float)canvas.Height)/(float)_tategumiView.Height);
          _tateView.SendDraw (skcanvas);
				}
			}
			finally {
				_bitmap.UnlockPixels ();
			}

			canvas.DrawBitmap (_bitmap, 0, 0, null);
		}
    
    //タップしたら左半分で次頁、右半分で前頁
    //→ほんとは処理をModelに渡したい
    void OnTouched(object sender, TouchEventArgs e)
    {
      if (e.Event.Action != MotionEventActions.Up)
        return;
      if (Width / 2f < e.Event.RawX)
        _tateView.PrevPage();
      else
        _tateView.NextPage();
    }
  }
}

