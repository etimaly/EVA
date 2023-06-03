using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrazyRobot.model
{
    public class GameEvent : EventArgs
    {
        #region Members
        private Int32 _gameTime;
        private Int32 _walls;
        private Int32 _brokenWalls;
        private Boolean _isWon;
        #endregion

        #region Properties
        public Int32 GameTime { get { return _gameTime; } }
        public Int32 Walls { get { return _walls; } }
        public Int32 BrokenWalls { get { return _brokenWalls; } }
        public Boolean IsWon { get { return _isWon; } }
        #endregion

        #region Consrtuctor
        public GameEvent(Boolean isWon, Int32 walls, Int32 brokenWalls, Int32 gameTime)
        {
            _isWon = isWon;
            _walls = walls;
            _brokenWalls = brokenWalls;
            _gameTime = gameTime;
        }
        #endregion
    }
}
