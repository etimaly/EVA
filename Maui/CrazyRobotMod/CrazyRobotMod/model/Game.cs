using CrazyRobotMod.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrazyRobotMod.model
{
    public class Game
    {
        #region Members
        private GameFromFile file;
        private Robot robot;
        private Table table;
        private Int32 time = 0;
        private int walls = 0;
        private int brokenWalls = 0;
        #endregion

        #region Events
        public event EventHandler<GameEvent>? GameAdvanced;
        public event EventHandler<GameEvent>? GameOver;
        #endregion

        #region Constructors
        public Game(GameFromFile gmf)
        {
            file = gmf;
            file.Dir = gmf.Dir;
            table = new Table(7);
            Random random = new Random();
            int way = random.Next(0, 4);
            int rndx, rndy, n = 7;
            do
            {
                rndx = random.Next(0, n);
                rndy = random.Next(0, n);
            } while (rndx == n / 2 & rndy == n / 2);
            Way rway;
            if (way == 0) { rway = Way.NORTH; }
            else if (way == 1) { rway = Way.SOUTH; }
            else if (way == 2) { rway = Way.EAST; }
            else { rway = Way.WEST; }
            this.robot = new Robot(rway, rndx, rndy, 10);
        }
        public Game(int n)
        {
            this.file = new GameFromText("");
            Random random = new Random();
            int way = random.Next(0, 4);
            int rndx, rndy;
            do
            {
                rndx = random.Next(0, n);
                rndy = random.Next(0, n);
            } while (rndx == n / 2 & rndy == n / 2);
            Way rway;
            if (way == 0) { rway = Way.NORTH; }
            else if (way == 1) { rway = Way.SOUTH; }
            else if (way == 2) { rway = Way.EAST; }
            else { rway = Way.WEST; }
            this.robot = new Robot(rway, rndx, rndy, 10);
            this.table = new Table(n);
        }
        #endregion

        #region Properties
        public Robot _robot
        {
            get { return this.robot; }
        }
        public Table _table
        {
            get { return this.table; }
        }
        public GameFromFile _file
        {
            get { return this.file; }
        }
        public Int32 _time { get { return time; } }
        public int Walls { get { return walls; } }
        public int BrokenWalls { get { return brokenWalls; } }
        public Boolean isGameOver
        {
            get
            {
                return (brokenWalls == (table.N * table.N)
                    || (robot.X == table.N / 2 && robot.Y == table.N / 2));
            }
        }
        #endregion

        #region public Functions
        public void round()
        {
            time += 1;
            if (isGameOver)
            {
                if (robot.X == (table.N / 2) & robot.Y == (table.N / 2))
                {
                    OnGameOver(true);
                }
                else
                {
                    OnGameOver(false);
                }
            }
            OnGameAdvanced();
            Random random = new Random();
            int rand = random.Next(0, 100);
            if (rand <= robot.Crazy)
            {
                changeWay();
            }
            else
            {

            }

            step();
            //print();
        }
        public void place(int x, int y)
        {
            if (table.Matrix[x][y] == 0)
            {
                table.Matrix[x][y] = 1;
                walls += 1;
            }
        }
        public void NewGame(int n)
        {
            this.table = new Table(n);
            this.file = new GameFromText("");
            Random random = new Random();
            int rndx, rndy;
            do
            {
                rndx = random.Next(0, n);
                rndy = random.Next(0, n);
            } while (rndx == n / 2 & rndy == n / 2);
            int way = random.Next(0, 4);

            Way rway;
            if (way == 0) { rway = Way.NORTH; }
            else if (way == 1) { rway = Way.SOUTH; }
            else if (way == 2) { rway = Way.EAST; }
            else { rway = Way.WEST; }
            this.robot = new Robot(rway, rndx, rndy, 10);
            this.time = 0;
            this.walls = 0;
            this.brokenWalls = 0;
        }
        public void Save(string path)
        {
            if (file == null)
            {
                throw new InvalidOperationException("No data access is provided.");
            }
            file.Save(path, this);
        }
        public void Load(string path)
        {
            if (file == null)
            {
                throw new InvalidOperationException("No data access is provided.");
            }
            Tuple<Table, Robot, Int32, int, int> tr = file.Load(path);
            this.table = tr.Item1;
            this.robot = tr.Item2;
            this.time = tr.Item3;
            this.walls = tr.Item4;
            this.brokenWalls = tr.Item5;
        }

        public async Task LoadGameAsync(string path)
        {
            if (file == null)
            {
                throw new InvalidOperationException("No data access is provided.");
            }
            Tuple<Table, Robot, Int32, int, int> tr = await file.LoadAsync(path);
            this.table = tr.Item1;
            this.robot = tr.Item2;
            this.time = tr.Item3;
            this.walls = tr.Item4;
            this.brokenWalls = tr.Item5;
        }
        public async Task SaveGameAsync(string path)
        {
            if (file == null)
            {
                throw new InvalidOperationException("No data access is provided.");
            }
            await file.SaveAsync(path, this);
        }
        #endregion

        #region private Functions
        private void step()
        {
            switch (robot.Way)
            {
                case Way.NORTH:
                    if (check(robot.X - 1, robot.Y))
                    {
                        robot.X -= 1;
                    }
                    else { changeWay(); }
                    break;
                case Way.SOUTH:
                    if (check(robot.X + 1, robot.Y))
                    {
                        robot.X += 1;
                    }
                    else { changeWay(); }
                    break;
                case Way.EAST:
                    if (check(robot.X, robot.Y + 1))
                    {
                        robot.Y += 1;
                    }
                    else { changeWay(); }
                    break;
                case Way.WEST:
                    if (check(robot.X, robot.Y - 1))
                    {
                        robot.Y -= 1;
                    }
                    else { changeWay(); }
                    break;
                default:
                    break;

            }
        }
        private bool check(int x, int y)
        {
            if (x == table.N || y == table.N || y < 0 || x < 0)
            {
                return false;
            }
            if (table.Matrix[x][y] == 1)
            {
                table.Matrix[x][y] = 2;
                brokenWalls += 1;
                walls -= 1;
                return false;
            }
            return true;
        }
        private void changeWay()
        {
            //Console.WriteLine("CHANGE");
            if (robot.Way == Way.SOUTH & robot.Y == table.N - 1 &
                robot.X == table.N - 1) { robot.Way = Way.WEST; }

            else if (robot.Way == Way.EAST & robot.Y == table.N - 1 &
                robot.X == table.N - 1) { robot.Way = Way.NORTH; }

            else if (robot.Way == Way.SOUTH & robot.Y == 0 &
                robot.X == table.N - 1) { robot.Way = Way.WEST; }

            else if (robot.Way == Way.WEST & robot.Y == 0 &
                robot.X == table.N - 1) { robot.Way = Way.NORTH; }

            else if (robot.Way == Way.WEST & robot.Y == 0 &
                robot.X == 0) { robot.Way = Way.SOUTH; }

            else if (robot.Way == Way.NORTH & robot.Y == 0 &
                robot.X == 0) { robot.Way = Way.EAST; }

            else if (robot.Way == Way.NORTH & robot.Y == table.N - 1 &
                robot.X == 0) { robot.Way = Way.WEST; }

            else if (robot.Way == Way.EAST & robot.Y == table.N - 1 &
                robot.X == 0) { robot.Way = Way.SOUTH; }
            else
            {
                Random random = new Random();
                int rand = random.Next(1, 3);
                switch (robot.Way)
                {
                    case Way.NORTH:
                        if (rand == 1 & robot.Y - 1 > 0) { robot.Way = Way.EAST; }
                        else { robot.Way = Way.WEST; }
                        break;
                    case Way.SOUTH:
                        if (rand == 1 & robot.Y + 1 < table.N) { robot.Way = Way.EAST; }
                        else { robot.Way = Way.WEST; }
                        break;
                    case Way.EAST:
                        if (rand == 1 & robot.X + 1 < table.N) { robot.Way = Way.NORTH; }
                        else { robot.Way = Way.SOUTH; }
                        break;
                    case Way.WEST:
                        if (rand == 1 & robot.X - 1 > 0) { robot.Way = Way.NORTH; }
                        else { robot.Way = Way.SOUTH; }
                        break;
                    default:
                        break;

                }
            }
        }
        #endregion

        #region Private event
        private void OnGameAdvanced()
        {
            GameAdvanced?.Invoke(this, new GameEvent(false, walls, brokenWalls, time));
        }
        private void OnGameOver(Boolean isWon)
        {
            GameOver?.Invoke(this, new GameEvent(isWon, walls, brokenWalls, time));
        }
        #endregion
    }
}
