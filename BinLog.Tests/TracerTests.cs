using System.IO;
using System.Linq;
using BinLog.Decoding;
using BinLog.Primitives;
using BinLog.Tests.Impl.Decoding;
using BinLog.Tests.Impl.Loggers;
using Xunit;

namespace BinLog.Tests {
  public class TracerTests {
    private readonly MemoryStream _stream;

    private readonly SampleTracer _tracer;
    private readonly FooLogger _logger;
    private readonly LogDecoder _decoder;

    public TracerTests() {
      _stream = new MemoryStream(1024);

      _tracer = new SampleTracer();
      _logger = new FooLogger(_stream, new byte[64], _tracer);
      _decoder = new LogDecoder(128, new IChannelDecoder[] {new FooDecoder(new CustomArgumentDecoder())});
    }

    [Fact]
    public void TracerMessagesTest() {
      const LogLevel lvl = LogLevel.Info;

      _logger.Log(lvl, FooMsgId.Foo);
      _logger.Log(lvl, FooMsgId.Foo1, 1.ForLog());
      _logger.Log(lvl, FooMsgId.Foo2, 1.ForLog(), 2.ForLog());
      _logger.Log(lvl, FooMsgId.Foo3, 1.ForLog(), 2.ForLog(), 3.ForLog());
      _logger.Log(lvl, FooMsgId.Foo4, 1.ForLog(), 2.ForLog(), 3.ForLog(), 4.ForLog());

      _stream.Seek(0, SeekOrigin.Begin);
      var messages = _decoder.Decode(_stream).Select(e => e.Message).ToArray();

      for (var i = 0; i < messages.Length; i++) {
        Assert.True(messages[i] == _tracer.Messages[i]);
      }
    }
  }
}