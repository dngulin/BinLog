using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace BinLog.Internal {
  internal static class LogEnum {
    public static unsafe bool CheckSizeOf<T>() where T : unmanaged => sizeof(T) == sizeof(ushort);
    public static unsafe ushort ToUInt16<T>(T value) where T : unmanaged => *(ushort*) &value;

    public static string GetMsg<T>(T value) where T : unmanaged {
      return typeof(T)
        .GetMember(value.ToString())
        .FirstOrDefault()?
        .GetCustomAttribute<DescriptionAttribute>()?
        .Description ?? value + " {0} {1} {2} {3}";
    }

    public static unsafe string GetMsg<T>(ushort value) where T : unmanaged => GetMsg(*(T*) &value);
  }
}