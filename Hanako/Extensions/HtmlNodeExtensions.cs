using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;

namespace Hanako.Extensions
{
  public static class HtmlNodeExtensions
  {
    public static HtmlNode FindFirst(this HtmlNode source,string name)
    {
      var node = source;
      while (true)
      {
        if (node.Name == name) return node;
        if (node.HasChildNodes == true)  //子供がいれば
        {
          var res = node.ChildNodes[0].FindFirst(name);
          if (res != null) return res;
        }
        if (node.NextSibling != null)  //弟がいれば
        {
          var res= node.NextSibling.FindFirst(name);
          if (res != null) return res;
        }
        //一番上になったら抜ける
        if (node.ParentNode.Name == "#document")
          break;
        if (node.ParentNode.NextSibling == null) //親に子供がいれば
          break;
        if (node.ParentNode == null) //親に子供がいれば
          break;
        {
          var res = node.ParentNode.NextSibling.FindFirst(name);
          if (res != null) return res;
        }
      }
      return null;
    }
  }
}
