using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace CrazyRobot.model
{
    public class Table
    {
        #region Members
        private int _n;
        private List<List<int>> _matrix;
        #endregion

        #region Constructor
        public Table(int n)
        {
            _n = n;
            _matrix = new List<List<int>>();
            for (int i = 0; i < n; i++)
            {
                _matrix.Add(new List<int>());
                for (int j = 0; j < n; j++) { _matrix[i].Add(0); }
            }
        }
        #endregion

        #region Properties
        public int N
        {
            get { return _n; }
            set { _n = value; }
        }
        public List<List<int>> Matrix
        {
            get { return _matrix; }
            set { _matrix = value; }
        }
        #endregion
    }
}
