using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hanako.Models
{
  public class HKDevice
  {
    public float FontSize { get; set; }
    public float DevWidth { get; set; }
    public float DevHeight { get; set; }
    public float MarginRight { get; set; }
    public float MarginLeft { get; set; }
    public float MarginTop { get; set; }
    public float MarginBottom { get; set; }
    //float Bottom { get { return DevHeight - MarginBottom; } }
    //float Left { get { return MarginLeft; } }

    //描画領域
    public float ViewWidth { get { return DevWidth - MarginRight - MarginLeft; } }

    public void setup(float fntsz,float dev_w, float dev_h,float mg_lft,float mg_top,float mg_rgt,float mg_btm)
    {
      FontSize = fntsz;
      DevWidth = dev_w;
      DevHeight = dev_h;
      MarginLeft = mg_lft;
      MarginTop = mg_top;
      MarginRight = mg_rgt;
      MarginBottom = mg_btm;
    }

    //デバイスの単位に変換
    // lnlst 右上をゼロとして左にプラス
    // デバイス 左上がゼロ、右へプラス
    public void calcToDevice(ref IList<IHKWaxPage> pglst)
    {
      if (DevWidth <=0 || DevHeight <=0 || FontSize<=0)
        throw new Exception("幅/高さ/文字サイズが定義されていない");
      foreach (var pg in pglst)
      {
        foreach (var ln in pg.Lines)  //行
        {
          foreach (var ch in ln.Chars)  //文字
          {
            //右ゼロからの座標を左からの余白含めての座標に変換
            float ch_x = ch.X - ViewWidth * (pg.Page - 1);  //ページの位置分Xを引く
            ch.DevX = (ViewWidth - ch_x) + MarginLeft - FontSize;
            ch.DevY = MarginTop + ch.Y /*+ FontSize*//*11文字分下げる*/;
          }
        }
      }
    }
  }
}
