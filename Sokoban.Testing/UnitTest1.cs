using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sokoban.Domain;
using Sokoban.Domain.Domain;
using Sokoban.Domain.Domain.Floor;

namespace Sokoban.Testing
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void LevelChecking()
        {
            var levels = Domain.Highscores.List.Get();

            foreach (var game in levels)
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
