using System.Collections.Generic;
using System.Threading.Tasks;
using Tategumi.Models;
using Tategumi.Repositories;

namespace Tategumi.Services
{
  public class BookService2
  {
    readonly IBookRepository _repo;
    public BookService2(IBookRepository repo) { _repo = repo; }

    public string GetBook(string path)
    {
      return _repo.GetBookFromFileName(path);
    }
    public List<BookItem> GetBooks()
    {
      return _repo.GetBooks();
    }
  }
}
