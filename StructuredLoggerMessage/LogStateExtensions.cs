using System.Collections.Generic;

namespace Microsoft.Extensions.Logging.Structured
{
    /// <summary>
    /// Contains extension methods for utilizing and manipulating standard log state containers.
    /// </summary>
    public static class LogStateExtensions
    {
        /// <summary>
        /// Constructs a wrapper around a given log state container, which will directly extract data values from the container.
        /// </summary>
        /// <typeparam name="TState">The type of state container whose values are to be extracted.</typeparam>
        /// <param name="state">The state container whose values are to be extracted.</param>
        /// <returns>A <see cref="LogStateValuesSelector{TState}"/> instance, constructed from <paramref name="state"/>.</returns>
        public static LogStateValuesSelector<TState> SelectValues<TState>(this TState state)
                where TState : IReadOnlyList<KeyValuePair<string, object?>>
            => new(state);
    }
}
