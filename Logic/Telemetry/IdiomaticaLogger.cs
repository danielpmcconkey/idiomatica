using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Logic.Telemetry.LoggerConfiguration;

namespace Logic.Telemetry
{
    public sealed class IdiomaticaLogger : ILogger
    {
        private readonly string name;
        private readonly Func<LoggerConfiguration> getCurrentConfig;

        public IdiomaticaLogger(
            string name,
            Func<LoggerConfiguration> getCurrentConfig) =>
            (this.name, this.getCurrentConfig) = (name, getCurrentConfig);

        public IDisposable BeginScope<TState>(TState state) => default!;

        public bool IsEnabled(LogLevel logLevel) =>
            getCurrentConfig().LogLevels.ContainsKey(logLevel);

        public void Log<TState>(
            LogLevel logLevel,
            EventId eventId,
            TState state,
            Exception? exception,
            Func<TState, Exception?, string> formatter)
        {
            if (!IsEnabled(logLevel))
            {
                return;
            }

            LoggerConfiguration config = getCurrentConfig();

            if (config.EventId == 0 || config.EventId == eventId.Id)
            {
                switch (config.LogLevels[logLevel])
                {
                    case LogFormat.Short:
                        Console.WriteLine($"{name}: {formatter(state, exception)}");
                        break;
                    case LogFormat.Long:
                        Console.WriteLine($"[{eventId.Id,2}: {logLevel,-12}] {name} - {formatter(state, exception)}");
                        break;
                    default:
                        // No-op
                        break;
                }
            }
        }
    }
}
