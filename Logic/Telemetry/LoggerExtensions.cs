using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Configuration;

namespace Logic.Telemetry
{
    public static class LoggerExtensions
    {
        public static ILoggingBuilder AddIdiomaticaLogger(
            this ILoggingBuilder builder)
        {
            builder.AddConfiguration();

            builder.Services.TryAddEnumerable(
                ServiceDescriptor.Singleton<ILoggerProvider, LoggerProvider>());

            LoggerProviderOptions.RegisterProviderOptions
                <LoggerConfiguration, LoggerProvider>(builder.Services);

            return builder;
        }
    }
}
