using System;
using BinLog.Exceptions;
using BinLog.Serialization;

namespace BinLog.Internal {
  internal readonly struct EntryHeader : ILoggable {
    public readonly ushort EntryLength;
    public readonly ushort ChannelId;
    public readonly ushort MessageId;
    public readonly byte LogLevel;
    public readonly byte ArgCount;
    private readonly uint _timeStamp;

    public const ushort Size = sizeof(ushort) * 3 + sizeof(byte) * 2 + sizeof(uint);

    public DateTime UtcDateTime => DateTimeOffset.FromUnixTimeSeconds(_timeStamp).UtcDateTime;

    public EntryHeader(ushort entryLength, ushort channelId, ushort messageId, LogLevel logLevel, byte argCount) {
      EntryLength = entryLength;
      ChannelId = channelId;
      MessageId = messageId;
      LogLevel = (byte)logLevel;
      ArgCount = argCount;
      _timeStamp = unchecked((uint)((DateTimeOffset) DateTime.UtcNow).ToUnixTimeSeconds());
    }

    public EntryHeader(ReadOnlySpan<byte> src) {
      var bytesRead = src.Read(out EntryLength);
      bytesRead += src.Slice(bytesRead).Read(out ChannelId);
      bytesRead += src.Slice(bytesRead).Read(out MessageId);
      bytesRead += src.Slice(bytesRead).Read(out LogLevel);
      bytesRead += src.Slice(bytesRead).Read(out ArgCount);
      bytesRead += src.Slice(bytesRead).Read(out _timeStamp);

      if (bytesRead != Size)
        throw new BinLogSerializationException($"Failed to deserialize {nameof(EntryHeader)}");
    }

    public int SizeOf() => Size;

    public int WriteTo(Span<byte> dst) {
      var bytesWritten = dst.Write(EntryLength);
      bytesWritten += dst.Slice(bytesWritten).Write(ChannelId);
      bytesWritten += dst.Slice(bytesWritten).Write(MessageId);
      bytesWritten += dst.Slice(bytesWritten).Write(LogLevel);
      bytesWritten += dst.Slice(bytesWritten).Write(ArgCount);
      bytesWritten += dst.Slice(bytesWritten).Write(_timeStamp);

      if (bytesWritten != Size)
        throw new BinLogSerializationException($"Failed to serialize {nameof(EntryHeader)}");

      return bytesWritten;
    }
  }
}