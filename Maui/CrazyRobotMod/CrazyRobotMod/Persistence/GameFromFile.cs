using CrazyRobotMod.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrazyRobotMod.Persistence
{
    public interface GameFromFile
    {
        #region Functions
        public Tuple<Table, Robot, Int32, int, int> Load(string path);
        public void Save(string path, Game game);
        public Task<Tuple<Table, Robot, Int32, int, int>> LoadAsync(string path);
        public Task SaveAsync(string path, Game game);
        public string Dir { get; set; }
        #endregion
    }
}
