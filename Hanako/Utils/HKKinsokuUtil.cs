using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hanako.Utils
{
  public class HKKinsokuUtil
  {
    public static bool isStartChar(string ch)
    {
      var tbl = "［（「〈《『【∧";
      if (!string.IsNullOrEmpty(ch) && tbl.IndexOf(ch[0]) >= 0)
        return true;
      return false;
    }
    public static bool isEndChar(string ch)
    {
      var tbl = "］、。）」〉》〕】』∨";
      if (!string.IsNullOrEmpty(ch) && tbl.IndexOf(ch[0]) >= 0)
        return true;
      return false;
    }
  }
}
