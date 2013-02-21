using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sokoban.Domain;
using Sokoban.Domain.Domain;
using Sokoban.Domain.Domain.Floor;
using Sokoban.Domain.Domain.Highscores;

namespace Sokoban.Testing
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void LevelChecking()
        {
            var levels = List.Get();

            foreach (var game in levels.Values)
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
