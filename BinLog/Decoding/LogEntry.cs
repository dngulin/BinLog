using System;

namespace BinLog.Decoding {
  public struct LogEntry {
    public string Channel;
    public string Message;
    public LogLevel LogLevel;
    public DateTime UtcDateTime;
  }
}