using System;

namespace BinLog.Decoding {
  public interface IChannelDecoder {
    ushort ChannelId { get; }
    string ChannelName { get; }
    int DecodeArgument(ReadOnlySpan<byte> source, out object result);
    string DecodeMessage(ushort msgId);
  }
}