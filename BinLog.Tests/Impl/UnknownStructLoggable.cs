using System;
using BinLog.Serialization;

namespace BinLog.Tests.Impl {
  public readonly struct UnknownStructLoggable : ILoggableValue {
    private const int Value = 42;

    public int SizeOf() => sizeof(ushort) + sizeof(int);

    public object Unwrap() => Value;

    public int WriteTo(Span<byte> dst) {
      var bytesWritten = dst.Write((ushort) 4242);
      bytesWritten += dst.Slice(bytesWritten).Write(Value);
      return bytesWritten;
    }
  }
}