using System;
using System.Runtime.Serialization;

namespace Microsoft.Extensions.Logging.Structured
{
    /// <summary>
    /// Describes an exception resulting from a malformed message format string.
    /// </summary>
    public class MalformedFormatStringException
        : ArgumentException
    {
        internal MalformedFormatStringException(
                int     index,
                string  formatString,
                string  paramName)
            : base(
                $"Format string is malformed, at index {index}",
                paramName)
        {
            Index           = index;
            FormatString    = formatString;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MalformedFormatStringException"/> class with serialized data.
        /// </summary>
        /// <param name="info">The object that holds the serialized object data.</param>
        /// <param name="context">The contextual information about the source or destination.</param>
        protected MalformedFormatStringException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            Index           = info.GetInt32(nameof(Index));
            FormatString    = info.GetString(nameof(FormatString));
        }

        /// <summary>
        /// The index within <see cref="FormatString"/> at which the malformation was detected.
        /// </summary>
        public int Index { get; }

        /// <summary>
        /// The malformed format string that was unable to be parsed.
        /// </summary>
        public string FormatString { get; }
    }
}
