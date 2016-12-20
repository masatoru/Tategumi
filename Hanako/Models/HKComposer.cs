using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hanako.Utils;

namespace Hanako.Models
{
  public class HKComposer
  {
    StringBuilder log_ = new StringBuilder();
    public string Log { get { return log_.ToString(); } }

    public List<HKWaxLine> LineList { get; set; }

    public float FontSize { get; set; }
    public float GyokanSize { get; set; }

    public float Width { get; set; }
    public float Height { get; set; }
#if false
    public float MarginRight { get; set; }
    public float MarginLeft { get; set; }
    public float MarginTop { get; set; }
    public float MarginBottom { get; set; }
    float Bottom { get { return Height - MarginBottom; } }
    float Left { get { return MarginLeft; } }
#endif
    float StartX { get { return 0; } }
    float StartY { get { return 0; } }
    float CurX { get; set; }
    float CurX2 { get; set; }   //縦中横で進んだやつを戻す
    float CurY { get; set; }
    float CurFontSize { get; set; }   //現在のフォントサイズ

    public HKComposer()
    {
      FontSize = 34;
      GyokanSize = FontSize*0.5f;
#if false
      MarginRight = 0;
      MarginLeft = 0;
      MarginTop = 0;
      MarginBottom = 0;
#endif
    }
    public void Init(float view_w,float view_h)
    {
      Width = view_w;
      Height = view_h;
      CurFontSize = FontSize;
    }
    //右上に戻る
    void InitPosition()
    {
      CurX = StartX;
      CurX2 = StartX;
      CurY = StartY;
    }
    //行末にきたかどうか
    bool IsOverLine(char ch)
    {
      if (CurY + CurFontSize * 2 > Height &&
           HKKinsokuUtil.isStartChar(ch.ToString()) == true)
      {
        return true;
      }
      //閉じ括弧などは下につっこむ
      if (HKKinsokuUtil.isEndChar(ch.ToString()) == true)
      {
        return false;
      }
      return (CurY + CurFontSize > Height) ? true : false;
    }
    void nextLine()
    {
      //1行追加
      LineList.Add(new HKWaxLine(LineList.Count+1));
      //左に移動する
      CurX += FontSize + GyokanSize;
      CurX2 = CurX;
      CurY = StartY;
    }
    void addChar(string ch,string ruby)
    {
      if (LineList.Count==0)
        LineList.Add(new HKWaxLine(LineList.Count + 1));
      //Debug.WriteLineIf(!string.IsNullOrEmpty(hinshi),$"composer addchar honbun={ch} hinsi={hinshi}");

      LineList.Last()
        .Chars
        .Add(new HKWaxChar
        {
          Char=ch,
          X=CurX,
          Y=CurY,
          FontSize=CurFontSize,
          Ruby=ruby
        });
    }

    void nextChar(Orientaion o, int index, int len)
    {
      //縦中横は最後の文字以外は右へ
      if (o == Orientaion.Horizontal && index + 1 < len)
      { 
        CurX -= CurFontSize*0.5f;
        return;
      }
      CurX = CurX2;
      CurY += CurFontSize;
    }

    public void Compose(List<HKPara> paralst, ref List<HKWaxLine> lnlst)
    {
      LineList = lnlst;

      InitPosition();

      //段落ごとにまわす
      foreach (var item in paralst.Select((v,i)=>new {v,i}))
      {
        HKPara para = item.v;
        if(item.i!=0)
          nextLine(); //最初以外
        foreach (IHKTagBase txt in para.Texts)
        {
          //Debug.WriteLine($"composer composer hinshi={txt.Hinshi}");
          for(int m=0; m<txt.Text.Length;m++)  //一文字ずつ追加する
          {
            var ch = txt.Text[m];
            var ruby = (m == 0) ? txt.Ruby : "";   //グループもあるので最初の親文字にルビをぶらさげる
            //Debug.WriteLine($"{m}:{ch}");
            //折り返しによる改行
            var preY = CurY;
            //追い出し,ぶらさげチェック
            if (m < txt.Text.Length-1)  //最後の文字は改行チェックをしない
            {
              if (IsOverLine(ch) == true)
              {
                log_.AppendLine($"{m}/{txt.Text.Length}: IsOverLine preY={preY} CurY({CurY}) + CurFontSize({CurFontSize}) > Height({Height}) SrartY={StartY}");
                nextLine();
              }
            }
            //文字追加
            addChar(ch.ToString(),ruby);
            //移動する 縦中横:最後の文字は+Y,それ以外は+X
            nextChar(txt.Orientaion,m,txt.Text.Length);            
          }
        }
      }
    }
  }
}
