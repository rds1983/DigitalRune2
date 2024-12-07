using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using NUnit.Framework;


namespace DigitalRune.Graphics.Tests
{
  [TestFixture]
  public class GraphicsExceptionTest
  {
    [Test]
    public void TestConstructors()
    {
      var exception = new GraphicsException();

      exception = new GraphicsException("message");
      Assert.AreEqual("message", exception.Message);

      exception = new GraphicsException("message", new Exception("inner"));
      Assert.AreEqual("message", exception.Message);
      Assert.AreEqual("inner", exception.InnerException.Message);
    }
  }
}