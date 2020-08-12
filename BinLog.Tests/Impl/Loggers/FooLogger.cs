using System.ComponentModel;
using System.IO;

namespace BinLog.Tests.Impl.Loggers {
  public class FooLogger : Logger<LoggerId, FooMsgId> {
    public FooLogger(Stream stream, byte[] buffer, LogTracer tracer = null)
      : base(LoggerId.Foo, stream, buffer, tracer) { }
  }

  public enum FooMsgId : ushort {
    [Description("Foo")] Foo = 0,
    [Description("Foo {0}")] Foo1 = 1,
    [Description("Foo {0} {1}")] Foo2 = 2,
    [Description("Foo {0} {1} {2}")] Foo3 = 3,
    [Description("Foo {0} {1} {2} {3}")] Foo4 = 4,
  }
}