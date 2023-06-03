using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrazyRobotMaui.ViewModel
{
    public class CrazyRobotField : ViewModelBase
    {

        private bool _wall;
        private bool _brokenWall;
        private bool _robot;
        private bool _isLocked;
        private string _text;
#nullable enable
        public string T
        {
            get { return _text; }
            set
            {
                if (_text != value)
                {
                    _text = value;
                    OnPropertyChanged();
                }
            }
        }
        public bool Wall
        {
            get { return _wall; }
            set
            {
                if (_wall != value)
                {
                    _wall = value;
                    OnPropertyChanged();
                }
            }
        }
        public bool BrokenWall
        {
            get { return _brokenWall; }
            set
            {
                if (_brokenWall != value)
                {
                    _brokenWall = value;
                    OnPropertyChanged();
                }
            }
        }
        public bool Robot
        {
            get { return _robot; }
            set
            {
                if (_robot != value)
                {
                    _robot = value;
                    OnPropertyChanged();
                }
            }
        }
        public bool IsLocked
        {
            get { return _isLocked; }
            set
            {
                if (_isLocked != value)
                {
                    _isLocked = value;
                    OnPropertyChanged();
                }
            }
        }
        public int X { get; set; }
        public int Y { get; set; }
        public int Number { get; set; }

        public DelegateCommand? StepCommand { get; set; }
    }
}
