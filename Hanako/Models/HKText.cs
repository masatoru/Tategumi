using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hanako.Models
{
  public enum TagType
  {
    Text,TateChuYoko,
  };
  public enum Orientaion
  {
    Vertical, Horizontal,
  };

  public interface IHKTagBase
  {
    TagType TagType{ get;}
    Orientaion Orientaion { get; }
    string Text { get; set; }
    string Ruby { get; set; }
  }
  public class HKText : IHKTagBase
  {
    public TagType TagType => TagType.Text;
    public Orientaion Orientaion => Orientaion.Vertical;
    public string Text { get; set; }
    public string Ruby { get; set; }
    public override string ToString()
    {
      if(!string.IsNullOrEmpty(Ruby))
        return $"<ruby>{Text}<rt>{Ruby}</rt></ruby>";
      return Text;
    }
  }
  public class HKTateChuYoko : IHKTagBase
  {
    public TagType TagType => TagType.TateChuYoko;
    public Orientaion Orientaion => Orientaion.Horizontal;
    public string Text { get; set; }
    public string Ruby { get; set; }
    public string Hinshi { get; set; }
  }
}
