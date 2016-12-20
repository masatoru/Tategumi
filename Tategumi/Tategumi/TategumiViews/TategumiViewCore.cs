using System;
using System.Diagnostics;
using System.Linq;
using Hanako.Models;
using SkiaSharp;

namespace Tategumi.TategumiViews
{
  public class TategumiViewCore
  {

    public static void DrawHonbunPage(SKCanvas canvas, IHKWaxPage page, bool isVisibleHinshi)
    {
      WaterTrans.TypeLoader.TypefaceInfo tfi;
      using (var fs = OpenFontStream())
        tfi = new WaterTrans.TypeLoader.TypefaceInfo(fs);

      using (var paint = new SKPaint())
      using (var tf = SKTypeface.FromStream(new SKManagedStream(OpenFontStream(), true)/*,0*/))
      {
        canvas.DrawColor(SKColors.White);     //背景を白に
        paint.IsAntialias = true;
        paint.Typeface = tf;
        //paint.IsVerticalText = true;

        //ページを描画
        drawPage(canvas,paint,tfi, page, isVisibleHinshi);
      }
    }

    static void drawHonbunChar(SKCanvas canvas, SKPaint paint, WaterTrans.TypeLoader.TypefaceInfo tfi, HKWaxBase ch)
    {
      //文字をGlyphIdに変換
      var glyphs = stringToVerticalGlyphs(ch.Char, paint, tfi);
      //描画の文字サイズを設定
      paint.TextSize = ch.FontSize;
      //本文を描画
      drawText(canvas, glyphs, ch.DevX, ch.DevY, paint);
    }
    static unsafe void drawText(SKCanvas canvas, ushort[] glyphs, float x, float y, SKPaint paint)
    {
      x -= SHIFT_X;
      y += SHIFT_Y;
      paint.TextEncoding = SKTextEncoding.GlyphId;
      fixed (ushort* p = glyphs)
      {
        canvas.DrawText((IntPtr)p, glyphs.Length * 2, x, y, paint);
      }
    }

    static void drawRubyText(SKCanvas canvas, SKPaint paint, WaterTrans.TypeLoader.TypefaceInfo tfi, HKWaxBase ch)
    {
      if (string.IsNullOrEmpty(ch.Ruby))
        return;
      float rubyFontSize = ch.FontSize * 0.6f;  //ルビの文字サイズは本文の半分
      paint.TextSize = rubyFontSize;
      var glyphs = stringToVerticalGlyphs(ch.Ruby, paint, tfi);
      float curY = ch.DevY;
      foreach (var glyph in glyphs)
      {
        drawChar(canvas, glyph, ch.DevX + ch.FontSize /*本文の右*/,
          curY, paint);
        curY += paint.TextSize;/*下に進む*/
      }
    }
#if false  //品詞を描画
    static void drawHinshiText(SKCanvas canvas, SKPaint paint, WaterTrans.TypeLoader.TypefaceInfo tfi, HKWaxBase ch,bool bRightLeft)
    {
      if (string.IsNullOrEmpty(ch.Hinshi))
        return;
      //Debug.WriteLine($"drawhinshi hinsi={ch.Hinshi}");

      float hinshiFontSize = ch.FontSize * 0.5f;  //ルビの文字サイズは本文の半分
      paint.TextSize = hinshiFontSize;
      var glyphs = stringToVerticalGlyphs(ch.Hinshi, paint, tfi);
      float curX = ch.DevX;
      if (bRightLeft != true)   //左右交互
        curX += paint.TextSize; 
      //if (ch.Hinshi.Length >= 2)
      //  curX -= paint.TextSize;   //左から2文字
      float curY = ch.DevY;
      foreach (var glyph in glyphs)
      {
        drawChar(canvas, glyph, curX - ch.FontSize/*本文の左側*/,
          curY, paint);
        curY += paint.TextSize; /*下に進む*/
      }
    }
#endif
    //ページを描画
    static void drawPage(SKCanvas canvas, SKPaint paint, WaterTrans.TypeLoader.TypefaceInfo tfi, IHKWaxPage page, bool isVisibleHinshi)
    {
      foreach (var item in page.Lines.Select((v,i)=>new {v,i}))
      {
        foreach (var ch in item.v.Chars)
        {
          paint.Color = new SKColor(0,0,0); 
          //本文を描画
          drawHonbunChar(canvas, paint, tfi, ch);
          //右ルビを描画
          drawRubyText(canvas,paint,tfi,ch);
        }
      }
    }

    //文字列からGlyphIdを取得
    static ushort[] stringToVerticalGlyphs(string text, SKPaint paint, WaterTrans.TypeLoader.TypefaceInfo typefaceInfo)
    {
      ushort[] glyphs;
      paint.Typeface.CharsToGlyphs(text, out glyphs);
      var conv = typefaceInfo.GetVerticalGlyphConverter();
      for (int i = 0; i < glyphs.Length; i++)
      {
        if (conv.CanConvert(glyphs[i]))
          glyphs[i] = conv.Convert(glyphs[i]);
      }
      return glyphs;
    }

    private const float SHIFT_X = 15f;
    private const float SHIFT_Y = 15f;

    static unsafe void drawChar(SKCanvas canvas, ushort glyph, float x, float y, SKPaint paint)
    {
      x -= SHIFT_X;
      //y += SHIFT_Y;
      paint.TextEncoding = SKTextEncoding.GlyphId;
      ushort* p = &glyph;
      {
        canvas.DrawText((IntPtr)p, 2, x, y, paint);
      }
    }


    [Flags]
    public enum Platform
    {
      iOS = 1,
      Android = 2,
      OSX = 4,
      WindowsDesktop = 8,
      UWP = 16,
      tvOS = 32,
      All = 0xFFFF,
    }

    public class DrawMethod
    {
      public Action<SKCanvas, IHKWaxPage, bool> Method { get; internal set; }
      public Platform Platform { get; internal set; }
      public Action TapMethod { get; internal set; }
    }
    public static DrawMethod get()
    {
      return new DrawMethod { Method = DrawHonbunPage /*, Platform = Platform.All*/ };
    }
    public static Func<System.IO.Stream> OpenFontStream { get; set; }
    public static string WorkingDirectory { get; set; }
    public static Action<string> OpenFileDelegate { get; set; }
  }
}
