using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hanako.Utils;

namespace Hanako.Test.Tests
{
  [TestFixture]
  public class TextUtilTest
  {
    [Test]
    public void DivTateYoko()
    {
      var lst = HKTextUtil.DivTateYoko("あああ123いいい1うう00えええ");
      Assert.AreEqual(7, lst.Count);
      Assert.AreEqual("あああ", lst[0].Key);
      Assert.AreEqual(false, lst[0].Value);
      Assert.AreEqual("123", lst[1].Key);
      Assert.AreEqual(true, lst[1].Value);
      Assert.AreEqual("いいい", lst[2].Key);
      Assert.AreEqual(false, lst[2].Value);
      Assert.AreEqual("1", lst[3].Key);
      Assert.AreEqual(true, lst[3].Value);
    }
  }
}
