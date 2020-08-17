using System.Collections.Generic;

namespace BinLog.Tests.Impl.Loggers {
  public class SampleTracer : LogTracer {
    public readonly List<string> Messages = new List<string>();

    protected override void TraceImpl(LogLevel level, string channel, string message) => Messages.Add(message);
  }
}