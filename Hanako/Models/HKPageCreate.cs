using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hanako.Models
{
  public class HKPageCreate
  {
    //描画するページのWaxLineListを取得
    // lnlst : X は 15 ==> 15+22.5... ==> 15+22.5+22.5...
    public static void CreatePageList(float viewW,float fntSz, List<HKWaxLine> lnlst, ref List<IHKWaxPage> pglst)
    {
      var curPage = 1;
      var pageStartX = 0f;   //改頁したことでXの位置をゼロに戻す
      pglst.Add(new HKWaxPage(curPage));

      foreach (var line in lnlst)
      {
        //超えるなら改頁
        if (line.X > viewW * curPage - fntSz)  //line.Xの値は 15 ==> 15+22.5 ==> 15+22.5+22.5
        {
          curPage++;
          pglst.Add(new HKWaxPage(curPage));
          //continue;
        }
        //ページの最初ならXを調整してPageXに入れる
        if (pglst.Last().Lines.Count == 0)
          pageStartX = line.X;
        //ページの開始位置からのPageXを反映
        foreach (var ch in line.Chars)
          ch.PageX = ch.X - pageStartX;
        //ここで行を追加
        pglst.Last().Lines.Add(line);
      }
    }
  }
}
