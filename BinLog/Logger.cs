using System;
using System.IO;
using BinLog.Internal;

namespace BinLog {
  public abstract class Logger<TChannelEnum, TMessageEnum>
    where TChannelEnum : unmanaged
    where TMessageEnum : unmanaged {

    private readonly ushort _channelId;

    private readonly Stream _stream;
    private readonly byte[] _buffer;

    protected unsafe Logger(TChannelEnum channel, Stream stream, byte[] buffer) {
      if (sizeof(TChannelEnum) != sizeof(ushort))
        throw null;

      if (sizeof(TMessageEnum) != sizeof(ushort))
        throw null;

      if (!_stream.CanWrite)
        throw null;

      _stream = stream;
      _buffer = buffer;

      _channelId = *(ushort*) &channel;
    }

    public void Log(LogLevel level, TMessageEnum msgId) {
      var span = new Span<byte>(_buffer);
      var header = new EntryHeader(EntryHeader.Size, _channelId, MsgIdToUInt16(msgId), level, 0);
      _stream.Write(_buffer, 0, header.WriteTo(span));
    }

    public void Log<T1>(LogLevel level, TMessageEnum msgId, T1 arg1) where T1 : ILoggableValue {
      var span = new Span<byte>(_buffer);

      var length = EntryHeader.Size + arg1.SizeOf();
      var header = new EntryHeader((ushort) length, _channelId, MsgIdToUInt16(msgId), level, 1);

      var bytesWritten = header.WriteTo(span);
      bytesWritten += arg1.WriteTo(span.Slice(bytesWritten));

      _stream.Write(_buffer, 0, bytesWritten);
    }

    public void Log<T1, T2>(LogLevel level, TMessageEnum msgId, T1 arg1, T2 arg2)
      where T1 : ILoggableValue
      where T2 : ILoggableValue {
      var span = new Span<byte>(_buffer);

      var length = EntryHeader.Size + arg1.SizeOf() + arg2.SizeOf();
      var header = new EntryHeader((ushort) length, _channelId, MsgIdToUInt16(msgId), level, 2);

      var bytesWritten = header.WriteTo(span);
      bytesWritten += arg1.WriteTo(span.Slice(bytesWritten));
      bytesWritten += arg2.WriteTo(span.Slice(bytesWritten));

      _stream.Write(_buffer, 0, bytesWritten);
    }

    public void Log<T1, T2, T3>(LogLevel level, TMessageEnum msgId, T1 arg1, T2 arg2, T3 arg3)
      where T1 : ILoggableValue
      where T2 : ILoggableValue
      where T3 : ILoggableValue {
      var span = new Span<byte>(_buffer);

      var length = EntryHeader.Size + arg1.SizeOf() + arg2.SizeOf() + arg3.SizeOf();
      var header = new EntryHeader((ushort) length, _channelId, MsgIdToUInt16(msgId), level, 3);

      var bytesWritten = header.WriteTo(span);
      bytesWritten += arg1.WriteTo(span.Slice(bytesWritten));
      bytesWritten += arg2.WriteTo(span.Slice(bytesWritten));
      bytesWritten += arg3.WriteTo(span.Slice(bytesWritten));

      _stream.Write(_buffer, 0, bytesWritten);
    }

    public void Log<T1, T2, T3, T4>(LogLevel level, TMessageEnum msgId, T1 arg1, T2 arg2, T3 arg3, T4 arg4)
      where T1 : ILoggableValue
      where T2 : ILoggableValue
      where T3 : ILoggableValue
      where T4 : ILoggableValue {
      var span = new Span<byte>(_buffer);

      var length = EntryHeader.Size + arg1.SizeOf() + arg2.SizeOf() + arg3.SizeOf() + arg4.SizeOf();
      var header = new EntryHeader((ushort) length, _channelId, MsgIdToUInt16(msgId), level, 4);

      var bytesWritten = header.WriteTo(span);
      bytesWritten += arg1.WriteTo(span.Slice(bytesWritten));
      bytesWritten += arg2.WriteTo(span.Slice(bytesWritten));
      bytesWritten += arg3.WriteTo(span.Slice(bytesWritten));
      bytesWritten += arg4.WriteTo(span.Slice(bytesWritten));

      _stream.Write(_buffer, 0, bytesWritten);
    }

    private static unsafe ushort MsgIdToUInt16(TMessageEnum value) => *(ushort*) &value;
  }
}