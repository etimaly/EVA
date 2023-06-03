using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;
using CrazyRobot.model;
using System.Timers;
using System.Windows.Media;
using CrazyRobot.Persistence;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Security.Cryptography;
using System.Windows.Navigation;

namespace CrazyRobotWPF.ViewModel
{
    public class CrazyRobotViewModel : ViewModelBase
    {
        #region Field
        private Game _model;
        #endregion

        #region Properties
        public DelegateCommand NewGameCommand { get; private set; }
        public DelegateCommand LoadGameCommand { get; private set; }
        public DelegateCommand SaveGameCommand { get; private set; }
        public String GameTime { get { return TimeSpan.FromSeconds(_model._time).ToString("g"); } }
        public String GamePlacedWallsCount { get { return (_model.Walls + _model.BrokenWalls).ToString(); } }
        public int N { 
            get { return _model._table.N; }
            set { N = value; }
        }

        public DelegateCommand PauseGameCommand { get; private set; }

        public ObservableCollection<CrazyRobotField> Fields { get; set; }

        public Boolean SmallCommand {
            get { return (_model._table.N == 7); }
            set
            {
                if (_model._table.N == 7) 
                    return;
                
                _model._table.N = 7;
                OnPropertyChanged(nameof(SmallCommand));
                OnPropertyChanged(nameof(MediumCommand));
                OnPropertyChanged(nameof(BigCommand));
            }
        }
        public Boolean MediumCommand
        {
            get { return (_model._table.N == 11); }
            set
            {
                if (_model._table.N == 11) 
                    return;

                _model._table.N = 11;
                OnPropertyChanged(nameof(SmallCommand));
                OnPropertyChanged(nameof(MediumCommand));
                OnPropertyChanged(nameof(BigCommand));
            }
        }
        public Boolean BigCommand
        {
            get { return (_model._table.N == 15); }
            set
            {
                if (_model._table.N == 15) 
                    return;

                _model._table.N = 15;
                OnPropertyChanged(nameof(SmallCommand));
                OnPropertyChanged(nameof(MediumCommand));
                OnPropertyChanged(nameof(BigCommand));
            }
        }
        #endregion

        #region Events
        public event EventHandler? NewGame;
        public event EventHandler? LoadGame;
        public event EventHandler? SaveGame;
        public event EventHandler? PauseGame;
        #endregion

        #region Constructors
        public CrazyRobotViewModel(Game model)
        {
            _model = model;
            _model.GameAdvanced += new EventHandler<GameEvent>(Model_GameAdvanced);
            _model.GameOver += new EventHandler<GameEvent>(Model_GameOver);

            NewGameCommand = new DelegateCommand(param => OnNewGame());
            LoadGameCommand = new DelegateCommand(param => OnLoadGame());
            SaveGameCommand = new DelegateCommand(param => OnSaveGame());
            PauseGameCommand = new DelegateCommand(param => OnPauseGame());

            Fields = new ObservableCollection<CrazyRobotField>();
            for (int i = 0; i < _model._table.N; ++i)
            {
                for (int j = 0; j < _model._table.N; ++j)
                {
                    if (_model._robot.X == i && _model._robot.Y == j)
                    {
                        Fields.Add(new CrazyRobotField
                        {
                            X = i,
                            Y = j,
                            Number = i * _model._table.N + j,
                            Wall = false,
                            BrokenWall = false,
                            Robot = true,
                            IsLocked = true,
                            StepCommand = new DelegateCommand(param => StepGame(Convert.ToInt32(param)))
                        });
                    }
                    else if (_model._table.Matrix[i][j] == 1)
                    {
                        Fields.Add(new CrazyRobotField
                        {
                            X = i,
                            Y = j,
                            Number = i * _model._table.N + j,
                            Wall = true,
                            BrokenWall = false,
                            Robot = false,
                            IsLocked = true,
                            StepCommand = new DelegateCommand(param => StepGame(Convert.ToInt32(param)))
                        });
                    }
                    else if (_model._table.Matrix[i][j] == 2)
                    {
                        Fields.Add(new CrazyRobotField
                        {
                            X = i,
                            Y = j,
                            Number = i * _model._table.N + j,
                            Wall = false,
                            BrokenWall = true,
                            Robot = false,
                            IsLocked = true,
                            StepCommand = new DelegateCommand(param => StepGame(Convert.ToInt32(param)))
                        });
                    }
                    else
                    {
                        Fields.Add(new CrazyRobotField
                        {
                            X = i,
                            Y = j,
                            Number = i * _model._table.N + j,
                            Wall = false,
                            BrokenWall = false,
                            Robot = false,
                            IsLocked = true,
                            StepCommand = new DelegateCommand(param => StepGame(Convert.ToInt32(param)))
                        });
                    }
                    
                }
            }
            RefreshTable();
        }
        #endregion

        #region Private Methods
        private void RefreshTable()
        {
            foreach (CrazyRobotField field in Fields)
            {
                if (_model._robot.X == field.X && _model._robot.Y == field.Y)
                {
                    field.Robot = true;
                    field.Wall = false;
                    field.BrokenWall = false;
                    field.IsLocked = true;
                }
                else if (_model._table.Matrix[field.X][field.Y] == 1)
                {
                    field.Robot = false;
                    field.Wall = true;
                    field.BrokenWall = false;
                    field.IsLocked = true;
                }
                else if (_model._table.Matrix[field.X][field.Y] == 2)
                {
                    field.Robot = false;
                    field.Wall = false;
                    field.BrokenWall = true;
                    field.IsLocked = true;
                }
                else
                {
                    field.Robot = false;
                    field.Wall = false;
                    field.BrokenWall = false;
                    field.IsLocked = true;
                }
            }
            OnPropertyChanged(nameof(GameTime));
            OnPropertyChanged(nameof(GamePlacedWallsCount));
        }
        private void StepGame(int ind)
        {
            CrazyRobotField field = Fields[ind];
            if ((field.X != _model._robot.X || field.Y != _model._robot.Y) )
            {
                _model.place(field.X, field.Y);
                field.Wall = true;
                RefreshTable();
                OnPropertyChanged(GamePlacedWallsCount);
            }
            

        }
        #endregion

        #region Game Event Handlers
        private void Model_GameOver(object? sender, EventArgs e)
        {
            foreach (CrazyRobotField field in Fields)
            {
                field.IsLocked = false;
            }
        }
        private void Model_GameAdvanced(object? sender, EventArgs e)
        {
            RefreshTable();
            OnPropertyChanged(nameof(GameTime));
        }
        #endregion

        #region Event Methods
        private void OnNewGame()
        {
            OnPropertyChanged(nameof(N));
            NewGame?.Invoke(this, EventArgs.Empty);
            Fields.Clear();
            for (int i = 0; i < _model._table.N; ++i)
            {
                for (int j = 0; j < _model._table.N; ++j)
                {
                    if (_model._robot.X == i && _model._robot.Y == j)
                    {
                        Fields.Add(new CrazyRobotField
                        {
                            X = i,
                            Y = j,
                            Number = i * _model._table.N + j,
                            Wall = false,
                            BrokenWall = false,
                            Robot = true,
                            IsLocked = true,
                            StepCommand = new DelegateCommand(param => StepGame(Convert.ToInt32(param)))
                        });
                    }
                    else if (_model._table.Matrix[i][j] == 1)
                    {
                        Fields.Add(new CrazyRobotField
                        {
                            X = i,
                            Y = j,
                            Number = i * _model._table.N + j,
                            Wall = true,
                            BrokenWall = false,
                            Robot = false,
                            IsLocked = true,
                            StepCommand = new DelegateCommand(param => StepGame(Convert.ToInt32(param)))
                        });
                    }
                    else if (_model._table.Matrix[i][j] == 2)
                    {
                        Fields.Add(new CrazyRobotField
                        {
                            X = i,
                            Y = j,
                            Number = i * _model._table.N + j,
                            Wall = false,
                            BrokenWall = true,
                            Robot = false,
                            IsLocked = true,
                            StepCommand = new DelegateCommand(param => StepGame(Convert.ToInt32(param)))
                        });
                    }
                    else
                    {
                        Fields.Add(new CrazyRobotField
                        {
                            X = i,
                            Y = j,
                            Number = i * _model._table.N + j,
                            Wall = false,
                            BrokenWall = false,
                            Robot = false,
                            IsLocked = true,
                            StepCommand = new DelegateCommand(param => StepGame(Convert.ToInt32(param)))
                        });
                    }

                }
            }
            RefreshTable();
        }
        private void OnLoadGame()
        {
            
            LoadGame?.Invoke(this, EventArgs.Empty);
            OnPropertyChanged(nameof(N));
            OnPropertyChanged(nameof(SmallCommand));
            OnPropertyChanged(nameof(MediumCommand));
            OnPropertyChanged(nameof(BigCommand));
            Fields.Clear();
            for (int i = 0; i < _model._table.N; ++i)
            {
                for (int j = 0; j < _model._table.N; ++j)
                {
                    if (_model._robot.X == i && _model._robot.Y == j)
                    {
                        Fields.Add(new CrazyRobotField
                        {
                            X = i,
                            Y = j,
                            Number = i * _model._table.N + j,
                            Wall = false,
                            BrokenWall = false,
                            Robot = true,
                            IsLocked = true,
                            StepCommand = new DelegateCommand(param => StepGame(Convert.ToInt32(param)))
                        });
                    }
                    else if (_model._table.Matrix[i][j] == 1)
                    {
                        Fields.Add(new CrazyRobotField
                        {
                            X = i,
                            Y = j,
                            Number = i * _model._table.N + j,
                            Wall = true,
                            BrokenWall = false,
                            Robot = false,
                            IsLocked = true,
                            StepCommand = new DelegateCommand(param => StepGame(Convert.ToInt32(param)))
                        });
                    }
                    else if (_model._table.Matrix[i][j] == 2)
                    {
                        Fields.Add(new CrazyRobotField
                        {
                            X = i,
                            Y = j,
                            Number = i * _model._table.N + j,
                            Wall = false,
                            BrokenWall = true,
                            Robot = false,
                            IsLocked = true,
                            StepCommand = new DelegateCommand(param => StepGame(Convert.ToInt32(param)))
                        });
                    }
                    else
                    {
                        Fields.Add(new CrazyRobotField
                        {
                            X = i,
                            Y = j,
                            Number = i * _model._table.N + j,
                            Wall = false,
                            BrokenWall = false,
                            Robot = false,
                            IsLocked = true,
                            StepCommand = new DelegateCommand(param => StepGame(Convert.ToInt32(param)))
                        });
                    }
                }
            }
            //RefreshTable();
        }
        private void OnSaveGame()
        {
            SaveGame?.Invoke(this, EventArgs.Empty);
        }
        private void OnPauseGame()
        {
            PauseGame?.Invoke(this, EventArgs.Empty);
        }
        #endregion
    }
}
