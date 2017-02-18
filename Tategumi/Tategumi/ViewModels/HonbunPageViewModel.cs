using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Hanako.Models;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using Tategumi.Models;

namespace Tategumi.ViewModels
{
  public class HonbunPageViewModel : BindableBase, INavigationAware
  {
    //public ObservableCollection<IHKWaxPage> PageList { get; set; }
    public ObservableCollection<IHKWaxPage> PageList { get; private set; }

    readonly IBookManager _bookmg;
    //public ReactiveProperty<IHKWaxPage> CurrentPage { get; }
    public ReactiveProperty<int> PageNum { get; }
    public ReactiveProperty<int> TateviewWidth { get; }
    public ReactiveProperty<int> TateviewHeight { get; }
    public ReactiveProperty<int> PageIndex { get; }
    public ReactiveProperty<string> Title { get; }
    public ReactiveProperty<bool> IsEnableView { get; }
    public ICommand NextPageCommand { get; }
    public ICommand PrevPageCommand { get; }
    public void SizeChanged() { _bookmg.Compose(); }
    public HonbunPageViewModel(IBookManager bookmg)
    {
      _bookmg = bookmg;
      //CurrentPage = _bookmg.ObserveProperty(b => b.CurrentPage).ToReactiveProperty();
      PageNum = _bookmg.ToReactivePropertyAsSynchronized(b => b.PageNum);
      TateviewHeight = _bookmg.ToReactivePropertyAsSynchronized(b => b.TateviewHeight);
      TateviewWidth = _bookmg.ToReactivePropertyAsSynchronized(b => b.TateviewWidth);
      PageIndex = _bookmg.ToReactivePropertyAsSynchronized(b => b.PageIndex);
      Title = _bookmg.ObserveProperty(b => b.Title).ToReactiveProperty();
      NextPageCommand = new DelegateCommand(() => _bookmg.GoToNextPage());
      PrevPageCommand = new DelegateCommand(() => _bookmg.GoToPrevPage());
      PageList = new ObservableCollection<IHKWaxPage>();
      //IsEnableView = _bookmg.ObserveProperty(b => b.IsEnableView).ToReactiveProperty();
      //IsEnableView = _bookmg.ToReactivePropertyAsSynchronized(b => b.IsEnableView);
      //IsEnableView = new ReactiveProperty<bool>(false);

      //PageList = _bookmg.PageList.ToReadOnlyReactiveCollection(m => m);
      //PageList = _bookmg.PageList.ToReadOnlyReactiveCollection(m => m);
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
      //_bookmg.IsEnableView = true;

      //foreach (var pg in _bookmg.PageList.Skip(_bookmg.PageList.Count-2).Take(2))
      //  PageList.Add(pg);

      PageList.Clear();
      foreach (var pg in _bookmg.PageList)
        PageList.Add(pg);

      //PageIndex.Value = 10;
      PageIndex.Value = PageList.Count - 1;
      //_bookmg.IsEnableView = true;
    }
    void INavigationAware.OnNavigatedFrom(NavigationParameters parameters)
    {
    }
    #endregion
  }
}
