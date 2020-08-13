using System.IO;
using System.Linq;
using BinLog.Decoding;
using BinLog.Exceptions;
using BinLog.Tests.Impl.Loggers;
using Xunit;

namespace BinLog.Tests {
  public class BadUsageTests {
    private enum BadLoggerId : byte { Sample }
    private enum BadMsgId : uint {}

    private class LoggerWithWrongChId : Logger<BadLoggerId, FooMsgId> {
      public LoggerWithWrongChId() : base(BadLoggerId.Sample, null, null) { }
    }

    private class LoggerWithWrongMsgId : Logger<LoggerId, BadMsgId> {
      public LoggerWithWrongMsgId() : base(LoggerId.Foo, null, null) { }
    }

    private class DecoderWithWrongChId : ChannelDecoder<BadLoggerId, FooMsgId> {
      public DecoderWithWrongChId() : base(BadLoggerId.Sample, null) { }
    }

    private class DecoderWithWrongMsgId : ChannelDecoder<BadLoggerId, FooMsgId> {
      public DecoderWithWrongMsgId() : base(BadLoggerId.Sample, null) { }
    }

    private class MockStream : Stream {
      public override bool CanRead => false;
      public override bool CanWrite => false;
      public override bool CanSeek => false;
      public override long Length => default;
      public override long Position { get; set; }

      public override void Flush() { }
      public override int Read(byte[] buffer, int offset, int count) => default;
      public override long Seek(long offset, SeekOrigin origin) => default;
      public override void SetLength(long value) { }
      public override void Write(byte[] buffer, int offset, int count) { }
    }

    [Fact]
    public void BadEnumSizeTest() {
      Assert.Throws<BinLogException>(() => new LoggerWithWrongChId());
      Assert.Throws<BinLogException>(() => new LoggerWithWrongMsgId());
      Assert.Throws<BinLogException>(() => new DecoderWithWrongChId());
      Assert.Throws<BinLogException>(() => new DecoderWithWrongMsgId());
    }

    [Fact]
    public void BadStreamTest() {
      Assert.Throws<BinLogException>(() => new FooLogger(new MockStream(), new byte[16]));

      var emptyDecoder = new LogDecoder(42, new IChannelDecoder[] {});
      Assert.Throws<BinLogException>(() => emptyDecoder.Decode(new MockStream()).First());
    }
  }
}