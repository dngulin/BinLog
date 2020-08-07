using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace BinLog.Internal {
  internal static class LogEnum {
    public static string GetMsg<T>(T value) where T : unmanaged {
      return typeof(T)
        .GetMember(value.ToString())
        .FirstOrDefault()?
        .GetCustomAttribute<DescriptionAttribute>()?
        .Description ?? value + " {0} {1} {2} {3}";
    }

    public static unsafe string GetMsg<T>(ushort msgId) where T : unmanaged => GetMsg(*(T*) &msgId);
  }
}