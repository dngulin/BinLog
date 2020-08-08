using System;
using BinLog.Exceptions;
using BinLog.Serialization;
using Xunit;

namespace BinLog.Tests {
  public class StringSerializationTests {
    private readonly byte[] _buffer = new byte[128];
    private Span<byte> BufferWriteable => new Span<byte>(_buffer);
    private ReadOnlySpan<byte> BufferReadable => new ReadOnlySpan<byte>(_buffer);

    [Fact]
    public void NullStringSerializationTest() {
      Assert.Throws<BinLogSerializationException>(() => BufferWriteable.Write(null));
    }

    [Theory]
    [InlineData("")]
    [InlineData("ASCII string")]
    [InlineData("UTF-8 строка")]
    public void StringSerializationTest(string inValue) {
      var wLen = BufferWriteable.Write(inValue);
      var rLen = BufferReadable.Read(out string outValue);
      Assert.True(wLen == rLen);
      Assert.True(inValue == outValue);
    }
  }
}