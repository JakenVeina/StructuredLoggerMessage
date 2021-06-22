using System;

namespace Microsoft.Extensions.Logging.Structured.Test
{
    public struct LogValueDefinition
    {
        public string   Name { get; init; }
        public Type     Type { get; init; }
        public object?  Value { get; init; }
        public bool     IsFormatted { get; init; }
    }
}
