using System;
using System.IO;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Util;
using Tategumi.Droid.Services;
using Tategumi.TategumiViews;

namespace Tategumi.Droid
{
  [Activity(Label = "Tategumi", Icon = "@drawable/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
  public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
  {
    private const string TAG = "MAIN";
    protected override void OnCreate(Bundle bundle)
    {
      Log.Info(TAG, "OnCreate 1");

      TabLayoutResource = Resource.Layout.Tabbar;
      ToolbarResource = Resource.Layout.Toolbar;

      // set up resource paths
      //string fontName = "content-font.ttf";
      string fontName = "ipaexm.ttf";   //IPAフォント
      string fontPath = Path.Combine(CacheDir.AbsolutePath, fontName);
      using (var asset = Assets.Open(fontName))
      using (var dest = File.Open(fontPath, FileMode.Create))
      {
        asset.CopyTo(dest);
      }
      TategumiViewCore.OpenFontStream = () => File.OpenRead(fontPath);

      //var dir = Path.Combine(Android.OS.Environment.ExternalStorageDirectory.AbsolutePath, "SkiaSharp.Demos");
      var dir = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "Tategumi");
      if (!Directory.Exists(dir))
      {
        Directory.CreateDirectory(dir);
      }
      Log.Info(TAG, $"OnCreate dir={dir}");

      TategumiViewCore.WorkingDirectory = dir;
      base.OnCreate(bundle);

      Log.Info(TAG, "OnCreate Forms.Init");
      global::Xamarin.Forms.Forms.Init(this, bundle);

      Xamarin.Forms.DependencyService.Register<ResourceDirectoryImpl>();

      Log.Info(TAG, "OnCreate LoadApplication");
      LoadApplication(new App());
    }
  }
}

