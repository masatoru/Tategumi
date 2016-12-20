using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hanako.Models;

namespace Hanako.Test
{
  public static class StringExtensions
  {
    public static string Combine(this string source, string subdir)
    {
      return System.IO.Path.Combine(source, subdir);
    }
  }

  [TestFixture]
  public class ParserTest
  {
    [Test]
    public void ParserText()
    {
      var buf = $"<p>如実に<RUBY>形<RT>けい</RT></RUBY><RUBY>骸<RT>がい</RT></RUBY>の中に</p>" +
        "<p>精神的不満、<RUBY>煩<RT>はん</RT></RUBY><RUBY>悶<RT>もん</RT></RUBY>は人間</p>";
      var p = new HKSimpleParser();
      p.ParseFromText(buf);

      var lst = p.ResultParaList;
      Assert.AreEqual(2, lst.Count);
      Assert.AreEqual("如実に", lst[0].Texts[0].Text);
      Assert.AreEqual("形", lst[0].Texts[1].Text);
      Assert.AreEqual("けい", lst[0].Texts[1].Ruby);
      Assert.AreEqual("骸", lst[0].Texts[2].Text);
      Assert.AreEqual("がい", lst[0].Texts[2].Ruby);
      Assert.AreEqual("精神的不満、", lst[1].Texts[0].Text);
    }
    [Test]
    public void ParserFile()
    {
      var path = AppDomain
        .CurrentDomain
        .BaseDirectory
        .Combine("data")
        .Combine("kokoro.htm");
      Assert.AreEqual(true, System.IO.File.Exists(path),"ファイルがない");

      var p = new HKSimpleParser();
      p.ParseFromPath(path);

      var lst = p.ResultParaList;
      Assert.AreEqual(2908, lst.Count);
    }
  }
}
