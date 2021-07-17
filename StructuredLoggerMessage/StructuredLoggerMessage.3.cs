using System;
using System.Collections;
using System.Collections.Generic;

namespace Microsoft.Extensions.Logging.Structured
{
    public partial class StructuredLoggerMessage
    {
        /// <summary>
        /// Creates a delegate which can be invoked for writing a well-defined message to an <see cref="ILogger"/>.
        /// </summary>
        /// <typeparam name="T1">The type of the first data value passed to the logger.</typeparam>
        /// <typeparam name="T2">The type of the second data value passed to the logger.</typeparam>
        /// <typeparam name="T3">The type of the third data value passed to the logger.</typeparam>
        /// <param name="logLevel">The <see cref="LogLevel"/> value to be passed to the logger.</param>
        /// <param name="eventId">The <see cref="EventId"/> value to be passed to the logger.</param>
        /// <param name="formatString">The named format string to be used to generate text messages for text loggers.</param>
        /// <param name="unformattedValueNames">A list of names for values that are not named in <paramref name="formatString"/>.</param>
        /// <exception cref="ArgumentException">Throws if the total number of named values specified in <paramref name="formatString"/> and <paramref name="unformattedValueNames"/> is not 3.</exception>
        /// <exception cref="MalformedFormatStringException">Throws if <paramref name="formatString"/> is malformed.</exception>
        /// <returns>A delegate which, when invoked, writes the given data to the given <see cref="ILogger"/>.</returns>
        public static Action<ILogger, T1, T2, T3, Exception?> Define<T1, T2, T3>(
            LogLevel        logLevel,
            EventId         eventId,
            string          formatString,
            params string[] unformattedValueNames)
        {
            var formatter = StructuredLoggerMessageFormatter.CreateFromFormatString(formatString);

            if ((formatter.ValueNames.Count + unformattedValueNames.Length) != 3)
                throw new ArgumentException($"The number of named log values that were defined is incorred: Expected a total of 3, but got {formatter.ValueNames.Count} formatted and {unformattedValueNames.Length} unformatted");

            var valueNames = CombineValueNames(formatter.ValueNames, unformattedValueNames);

            return (logger, value1, value2, value3, exception) =>
            {
                if (!logger.IsEnabled(logLevel))
                    return;

                var state = new StructuredLoggerMessageState<T1, T2, T3>(
                    formatter,
                    valueNames,
                    value1,
                    value2,
                    value3);

                logger.Log(logLevel, eventId, state, exception, StructuredLoggerMessageState<T1, T2, T3>.FormatterDelegate);
            };
        }

        /// <summary>
        ///  Creates a delegate which can be invoked to create a log scope, with attached structural data, and no stringified message.
        /// </summary>
        /// <typeparam name="T1">The type of the first data value passed to the logger.</typeparam>
        /// <typeparam name="T2">The type of the second data value passed to the logger.</typeparam>
        /// <typeparam name="T3">The type of the third data value passed to the logger.</typeparam>
        /// <param name="name1">The name of the first data value passed to the logger.</param>
        /// <param name="name2">The name of the second data value passed to the logger.</param>
        /// <param name="name3">The name of the third data value passed to the logger.</param>
        /// <returns>A delegate which, when invoked, writes the given data to the given <see cref="ILogger"/>.</returns>
        public static Func<ILogger, T1, T2, T3, IDisposable> DefineScopeData<T1, T2, T3>(
                string name1,
                string name2,
                string name3)
            => (logger, value1, value2, value3)
                => logger.BeginScope(new StructuredLoggerState<T1, T2, T3>(
                    new(name1, value1),
                    new(name2, value2),
                    new(name3, value3)));

        private struct StructuredLoggerMessageState<T1, T2, T3>
            : IReadOnlyList<KeyValuePair<string, object?>>
        {
            public static Func<StructuredLoggerMessageState<T1, T2, T3>, Exception?, string> FormatterDelegate
                = (state, exception) => state.ToString();

            public StructuredLoggerMessageState(
                StructuredLoggerMessageFormatter    formatter,
                IReadOnlyList<string>               valueNames,
                T1                                  value1,
                T2                                  value2,
                T3                                  value3)
            {
                _formatter  = formatter;
                _valueNames = valueNames;
                _value1     = value1;
                _value2     = value2;
                _value3     = value3;
            }

            public KeyValuePair<string, object?> this[int index]
                => index switch
                {
                    0 => new(_valueNames[0],                                            _value1),
                    1 => new(_valueNames[1],                                            _value2),
                    2 => new(_valueNames[2],                                            _value3),
                    3 => new(StructuredLoggerMessageFormatter.OriginalFormatValueName,  _formatter.OriginalFormat),
                    _ => throw new IndexOutOfRangeException()
                };

            public int Count
                => 4;

            public IEnumerator<KeyValuePair<string, object?>> GetEnumerator()
            {
                yield return new(_valueNames[0],                                            _value1);
                yield return new(_valueNames[1],                                            _value2);
                yield return new(_valueNames[2],                                            _value3);
                yield return new(StructuredLoggerMessageFormatter.OriginalFormatValueName,  _formatter.OriginalFormat);
            }

            IEnumerator IEnumerable.GetEnumerator()
                => GetEnumerator();

            public override string ToString()
                => _formatter.Format(this.SelectValues());

            private readonly StructuredLoggerMessageFormatter   _formatter;
            private readonly IReadOnlyList<string>              _valueNames;
            private readonly T1                                 _value1;
            private readonly T2                                 _value2;
            private readonly T3                                 _value3;
        }

        private struct StructuredLoggerState<T1, T2, T3>
            : IReadOnlyList<KeyValuePair<string, object?>>
        {
            public StructuredLoggerState(
                KeyValuePair<string, T1> pair1,
                KeyValuePair<string, T2> pair2,
                KeyValuePair<string, T3> pair3)
            {
                _pair1 = pair1;
                _pair2 = pair2;
                _pair3 = pair3;
            }

            public KeyValuePair<string, object?> this[int index]
                => index switch
                {
                    0 => new(_pair1.Key, _pair1.Value),
                    1 => new(_pair2.Key, _pair2.Value),
                    2 => new(_pair3.Key, _pair3.Value),
                    _ => throw new IndexOutOfRangeException()
                };

            public int Count
                => 3;

            public IEnumerator<KeyValuePair<string, object?>> GetEnumerator()
            {
                yield return new(_pair1.Key, _pair1.Value);
                yield return new(_pair2.Key, _pair2.Value);
                yield return new(_pair3.Key, _pair3.Value);
            }

            IEnumerator IEnumerable.GetEnumerator()
                => GetEnumerator();

            public override string ToString()
                => $"{_pair1.Key}: {_pair1.Value}, {_pair2.Key}: {_pair2.Value}, {_pair3.Key}: {_pair3.Value}";

            private readonly KeyValuePair<string, T1> _pair1;
            private readonly KeyValuePair<string, T2> _pair2;
            private readonly KeyValuePair<string, T3> _pair3;
        }
    }
}
