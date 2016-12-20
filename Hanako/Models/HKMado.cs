using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hanako.Models
{
  public class HKMado : IHKMado
  {
    public int Index { get; set; }
    public int Level { get; set; }      //1-
    public string Title { get; set; }
    public int JumpId { get; set; }
  }
}
