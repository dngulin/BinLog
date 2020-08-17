using System;

namespace BinLog {
  /// <summary>
  /// The base binary logging interface. Used for log entries and parameters.
  /// </summary>
  public interface ILoggable {
    /// <summary>
    /// Get loggable serialization size. Use this value to check serialization buffer size.
    /// </summary>
    /// <returns>The serialized size in bytes.</returns>
    int SizeOf();

    /// <summary>
    /// Writes loggable value into buffer.
    /// </summary>
    /// <param name="dst">Destination buffer.</param>
    /// <returns>The count of written bytes.</returns>
    int WriteTo(Span<byte> dst);
  }

  /// <summary>
  /// Extended loggable interface. Used by <c>LogTracer</c>.
  /// </summary>
  /// <remarks>
  /// Create <c>ILoggableValue</c> struct wrapper for each type you want to log.
  /// Create <c>ForLog()</c> extension method to generate wrapped values.
  /// </remarks>
  public interface ILoggableValue : ILoggable {
    /// <summary>
    /// Unwraps loggable value into object. Can generate some garbage because of boxing.
    /// </summary>
    /// <returns>The wrapped value itself</returns>
    object Unwrap();
  }
}