using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using BinLog.Exceptions;
using BinLog.Internal;

namespace BinLog.Decoding {
  public unsafe class ChannelDecoder<TChannelEnum, TMessageEnum> : IChannelDecoder
    where TChannelEnum : unmanaged
    where TMessageEnum : unmanaged {

    private readonly ArgumentDecoder _argDecoder;

    public ChannelDecoder(TChannelEnum channel, ArgumentDecoder argDecoder) {
      if (sizeof(TChannelEnum) != sizeof(ushort))
        throw new BinLogException($"Size of {nameof(TChannelEnum)} should be {sizeof(ushort)}");

      if (sizeof(TMessageEnum) != sizeof(ushort))
        throw new BinLogException($"Size of {nameof(TMessageEnum)} should be {sizeof(ushort)}");

      _argDecoder = argDecoder;

      ChannelId = *(ushort*) &channel;
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