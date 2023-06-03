using CrazyRobot.model;
using CrazyRobot.Persistence;
using Timer = System.Windows.Forms.Timer;

namespace CrazyRobotView
{
    public partial class Form1 : Form
    {
        #region Members
        private Game _game;
        private Button[,] _buttonMatrix;
        private Timer _time;
        #endregion

        #region Constructor
        public Form1()
        {
            InitializeComponent();
            
            _game = new Game(7);
            _game.GameAdvanced += new EventHandler<GameEvent>(Game_GameAdvanced);
            _game.GameOver += new EventHandler<GameEvent>(Game_GameOver);
            
            GenerateButtonMatrix();
            Setup_Menu();
            
            _time = new Timer();
            _time.Interval = 1000;
            _time.Tick += MatrixRun;
            _time.Tick += UpdateLabels;
            _time.Start();
        }
        #endregion

        #region Private functions
        private void MatrixRun(Object? sender, EventArgs e)
        {
            this.SuspendLayout();
            int x = _game._robot.X;
            int y = _game._robot.Y;
            _game.round();
            if (_time.Enabled)
            {
                _buttonMatrix[x, y].Enabled = false;
                _buttonMatrix[x, y].BackColor = Color.Black;

                for (int i = 0; i < _game._table.N; i++)
                {
                    for (int j = 0; j < _game._table.N; j++)
                    {
                        if (_game._table.Matrix[i][j] == 2)
                        {
                            _buttonMatrix[i, j].BackColor = Color.Brown;
                            _buttonMatrix[i, j].Enabled = false;
                        }
                    }
                }
                _buttonMatrix[x, y].Enabled = true;
                if (_game._table.Matrix[x][y] == 2) { _buttonMatrix[x, y].BackColor = Color.Brown; }
                else { _buttonMatrix[x, y].BackColor = Color.White; }

                _buttonMatrix[_game._robot.X, _game._robot.Y].Enabled = false;
                _buttonMatrix[_game._robot.X, _game._robot.Y].BackColor = Color.Black;
            }

            this.ResumeLayout(false);
        }
        private void GenerateButtonMatrix()
        {
            _buttonMatrix = new System.Windows.Forms.Button[_game._table.N, _game._table.N];
            for (int x = 0; x < _game._table.N; x++)
            {
                for (int y = 0; y < _game._table.N; y++)
                {
                    _buttonMatrix[x, y] = new System.Windows.Forms.Button();
                    this.Controls.Add(_buttonMatrix[x, y]);
                    _buttonMatrix[x, y].Location = new System.Drawing.Point(x * 50 + 30, y * 50 + 30);
                    _buttonMatrix[x, y].Width = 50;
                    _buttonMatrix[x, y].Height = 50;
                    _buttonMatrix[x, y].Tag = new Tuple<int, int>(x, y);
                    _buttonMatrix[x, y].Size = new System.Drawing.Size(50, 50);
                    if (_game._table.Matrix[x][y] == 2)
                    {
                        _buttonMatrix[x, y].BackColor = Color.Brown;
                    }
                    else if (_game._table.Matrix[x][y] == 1)
                    {
                        _buttonMatrix[x, y].BackColor = Color.Red;
                    }
                    else
                    {
                    _buttonMatrix[x, y].BackColor = Color.White;
                    }
                    _buttonMatrix[x, y].Click += MatrixButtonClick;
                }
            }
            _buttonMatrix[_game._robot.X, _game._robot.Y].BackColor = Color.Black;
        }
        private void ClearButtons()
        {
            foreach (Button bttn in _buttonMatrix)
            {
                    Controls.Remove(bttn);
            }
        }
        private void newGame()
        {
            _time.Stop();
            int n;
            if (Small.Checked)
            {
                n = 7;
            }
            else if (Medium.Checked)
            {
                n = 11;
            }
            else
            {
                n = 15;
            }
            SetupSize();
            ClearButtons();
            _game.NewGame(n);
            GenerateButtonMatrix();
            Setup_Menu();
            Save.Enabled = true;
            Pause.Enabled = true;
            Pause.Checked = false;
            Pause.BackColor = Color.Transparent;
            _time.Start();
        }
        #endregion

        #region Click Event
        private void MatrixButtonClick(Object? sender, EventArgs e)
        {
            Button bttn = sender as Button;
            Tuple<int, int> a = (Tuple<int, int>)bttn.Tag;
            bttn.BackColor = Color.Red;
            _game.place(a.Item1, a.Item2);
            bttn.Enabled = false;
        }
        private void NewGame_Click(object sender, EventArgs e)
        {
            newGame();
        }
        private void Small_Click(object sender, EventArgs e)
        {
            Small.Checked = true;
            Medium.Checked = false;
            Big.Checked = false;
            newGame();
        }
        private void Medium_Click(object sender, EventArgs e)
        {
            Small.Checked = false;
            Medium.Checked = true;
            Big.Checked = false;
            newGame();
        }
        private void Big_Click(object sender, EventArgs e)
        {
            Small.Checked = false;
            Medium.Checked = false;
            Big.Checked = true;
            newGame();
        }
        private void Save_Click(object sender, EventArgs e)
        {
            Boolean restartTime = _time.Enabled;
            _time.Stop();
            
            if (_saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    // játé mentése
                    _game.Save(_saveFileDialog.FileName);
                }
                catch (GameFromFileException)
                {
                    MessageBox.Show("Játék mentése sikertelen!" + Environment.NewLine + "Hibás az elérési út, vagy a könyvtár nem írható.", "Hiba!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            if (restartTime) { _time.Start(); }
        }
        private void Load_Click(object sender, EventArgs e)
        {
            Boolean restartTime = _time.Enabled;
            _time.Stop();
            if (_openFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    _game.Load(_openFileDialog.FileName);
                    ClearButtons();
                    Setup_Menu();
                    SetupSize();
                    GenerateButtonMatrix();
                    Save.Enabled = true;
                }
                catch (GameFromFileException)
                {
                    MessageBox.Show("Játék betöltése sikertelen!" + Environment.NewLine + "Hibás az elérési út, vagy a fájlformátum.", "Hiba!", MessageBoxButtons.OK, MessageBoxIcon.Error);

                }
            }
            if (restartTime) { _time.Start(); }

        }
        private void Pause_Click(object sender, EventArgs e)
        {
            if (Pause.Checked) 
            { 
                Pause.BackColor = Color.Transparent;
                for (int i = 0; i < _game._table.N; i++)
                {
                    for (int j = 0; j < _game._table.N; j++) 
                    {
                        if (_game._table.Matrix[i][j] == 0)
                        {
                            _buttonMatrix[i, j].Enabled = true;
                        }
                    }
                }
                _buttonMatrix[_game._robot.X, _game._robot.Y].Enabled = false;
                _time.Start(); 
            }
            else 
            {
                Pause.BackColor = Color.Red;
                for (int i = 0; i < _game._table.N; i++)
                {
                    for (int j = 0; j < _game._table.N; j++) { _buttonMatrix[i, j].Enabled = false; }
                }
                _time.Stop(); 
            }
        }
        private void UpdateLabels(object sender, EventArgs e)
        {
            toolStripStatusLabelTime.Text = "Elapsed time: " + TimeSpan.FromSeconds(_game._time).ToString("g");
            toolStripStatusLabelWalls.Text = "Placed Walls: " + (_game.Walls + _game.BrokenWalls).ToString();
        }
        #endregion

        #region Setup
        private void Setup_Menu()
        {
            Small.Checked = (7 == _game._table.N);
            Medium.Checked = (11 == _game._table.N);
            Big.Checked = (15 == _game._table.N);
            toolStripStatusLabelTime.Text = "Elapsed time: " + TimeSpan.FromSeconds(_game._time).ToString("g");
            toolStripStatusLabelWalls.Text = "Placed Walls: " + (_game.Walls + _game.BrokenWalls).ToString();
        }
        private void SetupSize()
        {
            if (Small.Checked)
            {
                this.Size = new Size(450, 450);
                this.MaximumSize = new Size(450, 450);
                this.MinimumSize = new Size(450, 450);
            }
            else if (Medium.Checked)
            {
                this.Size = new Size(650, 650);
                this.MaximumSize = new Size(650, 650);
                this.MinimumSize = new Size(650, 650);
            }
            else
            {
                this.Size = new Size(850, 850);
                this.MaximumSize = new Size(850, 850);
                this.MinimumSize = new Size(850, 850);
            }
        }
        #endregion

        #region Event
        private void Game_GameAdvanced(Object? sender, GameEvent e)
        {
            toolStripStatusLabelTime.Text = "Elapsed time: " + TimeSpan.FromSeconds(_game._time).ToString("g");
            toolStripStatusLabelWalls.Text = "Placed Walls: " + (_game.Walls + _game.BrokenWalls).ToString();

        }
        private void Game_GameOver(Object? sender, GameEvent e)
        {
            _time.Stop();
            foreach (Button bttn in _buttonMatrix)
            {
                bttn.Enabled = false;
            }
            Save.Enabled = false;
            Pause.Enabled = false;
            if (e.IsWon) 
            {
                MessageBox.Show("Elapsed time: " + TimeSpan.FromSeconds(_game._time).ToString("g") + System.Environment.NewLine + "Placed walls: " + (_game.Walls + _game.BrokenWalls).ToString(), "You Won!", MessageBoxButtons.OK);
            }
            else
            {
                MessageBox.Show("Elapsed time: " + TimeSpan.FromSeconds(_game._time).ToString("g") + System.Environment.NewLine + "Placed walls: " + (_game.Walls + _game.BrokenWalls).ToString(), "Game Over", MessageBoxButtons.OK);
            }
        }
        #endregion

    }
}