using System.ComponentModel;
using System.IO;

namespace BinLog.Tests.Impl.Loggers {
  public class BarLogger : Logger<LoggerId, BarMsgId> {
    public BarLogger(Stream stream, byte[] buffer, LogTracer tracer = null)
      : base(LoggerId.Bar, stream, buffer, tracer) { }
  }

  public enum BarMsgId : ushort {
    [Description("Hello, World!")] HelloWorld = 0,
    [Description("Bar value: {0}")] BarValueIs = 1
  }
}