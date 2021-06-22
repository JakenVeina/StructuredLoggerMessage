using System;
using System.Collections.Generic;

namespace Microsoft.Extensions.Logging.Structured.Test
{
    public class FakeLogger
        : ILogger
    {
        public bool IsEnabled { get; init; }
            = true;

        public List<LoggerLogInvocation> LogInvocations { get; }
            = new();

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
            => LogInvocations.Add(new()
            {
                LogLevel    = logLevel,
                EventId     = eventId,
                State       = state,
                Exception   = exception,
                Message     = formatter.Invoke(state, exception)
            });

        IDisposable ILogger.BeginScope<TState>(TState state)
            => throw new NotSupportedException();

        bool ILogger.IsEnabled(LogLevel logLevel)
            => IsEnabled;
    }
}
