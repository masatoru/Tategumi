using kenkyu.mobi.iOS;
using kenkyu.mobi.TategumiViews;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer (typeof(kenkyu.mobi.TategumiViews.TategumiView), typeof(TategumiViewRenderer))]

namespace kenkyu.mobi.iOS
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

