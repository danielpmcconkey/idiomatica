using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.Telemetry
{
    public class IdiomaticaException : Exception
    {
        public int code;
        public IdiomaticaException()
        {
        }

        public IdiomaticaException(string message)
            : base(message)
        {
        }

        public IdiomaticaException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
