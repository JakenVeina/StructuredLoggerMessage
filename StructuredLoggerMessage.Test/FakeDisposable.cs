using System;

namespace Microsoft.Extensions.Logging.Structured.Test
{
    public sealed class FakeDisposable
        : IDisposable
    {
        public void Dispose() { }
    }
}
