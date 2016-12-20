using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using HtmlAgilityPack;
using Hanako.Extensions;

namespace Hanako.Models
{
  public class HKSimpleParser
  {
    public HKSimpleParser()
    {
      ResultParaList = new List<HKPara>();
    }
    public List<HKPara> ResultParaList { get; set; }

    void addTextNode(HtmlNode node)
    {
      var para = ResultParaList.Last();
      para.addText(node.InnerText,"",false);
    }
    void addRubyNode(HtmlNode node)
    {
      var para = ResultParaList.Last();
      var oya = node.ChildNodes?[0]?.InnerText;
      var ruby = "";
      if(node.ChildNodes.Count>=2)
        ruby = node.ChildNodes[1].InnerText;
      para.addText(oya, ruby, false);
    }

    public void ParseFromPath(string path)
    {
      var doc = new HtmlDocument();   //HtmlAgilityPack
      doc.Load(path);
      Parth(doc);
    }
    /**
         <p>文字<ruby>漢字<rt>かんじ</rt></ruby></p> 
    */
    public void ParseFromText(string buf)
    {
      var doc = new HtmlDocument();   //HtmlAgilityPack
      doc.LoadHtml(buf);
      Parth(doc);
    }

    private void Parth(HtmlDocument doc)
    {
      var para = doc.DocumentNode.FindFirst("p"); //Extension 最初の<p>を見つける
      while (para != null)
      {
        ResultParaList.Add(new HKPara());
        foreach (var item in para.ChildNodes) //<p>...</p>
        {
          if (item.NodeType == HtmlNodeType.Text)
          {
            addTextNode(item);
            continue;
          }
          if (item.NodeType == HtmlNodeType.Element && item.Name == "ruby")
          {
            addRubyNode(item);
          }
        }
        para = para.NextSibling; //弟を全部回す
      }
    }
  }
}
