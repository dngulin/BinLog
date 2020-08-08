using System;
using System.Buffers.Binary;
using System.Text;

namespace BinLog.Serialization {
  public static class SpanReadExtensions {
    public static int Read(this ReadOnlySpan<byte> span, out byte value) {
      value = span[0];
      return sizeof(byte);
    }

    public static int Read(this ReadOnlySpan<byte> span, out short value) {
      value = BinaryPrimitives.ReadInt16LittleEndian(span);
      return sizeof(short);
    }

    public static int Read(this ReadOnlySpan<byte> span, out ushort value) {
      value = BinaryPrimitives.ReadUInt16LittleEndian(span);
      return sizeof(ushort);
    }

    public static int Read(this ReadOnlySpan<byte> span, out int value) {
      value = BinaryPrimitives.ReadInt32LittleEndian(span);
      return sizeof(int);
    }

    public static int Read(this ReadOnlySpan<byte> span, out uint value) {
      value = BinaryPrimitives.ReadUInt32LittleEndian(span);
      return sizeof(uint);
    }

    public static int Read(this ReadOnlySpan<byte> span, out long value) {
      value = BinaryPrimitives.ReadInt64LittleEndian(span);
      return sizeof(long);
    }

    public static int Read(this ReadOnlySpan<byte> span, out ulong value) {
      value = BinaryPrimitives.ReadUInt64LittleEndian(span);
      return sizeof(ulong);
    }

    public static unsafe int Read(this ReadOnlySpan<byte> span, out float value) {
      var len = span.Read(out uint binary);
      value = *(float*) &binary;
      return len;
    }

    public static unsafe int Read(this ReadOnlySpan<byte> span, out double value) {
      var len = span.Read(out ulong binary);
      value = *(double*) &binary;
      return len;
    }

    public static int Read(this ReadOnlySpan<byte> span, out bool value) {
      var len = span.Read(out byte binary);
      value = binary != 0;
      return len;
    }

    public static int Read(this ReadOnlySpan<byte> span, out string value) {
      var strSize = BinaryPrimitives.ReadUInt16LittleEndian(span);
      value = ReadUtf8Bytes(span.Slice(sizeof(ushort), strSize));
      return sizeof(ushort) + strSize;
    }

    private static unsafe string ReadUtf8Bytes(ReadOnlySpan<byte> span) {
      fixed (byte* bytesPtr = &span.GetPinnableReference())
        return Encoding.UTF8.GetString(bytesPtr, span.Length);
    }
  }
}