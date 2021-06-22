using System;
using System.Collections;
using System.Collections.Generic;

#pragma warning disable CA1815 // This type does not need equality comparison

namespace Microsoft.Extensions.Logging.Structured
{
    /// <summary>
    /// Provides a performant mechanism for retrieving state values from standard log-state objects (I.E. objects listing log state values by both name and value, rather than just value).
    /// </summary>
    /// <typeparam name="TState">The type of state container whose values are to be retrieved.</typeparam>
    /// <remarks>
    /// This is primarily for use in conjunction with <see cref="LogStateExtensions.SelectValues{TState}(TState)"/>. to avoid the need for allocations when attempting to format a state object using <see cref="StructuredLoggerMessageFormatter.Format{TValues}(TValues)"/>.
    /// 
    /// As such, this type does not actually implement <see cref="IEnumerable"/> and <see cref="IEnumerable{T}"/> members, and will throw <see cref="NotSupportedException"/> if they are accessed.
    /// </remarks>
    public struct LogStateValuesSelector<TState>
            : IReadOnlyList<object?>
        where TState : IReadOnlyList<KeyValuePair<string, object?>>
    {
        /// <summary>
        /// Initializes a new <see cref="LogStateValuesSelector{TState}"/> instance, upon a given state container.
        /// </summary>
        /// <param name="state">The state container whose values are to be extracted.</param>
        public LogStateValuesSelector(TState state)
            => _state = state;

        /// <inheritdoc/>
        public object? this[int index]
            => _state[index].Value;

        /// <inheritdoc/>
        public int Count
            => _state.Count;

        private readonly TState _state;

        IEnumerator<object?> IEnumerable<object?>.GetEnumerator()
            => throw new NotSupportedException();

        IEnumerator IEnumerable.GetEnumerator()
            => throw new NotSupportedException();
    }
}
