using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using BinLog.Internal;

namespace BinLog.Decoding {
  public class DecodeEnumerable : IEnumerable<LogEntry> {
    private readonly Stream _stream;
    private readonly byte[] _buffer;
    private readonly Dictionary<ushort, IChannelDecoder> _decoders;

    public DecodeEnumerable(Stream stream, byte[] buffer, Dictionary<ushort, IChannelDecoder> decoders) {
      _stream = stream;
      _buffer = buffer;
      _decoders = decoders;
    }

    public IEnumerator<LogEntry> GetEnumerator() => new DecodeEnumerator(_stream, _buffer, _decoders);

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
  }

  public class DecodeEnumerator : IEnumerator<LogEntry> {
    private readonly Stream _stream;
    private readonly byte[] _buffer;
    private readonly Dictionary<ushort, IChannelDecoder> _decoders;

    private readonly List<object> _currentArgs = new List<object>(4);

    private LogEntry _current;
    public LogEntry Current => _current;
    object IEnumerator.Current => _current;

    public DecodeEnumerator(Stream stream, byte[] buffer, Dictionary<ushort, IChannelDecoder> decoders) {
      _stream = stream;
      _buffer = buffer;
      _decoders = decoders;

      if (!_stream.CanRead)
        throw null;
    }

    public bool MoveNext() => ReadEntry(_stream, out _current);

    public void Dispose() {}

    public void Reset() => throw new NotSupportedException();

    private bool ReadEntry(Stream stream, out LogEntry entry) {
      _currentArgs.Clear();

      entry = default;

      var bytesRead = stream.Read(_buffer, 0, EntryHeader.Size);
      if (bytesRead == 0)
        return false;

      if (bytesRead != EntryHeader.Size)
        throw null;

      var header = new EntryHeader(new ReadOnlySpan<byte>(_buffer, 0, EntryHeader.Size));
      entry.LogLevel = (LogLevel) header.LogLevel;

      if (!_decoders.TryGetValue(header.ChannelId, out var decoder))
        throw null;

      entry.Channel = decoder.ChannelName;

      if (header.EntryLength < bytesRead) {
        var remains = header.EntryLength - EntryHeader.Size;
        bytesRead += stream.Read(_buffer, 0, remains);

        if (bytesRead != header.EntryLength)
          throw null;

        DecodeArguments(header.ArgCount, new ReadOnlySpan<byte>(_buffer, 0, remains), decoder, _currentArgs);
      }

      entry.Message = DecodeMessage(header.MessageId, decoder, _currentArgs);
      return true;
    }

    private static void DecodeArguments(int count, ReadOnlySpan<byte> src, IChannelDecoder decoder, List<object> args) {
      var bytesRead = 0;

      for (var i = 0; i < count; i++) {
        bytesRead += decoder.DecodeArgument(src.Slice(bytesRead), out var arg);
        args.Add(arg);
      }
    }

    private static string DecodeMessage(ushort msgId, IChannelDecoder decoder, List<object> args) {
      var message = decoder.DecodeMessage(msgId);

      switch (args.Count) {
        case 0:
          return message;
        case 1:
          return string.Format(message, args[0]);
        case 2:
          return string.Format(message, args[0], args[1]);
        case 3:
          return string.Format(message, args[0], args[1], args[2]);
        case 4:
          return string.Format(message, args[0], args[1], args[2], args[3]);
        default:
          throw null;
      }
    }
  }
}