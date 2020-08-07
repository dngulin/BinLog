using System;

namespace BinLog {
  public interface ILoggable {
    int SizeOf();
    int WriteTo(Span<byte> dst);
  }

  public interface ILoggableValue : ILoggable {
    object Unwrap();
  }
}