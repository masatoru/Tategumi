using System.IO;
using Foundation;
using Tategumi.iOS.Services;
using Tategumi.Services;
using Xamarin.Forms;

[assembly: Xamarin.Forms.Dependency(typeof(ResourceDirectoryImpl))]
namespace Tategumi.iOS.Services
{
  public class ResourceDirectoryImpl : IResourceDirectory
  {
    public string ReadText(string fileName)
    {
      var stream = NSBundle.MainBundle.PathForResource(Path.GetFileNameWithoutExtension(fileName), Path.GetExtension(fileName));
      var text = "";
      using (var reader = new System.IO.StreamReader(stream))
      {
        text = reader.ReadToEnd();
      }
      return text;
    }
  }
}


