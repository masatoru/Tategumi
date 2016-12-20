using Tategumi.Droid.Renderers;
using Tategumi.Droid.Views;
using Tategumi.TategumiViews;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer (typeof(TategumiView), typeof(TategumiViewRenderer))]

namespace Tategumi.Droid.Renderers
{
	public class TategumiViewRenderer : ViewRenderer<TategumiView, NativeTategumiView>
	{
	  private const string TAG = "REND";
		NativeTategumiView _view;
    public TategumiViewRenderer()
    {
    }
    protected override void OnElementChanged(ElementChangedEventArgs<TategumiView> e)
    {
      base.OnElementChanged(e);

      if (Control == null)
      {
        _view = new NativeTategumiView(Context, Element);
        SetNativeControl(_view);
      }
      if (e.NewElement != null)
      {
        _view.Invalidate();  //描画!!
      }
    }
    protected override void OnElementPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
      base.OnElementPropertyChanged(sender, e);
      if (e.PropertyName == TategumiView.CurrentPageProperty.PropertyName)
      {
        _view.Invalidate();  //描画!!
      }
    }
  }
}

