using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tategumi.Models
{
  public class BookItem
  {
    public string FileName { get; set; }
    public string Title { get; set; }
    public string Chosha { get; set; }
    public string TitleChosha
    {
      get
      {
        if (string.IsNullOrEmpty(Chosha))
          return Title;
        return $"{Title}／{Chosha}";
      }
    }
  }
}