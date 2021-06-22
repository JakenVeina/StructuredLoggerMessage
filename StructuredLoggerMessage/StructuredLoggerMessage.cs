using System;
using System.Collections.Generic;

namespace Microsoft.Extensions.Logging.Structured
{
    /// <summary>
    /// Creates reusable delegates for writing data to <see cref="ILogger"/> instances, allowing for more performant log writes.
    /// 
    /// Each reusable delegate contains cached logic for processing data values given upon each invocation, and either formatting them for text loggers, or supplying them to structural loggers.
    /// </summary>
    /// <remarks>
    /// This API is basically a duplication of <see cref="LoggerMessage"/>, but with the ability to define "unformatted" values that are only used by structural loggers, without having to include them in the text-logger messages.
    /// 
    /// Format strings, likewise, follow the same formatting as <see cref="System.String.Format(string, object[])"/>, except that interpolated values within the string may be given text names, rather than simply identified by index. However, this does impose the limitation that value parameters must be given in the order that they appear in the format string.
    /// </remarks>
    public partial class StructuredLoggerMessage
    {
        /// <summary>
        /// A helper method for building logger message delegates, which concatenates together lists of formatted and unformatted value names together, and validates that no value names are duplicated.
        /// </summary>
        /// <param name="formattedValueNames">The formatted value names, to be included first in the result list.</param>
        /// <param name="unformattedValueNames">The unformatted value names, to be included second in the result list.</param>
        /// <exception cref="ArgumentException">Throws if any value names appear more than once, across the two input lists.</exception>
        /// <returns>The concatenation of <paramref name="formattedValueNames"/> and <paramref name="unformattedValueNames"/>.</returns>
        protected static IReadOnlyList<string> CombineValueNames(
            IReadOnlyList<string> formattedValueNames,
            IReadOnlyList<string> unformattedValueNames)
        {
            var count = formattedValueNames.Count + unformattedValueNames.Count;
            var valueNames = new string[count];
            var valueNamesSet = new HashSet<string>();

            var i = 0;
            for (; i < formattedValueNames.Count; ++i)
            {
                var valueName = formattedValueNames[i];
                if (valueNamesSet.Contains(valueName))
                    throw new ArgumentException($"A log value named {valueName} was defined more than once");
                valueNamesSet.Add(valueName);
                valueNames[i] = valueName;
            }
            for (var j = 0; i < count; ++i, ++j)
            {
                var valueName = unformattedValueNames[j];
                if (valueNamesSet.Contains(valueName))
                    throw new ArgumentException($"A log value named {valueName} was defined more than once");
                valueNamesSet.Add(valueName);
                valueNames[i] = valueName;
            }

            return valueNames;
        }
    }
}
