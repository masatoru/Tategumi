using Microsoft.Practices.Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hanako.Models
{
  public class HKWaxPage : BindableBase, IHKWaxPage
  {
    public HKWaxPage(int page)
    {
      Lines = new List<HKWaxLine>();
      Page = page;
    }
    public IList<HKWaxLine> Lines { get; set; }
    public int Page { get; set; }
  };
}
