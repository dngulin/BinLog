# BinLog

BinLog is simple binary logging library written in C# for .NET Standard 2.0.
It is developed just for fun as a pet-project.

The library is designed for intensive logging with minimal memory allocations.
The library provides API to implement intensive zero-allocation logging.

## Usage

### Setup loggers

0. Create an `enum $ChannelIdEnum : ushort` representing each logger (log channel) in application.
See [LoggerId](BinLog.Tests/Impl/Loggers/LoggerId.cs).
0. For each logger create an `enum $MessageIdEnum : ushort` representing each log message id.
0. Annotate each enum member `$MessageIdEnum` with the `Description` attribute.
You can use format strings in description: `[Description("Log value: {0};")]`
0. Specify enum member numeric values. It is useful in cases when you will remove some loggers or messages in future.
0. Create specific `Logger<TChannelId, TMessageId>` subclasses using that enums.
See [FooLogger](BinLog.Tests/Impl/Loggers/FooLogger.cs) and [BarLogger](BinLog.Tests/Impl/Loggers/BarLogger.cs) implementations.

### Arguments logging

You can pass up to 4 arguments in log entry.
The message description string should be matching format string (with the same argument count).

Each argument should be represented with a serialization wrapper inherited form `ILoggableValue`.
For primitive types you can use the `ForLog()` extension method to make arguments loggable.

For custom types you should implement your own serialization wrapper and create `ForLog()` extension method.
See [CustomStructLoggable](BinLog.Tests/Impl/CustomStructLoggable.cs) and [CustomTypeId](BinLog.Tests/Impl/CustomTypeId.cs).

## TODO

- Serialization/deserialization buffer reallocation (if a log entry is too big)
- Multithread logging