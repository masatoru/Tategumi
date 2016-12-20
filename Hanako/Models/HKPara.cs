#if __TEST__
using kj.kihon;
#endif
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hanako.Models
{
  public class HKPara
  {
    public HKPara()
    {
      Texts = new List<IHKTagBase>();
    }

    public override string ToString()
    {
      var sb=new StringBuilder();
      foreach (var ch in Texts)
      {
        sb.Append(ch.ToString());
      }
      return sb.ToString();
    }

    public List<IHKTagBase> Texts { get; set; }
    public void addText(string oya, string ruby, bool isTateYoko)
    {
      //Debug.WriteLineIf(!string.IsNullOrEmpty(hinshi),$"hkpara addtext honbun={oya} hinshi={hinshi}");
      if (isTateYoko == true)
      {
        Texts.Add(new HKTateChuYoko
        {
          Text = oya,
          Ruby = ruby
        });
      }
      else
      {
        Texts.Add(new HKText
        {
          Text = oya,
          Ruby = ruby
        });
      }
    }
  }
}
