using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Sokoban.Testing
{
    [TestClass]
    public class UnitTest2
    {
        [TestMethod]
        public void TestMethod1()
        {
            var list = Sokoban.Domain.Domain.Highscores.List.Get();
        }
    }
}
