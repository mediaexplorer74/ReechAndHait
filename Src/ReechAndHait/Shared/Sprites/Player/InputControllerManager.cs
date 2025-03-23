
// Type: ReachHigh.Shared.InputControllerManager
// Assembly: ReachHigh.Windows, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E7AAE051-E6AD-4505-A6E0-970B36D3CBD7
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework;
using System.Collections.Generic;

#nullable disable
namespace ReachHigh.Shared
{
  public static class InputControllerManager
  {
    private static readonly List<IInputController> _register = new List<IInputController>();

    public static void Update(GameTime gameTime)
    {
      foreach (IInputController inputController in InputControllerManager._register)
        inputController.Update(gameTime);
    }

    public static void RegisterInputController(InputController controller)
    {
      if (InputControllerManager._register.Contains((IInputController) controller))
        return;
      InputControllerManager._register.Add((IInputController) controller);
    }

    public static bool UnregisterInputController(InputController controller)
    {
      return InputControllerManager._register.Remove((IInputController) controller);
    }
  }
}
