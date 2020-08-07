namespace BinLog {
  public abstract class LogTracer {
    public void Trace(LogLevel level, string channel, string msg)
      => TraceImpl(level, channel, msg);

    public void Trace(LogLevel level, string channel, string msg, object arg1)
      => TraceImpl(level, channel, string.Format(msg, arg1));

    public void Trace(LogLevel level, string channel, string msg, object arg1, object arg2)
      => TraceImpl(level, channel, string.Format(msg, arg1, arg2));

    public void Trace(LogLevel level, string channel, string msg, object arg1, object arg2, object arg3)
      => TraceImpl(level, channel, string.Format(msg, arg1, arg2, arg3));

    public void Trace(LogLevel level, string channel, string msg, object arg1, object arg2, object arg3, object arg4)
      => TraceImpl(level, channel, string.Format(msg, arg1, arg2, arg3, arg4));

    protected abstract void TraceImpl(LogLevel level, string channel, string message);
  }
}