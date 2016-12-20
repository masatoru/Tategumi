using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Windows.Input;
using Hanako.Models;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using Tategumi.Models;
using Xamarin.Forms;

namespace Tategumi.ViewModels
{
  public class HonbunPageViewModel : BindableBase, INavigationAware
  {
    readonly IBookManager _bookmg;
    public ReactiveProperty<IHKWaxPage> CurrentPage { get; }
    public ReactiveProperty<int> PageNum { get; }
    public ReactiveProperty<int> TateviewWidth { get; }
    public ReactiveProperty<int> TateviewHeight { get; }
    public ReactiveProperty<int> PageIndex { get; }
    public ReactiveProperty<string> Title { get; }
    public ICommand NextPageCommand { get; }
    public ICommand PrevPageCommand { get; }
    public void SizeChanged() { _bookmg.Compose(); }
    public HonbunPageViewModel(IBookManager bookmg)
    {
      Debug.WriteLine($"HonbunViewModel constructor 1");

      _bookmg = bookmg;
      CurrentPage = _bookmg.ObserveProperty(b => b.CurrentPage).ToReactiveProperty();
      PageNum = _bookmg.ToReactivePropertyAsSynchronized(b => b.PageNum);
      TateviewHeight = _bookmg.ToReactivePropertyAsSynchronized(b => b.TateviewHeight);
      TateviewWidth = _bookmg.ToReactivePropertyAsSynchronized(b => b.TateviewWidth);
      PageIndex = _bookmg.ToReactivePropertyAsSynchronized(b => b.PageIndex);
      Title = _bookmg.ObserveProperty(b => b.Title).ToReactiveProperty();
      NextPageCommand = new DelegateCommand(() => _bookmg.GoToNextPage());
      PrevPageCommand = new DelegateCommand(() => _bookmg.GoToPrevPage());
    }
    #region  本文ContentPageを表示する
    void INavigationAware.OnNavigatedTo(NavigationParameters parameters)
    {
      //引数から取得
      var book = (BookItem)parameters["book"];

      //HTML読み込み
      _bookmg.ReadHonbunHtmlFromUrl(book.FileName);

      //組版
      _bookmg.Compose();
    }
    void INavigationAware.OnNavigatedFrom(NavigationParameters parameters)
    {
    }
    #endregion
  }
}
