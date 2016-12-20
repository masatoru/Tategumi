using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hanako.Models
{
  public interface IHKWaxPage : INotifyPropertyChanged
  {
    IList<HKWaxLine> Lines { get; set; }
    int Page { get; }
  }
}
