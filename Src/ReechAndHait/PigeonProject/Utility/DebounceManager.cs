
// Type: PigeonProject.Utility.DebounceManager
// Assembly: PigeonProject, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ED64DA4-BF9A-4998-A25D-EAF7C565AAB8
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;

#nullable disable
namespace PigeonProject.Utility
{
  public static class DebounceManager
  {
    public static List<Debounce> register = new List<Debounce>();

    public static void Update(GameTime gameTime)
    {
      for (int index = DebounceManager.register.Count - 1; index >= 0; --index)
        DebounceManager.register[index].Update(gameTime);
    }

    public static bool Register(Debounce debounceInstance)
    {
      if (!DebounceManager.register.Contains(debounceInstance))
      {
        DebounceManager.register.Add(debounceInstance);
        return true;
      }
      Debug.WriteLine("WARNING: Trying to register debounce that is already registered!");
      return false;
    }

    public static bool UnRegister(Debounce debounceInstance)
    {
      return DebounceManager.register.Remove(debounceInstance);
    }
  }
}
