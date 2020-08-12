using BinLog.Decoding;
using BinLog.Tests.Impl.Loggers;

namespace BinLog.Tests.Impl.Decoding {
  public class FooDecoder : ChannelDecoder<LoggerId, FooMsgId> {
    public FooDecoder(ArgumentDecoder argDecoder) : base(LoggerId.Foo, argDecoder) { }
  }
}