using System;
using BinLog.Primitives;
using BinLog.Serialization;

namespace BinLog.Decoding {
  public class ArgumentDecoder {
    public int Decode(ReadOnlySpan<byte> source, out object result) {
      var typeLength = source.Read(out ushort type);
      var dataLength = DecodeImpl(type, source.Slice(typeLength), out result) ?? throw null;
      return typeLength + dataLength;
    }

    protected virtual int? DecodeImpl(ushort type, ReadOnlySpan<byte> source, out object result) {
      switch ((PrimitiveTypeId) type) {
        case PrimitiveTypeId.Bool: {
          return WrapAndCast(source.Read(out bool value), value, out result);
        }
        case PrimitiveTypeId.Byte: {
          return WrapAndCast(source.Read(out byte value), value, out result);
        }
        case PrimitiveTypeId.I16: {
          return WrapAndCast(source.Read(out short value), value, out result);
        }
        case PrimitiveTypeId.U16: {
          return WrapAndCast(source.Read(out ushort value), value, out result);
        }
        case PrimitiveTypeId.I32: {
          return WrapAndCast(source.Read(out int value), value, out result);
        }
        case PrimitiveTypeId.U32: {
          return WrapAndCast(source.Read(out uint value), value, out result);
        }
        case PrimitiveTypeId.I64: {
          return WrapAndCast(source.Read(out long value), value, out result);
        }
        case PrimitiveTypeId.U64: {
          return WrapAndCast(source.Read(out ulong value), value, out result);
        }
        case PrimitiveTypeId.F32: {
          return WrapAndCast(source.Read(out float value), value, out result);
        }
        case PrimitiveTypeId.F64: {
          return WrapAndCast(source.Read(out double value), value, out result);
        }
        case PrimitiveTypeId.String: {
          return WrapAndCast(source.Read(out string value), value, out result);
        }
        default:
          result = null;
          return null;
      }
    }

    private static int WrapAndCast<T>(int bytesRead, T value, out object casted) {
      casted = value;
      return bytesRead;
    }
  }
}