using System;
using BinLog.Serialization;

namespace BinLog.Internal {
  internal readonly struct EntryHeader : ILoggable {
    public readonly ushort EntryLength;
    public readonly ushort ChannelId;
    public readonly ushort MessageId;
    public readonly byte LogLevel;
    public readonly byte ArgCount;

    public const ushort Size = sizeof(ushort) * 3 + sizeof(byte) * 2;

    public EntryHeader(ushort entryLength, ushort channelId, ushort messageId, LogLevel logLevel, byte argCount) {
      EntryLength = entryLength;
      ChannelId = channelId;
      MessageId = messageId;
      LogLevel = (byte)logLevel;
      ArgCount = argCount;
    }

    public EntryHeader(ReadOnlySpan<byte> src) {

      var bytesRead = src.Read(out EntryLength);
      bytesRead += src.Slice(bytesRead).Read(out ChannelId);
      bytesRead += src.Slice(bytesRead).Read(out MessageId);
      bytesRead += src.Slice(bytesRead).Read(out LogLevel);
      bytesRead += src.Slice(bytesRead).Read(out ArgCount);

      if (bytesRead != Size)
        throw null;
    }

    public int SizeOf() => Size;

    public int WriteTo(Span<byte> dst) {
      var bytesWritten = dst.Write(EntryLength);
      bytesWritten += dst.Slice(bytesWritten).Write(ChannelId);
      bytesWritten += dst.Slice(bytesWritten).Write(MessageId);
      bytesWritten += dst.Slice(bytesWritten).Write(LogLevel);
      bytesWritten += dst.Slice(bytesWritten).Write(ArgCount);

      return bytesWritten;
    }
  }
}