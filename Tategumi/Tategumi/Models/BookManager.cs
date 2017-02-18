using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Hanako.Models;
using Prism.Mvvm;
using Tategumi.Repositories;
using Tategumi.Services;

namespace Tategumi.Models
{
  public class BookManager : BindableBase,IBookManager
  {
    private int CurrentArticleId { get; set; }
    List<HKPara> _paralst;  //段落
    public List<IHKWaxPage> PageList { get; set; } //組版されたページ一覧
    List<HKWaxLine> _lnlst;

    public BookManager()
    {
      _paralst = new List<HKPara>();
      _lnlst = new List<HKWaxLine>();
      PageList =new List<IHKWaxPage>();
      CurrentArticleId = -1;
    }
  
    #region 目次とかページ数、ページなど
    /*private IHKWaxPage _waxpage;
    public IHKWaxPage CurrentPage
    {
      get { return this._waxpage; }
      set { SetProperty(ref _waxpage, value); }
    }*/

    private bool _isEnableView;
    public bool IsEnableView
    {
      get { return this._isEnableView; }
      set { SetProperty(ref _isEnableView, value); }
    }

    private int _pagenum;
    public int PageNum
    {
      get { return this._pagenum; }
      set { SetProperty(ref _pagenum, value); }
    }
    private int _pageIndex;
    public int PageIndex
    {
      get { return this._pageIndex; }
      set
      {
        if (value != _pageIndex)
        {
          //Debug.WriteLine($"BookManager before PageIndex={_pageIndex}");
          //PageList[_pageIndex].CanDraw = true;
          SetProperty(ref _pageIndex, value);
          //Debug.WriteLine($"BookManager after PageIndex={_pageIndex}");
        }
      }
    }
    #endregion

    #region Tateview Width,Height
    private int _tateviewWidth;
    public int TateviewWidth
    {
      get { return this._tateviewWidth; }
      set
      {
        SetProperty(ref _tateviewWidth, value);
        Compose();
      }
    }
    private int _tateviewHeight;
    public int TateviewHeight
    {
      get { return this._tateviewHeight; }
      set
      {
        SetProperty(ref _tateviewHeight, value);
        Compose();
      }
    }

    private string _title;
    public string Title
    {
      get { return this._title; }
      set
      {
        SetProperty(ref _title, value);
      }
    }

    #endregion

    #region 文字サイズ、行間
    private bool _isFontSizeLarge;
    public bool IsFontSizeLarge
    {
      get { return this._isFontSizeLarge; }
      set
      {
        SetProperty(ref _isFontSizeLarge, value);
        Compose();
      }
    }
    private bool _isVisibleHinshi;
    public bool IsVisibleHinshi
    {
      get { return this._isVisibleHinshi; }
      set
      {
        SetProperty(ref _isVisibleHinshi, value);
        Compose();
      }
    }
    #endregion

    #region 本の一覧を読み込み（クローリング）
    public void ReadContents(int articleId)
    {
#if false
      //if (CurrentArticleId == articleId)
      //  return;
      CurrentArticleId = articleId;

      var ser = new BookService2(new HeianRepository());
      Task<BookItem> hbn = ser.GetArticle(articleId, true, true);
      Debug.WriteLine($"BOOK ReadContents contents.count={hbn.Result.Contents.Count}");
      hbn.Wait();
      _madolst?.Clear();
      foreach (var item in hbn.Result.Contents)
      {
        _madolst.Add(new HKMado
        {
          Title = item.Title
        });
      }
#endif
    }
    #endregion

    #region 本文をファイルから読み込み
    public void ReadHonbunHtmlFromPath(string path)
    {
      _paralst?.Clear();
      var parser = new HKSimpleParser();
      parser.ParseFromPath(path);
      _paralst = parser.ResultParaList;
    }
    #endregion
  
    #region 本文をURLから読み込み(未使用)
    public void ReadHonbunHtmlFromUrl(string url)
    {
      var ser = new BookService2(new BookRepository());
      string hbn = ser.GetBook(url);
      _paralst?.Clear();
      var parser = new HKSimpleParser();
      parser.ParseFromText(hbn);
      _paralst = parser.ResultParaList;
    }
#endregion

#region ページを組版
    public void Compose()
    {
      Debug.WriteLine($"Compose para.count={_paralst.Count}");
      //if (_paralst?.Count == 0 ) return;
      if (isValid() != true) return;

      int devw = _tateviewWidth;
      int devh = _tateviewHeight;

      //1.Compose
      float fntSz = 24;
      if (_isFontSizeLarge == true)
        fntSz += 24;
      //float gyokanSz = fntSz * 0.5f;
      float gyokanSz = fntSz * 1.0f;
      if(_isVisibleHinshi==true)
        gyokanSz += fntSz * 1.0f;
      float mg = 10;    //デバイスに対しての余白
      HKComposer comp = new HKComposer();
      comp.FontSize = fntSz;
      comp.GyokanSize = gyokanSz;
      comp.Init((float)devw - mg * 2, (float)devh - mg * 2);
      _lnlst.Clear();
      comp.Compose(_paralst, ref _lnlst);
      Debug.WriteLine($"Compose line.count={_lnlst.Count}");

      //2.Page一覧
      PageList.Clear();
      var pglst = PageList;
      HKPageCreate.CreatePageList((float)devw - mg * 2, fntSz, _lnlst, ref pglst);

      //先頭ページだけ描画OK
      pglst[0].CanDraw = true;

      //反転させる
      pglst.Reverse();  

      Debug.WriteLine($"Compose page.count={pglst.Count}");

      //3.Deviceの値に変換
      HKDevice dev = new HKDevice();
      dev.setup(fntSz, (float)devw, (float)devh, mg, mg, mg, mg);
      dev.calcToDevice(ref pglst);

      //CurrentPage = PageList[0];
      //CurrentPage = PageList.Last();
      PageNum = PageList.Count;
    }
#endregion

#region ページを移動
    bool isValid()
    {
      if (_paralst?.Count == 0) return false;
      if (_tateviewWidth <= 0 || _tateviewHeight <= 0) return false;
      return true;
    }
    public void GoToNextPage()
    {
      PageIndex++;
    }
    public void GoToPrevPage()
    {
      PageIndex--;
    }
#endregion
  }
}
