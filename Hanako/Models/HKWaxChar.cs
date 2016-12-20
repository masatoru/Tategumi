using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hanako.Models
{
  public enum Type
  {
    Text,Ruby,Tateyoko,
    KaiGyo, KaiPage,KaidDan,
  };
  public interface IHKWaxBase
  {
    Type Type { get; }
  };
  public class HKWaxBase
  {
    public string Msg { get; set; }
    //1.Compose
    public float X { get; set; }
    public float Y { get; set; }
    
    //2.createPageList
    public float PageX { get; set; }
    //public float PageY { get; set; }
    
    //3.calcToDevice
    public float DevX { get; set; }
    public float DevY { get; set; }
    public float FlowLength { get; set; }  //組向きに対しての文字の長さ
    //public float Height { get; set; }
    public float FontSize { get; set; }
    public string Char { get; set; }
    public string Ruby { get; set; }
  };
  public class HKWaxChar : HKWaxBase,IHKWaxBase
  {
    public Type Type { get { return Type.Text; } }
  };
  public class HKWaxRuby : HKWaxBase, IHKWaxBase
  {
    public Type Type { get { return Type.Ruby; }}
    //public string Ruby { get; set; }
  };
  public class HKWaxKaigyo : HKWaxBase, IHKWaxBase
  {
    public Type Type { get { return Type.KaiGyo; } }
  };
}
