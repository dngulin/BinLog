using System;
using System.IO;
using System.Linq;
using BinLog.Decoding;
using BinLog.Exceptions;
using BinLog.Primitives;
using BinLog.Serialization;
using BinLog.Tests.Impl.Decoding;
using BinLog.Tests.Impl.Loggers;
using Xunit;

namespace BinLog.Tests {
  public class CorruptedDataTests {
    private readonly byte[] _data;
    private readonly MemoryStream _stream;

    private readonly FooLogger _fooLogger;
    private readonly LogDecoder _logDecoder;

    public CorruptedDataTests() {
      _data = new byte[1024];
      _stream = new MemoryStream(_data);
      _fooLogger = new FooLogger(_stream, new byte[128]);
      _logDecoder = new LogDecoder(128, new IChannelDecoder[] {new FooDecoder(new CustomArgumentDecoder())});
    }

    [Fact]
    public void BadEntryLengthTest() {
      _fooLogger.Log(LogLevel.Info, FooMsgId.Foo1, long.MaxValue.ForLog());

      _data.AsSpan().Write((ushort)42); // EntryLength
      _stream.Seek(0, SeekOrigin.Begin);

      Assert.Throws<BinLogDecodingException>(() => _logDecoder.Decode(_stream).First());
    }

    [Fact]
    public void BadArgCountTest() {
      _fooLogger.Log(LogLevel.Info, FooMsgId.Foo2, 1.ForLog(), 2.ForLog());

      _data.AsSpan().Slice(7).Write((byte)1); // ArgCount
      _stream.Seek(0, SeekOrigin.Begin);

      Assert.Throws<BinLogDecodingException>(() => _logDecoder.Decode(_stream).First());
    }

    [Fact]
    public void BadArgTypeTest() {
      _fooLogger.Log(LogLevel.Info, FooMsgId.Foo1, 1.ForLog());

      _data.AsSpan().Slice(12).Write((ushort)12345); // ArgType
      _stream.Seek(0, SeekOrigin.Begin);

      Assert.Throws<BinLogDecodingException>(() => _logDecoder.Decode(_stream).First());
    }
  }
}