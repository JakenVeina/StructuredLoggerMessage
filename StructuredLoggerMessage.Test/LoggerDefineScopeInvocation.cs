using System;

namespace Microsoft.Extensions.Logging.Structured.Test
{
    public struct LoggerDefineScopeInvocation
    {
        public object? State { get; init; }

        public IDisposable Result { get; init; }
    }
}
