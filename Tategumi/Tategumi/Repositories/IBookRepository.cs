using System.Collections.Generic;
using System.Threading.Tasks;
using Tategumi.Models;

namespace Tategumi.Repositories
{
  public interface IBookRepository
  {
    string GetBookFromUrl(string url);
    string GetBookFromFileName(string fileName);
    List<BookItem> GetBooks();
  }
}
