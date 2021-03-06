using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using BinLog.Exceptions;
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
    private byte[] _buffer;
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
        throw new BinLogException("Stream doesn't support reading");
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
        throw new BinLogDecodingException("Invalid decoding stream length");

      var header = new EntryHeader(_buffer.AsSpan(0, EntryHeader.Size));
      if (_buffer.Length < header.EntryLength)
        _buffer = new byte[header.EntryLength];

      entry.LogLevel = (LogLevel) header.LogLevel;
      entry.DateTimeUtc = header.DateTimeUtc;

      if (!_decoders.TryGetValue(header.ChannelId, out var decoder))
        throw new BinLogDecodingException($"Unknown channel id {header.ChannelId}");

      entry.Channel = decoder.ChannelName;

      if (bytesRead < header.EntryLength) {
        var payloadSize = header.EntryLength - EntryHeader.Size;
        bytesRead += stream.Read(_buffer, 0, payloadSize);

        if (bytesRead != header.EntryLength)
          throw new BinLogDecodingException("Invalid decoding stream length");

        var argsSize = DecodeArguments(header.ArgCount, _buffer.AsSpan(0, payloadSize), decoder, _currentArgs);
        if (argsSize != payloadSize)
          throw new BinLogDecodingException("Invalid entry length");
      }

      if (_currentArgs.Count != header.ArgCount)
        throw new BinLogDecodingException("Failed to decode args");

      entry.Message = DecodeMessage(header.MessageId, decoder, _currentArgs);
      return true;
    }

    private static int DecodeArguments(int count, ReadOnlySpan<byte> src, IChannelDecoder decoder, List<object> args) {
      var bytesRead = 0;

      for (var i = 0; i < count; i++) {
        bytesRead += decoder.DecodeArgument(src.Slice(bytesRead), out var arg);
        args.Add(arg);
      }

      return bytesRead;
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
          throw new BinLogDecodingException($"Too many arguments in log entry: {args.Count}");
      }
    }
  }
}