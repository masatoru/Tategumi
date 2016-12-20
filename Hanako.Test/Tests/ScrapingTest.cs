using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Hanako.Extensions;
using HtmlAgilityPack;
using System.Xml;
using System.Xml.XPath;

namespace Hanako.Test.Tests
{
  [TestFixture]
  public class ScrapingTest
  {
    static async Task<string> readStringFromUrl(string url)
    {
      //Http.HttpClient
      using (var client = new System.Net.Http.HttpClient())
      using (var request = new System.Net.Http.HttpRequestMessage(System.Net.Http.HttpMethod.Get, url))
      using (var response = client.SendAsync(request, CancellationToken.None).Result)
      {
        return await response.Content.ReadAsStringAsync();
      }
    }

    //[Test]
    public async Task Scraping01()
    {
      var url = "http://www.aozora.gr.jp/cards/001542/card52214.html";
      var html = await readStringFromUrl(url);

      var doc = new HtmlDocument();   //HtmlAgilityPack
      doc.LoadHtml(html);
      HtmlNode p = doc.DocumentNode.FindFirst("p");

      Assert.AreEqual("aaaa",p.InnerText);
    }
  }
}
