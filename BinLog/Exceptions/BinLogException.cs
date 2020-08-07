using System;

namespace BinLog.Exceptions {
  public class BinLogException : Exception {
    public BinLogException(string message) : base(message) { }
  }
}