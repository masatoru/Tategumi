using Hanako.Models;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hanako.Test.Tests
{
  [TestFixture]
  public class PageCreateTest
  {
    [Test]
    public void CreatePageList()
    {
      var buf =
        "<p>あああああ<ruby>い" +
        "いいいい<rt>ううう</rt></ruby>ええ"+
        "えええ</p>" +
        "<p>おおおおお</p>" +
        "<p>あああああ<ruby>いいいいい<rt>ううう</rt></ruby>えええええ</p>" +
        "<p>おおおおお</p>" +
        "<p>あああああ<ruby>いいいいい<rt>ううう</rt></ruby>えええええ</p>" +
        "<p>おおおおお</p>" +
        "<p>あああああ<ruby>いいいいい<rt>ううう</rt></ruby>えええええ</p>" +
        "<p>おおおおお</p>" +
        "<p>あああああ<ruby>いいいいい<rt>ううう</rt></ruby>えええええ</p>" +
        "<p>おおおおお</p>"
        ;
      float view_w = 90;
      float view_h = 90; //余白を除いた描画領域   ==> 6文字/1行
      float fntsz = 15;
      float gyokan = 6f;  //15 + 21+21+21  ==> 4行/ページ
      List<HKWaxLine> lnlst = WaxComposerTest.createTestWaxLineList(buf, view_w, view_h, fntsz, gyokan);

      //HKPageCreate rep = new HKPageCreate();
      IList<IHKWaxPage> pglst=new List<IHKWaxPage>();
      HKPageCreate.CreatePageList(view_w, fntsz,lnlst,ref pglst);

      //Assert.AreEqual(4, pglst.Count);
      Assert.AreEqual(4, pglst[0].Lines.Count,"1ページ目行数");
      Assert.AreEqual(0, pglst[0].Lines[0].X);
      Assert.AreEqual("あああああい", pglst[0].Lines[0].getText(),"1ページ目1行目");
      Assert.AreEqual(fntsz+gyokan, pglst[0].Lines[1].PageX);
      Assert.AreEqual("いいいいええ", pglst[0].Lines[1].getText());
      Assert.AreEqual("えええ", pglst[0].Lines[2].getText());
      Assert.AreEqual("おおおおお", pglst[0].Lines[3].getText());
      Assert.AreEqual(0, pglst[1].Lines[0].PageX,"2ページ目の1文字目X");  //DAME!!
      Assert.AreEqual("あああああい", pglst[1].Lines[0].getText(),"2ページ目1行目");
      //最終頁最終行
      Assert.AreEqual("おおおおお", pglst.Last().Lines.Last().getText());
    }
  }
}
