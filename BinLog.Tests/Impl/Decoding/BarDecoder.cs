using BinLog.Decoding;
using BinLog.Tests.Impl.Loggers;

namespace BinLog.Tests.Impl.Decoding {
  public class BarDecoder : ChannelDecoder<LoggerId, BarMsgId> {
    public BarDecoder(ArgumentDecoder argDecoder) : base(LoggerId.Bar, argDecoder) { }
  }
}