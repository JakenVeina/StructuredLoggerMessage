using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using NUnit.Framework;
using Shouldly;

namespace Microsoft.Extensions.Logging.Structured.Test
{
    [TestFixture]
    public partial class StructuredLoggerMessageTests
    {
        public class DefineScopeDataTestContext
        {
            public static DefineScopeDataTestContext CreateWithStringValues(int dataCount)
                => new()
                {
                    Data = Enumerable.Range(1, dataCount)
                        .Select(x => new KeyValuePair<string, object?>($"name{x}", $"value{x}"))
                        .ToArray()
                };

            public static DefineScopeDataTestContext CreateWithNullValues(int dataCount)
                => new()
                {
                    Data = Enumerable.Range(1, dataCount)
                        .Select(x => new KeyValuePair<string, object?>($"name{x}", null))
                        .ToArray()
                };

            public static DefineScopeDataTestContext CreateWithNumericValues(int dataCount)
                => new()
                {
                    Data = Enumerable.Range(1, dataCount)
                        .Select(x => new KeyValuePair<string, object?>($"name{x}", x))
                        .ToArray()
                };

            public IReadOnlyList<KeyValuePair<string, object?>> Data { get; init; }
                = Array.Empty<KeyValuePair<string, object?>>();

            public FakeLogger Logger { get; init; }
                = new();

            public IDisposable Execute()
            {
                try
                {
                    return (IDisposable)GetType().GetMethods(BindingFlags.NonPublic | BindingFlags.Instance)
                        .Where(mi => mi.Name == nameof(Execute))
                        .Where(mi => mi.IsGenericMethodDefinition)
                        .Where(mi => mi.GetGenericArguments().Length == Data.Count)
                        .Select(mi => mi.MakeGenericMethod(Data
                            .Select(d => d.Value?.GetType() ?? typeof(object))
                            .ToArray()))
                        .First()
                        .Invoke(this, null)!;
                }
                catch (TargetInvocationException ex)
                {
                    throw ex.InnerException!;
                }
            }

            private IDisposable Execute<T1>()
            {
                var result = StructuredLoggerMessage.DefineScopeData<T1>(
                    Data[0].Key);

                return result.Invoke(
                    Logger,
                    (T1)Data[0].Value!);
            }

            private IDisposable Execute<T1, T2>()
            {
                var result = StructuredLoggerMessage.DefineScopeData<T1, T2>(
                    Data[0].Key,
                    Data[1].Key);

                return result.Invoke(
                    Logger,
                    (T1)Data[0].Value!,
                    (T2)Data[1].Value!);
            }

            private IDisposable Execute<T1, T2, T3>()
            {
                var result = StructuredLoggerMessage.DefineScopeData<T1, T2, T3>(
                    Data[0].Key,
                    Data[1].Key,
                    Data[2].Key);

                return result.Invoke(
                    Logger,
                    (T1)Data[0].Value!,
                    (T2)Data[1].Value!,
                    (T3)Data[2].Value!);
            }

            private IDisposable Execute<T1, T2, T3, T4>()
            {
                var result = StructuredLoggerMessage.DefineScopeData<T1, T2, T3, T4>(
                    Data[0].Key,
                    Data[1].Key,
                    Data[2].Key,
                    Data[3].Key);

                return result.Invoke(
                    Logger,
                    (T1)Data[0].Value!,
                    (T2)Data[1].Value!,
                    (T3)Data[2].Value!,
                    (T4)Data[3].Value!);
            }

            private IDisposable Execute<T1, T2, T3, T4, T5>()
            {
                var result = StructuredLoggerMessage.DefineScopeData<T1, T2, T3, T4, T5>(
                    Data[0].Key,
                    Data[1].Key,
                    Data[2].Key,
                    Data[3].Key,
                    Data[4].Key);

                return result.Invoke(
                    Logger,
                    (T1)Data[0].Value!,
                    (T2)Data[1].Value!,
                    (T3)Data[2].Value!,
                    (T4)Data[3].Value!,
                    (T5)Data[4].Value!);
            }

            private IDisposable Execute<T1, T2, T3, T4, T5, T6>()
            {
                var result = StructuredLoggerMessage.DefineScopeData<T1, T2, T3, T4, T5, T6>(
                    Data[0].Key,
                    Data[1].Key,
                    Data[2].Key,
                    Data[3].Key,
                    Data[4].Key,
                    Data[5].Key);

                return result.Invoke(
                    Logger,
                    (T1)Data[0].Value!,
                    (T2)Data[1].Value!,
                    (T3)Data[2].Value!,
                    (T4)Data[3].Value!,
                    (T5)Data[4].Value!,
                    (T6)Data[5].Value!);
            }
        }

        public static IEnumerable<TestCaseData> DefineScopeData_Otherwise_TestCaseData
            => Enumerable.Empty<TestCaseData>()
                .Append(new TestCaseData(DefineScopeDataTestContext.CreateWithStringValues(1)   ).SetName("{m}(1 value)"))
                .Append(new TestCaseData(DefineScopeDataTestContext.CreateWithNullValues(1)     ).SetName("{m}(1 value, null)"))
                .Append(new TestCaseData(DefineScopeDataTestContext.CreateWithNumericValues(1)  ).SetName("{m}(1 value, numeric)"))
                .Append(new TestCaseData(DefineScopeDataTestContext.CreateWithStringValues(2)   ).SetName("{m}(2 values)"))
                .Append(new TestCaseData(DefineScopeDataTestContext.CreateWithNullValues(2)     ).SetName("{m}(2 values, null)"))
                .Append(new TestCaseData(DefineScopeDataTestContext.CreateWithNumericValues(2)  ).SetName("{m}(2 values, numeric)"))
                .Append(new TestCaseData(DefineScopeDataTestContext.CreateWithStringValues(3)   ).SetName("{m}(3 values)"))
                .Append(new TestCaseData(DefineScopeDataTestContext.CreateWithNullValues(3)     ).SetName("{m}(3 values, null)"))
                .Append(new TestCaseData(DefineScopeDataTestContext.CreateWithNumericValues(3)  ).SetName("{m}(3 values, numeric)"))
                .Append(new TestCaseData(DefineScopeDataTestContext.CreateWithStringValues(4)   ).SetName("{m}(4 values)"))
                .Append(new TestCaseData(DefineScopeDataTestContext.CreateWithNullValues(4)     ).SetName("{m}(4 values, null)"))
                .Append(new TestCaseData(DefineScopeDataTestContext.CreateWithNumericValues(4)  ).SetName("{m}(4 values, numeric)"))
                .Append(new TestCaseData(DefineScopeDataTestContext.CreateWithStringValues(5)   ).SetName("{m}(5 values)"))
                .Append(new TestCaseData(DefineScopeDataTestContext.CreateWithNullValues(5)     ).SetName("{m}(5 values, null)"))
                .Append(new TestCaseData(DefineScopeDataTestContext.CreateWithNumericValues(5)  ).SetName("{m}(5 values, numeric)"))
                .Append(new TestCaseData(DefineScopeDataTestContext.CreateWithStringValues(6)   ).SetName("{m}(6 values)"))
                .Append(new TestCaseData(DefineScopeDataTestContext.CreateWithNullValues(6)     ).SetName("{m}(6 values, null)"))
                .Append(new TestCaseData(DefineScopeDataTestContext.CreateWithNumericValues(6)  ).SetName("{m}(6 values, numeric)"));

        [TestCaseSource(nameof(DefineScopeData_Otherwise_TestCaseData))]
        public void DefineScopeData_Always_LoggerReceivesDataFromResultInvocation(DefineScopeDataTestContext context)
        {
            var result = context.Execute();

            context.Logger.DefineScopeInvocations.Count.ShouldBe(1);

            var defineScopeInvocation = context.Logger.DefineScopeInvocations[0];
            defineScopeInvocation.Result.ShouldBe(result);

            defineScopeInvocation.State.ShouldNotBeNull();
            var state = defineScopeInvocation.State.ShouldBeAssignableTo<IReadOnlyList<KeyValuePair<string, object?>>>()!;
            state.Count.ShouldBe(context.Data.Count);
            foreach(var data in context.Data)
            {
                state.Select(vp => vp.Key).ShouldContain(data.Key);
                state.First(vp => vp.Key == data.Key).Value.ShouldBe(data.Value);
            }
        }
    }
}
