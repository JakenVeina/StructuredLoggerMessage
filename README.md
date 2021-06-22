
# StructuredLoggerMessage

[![License](https://img.shields.io/badge/license-MIT-blue.svg)](LICENSE)
[![Continuous Deployment](https://github.com/JakenVeina/StructuredLoggerMessage/workflows/Continuous%20Deployment/badge.svg)](https://github.com/JakenVeina/StructuredLoggerMessage/actions?query=workflow%3A%22Continuous+Deployment%22)
[![NuGet](https://img.shields.io/nuget/v/StructuredLoggerMessage.svg)](https://www.nuget.org/packages/StructuredLoggerMessage/)

A library for high-performance structural logging, utilizing the [.NET Extensions Logging Framework](https://docs.microsoft.com/en-us/dotnet/core/extensions/logging?tabs=command-line), and modeled after the [.NET API for high-performance logging](https://docs.microsoft.com/en-us/dotnet/core/extensions/high-performance-logging). The primary difference between the .NET `LoggerMessage` API, and the `StructuralLoggerMessage` API is that it allows for consumers to log data values for use by structural loggers, without requiring that those values be part of the format string used by text loggers.

## Usage

The `StructuralLoggerMessage` API allows consumers to define specific log messages ahead-of-time, in the form of `Action` delegates that are type-safe and include cached calculation results for optimizing text generation of log messages.

```cs
public static readonly Action<ILogger, string, int, string?> DataValueChanged
    = StructuredLoggerMessage.Define<string, int, string?>(
        LogLevel.Debug,
        new EventId(1001, nameof(DataValueChanged)),
        "{DataValueName} Changed",
        "DataValue",
        "ChangedBy");
```

This allows a consumer to invoke...

```cs
DataValueChanged.Invoke(logger, "MyValue", 7, "me");
```

...to produce the text log message...

```
MyValue Changed
```

...and a structural log state consisting of...

```cs
{
    ["DataValueName"] => "MyValue",
    ["DataValue"] => 7,
    ["ChangedBy"] => "me"
    ["{OriginalFormat}"] => "{DataValueName} Changed"
}
```

This is equivalent to a call to the corresponding Microsoft API...

```cs
public static readonly Action<ILogger, string, int, string?> DataValueChanged
    = LoggerMessage.Define<string, int, string?>(
        LogLevel.Debug,
        new EventId(1001, nameof(DataValueChanged)),
        "{DataValueName} Changed: {DataValue} {ChangedBy}");
```

...except, as you can see, not all logged values must be embedded in the format string of the log message.
