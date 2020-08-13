using System;
using System.IO;
using System.Linq;
using BinLog.Decoding;
using BinLog.Exceptions;
using BinLog.Primitives;
using BinLog.Tests.Impl;
using BinLog.Tests.Impl.Decoding;
using BinLog.Tests.Impl.Loggers;
using Xunit;

namespace BinLog.Tests {
  public class EncodeDecodeTests {
    private readonly MemoryStream _stream = new MemoryStream(1024);
    private readonly FooLogger _fooLogger;
    private readonly BarLogger _barLogger;

    private readonly FooDecoder _fooDecoder;
    private readonly BarDecoder _barDecoder;
    private readonly LogDecoder _logDecoder;

    public EncodeDecodeTests() {
      var buffer = new byte[128];
      _fooLogger = new FooLogger(_stream, buffer);
      _barLogger = new BarLogger(_stream, buffer);

      var argDecoder = new CustomArgumentDecoder();
      _fooDecoder = new FooDecoder(argDecoder);
      _barDecoder = new BarDecoder(argDecoder);
      _logDecoder = new LogDecoder(128, new IChannelDecoder[] {_fooDecoder, _barDecoder});
    }

    [Fact]
    public void ChannelNameTest() {
      Assert.True(_fooDecoder.ChannelName == LoggerId.Foo.ToString());
      Assert.True(_barDecoder.ChannelName == LoggerId.Bar.ToString());
    }

    [Fact]
    public void NoArgsEncodeTest() {
      _fooLogger.Log(LogLevel.Info, FooMsgId.Foo);
      _barLogger.Log(LogLevel.Warning, BarMsgId.HelloWorld);

      _stream.Seek(0, SeekOrigin.Begin);
      var entries = _logDecoder.Decode(_stream).ToArray();

      Assert.True(entries[0].Channel == _fooDecoder.ChannelName);
      Assert.True(entries[1].Channel == _barDecoder.ChannelName);

      Assert.True(entries[0].LogLevel == LogLevel.Info);
      Assert.True(entries[1].LogLevel == LogLevel.Warning);

      Assert.True(entries[0].Message == _fooDecoder.DecodeMessage(FooMsgId.Foo));
      Assert.True(entries[1].Message == _barDecoder.DecodeMessage(BarMsgId.HelloWorld));

      Assert.True(entries[0].DateTimeUtc <= entries[1].DateTimeUtc);
    }

    [Fact]
    public void ArgumentLoggingTest() {
      var args = new object[] {
        true,
        byte.MaxValue,
        short.MinValue,
        ushort.MaxValue,
        int.MinValue,
        uint.MaxValue,
        long.MinValue,
        ulong.MaxValue,
        MathF.PI,
        Math.PI,
        string.Empty,
        _fooDecoder.ChannelName,
        new CustomStruct(float.MaxValue / 3, float.MinValue / 3)
      };

      foreach (var arg in args) {
        _barLogger.Log(LogLevel.Error, BarMsgId.BarValueIs, BoxILoggable(arg));
      }


      _stream.Seek(0, SeekOrigin.Begin);

      var fmt = _barDecoder.DecodeMessage(BarMsgId.BarValueIs);
      var index = 0;
      foreach (var entry in _logDecoder.Decode(_stream)) {
        var message = string.Format(fmt, args[index]);
        Assert.True(entry.Message == message);
        Assert.True(entry.LogLevel == LogLevel.Error);
        index++;
      }

      Assert.True(index == args.Length);
    }

    [Fact]
    public void MultipleArgLoggingTest() {
      var arg = new[] {
        new[] {new string('a', 1)},
        new[] {new string('c', 2), new string('d', 3)},
        new[] {new string('e', 4), new string('f', 5), new string('g', 6)},
        new[] {new string('h', 7), new string('i', 8), new string('j', 9), new string('k', 10)},
      };

      var ids = new[] {
        FooMsgId.Foo1,
        FooMsgId.Foo2,
        FooMsgId.Foo3,
        FooMsgId.Foo4,
      };
      var msg = ids.Select(id => _fooDecoder.DecodeMessage(id)).ToArray();

      const LogLevel lvl = LogLevel.Info;
      _fooLogger.Log(lvl, ids[0], arg[0][0].ForLog());
      _fooLogger.Log(lvl, ids[1], arg[1][0].ForLog(), arg[1][1].ForLog());
      _fooLogger.Log(lvl, ids[2], arg[2][0].ForLog(), arg[2][1].ForLog(), arg[2][2].ForLog());
      _fooLogger.Log(lvl, ids[3], arg[3][0].ForLog(), arg[3][1].ForLog(), arg[3][2].ForLog(), arg[3][3].ForLog());

      _stream.Seek(0, SeekOrigin.Begin);
      var res = _logDecoder.Decode(_stream).Select(e => e.Message).ToArray();

      Assert.True(res[0] == string.Format(msg[0], arg[0][0]));
      Assert.True(res[1] == string.Format(msg[1], arg[1][0], arg[1][1]));
      Assert.True(res[2] == string.Format(msg[2], arg[2][0], arg[2][1], arg[2][2]));
      Assert.True(res[3] == string.Format(msg[3], arg[3][0], arg[3][1], arg[3][2], arg[3][3]));
    }

    [Fact]
    public void UnknownArgumentDecodingTest() {
      _barLogger.Log(LogLevel.Error, BarMsgId.BarValueIs, new UnknownStructLoggable());
      _stream.Seek(0, SeekOrigin.Begin);
      Assert.Throws<BinLogDecodingException>(() => _logDecoder.Decode(_stream).First());
    }

    private static ILoggableValue BoxILoggable(object arg) {
      switch (arg) {
        case bool value: return value.ForLog();
        case byte value: return value.ForLog();
        case short value: return value.ForLog();
        case ushort value: return value.ForLog();
        case int value: return value.ForLog();
        case uint value: return value.ForLog();
        case long value: return value.ForLog();
        case ulong value: return value.ForLog();
        case float value: return value.ForLog();
        case double value: return value.ForLog();
        case string value: return value.ForLog();
        case CustomStruct value: return value.ForLog();
      }
      throw new InvalidOperationException();
    }
  }
}