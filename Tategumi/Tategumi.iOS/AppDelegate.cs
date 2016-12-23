using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

using Foundation;
using Microsoft.Practices.Unity;
using Prism.Unity;
using Tategumi.iOS.Services;
using Tategumi.TategumiViews;
using UIKit;

namespace Tategumi.iOS
{
  public class iOSInitializer : IPlatformInitializer
  {
    public void RegisterTypes(IUnityContainer container)
    {
    }
  }
  // The UIApplicationDelegate for the application. This class is responsible for launching the 
  // User Interface of the application, as well as listening (and optionally responding) to 
  // application events from iOS.
  [Register("AppDelegate")]
  public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
  {

    //
    // This method is invoked when the application has loaded and is ready to run. In this 
    // method you should instantiate the window, load the UI into it and then make the window
    // visible.
    //
    // You have 17 seconds to return from this method, or iOS will terminate your application.
    //
    public override bool FinishedLaunching(UIApplication app, NSDictionary options)
    {
      Debug.WriteLine($"AppDelegate FinishedLaunching");
      // set up resource paths
      //string fontName = "content-font.ttf";
      string fontName = "ipaexm.ttf";
      var fontPath = NSBundle.MainBundle.PathForResource(Path.GetFileNameWithoutExtension(fontName), Path.GetExtension(fontName));
      if (!System.IO.File.Exists(fontPath))
      {
        new UIAlertView("Tateguni", $"ファイルがない {fontPath}", null, "OK").Show();
      }
      Debug.WriteLine($"AppDelegate FinishedLaunching FontPath={fontPath}");
      TategumiViewCore.OpenFontStream = () => File.OpenRead(fontPath);

      var dir = Path.Combine(Path.GetTempPath(), "Tategumi", Path.GetRandomFileName());
      if (!Directory.Exists(dir))
      {
        Directory.CreateDirectory(dir);
      }
      TategumiViewCore.WorkingDirectory = dir;

      global::Xamarin.Forms.Forms.Init();
      Debug.WriteLine($"AppDelegate FinishedLaunching Xamarin.Forms.Forms.Init");

      Xamarin.Forms.DependencyService.Register<ResourceDirectoryImpl>();
      LoadApplication(new App(new iOSInitializer()));

      return base.FinishedLaunching(app, options);
    }
  }
}
