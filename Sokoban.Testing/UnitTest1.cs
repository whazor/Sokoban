using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sokoban.Domain;

namespace Sokoban.Testing
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void LevelChecking()
        {
            foreach (var game in Game.GetLevels())
            {
                try
                {
                    new Game(game);
                }
                catch (LevelException e)
                {
                    e.Level = game.Name;
                    throw e;
                }
            }
        }
    }
}
