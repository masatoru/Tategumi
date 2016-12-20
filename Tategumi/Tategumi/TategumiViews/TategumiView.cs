using System;
using System.Diagnostics;
using Hanako.Models;
using SkiaSharp;
using Xamarin.Forms;

namespace Tategumi.TategumiViews
{
	public class TategumiView : View, ITategumiViewController
	{
    public TategumiView()   //XAML用
    {
      core_ = TategumiViewCore.get();
    }
    TategumiViewCore.DrawMethod core_;

    #region CurrentPage BindableProperty
    public static readonly BindableProperty CurrentPageProperty =
      BindableProperty.Create(nameof(IHKWaxPage), typeof(IHKWaxPage), typeof(TategumiView), 
        null/*default(int)*/, BindingMode.TwoWay,
         propertyChanged: (bindable, oldValue, newValue) =>
            ((TategumiView)bindable).CurrentPage = (IHKWaxPage)newValue);

    public IHKWaxPage CurrentPage
    {
      get { return (IHKWaxPage)GetValue(CurrentPageProperty); }
      set { SetValue(CurrentPageProperty, value);}
    }
    #endregion

    #region PageIndex BindableProperty
    public static readonly BindableProperty PageIndexProperty =
      BindableProperty.Create(nameof(PageIndex), typeof(int), typeof(TategumiView), default(int), BindingMode.TwoWay,
        propertyChanged: (bindable, oldValue, newValue) =>
            ((TategumiView)bindable).PageIndex = (int)newValue);

    public int PageIndex
    {
      get { return (int)GetValue(PageIndexProperty); }
      set
      {
        if (value != PageIndex)
        {
          SetValue(PageIndexProperty, value);
        }
      }
    }
    #endregion

    public bool NextPage()
    {
      PageIndex++;   //==>WaxPageをもらう
      return true;
    }
    public bool PrevPage()
    {
      PageIndex--;
      return true;
    }
    void ITategumiViewController.SendDraw(SKCanvas canvas)
    {
      if (canvas == null)
        throw new NullReferenceException("Canvasがnull");
      Draw(canvas);
    }
    protected virtual void Draw(SKCanvas canvas)
		{
      if (canvas == null)
        throw new NullReferenceException("Canvasがnull");
		  if (CurrentPage != null)
		  {
        core_?.Method?.Invoke(canvas, CurrentPage, false);
      }
    }
  }
}
