using System;

namespace Microsoft.Extensions.Logging.Structured.Test
{
    public struct LoggerLogInvocation
    {
        public LogLevel LogLevel { get; init; }

        public EventId EventId { get; init; }

        public object? State { get; init; }

        public Exception? Exception { get; init; }

        public string Message { get; init; }
    }
}
