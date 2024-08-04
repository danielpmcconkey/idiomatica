using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.Telemetry
{
    [ProviderAlias("IdiomaticaLog")]
    public sealed class LoggerProvider : ILoggerProvider
    {
        private readonly IDisposable? onChangeToken;
        private LoggerConfiguration config;
        private readonly ConcurrentDictionary<string, IdiomaticaLogger> loggers =
            new(StringComparer.OrdinalIgnoreCase);

        public LoggerProvider(
            IOptionsMonitor<LoggerConfiguration> config)
        {
            this.config = config.CurrentValue;
            onChangeToken = config.OnChange(updatedConfig => this.config = updatedConfig);
        }

        public ILogger CreateLogger(string categoryName) =>
            loggers.GetOrAdd(categoryName, name => new IdiomaticaLogger(name, GetCurrentConfig));

        private LoggerConfiguration GetCurrentConfig() => config;

        public void Dispose()
        {
            loggers.Clear();
            onChangeToken?.Dispose();
        }
    }
}
