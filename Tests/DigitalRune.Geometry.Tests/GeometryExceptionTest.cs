using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using NUnit.Framework;


namespace DigitalRune.Geometry.Tests
{
  // These unit tests do not make much sense. They are only here to get a test coverage of 100% in NCover.
  [TestFixture]
  public class GeometryExceptionTest
  {
    [Test]
    public void TestConstructors()
    {
      GeometryException m = new GeometryException();

      m = new GeometryException("hallo");
      Assert.AreEqual("hallo", m.Message);

      m = new GeometryException("hallo", new Exception("inner"));
      Assert.AreEqual("hallo", m.Message);
      Assert.AreEqual("inner", m.InnerException.Message);
    }
  }
}
