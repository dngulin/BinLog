using System;
using BinLog.Serialization;

namespace BinLog.Primitives {
  public readonly unsafe struct LoggablePrimitive<T> : ILoggableValue where T : unmanaged {
    public readonly T Value;

    public LoggablePrimitive(T value) {
      Value = value;
    }

    public object Unwrap() => Value;

    public int SizeOf() => sizeof(T);

    public int WriteTo(Span<byte> dst) {
      int bytesWritten;

      switch (Value) {
        case bool value:
          bytesWritten = dst.Write((ushort) PrimitiveTypeId.Bool);
          return bytesWritten + dst.Slice(bytesWritten).Write(value);

        case byte value:
          bytesWritten = dst.Write((ushort) PrimitiveTypeId.Byte);
          return bytesWritten + dst.Slice(bytesWritten).Write(value);

        case short value:
          bytesWritten = dst.Write((ushort) PrimitiveTypeId.I16);
          return bytesWritten + dst.Slice(bytesWritten).Write(value);

        case ushort value:
          bytesWritten = dst.Write((ushort) PrimitiveTypeId.U16);
          return bytesWritten + dst.Slice(bytesWritten).Write(value);

        case int value:
          bytesWritten = dst.Write((ushort) PrimitiveTypeId.I32);
          return bytesWritten + dst.Slice(bytesWritten).Write(value);

        case uint value:
          bytesWritten = dst.Write((ushort) PrimitiveTypeId.U32);
          return bytesWritten + dst.Slice(bytesWritten).Write(value);

        case long value:
          bytesWritten = dst.Write((ushort) PrimitiveTypeId.I64);
          return bytesWritten + dst.Slice(bytesWritten).Write(value);

        case ulong value:
          bytesWritten = dst.Write((ushort) PrimitiveTypeId.U64);
          return bytesWritten + dst.Slice(bytesWritten).Write(value);

        case float value:
          bytesWritten = dst.Write((ushort) PrimitiveTypeId.F32);
          return bytesWritten + dst.Slice(bytesWritten).Write(value);

        case double value:
          bytesWritten = dst.Write((ushort) PrimitiveTypeId.F64);
          return bytesWritten + dst.Slice(bytesWritten).Write(value);
      }

      throw null;
    }
  }
}