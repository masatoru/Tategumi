﻿using System;
using System.Diagnostics;
using CoreGraphics;
using SkiaSharp;
using Tategumi.TategumiViews;
using UIKit;

namespace Tategumi.iOS.Views
{
	public sealed class NativeTategumiView: UIView
	{
		const int bitmapInfo = ((int)CGBitmapFlags.ByteOrder32Big) | ((int)CGImageAlphaInfo.PremultipliedLast);

	  private ITategumiViewController _tateView;

		public NativeTategumiView (TategumiView tateView)
		{
			this._tateView = tateView;

			AddGestureRecognizer (new UITapGestureRecognizer (OnTapped));
		}

		public override void Draw (CGRect rect)
		{
			base.Draw (rect);

			var screenScale = UIScreen.MainScreen.Scale;
			var width = (int)(Bounds.Width * screenScale);
			var height = (int)(Bounds.Height * screenScale);

			IntPtr buff = System.Runtime.InteropServices.Marshal.AllocCoTaskMem (width * height * 4);
			try {
				using (var surface = SKSurface.Create (width, height, SKColorType.Rgba8888/*N_32*/, SKAlphaType.Premul, buff, width * 4)) {
					var skcanvas = surface.Canvas;
					skcanvas.Scale ((float)screenScale, (float)screenScale);
					using (new SKAutoCanvasRestore (skcanvas, true)) {
						_tateView.SendDraw (skcanvas);
					}
				}

				using (var colorSpace = CGColorSpace.CreateDeviceRGB ())
				using (var bContext = new CGBitmapContext (buff, width, height, 8, width * 4, colorSpace, (CGImageAlphaInfo)bitmapInfo))
				using (var image = bContext.ToImage ())
				using (var context = UIGraphics.GetCurrentContext ()) {
					// flip the image for CGContext.DrawImage
					context.TranslateCTM (0, Frame.Height);
					context.ScaleCTM (1, -1);
					context.DrawImage (Bounds, image);
				}
			} finally {
				if (buff != IntPtr.Zero)
					System.Runtime.InteropServices.Marshal.FreeCoTaskMem (buff);
			}
		}

		void OnTapped (UITapGestureRecognizer re)
		{
      var point = re.LocationInView(this);

      if (Bounds.Width / 2f < point.X)
        _tateView.PrevPage();
      else
        _tateView.NextPage();

      //if (res==true)
        SetNeedsDisplay();    //再描画
		}

		public override void LayoutSubviews ()
		{
			base.LayoutSubviews ();
			SetNeedsDisplay ();
		}
	}
}

