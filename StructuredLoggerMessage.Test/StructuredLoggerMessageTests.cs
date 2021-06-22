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
        public class TestContext
        {
            public LogLevel LogLevel { get; init; }

            public EventId EventId { get; init; }

            public string FormatString { get; init; }
                = "";

            public string[] UnformattedValueNames { get; init; }
                = Array.Empty<string>();

            public IReadOnlyList<object?> Values { get; init; }
                = Array.Empty<object?>();

            public FakeLogger Logger { get; init; }
                = new();

            public Exception? Exception { get; init; }

            public int ExpectedMalformationIndex { get; init; }

            public IReadOnlyList<string> ExpectedValueNames { get; init; }
                = Array.Empty<string>();

            public string ExpectedMessage { get; init; }
                = "";

            public void Execute()
            {
                try
                {
                    GetType().GetMethods(BindingFlags.NonPublic | BindingFlags.Instance)
                        .Where(mi => mi.Name == nameof(Execute))
                        .Where(mi => mi.IsGenericMethodDefinition)
                        .Where(mi => mi.GetGenericArguments().Length == Values.Count)
                        .Select(mi => mi.MakeGenericMethod(Values
                            .Select(v => v?.GetType() ?? typeof(object))
                            .ToArray()))
                        .First()
                        .Invoke(this, null);
                }
                catch (TargetInvocationException ex)
                {
                    throw ex.InnerException!;
                }
            }

            private void Execute<T1>()
            {
                var result = StructuredLoggerMessage.Define<T1>(
                logLevel:               LogLevel,
                eventId:                EventId,
                formatString:           FormatString,
                unformattedValueNames:  UnformattedValueNames);

                result.Invoke(
                    Logger,
                    (T1)Values[0]!,
                    Exception);
            }

            private void Execute<T1, T2>()
            {
                var result = StructuredLoggerMessage.Define<T1, T2>(
                logLevel:               LogLevel,
                eventId:                EventId,
                formatString:           FormatString,
                unformattedValueNames:  UnformattedValueNames);

                result.Invoke(
                    Logger,
                    (T1)Values[0]!,
                    (T2)Values[1]!,
                    Exception);
            }

            private void Execute<T1, T2, T3>()
            {
                var result = StructuredLoggerMessage.Define<T1, T2, T3>(
                logLevel:               LogLevel,
                eventId:                EventId,
                formatString:           FormatString,
                unformattedValueNames:  UnformattedValueNames);

                result.Invoke(
                    Logger,
                    (T1)Values[0]!,
                    (T2)Values[1]!,
                    (T3)Values[2]!,
                    Exception);
            }

            private void Execute<T1, T2, T3, T4>()
            {
                var result = StructuredLoggerMessage.Define<T1, T2, T3, T4>(
                logLevel:               LogLevel,
                eventId:                EventId,
                formatString:           FormatString,
                unformattedValueNames:  UnformattedValueNames);

                result.Invoke(
                    Logger,
                    (T1)Values[0]!,
                    (T2)Values[1]!,
                    (T3)Values[2]!,
                    (T4)Values[3]!,
                    Exception);
            }

            private void Execute<T1, T2, T3, T4, T5>()
            {
                var result = StructuredLoggerMessage.Define<T1, T2, T3, T4, T5>(
                logLevel:               LogLevel,
                eventId:                EventId,
                formatString:           FormatString,
                unformattedValueNames:  UnformattedValueNames);

                result.Invoke(
                    Logger,
                    (T1)Values[0]!,
                    (T2)Values[1]!,
                    (T3)Values[2]!,
                    (T4)Values[3]!,
                    (T5)Values[4]!,
                    Exception);
            }

            private void Execute<T1, T2, T3, T4, T5, T6>()
            {
                var result = StructuredLoggerMessage.Define<T1, T2, T3, T4, T5, T6>(
                logLevel:               LogLevel,
                eventId:                EventId,
                formatString:           FormatString,
                unformattedValueNames:  UnformattedValueNames);

                result.Invoke(
                    Logger,
                    (T1)Values[0]!,
                    (T2)Values[1]!,
                    (T3)Values[2]!,
                    (T4)Values[3]!,
                    (T5)Values[4]!,
                    (T6)Values[5]!,
                    Exception);
            }
        }

        public static IEnumerable<TestCaseData> FormatStringIsMalformed_TestCaseData
            => Enumerable.Empty<TestCaseData>()
                .Append(new TestCaseData(new TestContext() { FormatString = "{}", UnformattedValueNames = new[] { "name1" },                                                Values = new object?[] { "value1" },                                                    ExpectedMalformationIndex = 1   }).SetName("{m}(With 1 value)"))
                .Append(new TestCaseData(new TestContext() { FormatString = "{}", UnformattedValueNames = new[] { "name1", "name2" },                                       Values = new object?[] { "value1", "value2" },                                          ExpectedMalformationIndex = 1   }).SetName("{m}(With 2 values)"))
                .Append(new TestCaseData(new TestContext() { FormatString = "{}", UnformattedValueNames = new[] { "name1", "name2", "name3" },                              Values = new object?[] { "value1", "value2", "value3" },                                ExpectedMalformationIndex = 1   }).SetName("{m}(With 3 values)"))
                .Append(new TestCaseData(new TestContext() { FormatString = "{}", UnformattedValueNames = new[] { "name1", "name2", "name3", "name4" },                     Values = new object?[] { "value1", "value2", "value3", "value4" },                      ExpectedMalformationIndex = 1   }).SetName("{m}(With 4 values)"))
                .Append(new TestCaseData(new TestContext() { FormatString = "{}", UnformattedValueNames = new[] { "name1", "name2", "name3", "name4", "name5" },            Values = new object?[] { "value1", "value2", "value3", "value4", "value5" },            ExpectedMalformationIndex = 1   }).SetName("{m}(With 5 values)"))
                .Append(new TestCaseData(new TestContext() { FormatString = "{}", UnformattedValueNames = new[] { "name1", "name2", "name3", "name4", "name5", "name6" },   Values = new object?[] { "value1", "value2", "value3", "value4", "value5", "value6" },  ExpectedMalformationIndex = 1   }).SetName("{m}(With 6 values)"));

        [TestCaseSource(nameof(FormatStringIsMalformed_TestCaseData))]
        public void Define_FormatStringIsMalformed_ThrowsException(TestContext context)
        {
            var result = Should.Throw<MalformedFormatStringException>(() =>
            {
                context.Execute();
            });

            result.Index.ShouldBe(context.ExpectedMalformationIndex);
            result.FormatString.ShouldBe(context.FormatString);
            result.ParamName.ShouldBe("formatString");
        }

        public static IEnumerable<TestCaseData> ValueDefinitionsAreInvalid_TestCaseData
            => Enumerable.Empty<TestCaseData>()
                .Append(new TestCaseData(new TestContext() { FormatString = "",                                                     UnformattedValueNames = Array.Empty<string>(),                                                      Values = new object?[] { "value1" }                                                     }).SetName("{m}(Not enough values defined, 1 expected)"))
                .Append(new TestCaseData(new TestContext() { FormatString = "",                                                     UnformattedValueNames = Array.Empty<string>(),                                                      Values = new object?[] { "value1", "value2" }                                           }).SetName("{m}(Not enough values defined, 2 expected)"))
                .Append(new TestCaseData(new TestContext() { FormatString = "",                                                     UnformattedValueNames = Array.Empty<string>(),                                                      Values = new object?[] { "value1", "value2", "value3" }                                 }).SetName("{m}(Not enough values defined, 3 expected)"))
                .Append(new TestCaseData(new TestContext() { FormatString = "",                                                     UnformattedValueNames = Array.Empty<string>(),                                                      Values = new object?[] { "value1", "value2", "value3", "value4" }                       }).SetName("{m}(Not enough values defined, 4 expected)"))
                .Append(new TestCaseData(new TestContext() { FormatString = "",                                                     UnformattedValueNames = Array.Empty<string>(),                                                      Values = new object?[] { "value1", "value2", "value3", "value4", "value5" }             }).SetName("{m}(Not enough values defined, 5 expected)"))
                .Append(new TestCaseData(new TestContext() { FormatString = "",                                                     UnformattedValueNames = Array.Empty<string>(),                                                      Values = new object?[] { "value1", "value2", "value3", "value4", "value5", "value6" }   }).SetName("{m}(Not enough values defined, 6 expected)"))
                .Append(new TestCaseData(new TestContext() { FormatString = "{name1}",                                              UnformattedValueNames = new[] { "name2" },                                                          Values = new object?[] { "value1", "value2", "value3" }                                 }).SetName("{m}(Not enough values defined, non-trivial)"))
                .Append(new TestCaseData(new TestContext() { FormatString = "{name1}{name2}",                                       UnformattedValueNames = Array.Empty<string>(),                                                      Values = new object?[] { "value1" }                                                     }).SetName("{m}(Too many values defined, formatted)"))
                .Append(new TestCaseData(new TestContext() { FormatString = "",                                                     UnformattedValueNames = new[] { "name1", "name2" },                                                 Values = new object?[] { "value1" }                                                     }).SetName("{m}(Too many values defined, unfformatted)"))
                .Append(new TestCaseData(new TestContext() { FormatString = "{name1}",                                              UnformattedValueNames = new[] { "name2" },                                                          Values = new object?[] { "value1" }                                                     }).SetName("{m}(Too many values defined, formatted and unformatted)"))
                .Append(new TestCaseData(new TestContext() { FormatString = "{name1}{name2}{name3}{name4}{name5}{name6}{name7}",    UnformattedValueNames = new[] { "name1", "name2", "name3", "name4", "name5", "name6", "name7" },    Values = new object?[] { "value1" }                                                     }).SetName("{m}(Too many values defined, 1 expected)"))
                .Append(new TestCaseData(new TestContext() { FormatString = "{name1}{name2}{name3}{name4}{name5}{name6}{name7}",    UnformattedValueNames = new[] { "name1", "name2", "name3", "name4", "name5", "name6", "name7" },    Values = new object?[] { "value1", "value2" }                                           }).SetName("{m}(Too many values defined, 2 expected)"))
                .Append(new TestCaseData(new TestContext() { FormatString = "{name1}{name2}{name3}{name4}{name5}{name6}{name7}",    UnformattedValueNames = new[] { "name1", "name2", "name3", "name4", "name5", "name6", "name7" },    Values = new object?[] { "value1", "value2", "value3" }                                 }).SetName("{m}(Too many values defined, 3 expected)"))
                .Append(new TestCaseData(new TestContext() { FormatString = "{name1}{name2}{name3}{name4}{name5}{name6}{name7}",    UnformattedValueNames = new[] { "name1", "name2", "name3", "name4", "name5", "name6", "name7" },    Values = new object?[] { "value1", "value2", "value3", "value4" }                       }).SetName("{m}(Too many values defined, 4 expected)"))
                .Append(new TestCaseData(new TestContext() { FormatString = "{name1}{name2}{name3}{name4}{name5}{name6}{name7}",    UnformattedValueNames = new[] { "name1", "name2", "name3", "name4", "name5", "name6", "name7" },    Values = new object?[] { "value1", "value2", "value3", "value4", "value5" }             }).SetName("{m}(Too many values defined, 5 expected)"))
                .Append(new TestCaseData(new TestContext() { FormatString = "{name1}{name2}{name3}{name4}{name5}{name6}{name7}",    UnformattedValueNames = new[] { "name1", "name2", "name3", "name4", "name5", "name6", "name7" },    Values = new object?[] { "value1", "value2", "value3", "value4", "value5", "value6" }   }).SetName("{m}(Too many values defined, 6 expected)"))
                .Append(new TestCaseData(new TestContext() { FormatString = "{name}{name}",                                         UnformattedValueNames = Array.Empty<string>(),                                                      Values = new object?[] { "value1", "value2" }                                           }).SetName("{m}(Duplicate names defined, formatted)"))
                .Append(new TestCaseData(new TestContext() { FormatString = "",                                                     UnformattedValueNames = new[] { "name", "name" },                                                   Values = new object?[] { "value1", "value2" }                                           }).SetName("{m}(Duplicate names defined, unformatted)"))
                .Append(new TestCaseData(new TestContext() { FormatString = "{name}",                                               UnformattedValueNames = new[] { "name" },                                                           Values = new object?[] { "value1", "value2" }                                           }).SetName("{m}(Duplicate names defined, formatted and unformatted)"));

        [TestCaseSource(nameof(ValueDefinitionsAreInvalid_TestCaseData))]
        public void Define_ValueDefinitionsAreInvalid_ThrowsException(TestContext context)
        {
            var result = Should.Throw<ArgumentException>(() =>
            {
                context.Execute();
            });

            result.ParamName.ShouldBeNull();
        }

        public static IEnumerable<TestCaseData> LoggerIsDisabled_TestCaseData
            => Enumerable.Empty<TestCaseData>()
                .Append(new TestCaseData(new TestContext() { FormatString = "{name1}",                                      Values = new object?[] { "value1" },                                                    Logger = new() { IsEnabled = false }    }).SetName("{m}(With 1 value)"))
                .Append(new TestCaseData(new TestContext() { FormatString = "{name1}{name2}",                               Values = new object?[] { "value1", "value2" },                                          Logger = new() { IsEnabled = false }    }).SetName("{m}(With 2 values)"))
                .Append(new TestCaseData(new TestContext() { FormatString = "{name1}{name2}{name3}",                        Values = new object?[] { "value1", "value2", "value3" },                                Logger = new() { IsEnabled = false }    }).SetName("{m}(With 3 values)"))
                .Append(new TestCaseData(new TestContext() { FormatString = "{name1}{name2}{name3}{name4}",                 Values = new object?[] { "value1", "value2", "value3", "value4" },                      Logger = new() { IsEnabled = false }    }).SetName("{m}(With 4 values)"))
                .Append(new TestCaseData(new TestContext() { FormatString = "{name1}{name2}{name3}{name4}{name5}",          Values = new object?[] { "value1", "value2", "value3", "value4", "value5" },            Logger = new() { IsEnabled = false }    }).SetName("{m}(With 5 values)"))
                .Append(new TestCaseData(new TestContext() { FormatString = "{name1}{name2}{name3}{name4}{name5}{name6}",   Values = new object?[] { "value1", "value2", "value3", "value4", "value5", "value6" },  Logger = new() { IsEnabled = false }    }).SetName("{m}(With 6 values)"));

        [TestCaseSource(nameof(LoggerIsDisabled_TestCaseData))]
        public void Define_LoggerIsDisabled_LoggerDoesNotReceiveLogFromResultInvocation(TestContext context)
        {
            context.Execute();

            context.Logger.LogInvocations.ShouldBeEmpty();
        }

        public static IEnumerable<TestCaseData> Otherwise_TestCaseData
            => Enumerable.Empty<TestCaseData>()
                .Append(new TestCaseData(new TestContext() { LogLevel = LogLevel.Trace,         EventId = new EventId(1,    "Event1"),  FormatString = "Message {name1}",                                           UnformattedValueNames = Array.Empty<string>(),                                          Values = new object?[] { "value1" },                                                    Exception = null,               ExpectedValueNames = new[] { "name1" },                                                 ExpectedMessage = "Message value1"                                      }).SetName("{m}(With 1 value, formatted)"))
                .Append(new TestCaseData(new TestContext() { LogLevel = LogLevel.Debug,         EventId = new EventId(2,    "Event2"),  FormatString = "Message",                                                   UnformattedValueNames = new[] { "name1" },                                              Values = new object?[] { "value1" },                                                    Exception = null,               ExpectedValueNames = new[] { "name1" },                                                 ExpectedMessage = "Message"                                             }).SetName("{m}(With 1 value, unformatted)"))
                .Append(new TestCaseData(new TestContext() { LogLevel = LogLevel.Information,   EventId = new EventId(3,    "Event3"),  FormatString = "Message {name1}",                                           UnformattedValueNames = Array.Empty<string>(),                                          Values = new object?[] { "value1" },                                                    Exception = new Exception(),    ExpectedValueNames = new[] { "name1" },                                                 ExpectedMessage = "Message value1"                                      }).SetName("{m}(With 1 value, with exception)"))
                .Append(new TestCaseData(new TestContext() { LogLevel = LogLevel.Warning,       EventId = new EventId(4,    "Event4"),  FormatString = "Message {name1}",                                           UnformattedValueNames = Array.Empty<string>(),                                          Values = new object?[] { null },                                                        Exception = null,               ExpectedValueNames = new[] { "name1" },                                                 ExpectedMessage = "Message null"                                        }).SetName("{m}(With 1 value, with null value)"))
                .Append(new TestCaseData(new TestContext() { LogLevel = LogLevel.Error,         EventId = new EventId(5,    "Event5"),  FormatString = "Message {name1}",                                           UnformattedValueNames = Array.Empty<string>(),                                          Values = new object?[] { 1 },                                                           Exception = null,               ExpectedValueNames = new[] { "name1" },                                                 ExpectedMessage = "Message 1"                                           }).SetName("{m}(With 1 value, with numeric value)"))
                .Append(new TestCaseData(new TestContext() { LogLevel = LogLevel.Critical,      EventId = new EventId(6,    "Event6"),  FormatString = "Message {name1} {name2}",                                   UnformattedValueNames = Array.Empty<string>(),                                          Values = new object?[] { "value1", "value2" },                                          Exception = null,               ExpectedValueNames = new[] { "name1", "name2" },                                        ExpectedMessage = "Message value1 value2"                               }).SetName("{m}(With 2 values, formatted)"))
                .Append(new TestCaseData(new TestContext() { LogLevel = LogLevel.None,          EventId = new EventId(7,    "Event7"),  FormatString = "Message",                                                   UnformattedValueNames = new[] { "name1", "name2" },                                     Values = new object?[] { "value1", "value2" },                                          Exception = null,               ExpectedValueNames = new[] { "name1", "name2" },                                        ExpectedMessage = "Message"                                             }).SetName("{m}(With 2 values, unformatted)"))
                .Append(new TestCaseData(new TestContext() { LogLevel = LogLevel.Trace,         EventId = new EventId(8,    "Event8"),  FormatString = "Message {name1} {name2}",                                   UnformattedValueNames = Array.Empty<string>(),                                          Values = new object?[] { "value1", "value2" },                                          Exception = new Exception(),    ExpectedValueNames = new[] { "name1", "name2" },                                        ExpectedMessage = "Message value1 value2"                               }).SetName("{m}(With 2 values, with exception)"))
                .Append(new TestCaseData(new TestContext() { LogLevel = LogLevel.Debug,         EventId = new EventId(9,    "Event9"),  FormatString = "Message {name1} {name2}",                                   UnformattedValueNames = Array.Empty<string>(),                                          Values = new object?[] { null, null },                                                  Exception = null,               ExpectedValueNames = new[] { "name1", "name2" },                                        ExpectedMessage = "Message null null"                                   }).SetName("{m}(With 2 values, with null values)"))
                .Append(new TestCaseData(new TestContext() { LogLevel = LogLevel.Information,   EventId = new EventId(10,   "Event10"), FormatString = "Message {name1} {name2}",                                   UnformattedValueNames = Array.Empty<string>(),                                          Values = new object?[] { 1, 2 },                                                        Exception = null,               ExpectedValueNames = new[] { "name1", "name2" },                                        ExpectedMessage = "Message 1 2"                                         }).SetName("{m}(With 2 values, with numeric values)"))
                .Append(new TestCaseData(new TestContext() { LogLevel = LogLevel.Warning,       EventId = new EventId(11,   "Event11"), FormatString = "Message {name1} {name2} {name3}",                           UnformattedValueNames = Array.Empty<string>(),                                          Values = new object?[] { "value1", "value2", "value3" },                                Exception = null,               ExpectedValueNames = new[] { "name1", "name2", "name3" },                               ExpectedMessage = "Message value1 value2 value3"                        }).SetName("{m}(With 3 values, formatted)"))
                .Append(new TestCaseData(new TestContext() { LogLevel = LogLevel.Error,         EventId = new EventId(12,   "Event12"), FormatString = "Message",                                                   UnformattedValueNames = new[] { "name1", "name2", "name3" },                            Values = new object?[] { "value1", "value2", "value3" },                                Exception = null,               ExpectedValueNames = new[] { "name1", "name2", "name3" },                               ExpectedMessage = "Message"                                             }).SetName("{m}(With 3 values, unformatted)"))
                .Append(new TestCaseData(new TestContext() { LogLevel = LogLevel.Critical,      EventId = new EventId(13,   "Event13"), FormatString = "Message {name1} {name2} {name3}",                           UnformattedValueNames = Array.Empty<string>(),                                          Values = new object?[] { "value1", "value2", "value3" },                                Exception = new Exception(),    ExpectedValueNames = new[] { "name1", "name2", "name3" },                               ExpectedMessage = "Message value1 value2 value3"                        }).SetName("{m}(With 3 values, with exception)"))
                .Append(new TestCaseData(new TestContext() { LogLevel = LogLevel.None,          EventId = new EventId(14,   "Event14"), FormatString = "Message {name1} {name2} {name3}",                           UnformattedValueNames = Array.Empty<string>(),                                          Values = new object?[] { null, null, null },                                            Exception = null,               ExpectedValueNames = new[] { "name1", "name2", "name3" },                               ExpectedMessage = "Message null null null"                              }).SetName("{m}(With 3 values, with null values)"))
                .Append(new TestCaseData(new TestContext() { LogLevel = LogLevel.Trace,         EventId = new EventId(15,   "Event15"), FormatString = "Message {name1} {name2} {name3}",                           UnformattedValueNames = Array.Empty<string>(),                                          Values = new object?[] { 1, 2, 3 },                                                     Exception = null,               ExpectedValueNames = new[] { "name1", "name2", "name3" },                               ExpectedMessage = "Message 1 2 3"                                       }).SetName("{m}(With 3 values, with numeric values)"))
                .Append(new TestCaseData(new TestContext() { LogLevel = LogLevel.Debug,         EventId = new EventId(16,   "Event16"), FormatString = "Message {name1} {name2} {name3} {name4}",                   UnformattedValueNames = Array.Empty<string>(),                                          Values = new object?[] { "value1", "value2", "value3", "value4" },                      Exception = null,               ExpectedValueNames = new[] { "name1", "name2", "name3", "name4" },                      ExpectedMessage = "Message value1 value2 value3 value4"                 }).SetName("{m}(With 4 values, formatted)"))
                .Append(new TestCaseData(new TestContext() { LogLevel = LogLevel.Information,   EventId = new EventId(17,   "Event17"), FormatString = "Message",                                                   UnformattedValueNames = new[] { "name1", "name2", "name3", "name4" },                   Values = new object?[] { "value1", "value2", "value3", "value4" },                      Exception = null,               ExpectedValueNames = new[] { "name1", "name2", "name3", "name4" },                      ExpectedMessage = "Message"                                             }).SetName("{m}(With 4 values, unformatted)"))
                .Append(new TestCaseData(new TestContext() { LogLevel = LogLevel.Warning,       EventId = new EventId(18,   "Event18"), FormatString = "Message {name1} {name2} {name3} {name4}",                   UnformattedValueNames = Array.Empty<string>(),                                          Values = new object?[] { "value1", "value2", "value3", "value4" },                      Exception = new Exception(),    ExpectedValueNames = new[] { "name1", "name2", "name3", "name4" },                      ExpectedMessage = "Message value1 value2 value3 value4"                 }).SetName("{m}(With 4 values, with exception)"))
                .Append(new TestCaseData(new TestContext() { LogLevel = LogLevel.Error,         EventId = new EventId(19,   "Event19"), FormatString = "Message {name1} {name2} {name3} {name4}",                   UnformattedValueNames = Array.Empty<string>(),                                          Values = new object?[] { null, null, null, null },                                      Exception = null,               ExpectedValueNames = new[] { "name1", "name2", "name3", "name4" },                      ExpectedMessage = "Message null null null null"                         }).SetName("{m}(With 4 values, with null values)"))
                .Append(new TestCaseData(new TestContext() { LogLevel = LogLevel.Critical,      EventId = new EventId(20,   "Event20"), FormatString = "Message {name1} {name2} {name3} {name4}",                   UnformattedValueNames = Array.Empty<string>(),                                          Values = new object?[] { 1, 2, 3, 4 },                                                  Exception = null,               ExpectedValueNames = new[] { "name1", "name2", "name3", "name4" },                      ExpectedMessage = "Message 1 2 3 4"                                     }).SetName("{m}(With 4 values, with numeric values)"))
                .Append(new TestCaseData(new TestContext() { LogLevel = LogLevel.None,          EventId = new EventId(21,   "Event21"), FormatString = "Message {name1} {name2} {name3} {name4} {name5}",           UnformattedValueNames = Array.Empty<string>(),                                          Values = new object?[] { "value1", "value2", "value3", "value4", "value5" },            Exception = null,               ExpectedValueNames = new[] { "name1", "name2", "name3", "name4", "name5" },             ExpectedMessage = "Message value1 value2 value3 value4 value5"          }).SetName("{m}(With 5 values, formatted)"))
                .Append(new TestCaseData(new TestContext() { LogLevel = LogLevel.Trace,         EventId = new EventId(22,   "Event22"), FormatString = "Message",                                                   UnformattedValueNames = new[] { "name1", "name2", "name3", "name4", "name5" },          Values = new object?[] { "value1", "value2", "value3", "value4", "value5" },            Exception = null,               ExpectedValueNames = new[] { "name1", "name2", "name3", "name4", "name5" },             ExpectedMessage = "Message"                                             }).SetName("{m}(With 5 values, unformatted)"))
                .Append(new TestCaseData(new TestContext() { LogLevel = LogLevel.Debug,         EventId = new EventId(23,   "Event23"), FormatString = "Message {name1} {name2} {name3} {name4} {name5}",           UnformattedValueNames = Array.Empty<string>(),                                          Values = new object?[] { "value1", "value2", "value3", "value4", "value5" },            Exception = new Exception(),    ExpectedValueNames = new[] { "name1", "name2", "name3", "name4", "name5" },             ExpectedMessage = "Message value1 value2 value3 value4 value5"          }).SetName("{m}(With 5 values, with exception)"))
                .Append(new TestCaseData(new TestContext() { LogLevel = LogLevel.Information,   EventId = new EventId(24,   "Event24"), FormatString = "Message {name1} {name2} {name3} {name4} {name5}",           UnformattedValueNames = Array.Empty<string>(),                                          Values = new object?[] { null, null, null, null, null },                                Exception = null,               ExpectedValueNames = new[] { "name1", "name2", "name3", "name4", "name5" },             ExpectedMessage = "Message null null null null null"                    }).SetName("{m}(With 5 values, with null values)"))
                .Append(new TestCaseData(new TestContext() { LogLevel = LogLevel.Warning,       EventId = new EventId(25,   "Event25"), FormatString = "Message {name1} {name2} {name3} {name4} {name5}",           UnformattedValueNames = Array.Empty<string>(),                                          Values = new object?[] { 1, 2, 3, 4, 5 },                                               Exception = null,               ExpectedValueNames = new[] { "name1", "name2", "name3", "name4", "name5" },             ExpectedMessage = "Message 1 2 3 4 5"                                   }).SetName("{m}(With 5 values, with numeric values)"))
                .Append(new TestCaseData(new TestContext() { LogLevel = LogLevel.Error,         EventId = new EventId(26,   "Event26"), FormatString = "Message {name1} {name2} {name3} {name4} {name5} {name6}",   UnformattedValueNames = Array.Empty<string>(),                                          Values = new object?[] { "value1", "value2", "value3", "value4", "value5", "value6" },  Exception = null,               ExpectedValueNames = new[] { "name1", "name2", "name3", "name4", "name5", "name6" },    ExpectedMessage = "Message value1 value2 value3 value4 value5 value6"   }).SetName("{m}(With 6 values, formatted)"))
                .Append(new TestCaseData(new TestContext() { LogLevel = LogLevel.Critical,      EventId = new EventId(27,   "Event27"), FormatString = "Message",                                                   UnformattedValueNames = new[] { "name1", "name2", "name3", "name4", "name5", "name6" }, Values = new object?[] { "value1", "value2", "value3", "value4", "value5", "value6" },  Exception = null,               ExpectedValueNames = new[] { "name1", "name2", "name3", "name4", "name5", "name6" },    ExpectedMessage = "Message"                                             }).SetName("{m}(With 6 values, unformatted)"))
                .Append(new TestCaseData(new TestContext() { LogLevel = LogLevel.None,          EventId = new EventId(28,   "Event28"), FormatString = "Message {name1} {name2} {name3} {name4} {name5} {name6}",   UnformattedValueNames = Array.Empty<string>(),                                          Values = new object?[] { "value1", "value2", "value3", "value4", "value5", "value6" },  Exception = new Exception(),    ExpectedValueNames = new[] { "name1", "name2", "name3", "name4", "name5", "name6" },    ExpectedMessage = "Message value1 value2 value3 value4 value5 value6"   }).SetName("{m}(With 6 values, with exception)"))
                .Append(new TestCaseData(new TestContext() { LogLevel = LogLevel.Trace,         EventId = new EventId(29,   "Event29"), FormatString = "Message {name1} {name2} {name3} {name4} {name5} {name6}",   UnformattedValueNames = Array.Empty<string>(),                                          Values = new object?[] { null, null, null, null, null, null },                          Exception = null,               ExpectedValueNames = new[] { "name1", "name2", "name3", "name4", "name5", "name6" },    ExpectedMessage = "Message null null null null null null"               }).SetName("{m}(With 6 values, with null values)"))
                .Append(new TestCaseData(new TestContext() { LogLevel = LogLevel.Debug,         EventId = new EventId(30,   "Event30"), FormatString = "Message {name1} {name2} {name3} {name4} {name5} {name6}",   UnformattedValueNames = Array.Empty<string>(),                                          Values = new object?[] { 1, 2, 3, 4, 5, 6 },                                            Exception = null,               ExpectedValueNames = new[] { "name1", "name2", "name3", "name4", "name5", "name6" },    ExpectedMessage = "Message 1 2 3 4 5 6"                                 }).SetName("{m}(With 6 values, with numeric values)"));

        [TestCaseSource(nameof(Otherwise_TestCaseData))]
        public void Define_Otherwise_LoggerReceivesLogFromResultInvocation(TestContext context)
        {
            context.Execute();

            context.Logger.LogInvocations.Count.ShouldBe(1);

            var logInvocation = context.Logger.LogInvocations[0];
            logInvocation.LogLevel.ShouldBe(context.LogLevel);
            logInvocation.EventId.Id.ShouldBe(context.EventId.Id);
            logInvocation.EventId.Name.ShouldBe(context.EventId.Name);

            logInvocation.State.ShouldNotBeNull();
            var state = logInvocation.State.ShouldBeAssignableTo<IReadOnlyList<KeyValuePair<string, object?>>>()!;
            state.Count.ShouldBe(context.Values.Count + 1);
            state.Select(vp => vp.Key).ShouldContain(StructuredLoggerMessageFormatter.OriginalFormatValueName);
            state.First(vp => vp.Key == StructuredLoggerMessageFormatter.OriginalFormatValueName).Value.ShouldBe(context.FormatString);

            for(var i = 0; i < context.Values.Count; ++i)
            {
                state.Select(vp => vp.Key).ShouldContain(context.ExpectedValueNames[i]);
                state.First(vp => vp.Key == context.ExpectedValueNames[i]).Value.ShouldBe(context.Values[i]);
            }

            logInvocation.Message.ShouldBe(context.ExpectedMessage);
        }
    }
}
