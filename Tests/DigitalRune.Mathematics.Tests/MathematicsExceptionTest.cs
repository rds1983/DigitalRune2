using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using NUnit.Framework;


namespace DigitalRune.Mathematics.Tests
{
  // These unit tests do not make much sense. They are only here to get a test coverage of 100% in NCover.
  [TestFixture]
  public class MathematicsExceptionTest
  {
    [Test]
    public void TestConstructors()
    {
      MathematicsException m = new MathematicsException();

      m = new MathematicsException("hallo");
      Assert.AreEqual("hallo", m.Message);

      m = new MathematicsException("hallo", new Exception("inner"));
      Assert.AreEqual("hallo", m.Message);
      Assert.AreEqual("inner", m.InnerException.Message);
    }
  }
}
