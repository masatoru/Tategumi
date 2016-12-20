using System.Collections.ObjectModel;
using System.ComponentModel;
using Hanako.Models;

namespace Tategumi.Models
{
  public interface IBookManager : INotifyPropertyChanged
  {
    void ReadHonbunHtmlFromUrl(string url);
    void ReadContents(int articleId);
    void Compose();
    IHKWaxPage CurrentPage { get; }
    int PageIndex { get; set; }
    int PageNum { get; set; }
    int TateviewWidth { get; set; }
    int TateviewHeight { get; set; }
    string Title { get; set; }
    void GoToNextPage();
    void GoToPrevPage();

    bool IsFontSizeLarge { get; set; }          //文字サイズ大きく
    bool IsVisibleHinshi { get; set; }
  }
}
