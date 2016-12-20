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
  public class DeviceTest
  {
    public static IList<IHKWaxPage> createTestCalcDevice(string buf,
        float fntsz,float gyokan,
        float view_w, float view_h,
        float mg_lft, float mg_rgt, float mg_top)
    {
      List<HKWaxLine> lnlst = WaxComposerTest.createTestWaxLineList(buf, view_w, view_h, fntsz, gyokan);

      //HKPageCreate page = new HKPageCreate();
      IList<IHKWaxPage> pglst =new List<IHKWaxPage>();
      HKPageCreate.CreatePageList(view_w,fntsz, lnlst,ref pglst);

      HKDevice dev = new HKDevice();
      dev.setup(fntsz, view_w + mg_lft + mg_rgt, view_h, mg_lft, mg_top, mg_rgt, 0);
      dev.calcToDevice(ref pglst);
      return pglst;
    }
    [Test]
    public void CalcToDevice()
    {
      var buf = "<p>あああああ<ruby>いいいいい<rt>ううう</rt></ruby>えええええ</p>" +
        "<p>おおおおお</p>";  //15文字+改行+5文字
      var fntsz = 15;
      var gyokan = 7.5f;
      var view_w = 90f;
      var view_h = 85f; //余白を除いた描画領域
      var mg_lft = 10f;
      var mg_rgt = 5f;
      var mg_top = 10f;
      var pglst = createTestCalcDevice(buf,
        fntsz,gyokan,view_w,view_h,mg_lft,mg_rgt,mg_top);

      var pg = pglst[0];
      Assert.AreEqual(1, pg.Page, "Page1");
      Assert.AreEqual(0, pg.Lines[0].Chars[0].X, "1行目一文字目のX(左上)");
      Assert.AreEqual(mg_lft + view_w - 15, pg.Lines[0].Chars[0].DevX, "1行目一文字目のDevX(左上)");
      Assert.AreEqual(mg_top, pg.Lines[0].Chars[0].DevY, "1行目1文字目のDevY(左上)");
      Assert.AreEqual(mg_top+15, pg.Lines[0].Chars[1].DevY, "1行目2文字目のDevY(左上)");
      Assert.AreEqual(fntsz+gyokan, pg.Lines[1].Chars[0].X, "2行目一文字目のX(左上)");
      Assert.AreEqual(mg_lft+view_w - (15 + 15 + 7.5f), pg.Lines[1].Chars[0].DevX, "2行目一文字目のX(左上)");
    }
    [Test]
    public void CalcToDevice_MultiPage_ちょうど改行でNG()
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
        "<p>おおおおお</p><p></p>"
        ;
      var fntsz = 15f;
      var gyokan = 7.5f;

      var view_w = 90f;  // 15+22.5+22.5+22.5 ==> 4行/1ページ
      var view_h = 90f; //余白を除いた描画領域   90/15=6文字
      var mg_lft = 10f;
      var mg_rgt = 5f;
      var mg_top = 10f;
      var pglst = createTestCalcDevice(buf,fntsz,gyokan,view_w, view_h, mg_lft, mg_rgt, mg_top);

      Assert.AreEqual(4, pglst.Count, "ページ数");
    }
    [Test]
    public void CalcToDevice_MultiPage()
    {
      var buf =
        "<p>あああああ<ruby>いいいいい<rt>ううう</rt></ruby>えええええ</p>" +  //15文字 3行
        "<p>おおおおお</p>" +  //5文字 1行
        "<p>あああああ<ruby>いいいいい<rt>ううう</rt></ruby>えええええ</p>" +  //15文字
        "<p>おおおおお</p>" +  //5文字
        "<p>あああああ<ruby>いいいいい<rt>ううう</rt></ruby>えええええ</p>" +  //15文字
        "<p>おおおおお</p>" +  //5文字
        "<p>あああああ<ruby>いいいいい<rt>ううう</rt></ruby>えええええ</p>" +  //15文字
        "<p>おおおおお</p>"    //5文字
        ;
      var fntsz = 15f;
      var gyokan = 7.5f;

      var view_w = 90f;  // 15+22.5+22.5+22.5 ==> 4行/1ページ
      var view_h = 90f; //余白を除いた描画領域   15*6文字
      var mg_lft = 10f;
      var mg_rgt = 5f;
      var mg_top = 10f;
      var pglst = createTestCalcDevice(buf,fntsz, gyokan, view_w, view_h, mg_lft, mg_rgt, mg_top);

      Assert.AreEqual(4, pglst.Count, "ページ数");
      var pg = pglst[0];
      Assert.AreEqual(1, pg.Page, "Page1");
      Assert.AreEqual(0, pg.Lines[0].Chars[0].X, "P1:1行目一文字目のX(左上)");
      Assert.AreEqual(85, pg.Lines[0].Chars[0].DevX, "P1:1行目一文字目のDevX(左上)");
      Assert.AreEqual(62.5f, pg.Lines[1].Chars[0].DevX, "P1:2行目1文字目のDevX(左上)");
      pg = pglst[1];
      Assert.AreEqual(2, pg.Page, "Page2");
      Assert.AreEqual(90, pg.Lines[0].Chars[0].X, "P2:1行目一文字目のX(左上)");
      Assert.AreEqual(85, pg.Lines[0].Chars[0].DevX, "P2:2行目1文字目のDevX(左上)");
      Assert.AreEqual(62.5f, pg.Lines[1].Chars[0].DevX, "P2:2行目1文字目のDevX(左上)");
    }
  }
}
