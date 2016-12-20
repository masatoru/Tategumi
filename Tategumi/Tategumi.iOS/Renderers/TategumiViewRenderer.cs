using Tategumi.iOS.Renderers;
using Tategumi.TategumiViews;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer (typeof(TategumiView), typeof(TategumiViewRenderer))]

namespace Tategumi.iOS.Renderers
{
	public class TategumiViewRenderer: ViewRenderer<TategumiView, NativeTategumiView>
	{
    NativeTategumiView _view;
    public TategumiViewRenderer ()
		{
		}

		protected override void OnElementChanged (ElementChangedEventArgs<TategumiView> e)
		{
			base.OnElementChanged (e);
		  if (Control == null)
		  {
		    _view = new NativeTategumiView(Element);
		    SetNativeControl(_view);
		  }
		  if (e.NewElement != null)
      {
        _view.SetNeedsDisplay();  //描画!!
      }
    }
    protected override void OnElementPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
      base.OnElementPropertyChanged(sender, e);
      if (e.PropertyName == TategumiView.CurrentPageProperty.PropertyName)
      {
        _view.SetNeedsDisplay();  //描画!!
      }
    }
  }
}

