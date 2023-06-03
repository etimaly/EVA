using CrazyRobot.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrazyRobot.Persistence
{
    public interface GameFromFile
    {
        #region Functions
        public Tuple<Table, Robot, Int32, int, int> Load(string path);
        public void Save(string path, Game game);
        public string Path { get { return Path; } }
        #endregion
    }
}
