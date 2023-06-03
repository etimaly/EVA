using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrazyRobotMod.Persistence
{
    public class GameFromFileException : IOException
    {
        public GameFromFileException() { }
        public GameFromFileException(string message) : base(message) { }
        public GameFromFileException(string message,
            Exception innerException) : base(message, innerException) { }
    }
}
