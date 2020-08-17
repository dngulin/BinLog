using System;
using System.IO;
using System.Runtime.Serialization;
using BinLog.Exceptions;
using BinLog.Internal;

namespace BinLog {
  /// <summary>
  /// The base logger class. Create a subclass for each domain an application.
  /// </summary>
  /// <remarks>
  /// Each logger should have unique numeric channel id encoded by enum value.
  /// </remarks>
  /// <remarks>
  /// Create an unique message id enum for each logger type.
  /// Use the <c>DescriptionAttribute</c> on message id values to set message's string representation.
  /// </remarks>
  /// <typeparam name="TChannelEnum">Channel id enum. Should be inherited from ushort.</typeparam>
  /// <typeparam name="TMessageEnum">Message id enum. Should be inherited from ushort.</typeparam>
  public abstract class Logger<TChannelEnum, TMessageEnum>
    where TChannelEnum : unmanaged
    where TMessageEnum : unmanaged {
    private readonly ushort _channelId;

    private readonly Stream _stream;
    private readonly byte[] _buffer;

    private readonly LogTracer _tracer;
    private readonly string _name;

    /// <summary>
    /// Channel logger constructor.
    /// </summary>
    /// <param name="channelId">Unique logger id</param>
    /// <param name="stream">Logging stream. Should be writable.</param>
    /// <param name="buffer">Serialization buffer. Can be shared between multiple loggers (non thread safe).</param>
    /// <param name="tracer">Optional runtime message tracer. Useful in debug environment.</param>
    /// <exception cref="BinLogException">
    /// Thrown when size of <c>TChannelEnum</c> or <c>TMessageEnum</c> is invalid.
    /// </exception>
    protected Logger(TChannelEnum channelId, Stream stream, byte[] buffer, LogTracer tracer = null) {
      if (!LogEnum.CheckSizeOf<TChannelEnum>())
        throw new BinLogException($"Size of {nameof(TChannelEnum)} should be {sizeof(ushort)}");

      if (!LogEnum.CheckSizeOf<TMessageEnum>())
        throw new BinLogException($"Size of {nameof(TMessageEnum)} should be {sizeof(ushort)}");

      _stream = stream;
      _buffer = buffer;

      if (!_stream.CanWrite)
        throw new BinLogException("Stream doesn't support writing");

      _tracer = tracer;
      _name = channelId.ToString();

      _channelId = LogEnum.ToUInt16(channelId);
    }

    public void Log(LogLevel level, TMessageEnum msgId) {
      _tracer?.Trace(level, _name, LogEnum.GetMsg(msgId));

      var span = new Span<byte>(_buffer);
      var header = new EntryHeader(EntryHeader.Size, _channelId, LogEnum.ToUInt16(msgId), level, 0);
      var bytesWritten = header.WriteTo(span);

      if (bytesWritten != EntryHeader.Size)
        throw new SerializationException("Failed to serialize log entry");

      _stream.Write(_buffer, 0, bytesWritten);
    }

    public void Log<T1>(LogLevel level, TMessageEnum msgId, T1 arg1) where T1 : ILoggableValue {
      _tracer?.Trace(level, _name, LogEnum.GetMsg(msgId), arg1.Unwrap());

      var span = new Span<byte>(_buffer);

      var length = EntryHeader.Size + arg1.SizeOf();
      var header = new EntryHeader((ushort) length, _channelId, LogEnum.ToUInt16(msgId), level, 1);

      var bytesWritten = header.WriteTo(span);
      bytesWritten += arg1.WriteTo(span.Slice(bytesWritten));

      if (bytesWritten != length)
        throw new SerializationException("Failed to serialize log entry");

      _stream.Write(_buffer, 0, bytesWritten);
    }

    public void Log<T1, T2>(LogLevel level, TMessageEnum msgId, T1 arg1, T2 arg2)
      where T1 : ILoggableValue
      where T2 : ILoggableValue {
      _tracer?.Trace(level, _name, LogEnum.GetMsg(msgId), arg1.Unwrap(), arg2.Unwrap());

      var span = new Span<byte>(_buffer);

      var length = EntryHeader.Size + arg1.SizeOf() + arg2.SizeOf();
      var header = new EntryHeader((ushort) length, _channelId, LogEnum.ToUInt16(msgId), level, 2);

      var bytesWritten = header.WriteTo(span);
      bytesWritten += arg1.WriteTo(span.Slice(bytesWritten));
      bytesWritten += arg2.WriteTo(span.Slice(bytesWritten));

      if (bytesWritten != length)
        throw new SerializationException("Failed to serialize log entry");

      _stream.Write(_buffer, 0, bytesWritten);
    }

    public void Log<T1, T2, T3>(LogLevel level, TMessageEnum msgId, T1 arg1, T2 arg2, T3 arg3)
      where T1 : ILoggableValue
      where T2 : ILoggableValue
      where T3 : ILoggableValue {
      _tracer?.Trace(level, _name, LogEnum.GetMsg(msgId), arg1.Unwrap(), arg2.Unwrap(), arg3.Unwrap());

      var span = new Span<byte>(_buffer);

      var length = EntryHeader.Size + arg1.SizeOf() + arg2.SizeOf() + arg3.SizeOf();
      var header = new EntryHeader((ushort) length, _channelId, LogEnum.ToUInt16(msgId), level, 3);

      var bytesWritten = header.WriteTo(span);
      bytesWritten += arg1.WriteTo(span.Slice(bytesWritten));
      bytesWritten += arg2.WriteTo(span.Slice(bytesWritten));
      bytesWritten += arg3.WriteTo(span.Slice(bytesWritten));

      if (bytesWritten != length)
        throw new SerializationException("Failed to serialize log entry");

      _stream.Write(_buffer, 0, bytesWritten);
    }

    public void Log<T1, T2, T3, T4>(LogLevel level, TMessageEnum msgId, T1 arg1, T2 arg2, T3 arg3, T4 arg4)
      where T1 : ILoggableValue
      where T2 : ILoggableValue
      where T3 : ILoggableValue
      where T4 : ILoggableValue {
      _tracer?.Trace(level, _name, LogEnum.GetMsg(msgId), arg1.Unwrap(), arg2.Unwrap(), arg3.Unwrap(), arg4.Unwrap());

      var span = new Span<byte>(_buffer);

      var length = EntryHeader.Size + arg1.SizeOf() + arg2.SizeOf() + arg3.SizeOf() + arg4.SizeOf();
      var header = new EntryHeader((ushort) length, _channelId, LogEnum.ToUInt16(msgId), level, 4);

      var bytesWritten = header.WriteTo(span);
      bytesWritten += arg1.WriteTo(span.Slice(bytesWritten));
      bytesWritten += arg2.WriteTo(span.Slice(bytesWritten));
      bytesWritten += arg3.WriteTo(span.Slice(bytesWritten));
      bytesWritten += arg4.WriteTo(span.Slice(bytesWritten));

      if (bytesWritten != length)
        throw new SerializationException("Failed to serialize log entry");

      _stream.Write(_buffer, 0, bytesWritten);
    }
  }
}