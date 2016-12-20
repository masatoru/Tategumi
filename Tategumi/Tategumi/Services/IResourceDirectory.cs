using System.IO;

namespace Tategumi.Services
{
  public interface IResourceDirectory
  {
     string ReadText(string fileName);
  }
}
