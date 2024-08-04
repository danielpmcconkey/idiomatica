using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.Telemetry
{
    /// <summary>
    /// ErrorState is used so parent and child components can share a single
    /// error state between one another
    /// </summary>
    public class ErrorState
    {
        public bool isError = false;
        public string? errorMessage;
        public int? code;
        public string? memberName;
        public string? sourceFilePath;
        public int? sourceLineNumber;
    }
}
