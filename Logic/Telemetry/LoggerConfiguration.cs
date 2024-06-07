using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Logic.Telemetry
{
    public class LoggerConfiguration
    {
        public int EventId { get; set; }

        public Dictionary<LogLevel, LogFormat> LogLevels { get; set; } =
            new()
            {
                [LogLevel.Information] = LogFormat.Short,
                [LogLevel.Warning] = LogFormat.Short,
                [LogLevel.Error] = LogFormat.Long
            };

        public enum LogFormat
        {
            Short,
            Long
        }
    }
}
