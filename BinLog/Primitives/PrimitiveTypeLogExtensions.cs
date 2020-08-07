namespace BinLog.Primitives {
  public static class PrimitiveTypeLogExtensions {
    public static LoggablePrimitive<bool> ForLog(this bool value) => new LoggablePrimitive<bool>(value);
    public static LoggablePrimitive<byte> ForLog(this byte value) => new LoggablePrimitive<byte>(value);

    public static LoggablePrimitive<short> ForLog(this short value) => new LoggablePrimitive<short>(value);
    public static LoggablePrimitive<ushort> ForLog(this ushort value) => new LoggablePrimitive<ushort>(value);

    public static LoggablePrimitive<int> ForLog(this int value) => new LoggablePrimitive<int>(value);
    public static LoggablePrimitive<uint> ForLog(this uint value) => new LoggablePrimitive<uint>(value);

    public static LoggablePrimitive<long> ForLog(this long value) => new LoggablePrimitive<long>(value);
    public static LoggablePrimitive<ulong> ForLog(this ulong value) => new LoggablePrimitive<ulong>(value);

    public static LoggablePrimitive<float> ForLog(this float value) => new LoggablePrimitive<float>(value);
    public static LoggablePrimitive<double> ForLog(this double value) => new LoggablePrimitive<double>(value);

    public static LoggableString ForLog(this string value) => new LoggableString(value);
  }
}