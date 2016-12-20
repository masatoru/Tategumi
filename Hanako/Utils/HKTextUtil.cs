using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hanako.Models;

namespace Hanako.Utils
{
  public class HKTextUtil
  {
    //数字とそれ以外を分ける 数字はtrue
    public static List<Pair<string, bool>> DivTateYoko(string buf)
    {
      var lst = new List<Pair<string, bool>>();
      var cur = "";
      var isPreDigit = false;
      for (int m = 0; m < buf.Length; m++)
      {
        var ch = buf[m];
        if (m == 0)
        {  //最初
          cur = ch.ToString();
          isPreDigit = char.IsDigit(ch);
          continue;
        }
        if (isPreDigit != char.IsDigit(ch))  //違っていたらそれまでを追加
        {
          lst.Add(new Pair<string, bool>
          {
            Key = cur,
            Value = isPreDigit
          });
          cur = "";
        }
        cur += ch;
        isPreDigit = char.IsDigit(ch);
      }
      if (cur.Length > 0)
      {
        lst.Add(new Pair<string, bool>
        {
          Key = cur,
          Value = isPreDigit
        });
      }
      return lst;
    }
  }
}
