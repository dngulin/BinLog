namespace BinLog.Tests.Impl {
  public readonly struct CustomStruct {
    public readonly float X;
    public readonly float Y;

    public const int SerializedSize = sizeof(float) * 2;

    public CustomStruct(float x, float y) {
      X = x;
      Y = y;
    }

    public override string ToString() => $"CustomStruct({X:F3}, {Y:F3})";
  }
}