using System;
using BinLog.Exceptions;
using BinLog.Internal;

namespace BinLog.Decoding {
  public class ChannelDecoder<TChannelEnum, TMessageEnum> : IChannelDecoder
    where TChannelEnum : unmanaged
    where TMessageEnum : unmanaged {

    private readonly ArgumentDecoder _argDecoder;

    public ChannelDecoder(TChannelEnum channel, ArgumentDecoder argDecoder) {
      if (!LogEnum.CheckSizeOf<TChannelEnum>())
        throw new BinLogException($"Size of {nameof(TChannelEnum)} should be {sizeof(ushort)}");

      if (!LogEnum.CheckSizeOf<TMessageEnum>())
        throw new BinLogException($"Size of {nameof(TMessageEnum)} should be {sizeof(ushort)}");

      _argDecoder = argDecoder;

      ChannelId = LogEnum.ToUInt16(channel);
      ChannelName = channel.ToString();
    }

    public ushort ChannelId { get; }
    public string ChannelName { get; }

    public int DecodeArgument(ReadOnlySpan<byte> span, out object result) {
      return _argDecoder.Decode(ChannelId, span, out result);
    }

    public string DecodeMessage(ushort msgId) => LogEnum.GetMsg<TMessageEnum>(msgId);
  }
}