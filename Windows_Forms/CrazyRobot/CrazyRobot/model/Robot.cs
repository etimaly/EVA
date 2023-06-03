using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public enum Way
{
    NORTH, SOUTH, EAST, WEST
};

namespace CrazyRobot.model
{
    public class Robot
    {
        #region Members
        private Way _way;
        private int _x;
        private int _y;
        private int _crazy;
        #endregion

        #region Constructor
        public Robot(Way way, int x, int y, int crazy)
        {
            _way = way;
            _x = x;
            _y = y;
            _crazy = crazy;
        }
        #endregion

        #region Properties

        public Way Way
        {
            get { return _way; }
            set { _way = value; }
        }
        public int X
        {
            get { return _x; }
            set { _x = value; }
        }
        public int Y
        {
            get { return _y; }
            set { _y = value; }
        }
        public int Crazy
        {
            get { return _crazy; }
            set { _crazy = value; }
        }
        #endregion
    }
}
