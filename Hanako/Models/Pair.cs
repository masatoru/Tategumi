using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hanako.Models
{
  public class Pair<K, V>
  {
    K key;
    V val;

    public K Key { get { return this.key; } set { this.key = value; } }
    public V Value { get { return this.val; } set { this.val = value; } }
  }
}
