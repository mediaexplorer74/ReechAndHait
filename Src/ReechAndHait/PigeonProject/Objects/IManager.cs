
// Type: PigeonProject.Objects.IManager
// Assembly: PigeonProject, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ED64DA4-BF9A-4998-A25D-EAF7C565AAB8
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework;
using PigeonProject.StateMachine;

#nullable disable
namespace PigeonProject.Objects
{
  internal interface IManager
  {
    StateManager StateManager { get; }

    void Update(GameTime gameTime);

    void Reset();
  }
}
