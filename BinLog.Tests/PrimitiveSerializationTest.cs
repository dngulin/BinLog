using System;
using System.Diagnostics.CodeAnalysis;
using BinLog.Serialization;
using Xunit;

namespace BinLog.Tests {
  public class PrimitiveSerializationTest {
    private readonly byte[] _buffer = new byte[sizeof(ulong)];
    private Span<byte> BufferWriteable => new Span<byte>(_buffer);
    private ReadOnlySpan<byte> BufferReadable => new ReadOnlySpan<byte>(_buffer);

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void BoolSerializationTest(bool inValue) {
      var wLen = BufferWriteable.Write(inValue);
      var rLen = BufferReadable.Read(out bool outValue);
      Assert.True(wLen == rLen);
      Assert.True(inValue == outValue);
    }

    [Theory]
    [InlineData(byte.MaxValue)]
    [InlineData(byte.MinValue)]
    [InlineData(byte.MaxValue / 2)]
    [InlineData(byte.MaxValue / 3)]
    [InlineData(byte.MaxValue / 17)]
    public void ByteSerializationTest(byte inValue) {
      var wLen = BufferWriteable.Write(inValue);
      var rLen = BufferReadable.Read(out byte outValue);
      Assert.True(wLen == rLen);
      Assert.True(inValue == outValue);
    }

    [Theory]
    [InlineData(short.MaxValue)]
    [InlineData(short.MinValue)]
    [InlineData(short.MaxValue / 17)]
    [InlineData(short.MinValue / 31)]
    public void I16SerializationTest(short inValue) {
      var wLen = BufferWriteable.Write(inValue);
      var rLen = BufferReadable.Read(out short outValue);
      Assert.True(wLen == rLen);
      Assert.True(inValue == outValue);
    }

    [Theory]
    [InlineData(ushort.MaxValue)]
    [InlineData(ushort.MinValue)]
    [InlineData(ushort.MaxValue / 17)]
    [InlineData(ushort.MaxValue / 31)]
    public void U16SerializationTest(ushort inValue) {
      var wLen = BufferWriteable.Write(inValue);
      var rLen = BufferReadable.Read(out ushort outValue);
      Assert.True(wLen == rLen);
      Assert.True(inValue == outValue);
    }

    [Theory]
    [InlineData(int.MaxValue)]
    [InlineData(int.MinValue)]
    [InlineData(int.MaxValue / 17)]
    [InlineData(int.MinValue / 31)]
    public void I32SerializationTest(int inValue) {
      var wLen = BufferWriteable.Write(inValue);
      var rLen = BufferReadable.Read(out int outValue);
      Assert.True(wLen == rLen);
      Assert.True(inValue == outValue);
    }

    [Theory]
    [InlineData(uint.MaxValue)]
    [InlineData(uint.MinValue)]
    [InlineData(uint.MaxValue / 17)]
    [InlineData(uint.MaxValue / 31)]
    public void U32SerializationTest(uint inValue) {
      var wLen = BufferWriteable.Write(inValue);
      var rLen = BufferReadable.Read(out uint outValue);
      Assert.True(wLen == rLen);
      Assert.True(inValue == outValue);
    }

    [Theory]
    [InlineData(long.MaxValue)]
    [InlineData(long.MinValue)]
    [InlineData(long.MaxValue / 17)]
    [InlineData(long.MinValue / 31)]
    public void I64SerializationTest(long inValue) {
      var wLen = BufferWriteable.Write(inValue);
      var rLen = BufferReadable.Read(out long outValue);
      Assert.True(wLen == rLen);
      Assert.True(inValue == outValue);
    }

    [Theory]
    [InlineData(ulong.MaxValue)]
    [InlineData(ulong.MinValue)]
    [InlineData(ulong.MaxValue / 17)]
    [InlineData(ulong.MaxValue / 31)]
    public void U64SerializationTest(ulong inValue) {
      var wLen = BufferWriteable.Write(inValue);
      var rLen = BufferReadable.Read(out ulong outValue);
      Assert.True(wLen == rLen);
      Assert.True(inValue == outValue);
    }

    [Theory]
    [InlineData(float.MaxValue)]
    [InlineData(float.MinValue)]
    [InlineData(float.NegativeInfinity)]
    [InlineData(float.PositiveInfinity)]
    [InlineData(float.NaN)]
    [InlineData(MathF.PI)]
    [InlineData(MathF.E)]
    [SuppressMessage("ReSharper", "CompareOfFloatsByEqualityOperator")]
    public void F32SerializationTest(float inValue) {
      var wLen = BufferWriteable.Write(inValue);
      var rLen = BufferReadable.Read(out float outValue);
      Assert.True(wLen == rLen);

      Assert.True(float.IsNaN(inValue) == float.IsNaN(outValue));
      if (!float.IsNaN(inValue))
        Assert.True(inValue == outValue);
    }

    [Theory]
    [InlineData(double.MaxValue)]
    [InlineData(double.MinValue)]
    [InlineData(double.NegativeInfinity)]
    [InlineData(double.PositiveInfinity)]
    [InlineData(double.NaN)]
    [InlineData(Math.PI)]
    [InlineData(Math.E)]
    [SuppressMessage("ReSharper", "CompareOfFloatsByEqualityOperator")]
    public void F64SerializationTest(double inValue) {
      var wLen = BufferWriteable.Write(inValue);
      var rLen = BufferReadable.Read(out double outValue);
      Assert.True(wLen == rLen);

      Assert.True(double.IsNaN(inValue) == double.IsNaN(outValue));
      if (!double.IsNaN(inValue))
        Assert.True(inValue == outValue);
    }
  }
}