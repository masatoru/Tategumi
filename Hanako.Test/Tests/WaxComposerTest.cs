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
  public class WaxComposerTest
  {
    public static List<HKWaxLine> createTestWaxLineList(string buf,float width, float height,
      float fntsz, float gyokan)
    {
      var p = new HKSimpleParser();
      p.ParseFromText(buf);

      var paralst = p.ResultParaList;
      Console.WriteLine($"createTestWaxLineList paralist.count={paralst.Count}");

      var comp = new HKComposer();
      comp.FontSize = fntsz;  // 15;   //文字サイズ
      comp.GyokanSize = gyokan;   //7.5        //行間 行送=FontSize+Gyokan
      comp.Init(width, height);
      var lnlst=new List<HKWaxLine>();
      comp.Compose(paralst, ref lnlst);
      Console.Write(comp.Log);

      return lnlst;
    }
    [Test]
    public void Compose()
    {
      string buf = "<p>あああああ<ruby>いいいいい<rt>ううう</rt></ruby>えええええ</p>" +
        "<p>おおおおお</p>";  //15文字+改行+5文字
      var view_w = 90f;
      var view_h = 85f; //余白を除いた描画領域
      var fntsz = 15f;
      var gyokan = 7.5f;
      var lnlst = createTestWaxLineList(buf, view_w, view_h, fntsz, gyokan);

      Assert.AreEqual(4, lnlst.Count);           //全体で4行
      Assert.AreEqual(5, lnlst[0].Chars.Count);  //1行目が5文字 
      Assert.AreEqual("あ", lnlst[0].Chars[0].Char, "1行目最初");
      Assert.AreEqual(0, lnlst[0].Chars[0].X, "1文字目のX");
      Assert.AreEqual(0, lnlst[0].Chars[0].Y, "1文字目のY");
      Assert.AreEqual(fntsz + gyokan, lnlst[1].Chars[0].X, "2行目2文字目のX");
      Assert.AreEqual(fntsz, lnlst[1].Chars[1].Y, "2行目2文字目のY");

      Assert.AreEqual("い", lnlst[1].Chars[0].Char, "2行目最初");
      Assert.AreEqual(fntsz + gyokan, lnlst[1].Chars[0].X, "2行目1文字目のX"); //1文字目のX    
      Assert.AreEqual(0, lnlst[1].Chars[0].Y, "2行目1文字目のY"); //1文字目のY    
    }
    [Test]
    public void Compose_禁則1()
    {
      string buf = "<p>あああああ。" +
        "おおおおお</p>";
      int view_w = 90;
      int view_h = 85; //余白を除いた描画領域
      float fntsz = 15;
      float gyokan = 7.5f;
      List<HKWaxLine> lnlst = createTestWaxLineList(buf, view_w, view_h, fntsz, gyokan);

      Assert.AreEqual(2, lnlst.Count);           //全体で２行
      Assert.AreEqual(5+1, lnlst[0].Chars.Count);  //1行目が5文字＋。 
      Assert.AreEqual("あ", lnlst[0].Chars[0].Char, "1行目最初");
      Assert.AreEqual("。", lnlst[0].Chars.Last().Char, "1行目最後");
      Assert.AreEqual("お", lnlst[1].Chars[0].Char, "2行目最初");
    }
    [Test]
    public void Compose_禁則2()
    {
      string buf = "<p>ああああ「" + "おおおおお</p>";
      int view_w = 90;
      int view_h = 85; //余白を除いた描画領域
      float fntsz = 15;
      float gyokan = 7.5f;
      List<HKWaxLine> lnlst = createTestWaxLineList(buf, view_w, view_h, fntsz, gyokan);

      List<HKWaxBase> chs = lnlst[0].Chars;
      Assert.AreEqual("あ", chs[0].Char, "1行目最初");
      Assert.AreEqual(2, lnlst.Count);           //全体で２行
      Assert.AreEqual(4, lnlst[0].Chars.Count, "1行目文字数");  //1行目が5文字＋。 
      Assert.AreEqual(6, lnlst[1].Chars.Count,"2行目文字数");
      Assert.AreEqual("「", lnlst[1].Chars[0].Char, "2行目最初");
    }
  }
}
