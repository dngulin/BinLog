using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace BinLog.Decoding {
  public unsafe class ChannelDecoder<TChannelEnum, TMessageEnum> : IChannelDecoder
    where TChannelEnum : unmanaged
    where TMessageEnum : unmanaged {

    private readonly ArgumentDecoder _argDecoder;

    public ChannelDecoder(TChannelEnum channel, ArgumentDecoder argDecoder) {
      if (sizeof(TChannelEnum) != sizeof(ushort))
        throw null;

      if (sizeof(TMessageEnum) != sizeof(ushort))
        throw null;

      _argDecoder = argDecoder;

      ChannelId = *(ushort*) &channel;
      ChannelName = channel.ToString();
    }

    public ushort ChannelId { get; }
    public string ChannelName { get; }

    public int DecodeArgument(ReadOnlySpan<byte> span, out object result) => _argDecoder.Decode(span, out result);

    public string DecodeMessage(ushort msgId) {
      var value = *(TMessageEnum*) &msgId;
      return typeof(TMessageEnum)
        .GetMember(value.ToString())
        .FirstOrDefault()?
        .GetCustomAttribute<DescriptionAttribute>()?
        .Description ?? value + " {0} {1} {2} {3}";
    }
  }
}