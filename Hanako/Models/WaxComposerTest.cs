using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using wakadraw.pcl.Model;

namespace wakadraw.test
{
  [TestFixture]
  public class WaxComposerTest
  {
    [Test]
    public void WaxComposer_Compose()
    {
      List<WakaPara> paralst;
      string buf = "あああああ<ruby>いいいいい<rt>ううう</rt></ruby>えええええ" +
        "<改行>おおおおお";  //15文字+改行+5文字
      WakaPara.createParaListFromText(buf, out paralst);

      List<WaxPage> pglst = new List<WaxPage>();
      WaxComposer comp = new WaxComposer(pglst);
      comp.FontSize = 15;
      comp.Gyokan = 0.5f;
      comp.MarginTop = 5;
      comp.MarginRight = 10;
      int width = 100;
      int height = 90; // (90-5)/15=5文字/1行 ==> 3行+1行
      comp.Init(width,height);
      comp.Compose(paralst);

      Assert.AreEqual(4, pglst[0].Lines.Count);           //全体で4行
      Assert.AreEqual(5, pglst[0].Lines[0].Chars.Count);  //1行目が5文字 
      Assert.AreEqual("あ", pglst[0].Lines[0].Chars[0].Char,"1行目最初");
      Assert.AreEqual(100 - 10 - 15, pglst[0].Lines[0].Chars[0].X, "1文字目のX"); //1文字目のX    
      Assert.AreEqual(5, pglst[0].Lines[0].Chars[0].Y, "1文字目のY"); //1文字目のY    

      Assert.AreEqual("い", pglst[0].Lines[1].Chars[0].Char,"2行目最初");
      Assert.AreEqual(100 - 10 - 15 - (15*1.5f),
        pglst[0].Lines[1].Chars[0].X, "2行目1文字目のX"); //1文字目のX    
      Assert.AreEqual(5, pglst[0].Lines[1].Chars[0].Y, "2行目1文字目のY"); //1文字目のY    
    }
  }
}
