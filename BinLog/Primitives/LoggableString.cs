using System;
using System.Text;
using BinLog.Serialization;

namespace BinLog.Primitives {
  public readonly struct LoggableString : ILoggableValue {
    public readonly string Value;

    public LoggableString(string value) {
      Value = value;
    }

    public object Unwrap() => Value;

    public int SizeOf() => sizeof(ushort) + Encoding.UTF8.GetByteCount(Value);

    public int WriteTo(Span<byte> dst) {
      var bytesWritten = dst.Write((byte) PrimitiveTypeId.String);
      return bytesWritten + dst.Slice(bytesWritten).Write(Value);
    }
  }
}