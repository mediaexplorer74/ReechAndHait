
// Type: PigeonProject.StateMachine.IState
// Assembly: PigeonProject, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ED64DA4-BF9A-4998-A25D-EAF7C565AAB8
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

#nullable disable
namespace PigeonProject.StateMachine
{
  public interface IState
  {
    bool IsInputLocked { get; set; }

    bool IsInputDelayed { get; set; }

    void Initialize();

    void Update(GameTime gameTime);

    void Draw(SpriteBatch spriteBatch, GameTime gameTime);

    void Terminate();
  }
}
