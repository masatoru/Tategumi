using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hanako.Models
{
  public class HKWaxLine
  {
    public HKWaxLine(int idx)
    {
      Index = idx;
      Chars = new List<HKWaxBase>();
    }
    public int Index { get; set; }
    public List<HKWaxBase> Chars { get; set; }

    public float X { get {
        if (Chars == null || Chars.Count == 0)
        {
          return 0;
          //throw new Exception($"Charsが空(WaxLine.X) Index={Index} Text={getText()}");
        }
        return Chars[0].X;
      }}
    public float PageX { get { return Chars[0].PageX; } }
    public string getText()
    {
      StringBuilder sb = new StringBuilder();
      foreach (var ch in Chars)
        sb.Append(ch.Char);
      return sb.ToString();
    }
  };
}
