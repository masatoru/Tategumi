using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
//using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using HtmlAgilityPack;
using Microsoft.Practices.Unity;
using Tategumi.Models;
using Tategumi.Services;
using Xamarin.Forms;

namespace Tategumi.Repositories
{
  public class BookRepository : IBookRepository
  {
    public List<BookItem> GetBooks()
    {
      return new List<BookItem>
      {
        new BookItem
        {
          Title = "こころ",
          Chosha = "夏目　漱石",
          FileName = "kokoro.htm",
        },
        new BookItem
        {
          Title = "赤いカブトムシ",
          Chosha = "江戸川乱歩",
          FileName = "akaikabuto.htm",
        },
      };
    }
#if true  //GetBooks ほんとはスクレイピングするはずだったけど時間がないのであとで
    public List<BookItem> GetBooks_xx()
    {
      Debug.WriteLine($"repo getBooks 1");
      const string url = "http://yozora.kazumi386.org/9/1/ndc910.html";
      var html = GetStringAsync(url).Result;
      Debug.WriteLine($"repo getBooks html length={html.Length}");

      //クローリングして著者/作品の一覧を取得する
      var booklst = new List<BookItem>();
      var doc = new HtmlAgilityPack.HtmlDocument();   //HtmlAgilityPack

      Debug.WriteLine($"repo getBooks loadhtml");
      doc.LoadHtml(html);

      foreach (var li in doc.DocumentNode.ChildNodes
        .First(m => m.Name=="html").ChildNodes
        .First(m => m.Name == "body").ChildNodes
        .First(m => m.Name == "ol").ChildNodes)  //この下に<li>がぶらさがる
      {
          //Debug.WriteLine($"child={item.ToString()} name={item.Name}");
        foreach(var title in li.ChildNodes.Where(m => m.Name=="a"))
        {
          Debug.WriteLine($"href={title.Attributes["href"].Value} chosha={title.InnerText}");
          booklst.Add(new BookItem
          {
            FileName = title.Attributes["href"].Value,
            Title = title.InnerText
          });
        }
      }
      Debug.WriteLine($"Repository GetBooks Count={booklst.Count}");
      return booklst;
    }
#endif
    public string GetBookFromUrl(string url)
    {
      Debug.WriteLine($"BookRepository getBook url={url}");
      return GetStringAsync(url).Result;
    }
    public string GetBookFromFileName(string fileName)
    {
      return DependencyService.Get<IResourceDirectory>().ReadText(fileName);
      
    }

    static async Task<string> GetStringAsync(string url)
    {
      var enc=Encoding.GetEncoding("Shift_JIS");
      var cli = new HttpClient();
      var msg = await cli.GetAsync(url);
      var binary = await msg.Content.ReadAsByteArrayAsync();
      Debug.WriteLine($"ReadString binary length={binary.Length}");
      var text = enc.GetString(binary, 0, binary.Length);
      return text;
    }

    static async Task<string> GetStringAsync2(string url)
    {
      var client = new System.Net.Http.HttpClient();
      client.DefaultRequestHeaders.Accept.Clear();
      client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/html"));
      var response = await client.GetAsync(url).ConfigureAwait(false);
      return await response.Content.ReadAsStringAsync();

      //http://www.atmarkit.co.jp/ait/articles/1501/06/news086.html
      // ユーザーエージェントをつけないと空で返ってくるケースがあるので
      /*        client.DefaultRequestHeaders.Add(
                  "User-Agent",
                  "Mozilla/5.0 (Windows NT 6.3; Trident/7.0; rv:11.0) like Gecko");
              // 受け入れ言語をセット（オプション）
              client.DefaultRequestHeaders.Add("Accept-Language", "ja-JP");
      */
      return await client.GetStringAsync(url);
    }
  }
}
