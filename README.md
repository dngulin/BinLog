# BinLog

BinLog is a simple binary logging library written in C# for .NET Standard 2.0.
The library is designed for logging with minimum (up to zero) memory allocations and producing very compact logs.
It is developed just for fun as a pet-project.

## Usage

As an simple usage example you can use [EncodeDecodeTests.cs](BinLog.Tests/EncodeDecodeTests.cs)
and [TracerTests.cs](BinLog.Tests/TracerTests.cs).

### Setup loggers

Create `Logger<TChannelEnum, TMessageEnum>` subclass for each logger (log channel) in application.

- The `TChannelEnum : ushort` should be _unique for the application_ and represent all logging domains (log channels).
- The `TMessageEnum : ushort` should be _unique for each logger_ and represent all logger-specific messages.
Annotate each value in that enums with the `Description` attribute to set string representation of message.

You can see sample implementation here:
[LoggerId.cs](BinLog.Tests/Impl/Loggers/LoggerId.cs),
[FooLogger.cs](BinLog.Tests/Impl/Loggers/FooLogger.cs),
[BarLogger.cs](BinLog.Tests/Impl/Loggers/BarLogger.cs).

### Arguments logging

You can pass up to 4 arguments in log entry.
The message description string should be matching format string (with the same argument count).

Each argument should be represented with a serialization wrapper inherited form `ILoggableValue`.
For primitive types you can use the `ForLog()` extension method to make arguments loggable.

For custom types you should implement your own serialization wrapper and create `ForLog()` extension method.
Each serialized argument should have unique id with the `ushort` representation.
See [CustomStructLoggable.cs](BinLog.Tests/Impl/CustomStructLoggable.cs) and [CustomTypeId.cs](BinLog.Tests/Impl/CustomTypeId.cs).

### Realtime logging

If you want to see log messages in realtime, you can pass an `LogTracer` subclass into the target logger constructor.

This feature is designed only for debugging purposes and can cause intensive memory allocations.
The `LogTracer` internally uses `string.Format`, that caused string allocations and value type boxing.

### Setup decoding

Implement corresponding `ChannelDecoder<TChannelEnum, TMessageEnum>` for each logger.
Create an instance of the `LogDecoder` using an array of channel decoders and use `Decode` method for reading the log data.

If you have custom loggable types, create subclass of the `ArgumentDecoder` and pass it into the `ChannelDecoder` constructor.  

For example see
[CustomArgumentDecoder.cs](BinLog.Tests/Impl/Decoding/CustomArgumentDecoder.cs),
[FooDecoder.cs](BinLog.Tests/Impl/Decoding/FooDecoder.cs),
[BarDecoder.cs](BinLog.Tests/Impl/Decoding/BarDecoder.cs).

### Limitations

- `TChannelEnum` and `TMessageEnum` should be inherited from `ushort`.
- Currently the argument log argument count is limited by 4 (by API). Technically this value can be increased up to 256.   
- Null string serialization/deserialization is not possible. The `LoggableString` struct uses empty string instead of `null`.
- Serialization/deserialization buffer has fixed size. If log entry is too big a new buffer will be allocated.

### Best practices

- Specify enum member numeric values. It is useful in cases when you will remove some loggers or messages in future.
- Use structs for `ILoggableValue` implementations to avoid unnecessary memory allocations.
- Do not delete obsolete decoders from codebase. Preserve it for old log files.
- Use realtime logging only in debug environment.

## TODO

- Roslyn analyzers
- Multithread logging