
// Type: PigeonProject.InputManagement.IPlayer
// Assembly: PigeonProject, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ED64DA4-BF9A-4998-A25D-EAF7C565AAB8
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

#nullable disable
namespace PigeonProject.InputManagement
{
  public interface IPlayer
  {
    void ProcessInput(GamePadState state, GameTime gameTime, InputController controller);
  }
}
