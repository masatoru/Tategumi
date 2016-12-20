using System.IO;
using Tategumi.Droid.Services;
using Tategumi.Services;
using Xamarin.Forms;

[assembly: Xamarin.Forms.Dependency(typeof(ResourceDirectoryImpl))]
namespace Tategumi.Droid.Services
{
  public class ResourceDirectoryImpl : IResourceDirectory
  {
    public string ReadText(string fileName)
    {
      var stream = Forms.Context.Assets.Open(fileName);
      var text = "";
      using (var reader = new System.IO.StreamReader(stream))
      {
        text = reader.ReadToEnd();
      }
      return text;
    }
  }
}


