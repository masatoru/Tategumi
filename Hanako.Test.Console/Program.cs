using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Hanako.Extensions;
using Hanako.Models;
using HtmlAgilityPack;

namespace Hanako.Test
{
  public static class StringExtensions
  {
    public static bool existFile(this string path)
    {
      try
      {
        if(System.IO.File.Exists(path) != true)
          throw new Exception($"ファイルがありません PATH={path}");
      }
      catch (Exception ex)
      {
        throw new Exception(ex.Message);
      }
      return true;
    }
  }

  class Program
  {
    static async Task<string> readStringFromUrl2(string url)
    {
      using (var client = new System.Net.Http.HttpClient())
      {
        //http://www.atmarkit.co.jp/ait/articles/1501/06/news086.html
        // ユーザーエージェント文字列をセット（オプション）
        client.DefaultRequestHeaders.Add(
            "User-Agent",
            "Mozilla/5.0 (Windows NT 6.3; Trident/7.0; rv:11.0) like Gecko");
        // 受け入れ言語をセット（オプション）
        client.DefaultRequestHeaders.Add("Accept-Language", "ja-JP");
        return await client.GetStringAsync(url);
      }  
    }

    static void Main(string[] args)
    {
      //testParser();
      //testGetContent();
      testParserPath();

      System.Console.WriteLine("終了しました");
      System.Console.ReadKey();
      return;
    }

    private static void testGetContent()
    {
      System.Console.WriteLine("testGetContent");
      var url = "http://www.aozora.gr.jp/cards/001542/files/52214_46221.html";  //UserAgentを付けないと空で返ってくる
      System.Console.WriteLine($"url={url}");
      var html = readStringFromUrl2(url).Result;
      if (string.IsNullOrEmpty(html))
        throw new NullReferenceException("なぜかNULL");
      Console.WriteLine($"html={html.Substring(0,200)}");


      var doc = new HtmlDocument(); //HtmlAgilityPack
      //doc.OptionAutoCloseOnEnd = false;  //最後に自動で閉じる（？）
      //doc.OptionCheckSyntax = false;     //文法チェック。
      //doc.OptionFixNestedTags = true;    //閉じタグが欠如している場合の処理
      doc.LoadHtml(html);
      Debug.WriteLine($"root name={doc.DocumentNode.Name}");

      var para = doc.DocumentNode.FindFirst("p");
      System.Console.WriteLine($"para={para.ToString()}");

      System.Console.WriteLine("終了しました");
      System.Console.ReadKey();
    }

    static async Task<string> readStringFromUrl(string url, Encoding enc)
    {
      var cli = new HttpClient();
      var msg = await cli.GetAsync(url);
      var binary = await msg.Content.ReadAsByteArrayAsync();
      System.Console.WriteLine($"ReadString binary length={binary.Length}");
      var text = enc.GetString(binary, 0, binary.Length);
      return text;
    }
    private static void testParser()
    {
      System.Console.WriteLine("testParser");
      var buf = $"<p>如実に<RUBY>形<RT>けい</RT></RUBY><RUBY>骸<RT>がい</RT></RUBY>の中に</p>" +
                "<p>精神的不満、<RUBY>煩<RT>はん</RT></RUBY><RUBY>悶<RT>もん</RT></RUBY>は人間</p>";
      var p = new HKSimpleParser();
      p.ParseFromText(buf);

      var lst = p.ResultParaList;
      System.Console.WriteLine($"paras.count={lst.Count}");
      System.Console.WriteLine($"{lst[0].ToString()}");
    }
    private static void testParserPath()
    {
      var path = ".\\data\\kokoro.htm";
      System.Console.WriteLine($"testParserPath path={path}");

      var p = new HKSimpleParser();
      p.ParseFromPath(path);

      var lst = p.ResultParaList;
      System.Console.WriteLine($"paras.count={lst.Count}");
      System.Console.WriteLine($"{lst[0].ToString()}");
    }
  }
}

