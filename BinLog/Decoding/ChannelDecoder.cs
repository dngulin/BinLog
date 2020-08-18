using System;
using BinLog.Exceptions;
using BinLog.Internal;

namespace BinLog.Decoding {
  /// <summary>
  /// The channel decoder. Create a matching subclass for each logger in your project.
  /// </summary>
  /// <typeparam name="TChannelEnum">Channel id enum. Should be inherited from ushort.</typeparam>
  /// <typeparam name="TMessageEnum">Message id enum. Should be inherited from ushort.</typeparam>
  public class ChannelDecoder<TChannelEnum, TMessageEnum> : IChannelDecoder
    where TChannelEnum : unmanaged
    where TMessageEnum : unmanaged {

    private readonly ArgumentDecoder _argDecoder;

    /// <summary>
    /// Channel decoder constructor.
    /// </summary>
    /// <param name="channel">Unique logger id</param>
    /// <param name="argDecoder">Binary argument deserializer</param>
    /// <exception cref="BinLogException">
    /// Thrown when size of <c>TChannelEnum</c> or <c>TMessageEnum</c> is invalid.
    /// </exception>
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

    public int DecodeArgument(ReadOnlySpan<byte> source, out object result) {
      return _argDecoder.Decode(source, out result);
    }

    public string DecodeMessage(ushort msgId) => LogEnum.GetMsg<TMessageEnum>(msgId);

    public string DecodeMessage(TMessageEnum msg) => LogEnum.GetMsg(msg);
  }
}