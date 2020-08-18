using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace BinLog.Decoding {
  /// <summary>
  /// Top level binary log decoder class.
  /// </summary>
  public class LogDecoder {
    private readonly byte[] _buffer;
    private readonly Dictionary<ushort, IChannelDecoder> _decoders;

    public LogDecoder(int bufferSize, IEnumerable<IChannelDecoder> decoders) {
      _buffer = new byte[bufferSize];
      _decoders = decoders.ToDictionary(d => d.ChannelId);
    }

    public IEnumerable<LogEntry> Decode(Stream stream) => new DecodeEnumerable(stream, _buffer, _decoders);
  }
}