using System;
using System.Collections.Generic;
using System.Linq;

using NUnit.Framework;
using Shouldly;

namespace Microsoft.Extensions.Logging.Structured.Test
{
    [TestFixture]
    public class StructuredLoggerMessageFormatterTests
    {
        public static IEnumerable<TestCaseData> FormatStringIsMalformed_TestCaseData
            => Enumerable.Empty<TestCaseData>()
                /*                          formatString,                           expectedMalformationIndex   */
                .Append(new TestCaseData(   "{",                                    1                           ).SetName("{m}(Unmatched open brace, trivial)"))
                .Append(new TestCaseData(   "One{Two}Three{Four{Five}Six",          18                          ).SetName("{m}(Unmatched open brace, non-trivial)"))
                .Append(new TestCaseData(   "One{Two}Three{Four{{Five{Six}Seven",   24                          ).SetName("{m}(Unmatched open brace, followed by escaped open brace)"))
                .Append(new TestCaseData(   "One{Two}Three{Four}}Five{Six}Seven",   24                          ).SetName("{m}(Unmatched open brace, followed by escaped close brace)"))
                .Append(new TestCaseData(   "One{Two}Three{{Four{Five{Six}Seven",   24                          ).SetName("{m}(Unmatched open brace, preceeded by escaped open brace)"))
                .Append(new TestCaseData(   "One{Two}Three}}Four{Five{Six}Seven",   24                          ).SetName("{m}(Unmatched open brace, preceeded by escaped close brace)"))
                .Append(new TestCaseData(   "}",                                    0                           ).SetName("{m}(Unmatched close brace, trivial)"))
                .Append(new TestCaseData(   "One{Two}Three}Four{Five}Six",          13                          ).SetName("{m}(Unmatched close brace, non-trivial)"))
                .Append(new TestCaseData(   "One{Two}Three}Four{{Five{Six}Seven",   13                          ).SetName("{m}(Unmatched close brace, followed by escaped open brace)"))
                .Append(new TestCaseData(   "One{Two}Three}Four}}Five{Six}Seven",   13                          ).SetName("{m}(Unmatched close brace, followed by escaped close brace)"))
                .Append(new TestCaseData(   "One{Two}Three{{Four}Five{Six}Seven",   19                          ).SetName("{m}(Unmatched close brace, preceeded by escaped open brace)"))
                .Append(new TestCaseData(   "One{Two}Three}}Four}Five{Six}Seven",   19                          ).SetName("{m}(Unmatched close brace, preceeded by escaped close brace)"))
                .Append(new TestCaseData(   "{}",                                   1                           ).SetName("{m}(Empty value name)"));

        [TestCaseSource(nameof(FormatStringIsMalformed_TestCaseData))]
        public void CreateFromFormatString_FormatStringIsMalformed_ThrowsException(
            string formatString,
            int expectedMalformationIndex)
        {
            var result = Should.Throw<MalformedFormatStringException>(() =>
            {
                var uut = StructuredLoggerMessageFormatter.CreateFromFormatString(formatString);
            });

            result.Index.ShouldBe(expectedMalformationIndex);
            result.FormatString.ShouldBe(formatString);
            result.ParamName.ShouldBe("formatString");
        }

        public static IEnumerable<TestCaseData> FormatStringIsNotMalformed_TestCaseData
            => Enumerable.Empty<TestCaseData>()
                /*                          formatString,                                   expectedValueNames                  */
                .Append(new TestCaseData(   "{name}",                                       new[] { "name"                      }).SetName("{m}(1 name, trivial)"))
                .Append(new TestCaseData(   "{name1}{name2}",                               new[] { "name1", "name2"            }).SetName("{m}(2 names, trivial)"))
                .Append(new TestCaseData(   "{name1}{name2}{name3}",                        new[] { "name1", "name2", "name3"   }).SetName("{m}(3 names, trivial)"))
                .Append(new TestCaseData(   "Start{name}End",                               new[] { "name"                      }).SetName("{m}(1 name, non-trivial)"))
                .Append(new TestCaseData(   "Start{name1}Middle{name2}End",                 new[] { "name1", "name2"            }).SetName("{m}(2 names, non-trivial)"))
                .Append(new TestCaseData(   "Start{name1}Middle1{name2}Middle2{name3}End",  new[] { "name1", "name2", "name3"   }).SetName("{m}(3 names, non-trivial)"));

        [TestCaseSource(nameof(FormatStringIsNotMalformed_TestCaseData))]
        public void CreateFromFormatString_FormatStringIsNotMalformed_ResultIsExpected(
            string formatString,
            string[] expectedValueNames)
        {
            var uut = StructuredLoggerMessageFormatter.CreateFromFormatString(formatString);

            uut.OriginalFormat.ShouldBe(formatString);
            uut.ValueNames.ShouldBe(expectedValueNames, ignoreOrder: false);
        }

        public static IEnumerable<TestCaseData> NotEnoughValuesGiven_TestCaseData
            => Enumerable.Empty<TestCaseData>()
                /*                          formatString,                   valueCount  */
                .Append(new TestCaseData("{name}", 0).SetName("{m}(Not enough values, trivial)"))
                .Append(new TestCaseData("Start{name1}Middle{name2}End", 1).SetName("{m}(Not enough values, non-trivial)"));

        [TestCaseSource(nameof(NotEnoughValuesGiven_TestCaseData))]
        public void Format_NotEnoughValuesGiven_ThrowsException(
            string formatString,
            int valueCount)
        {
            var uut = StructuredLoggerMessageFormatter.CreateFromFormatString(formatString);
            var values = new object?[valueCount];

            var result = Should.Throw<ArgumentException>(() =>
            {
                var message = uut.Format(values);
            });

            result.ParamName.ShouldBe("values");
            result.Message.ShouldContain(uut.ValueNames.Count.ToString());
            result.Message.ShouldContain(valueCount.ToString());
        }

        public static IEnumerable<TestCaseData> ValueCountIsCorrect_TestCaseData
            => Enumerable.Empty<TestCaseData>()
                /*                          formatString,                       values,                                                             expectedResult                                      */
                .Append(new TestCaseData(   "",                                 Array.Empty<object?>(),                                             ""                                                  ).SetName("{m}(0 values, trivial)"))
                .Append(new TestCaseData(   "{name}",                           new object?[] { "value" },                                          "value"                                             ).SetName("{m}(1 value, trivial)"))
                .Append(new TestCaseData(   "{name1}{name2}",                   new object?[] { "value1", "value2" },                               "value1value2"                                      ).SetName("{m}(2 values, trivial)"))
                .Append(new TestCaseData(   "{name1}{name2}{name3}",            new object?[] { "value1", "value2", "value3" },                     "value1value2value3"                                ).SetName("{m}(3 values, trivial)"))
                .Append(new TestCaseData(   "This is a test",                   Array.Empty<object?>(),                                             "This is a test"                                    ).SetName("{m}(static format)"))
                .Append(new TestCaseData(   "Start {name1} Middle {name2} End", new object?[] { "value1", "value2" },                               "Start value1 Middle value2 End"                    ).SetName("{m}(non-trivial format)"))
                .Append(new TestCaseData(   "{name1} {name2} {name3}",          new object?[] { int.MinValue, ulong.MaxValue, new object()  },      "-2147483648 18446744073709551615 System.Object"    ).SetName("{m}(3 values, trivial)"))
                .Append(new TestCaseData(   "{name1}",                          new object?[] { null },                                             "null"                                              ).SetName("{m}(null value)"))
                .Append(new TestCaseData(   "",                                 new object?[] { "value" },                                          ""                                                  ).SetName("{m}(Too many values, trivial)"))
                .Append(new TestCaseData(   "Start {name1} Middle {name2} End", new object?[] { "value1", "value2", "value3", "value4", "value5" }, "Start value1 Middle value2 End"                    ).SetName("{m}(Too many values, non-trivial)"));

        [TestCaseSource(nameof(ValueCountIsCorrect_TestCaseData))]
        public void Format_ValueCountIsCorrect_ResultIsExpected(
            string      formatString,
            object?[]   values,
            string      expectedResult)
        {
            var uut = StructuredLoggerMessageFormatter.CreateFromFormatString(formatString);

            var result = uut.Format(values);

            result.ShouldBe(expectedResult);
        }
    }
}
