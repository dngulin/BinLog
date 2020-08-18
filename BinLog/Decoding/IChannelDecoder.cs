using System;

namespace BinLog.Decoding {
  /// <summary>
  /// Non-generic channel decoding interface.
  /// </summary>
  public interface IChannelDecoder {
    ushort ChannelId { get; }
    string ChannelName { get; }
    int DecodeArgument(ReadOnlySpan<byte> source, out object result);
    string DecodeMessage(ushort msgId);
  }
}