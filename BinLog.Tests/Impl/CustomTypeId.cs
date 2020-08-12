using BinLog.Primitives;

namespace BinLog.Tests.Impl {
  public enum CustomTypeId : ushort {
    CustomStruct = PrimitiveTypeId.String + 1
  }
}