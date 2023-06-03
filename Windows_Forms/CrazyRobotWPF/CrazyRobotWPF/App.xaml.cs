using CrazyRobot.model;
using CrazyRobot.Persistence;
using CrazyRobotWPF.ViewModel;
using CrazyRobotWPF.View;
using Microsoft.VisualBasic;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using System.Xml.Linq;

namespace CrazyRobotWPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        #region Fields
        private Game _model = null!;
        private CrazyRobotViewModel _viewModel = null!;
        private MainWindow _view = null!;
        private DispatcherTimer _timer = null!;
        #endregion

        #region Constructor
        public App()
        {
            Startup += new StartupEventHandler(App_Startup);
        }
        #endregion

        #region Application Event Handlers
        private void App_Startup(object? sender, StartupEventArgs e)
        {
            _model = new Game(new GameFromText());
            _model.GameOver += new EventHandler<GameEvent>(Model_GameOver);
            _model.NewGame(7);

            _viewModel = new CrazyRobotViewModel(_model);
            _viewModel.NewGame += new EventHandler(ViewModel_NewGame);
            _viewModel.LoadGame += new EventHandler(ViewModel_LoadGame);
            _viewModel.SaveGame += new EventHandler(ViewModel_SaveGame);
            _viewModel.PauseGame += new EventHandler(ViewModel_PauseGame);

            _view = new MainWindow();
            _view.DataContext = _viewModel;
            _view.Show();

            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromSeconds(1);
            _timer.Tick += new EventHandler(Timer_Tick);
            _timer.Start();
        }
        private void Timer_Tick(object? sender, EventArgs e)
        {
            _model.round();
        }
        #endregion

        #region ViewModel Event Handlers
        private void ViewModel_NewGame(object? sender, EventArgs e)
        {
            if (!_timer.IsEnabled) 
            {
                ViewModel_PauseGame(sender, e); 
            }
            _timer.Stop();
            if (_viewModel.SmallCommand)
            {
                _model.NewGame(7);
            }
            else if (_viewModel.MediumCommand)
            {
                _model.NewGame(11);
            }
            else
            {
                _model.NewGame(15);
            }
            _timer.Start();
        }
        private async void ViewModel_LoadGameAsync(object? sender, System.EventArgs e)
        {
            Boolean restartTimer = _timer.IsEnabled;
            _timer.Stop();
            try
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Title = "Crazy Robot játék betöltése";
                openFileDialog.Filter = "Crazy Robot |*.txt";
                if (openFileDialog.ShowDialog() == true)
                {
                    await _model.LoadGameAsync(openFileDialog.FileName);
                    _timer.Start();
                }
            }
            catch (GameFromFileException)
            {
                MessageBox.Show("File problem!", "Crazy Robot", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            if (restartTimer)
            {
                _timer.Start();
            }
        }
        private void ViewModel_LoadGame(object? sender, EventArgs e)
        {
            Boolean restartTimer = _timer.IsEnabled;
            _timer.Stop();
            try
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Title = "Crazy Robot játék betöltése";
                openFileDialog.Filter = "Crazy Robot |*.txt";
                if (openFileDialog.ShowDialog() == true)
                {
                    _model.Load(openFileDialog.FileName);
                    _timer.Start();
                }
            }
            catch (GameFromFileException)
            {
                MessageBox.Show("File problem!", "Crazy Robot", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            if (restartTimer)
            {
                _timer.Start();
            }
        }
        private async void ViewModel_SaveGameAsync(object? sender, EventArgs e)
        {
            Boolean restartTimer = _timer.IsEnabled;
            _timer.Stop();
            try
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Title = "Crazy Robot mentése";
                saveFileDialog.Filter = "Crazy Robot | *.txt";
                if (saveFileDialog.ShowDialog() == true)
                {
                    try
                    {
                        await _model.SaveGameAsync(saveFileDialog.FileName);
                    }
                    catch (GameFromFileException)
                    {
                        MessageBox.Show("Save Problem!", "Crazy Robot", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            catch
            {
                MessageBox.Show("Save is unsuccesfull", "Crazy Robot", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            if (restartTimer)
            {
                _timer.Start();
            }
        }
        private void ViewModel_SaveGame(object? sender, EventArgs e)
        {
            Boolean restartTimer = _timer.IsEnabled;
            _timer.Stop();
            try
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Title = "Crazy Robot mentése";
                saveFileDialog.Filter = "Crazy Robot | *.txt";
                if (saveFileDialog.ShowDialog() == true)
                {
                    try
                    {
                        _model.Save(saveFileDialog.FileName);
                    }
                    catch (GameFromFileException)
                    {
                        MessageBox.Show("Save Problem!","Crazy Robot" ,MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            catch
            {
                MessageBox.Show("Save is unsuccesfull", "Crazy Robot", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            if (restartTimer)
            {
                _timer.Start();
            }
        }
        private void ViewModel_PauseGame(object? sender, EventArgs e)
        {
            Boolean restartTimer = _timer.IsEnabled;
            if (!restartTimer)
            {
                _timer.Start();
                foreach (CrazyRobotField field in _viewModel.Fields)
                {
                    field.IsLocked = true;
                }
            }
            else
            {
                _timer.Stop();
                foreach(CrazyRobotField field in _viewModel.Fields)
                {
                    field.IsLocked = false;
                }
            }
        }
        #endregion

        #region Model Event Handler
        private void Model_GameOver(object? sender, GameEvent e)
        {
            _timer.Stop();
            foreach (CrazyRobotField field in _viewModel.Fields)
            {
                field.IsLocked = false;
            }
            if (e.IsWon)
            {
                MessageBox.Show("Elapsed time: " + TimeSpan.FromSeconds(_model._time).ToString("g") + System.Environment.NewLine + "Placed walls: " + (_model.Walls + _model.BrokenWalls).ToString(), "You Won!", MessageBoxButton.OK);
            }
            else
            {
                MessageBox.Show("Elapsed time: " + TimeSpan.FromSeconds(_model._time).ToString("g") + System.Environment.NewLine + "Placed walls: " + (_model.Walls + _model.BrokenWalls).ToString(), "Game Over", MessageBoxButton.OK);
            }
        }
        #endregion
    }
}
