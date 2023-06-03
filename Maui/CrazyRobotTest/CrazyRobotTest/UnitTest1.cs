using CrazyRobotMod.model;
using CrazyRobotMod.Persistence;
using Moq;

namespace CrazyRobotTest
{
    [TestClass]
    public class UnitTest1
    {
        #region Members
        private Mock<GameFromFile> mock = null!;
        private Game game = null!;
        private Robot mockedRobot = null!;
        private Table mockedTable = null!;
        #endregion

        #region Initialize
        [TestInitialize]
        public void TestInitialize()
        {
            mockedTable = new Table(7);
            mockedRobot = new Robot(Way.NORTH, 3, 4, 10);
            mock = new Mock<GameFromFile>();
            game = new Game(mock.Object);

            game.GameAdvanced += new EventHandler<GameEvent>(Model_GameAdvanced);
            game.GameOver += new EventHandler<GameEvent>(Model_GameOver);
        }
        #endregion

        #region New game
        [TestMethod]
        public void NewGameSmallTest()
        {
            game.NewGame(7);
            for (int i = 0; i < 7; i++)
            {
                for (int j = 0; j < 7; j++)
                {
                    Assert.AreEqual(0, game._table.Matrix[i][j]);
                }
            }

            Assert.AreEqual(0, game.Walls);
            Assert.AreEqual(0, game.BrokenWalls);
            Assert.AreEqual(0, game._time);
            Assert.AreEqual(7, game._table.N);


        }

        [TestMethod]
        public void NewGameMediumTest()
        {
            game.NewGame(11);
            for (int i = 0; i < 11; i++)
            {
                for (int j = 0; j < 11; j++)
                {
                    Assert.AreEqual(0, game._table.Matrix[i][j]);
                }
            }
            Assert.AreEqual(0, game.Walls);
            Assert.AreEqual(0, game.BrokenWalls);
            Assert.AreEqual(0, game._time);
            Assert.AreEqual(11, game._table.N);

        }

        [TestMethod]
        public void NewGameBigTest()
        {
            game.NewGame(15);
            for (int i = 0; i < 15; i++)
            {
                for (int j = 0; j < 15; j++)
                {
                    Assert.AreEqual(0, game._table.Matrix[i][j]);
                }
            }
            Assert.AreEqual(0, game.Walls);
            Assert.AreEqual(0, game.BrokenWalls);
            Assert.AreEqual(0, game._time);
            Assert.AreEqual(15, game._table.N);

        }
        #endregion

        #region Place
        [TestMethod]
        public void PlaceTest()
        {
            game.place(5, 5);
            Assert.AreEqual(1, game.Walls);
            Assert.AreEqual(1, game._table.Matrix[5][5]);

        }

        [TestMethod]
        public void PlaceSamePlaceTest()
        {
            game.place(5, 5);
            Assert.AreEqual(1, game.Walls);
            Assert.AreEqual(1, game._table.Matrix[5][5]);
            game.place(5, 5);
            Assert.AreEqual(1, game.Walls);
            Assert.AreEqual(1, game._table.Matrix[5][5]);
        }
        #endregion

        #region Round
        [TestMethod]
        public void RoundWithNoWallTest()
        {
            game._robot.Crazy = 0; // Véletlenszerûen fog lépni, mozogni (10%-os eséllyel)
            game._robot.X = 0;
            game._robot.Y = 5;
            game._robot.Way = Way.NORTH;
            Assert.AreEqual(0, game._robot.X);
            Assert.AreEqual(5, game._robot.Y);
            Assert.AreEqual(0, game._robot.Crazy);
            Assert.AreEqual(Way.NORTH, game._robot.Way);

            game.round();
            game._robot.Way = Way.NORTH;
            Assert.AreEqual(0, game._robot.X);
            Assert.AreEqual(5, game._robot.Y);
            Assert.AreEqual(1, game._time);
            Assert.AreEqual(0, game._robot.Crazy);
            Assert.AreEqual(Way.NORTH, game._robot.Way);
        }
        [TestMethod]
        public void RoundWithWallTest()
        {
            game._robot.Crazy = 0; // Véletlenszerûen fog lépni, mozogni (10%-os eséllyel)
            game._robot.X = 0;
            game._robot.Y = 5;
            game._robot.Way = Way.NORTH;
            Assert.AreEqual(0, game._robot.X);
            Assert.AreEqual(5, game._robot.Y);
            Assert.AreEqual(0, game._robot.Crazy);
            Assert.AreEqual(Way.NORTH, game._robot.Way);

            game.place(4, 5);
            game.round();

            Assert.AreEqual(0, game._robot.X);
            Assert.AreEqual(5, game._robot.Y);
            Assert.AreEqual(1, game._time);
            Assert.AreEqual(0, game._robot.Crazy);
            //Assert.AreEqual(Way.NORTH, game._robot.Way); // A robot iránya random lesz ütközés után

            Assert.AreEqual(1, game._table.Matrix[4][5]);
        }
        #endregion

        #region Load
        [TestMethod]
        public void LoadTest()
        {
            Tuple<Table, Robot, int, int, int> tp = new Tuple<Table, Robot, int, int, int>(mockedTable, mockedRobot, 10, 0, 0);
            mock.Setup(gameFromFile => gameFromFile.Load("")).Returns(tp);
            game.Load("");

            for (int i = 0; i < 7; i++)
            {
                for (int j = 0; j < 7; j++)
                {
                    Assert.AreEqual(0, game._table.Matrix[i][j]);
                }
            }

            Assert.AreEqual(3, game._robot.X);
            Assert.AreEqual(4, game._robot.Y);
            Assert.AreEqual(Way.NORTH, game._robot.Way);
            Assert.AreEqual(10, game._robot.Crazy);

            Assert.AreEqual(10, game._time);

            Assert.AreEqual(0, game.Walls);
            Assert.AreEqual(0, game.BrokenWalls);

            mock.Verify(dataAccess => dataAccess.Load(String.Empty), Times.Once());
        }
        #endregion

        #region Event
        private void Model_GameAdvanced(Object? sender, GameEvent e)
        {
            Assert.IsTrue(game.BrokenWalls < game._table.N * game._table.N);
            Assert.AreEqual(game._robot.X == game._table.N/2 && game._robot.Y == game._table.N/2
                || game.BrokenWalls == game._table.N*game._table.N, game.isGameOver);
            Assert.AreEqual(e.GameTime, game._time);
            Assert.AreEqual(e.Walls, game.Walls);
            Assert.AreEqual(e.BrokenWalls, game.BrokenWalls);
            Assert.IsFalse(e.IsWon);
        }
        private void Model_GameOver(Object? sender, GameEvent e)
        {
            Assert.IsTrue(game.isGameOver);
            Assert.IsFalse(e.IsWon);
        }
        #endregion
    }
}