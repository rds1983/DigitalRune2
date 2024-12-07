using System;
using NUnit.Framework;


namespace DigitalRune.Graphics.Tests
{
  internal class MyGraphicsScreen : GraphicsScreen
  {
    public MyGraphicsScreen(IGraphicsService graphicsService) : base(graphicsService) { }
    protected override void OnUpdate(TimeSpan deltaTime) { }
    protected override void OnRender(RenderContext context) { }
  }


  [TestFixture]
  public class GraphicsScreenTest
  {
    [Test]
    public void DefaultConstructor()
    {
      GraphicsScreenCollection graphicsScreenCollection = new GraphicsScreenCollection();
      Assert.AreEqual(0, graphicsScreenCollection.Count);
    }
  }
}
