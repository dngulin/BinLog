using System;
using System.Buffers.Binary;
using System.Text;

namespace BinLog.Serialization {
  public static class SpanWriteExtensions {
    public static int Write(this Span<byte> span, byte value) {
      span[0] = value;
      return sizeof(byte);
    }

    public static int Write(this Span<byte> span, short value) {
      BinaryPrimitives.WriteInt16LittleEndian(span, value);
      return sizeof(short);
    }

    public static int Write(this Span<byte> span, ushort value) {
      BinaryPrimitives.WriteUInt16LittleEndian(span, value);
      return sizeof(ushort);
    }

    public static int Write(this Span<byte> span, int value) {
      BinaryPrimitives.WriteInt32LittleEndian(span, value);
      return sizeof(int);
    }

    public static int Write(this Span<byte> span, uint value) {
      BinaryPrimitives.WriteUInt32LittleEndian(span, value);
      return sizeof(uint);
    }

    public static int Write(this Span<byte> span, long value) {
      BinaryPrimitives.WriteInt64LittleEndian(span, value);
      return sizeof(long);
    }

    public static int Write(this Span<byte> span, ulong value) {
      BinaryPrimitives.WriteUInt64LittleEndian(span, value);
      return sizeof(ulong);
    }

    public static unsafe int Write(this Span<byte> span, float value) {
      return span.Write(*(uint*) &value);
    }

    public static unsafe int Write(this Span<byte> span, double value) {
      return span.Write(*(ulong*) &value);
    }

    public static int Write(this Span<byte> span, bool value) {
      return span.Write((byte) (value ? 1 : 0));
    }

    public static int Write(this Span<byte> span, string value) {
      var strSpan = span.Slice(sizeof(ushort));
      var strSize = GetUtf8Bytes(value.AsSpan(), strSpan);
      BinaryPrimitives.WriteUInt16LittleEndian(span, (ushort)strSize);
      return strSize + sizeof(ushort);
    }

    private static unsafe int GetUtf8Bytes(ReadOnlySpan<char> chars, Span<byte> bytes) {
      fixed (char* charsPtr = &chars.GetPinnableReference())
      fixed (byte* bytesPtr = &bytes.GetPinnableReference())
        return Encoding.UTF8.GetBytes(charsPtr, chars.Length, bytesPtr, bytes.Length);
    }
  }
}