using CrazyRobot.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrazyRobot.Persistence
{
    public class GameFromText : GameFromFile
    {
        #region Member
        private readonly string _path;
        #endregion

        #region Constructor
        public GameFromText(string path)
        {
            _path = path;
        }
        #endregion

        #region Properties
        public string Path{ get; }
        #endregion

        #region Functions
        public Tuple<Table, Robot, Int32, int, int> Load(string path)
        {
            try
            {
                string text = File.ReadAllText(path);
                string[] lines = text.Split(System.Environment.NewLine);
                string[] data = lines[0].Split(' ');

                Robot robot;
                Table table = new Table(Convert.ToInt32(data[0]));
                int x = Convert.ToInt32(data[1]);
                int y = Convert.ToInt32(data[2]);
                int crazy = Convert.ToInt32(data[3]);
                Int32 time = Convert.ToInt32(data[4]);
                int walls = Convert.ToInt32(data[5]);
                int brokenWalls = Convert.ToInt32(data[6]);
                switch (data[4])
                {
                    case "NORTH":
                        robot = new Robot(Way.NORTH, x, y, crazy);
                        break;
                    case "SOUTH":
                        robot = new Robot(Way.SOUTH, x, y, crazy);
                        break;
                    case "EAST":
                        robot = new Robot(Way.EAST, x, y, crazy);
                        break;
                    case "WEST":
                        robot = new Robot(Way.WEST, x, y, crazy);
                        break;
                    default:
                        robot = new Robot(Way.NORTH, x, y, crazy);
                        break;
                }

                x = 0;
                y = 0;
                for (int i = 1; i < lines.Length; ++i)
                {
                    data = lines[i].Split(' ');
                    y = 0;
                    for (int j = 0; j < data.Length; ++j)
                    {
                        table.Matrix[x][y++] = Convert.ToInt32(data[j]);
                    }
                    x++;
                }
                Tuple<Table, Robot, Int32, int, int> tr = new Tuple<Table, Robot, Int32, int, int>(table, robot, time, walls, brokenWalls);
                return tr;
            }
            catch (Exception e) 
            {
                throw new GameFromFileException(e.Message, e);
            }

        }
        public void Save(string path, Game game)
        {
            string data = game._table.N.ToString() + ' ' + game._robot.X +
                ' ' + game._robot.Y + ' ' + game._robot.Crazy + ' ' + game._time +
                ' ' + game.Walls + ' ' + game.BrokenWalls + ' ';
            switch (game._robot.Way)
            {
                case Way.NORTH:
                    data += "NORTH" + System.Environment.NewLine;
                    break;
                case Way.SOUTH:
                    data += "SOUTH" + System.Environment.NewLine;
                    break;
                case Way.EAST:
                    data += "EAST" + System.Environment.NewLine;
                    break;
                case Way.WEST:
                    data += "WEST" + System.Environment.NewLine;
                    break;
                default:
                    break;
            }
            for (int i = 0; i < game._table.N; i++)
            {
                for (int j = 0; j < game._table.N; j++)
                {
                    data += game._table.Matrix[i][j].ToString() + ' ';
                }
                data = data.Remove(data.Length - 1);
                data += System.Environment.NewLine;
            }
            data = data.Remove(data.Length - 1);
            try
            {
                File.WriteAllText(path, data);
            }
            catch (Exception e)
            {
                throw new GameFromFileException(e.Message, e);
            }
        }
        #endregion
    }
}
