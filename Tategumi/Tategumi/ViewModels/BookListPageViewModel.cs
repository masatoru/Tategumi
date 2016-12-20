using Prism.Commands;
using Prism.Mvvm;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows.Input;
using Prism.Navigation;
using Tategumi.Models;
using Tategumi.Repositories;
using Tategumi.Services;

namespace Tategumi.ViewModels
{
  public class BookListPageViewModel : BindableBase
  {
    public BookListPageViewModel(IBookManager bookmg, INavigationService navigationService)
    {
      _navigationService = navigationService;
      _bookmg = bookmg;
      BookList = new ObservableCollection<BookItem>();
      this.ViewBookCommand = new DelegateCommand(ViewBook);
      InitData();
    }
    public ObservableCollection<BookItem> BookList { get; set; }
    public ICommand ViewBookCommand { get; }

    //収載一覧
    private BookItem _bookSelected;
    public BookItem BookSelected
    {
      get { return this._bookSelected; }
      set { SetProperty(ref _bookSelected, value); }
    }

    readonly INavigationService _navigationService;
    private readonly IBookManager _bookmg;

    void InitData()
    {
      var ser = new BookService2(new BookRepository());
      var lst = ser.GetBooks();

      BookList.Clear();
      foreach (var article in lst)
        BookList.Add(article);
    }
    //本文表示 下の目次をタップ
    async void ViewBook()
    {
      var navParameters = new NavigationParameters
      {
        {"book", BookSelected},
      };
      await _navigationService.NavigateAsync("HonbunPage", navParameters);
    }
  }
}
