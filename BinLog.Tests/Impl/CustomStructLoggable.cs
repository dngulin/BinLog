using System;
using BinLog.Serialization;

namespace BinLog.Tests.Impl {
  public readonly struct CustomStructLoggable : ILoggableValue {
    public readonly CustomStruct Value;

    public CustomStructLoggable(CustomStruct value) => Value = value;

    public int SizeOf() => sizeof(ushort) + CustomStruct.SerializedSize;

    public object Unwrap() => Value;

    public int WriteTo(Span<byte> dst) {
      var bytesWritten = dst.Write((ushort) CustomTypeId.CustomStruct);
      bytesWritten += dst.Slice(bytesWritten).Write(Value.X);
      bytesWritten += dst.Slice(bytesWritten).Write(Value.Y);
      return bytesWritten;
    }

    public static int DecodeValue(ReadOnlySpan<byte> src, out object value) {
      var bytesRead = src.Read(out float x);
      bytesRead += src.Slice(bytesRead).Read(out float y);

      value = new CustomStruct(x, y);
      return bytesRead;
    }
  }

  public static class CustomStructForLogExtension {
    public static CustomStructLoggable ForLog(this CustomStruct value) => new CustomStructLoggable(value);
  }
}