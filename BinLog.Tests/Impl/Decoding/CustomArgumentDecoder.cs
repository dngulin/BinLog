using System;
using BinLog.Decoding;

namespace BinLog.Tests.Impl.Decoding {
  public class CustomArgumentDecoder : ArgumentDecoder {
    protected override int DecodeImpl(ushort typeId, ReadOnlySpan<byte> source, out object result) {
      switch ((CustomTypeId)typeId) {
        case CustomTypeId.CustomStruct:
          return CustomStructLoggable.DecodeValue(source, out result);
      }
      return base.DecodeImpl(typeId, source, out result);
    }
  }
}