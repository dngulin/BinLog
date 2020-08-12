using System;
using System.Text;
using BinLog.Serialization;

namespace BinLog.Primitives {
  public readonly struct LoggableString : ILoggableValue {
    public readonly string Value;

    public LoggableString(string value) => Value = value;

    public object Unwrap() => Value;

    public int SizeOf() => sizeof(ushort) * 2 + Encoding.UTF8.GetByteCount(Value ?? string.Empty);

    public int WriteTo(Span<byte> dst) {
      var bytesWritten = dst.Write((ushort) PrimitiveTypeId.String);
      return bytesWritten + dst.Slice(bytesWritten).Write(Value ?? string.Empty);
    }
  }
}