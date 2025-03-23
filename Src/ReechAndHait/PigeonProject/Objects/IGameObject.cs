
// Type: PigeonProject.Objects.IGameObject
// Assembly: PigeonProject, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ED64DA4-BF9A-4998-A25D-EAF7C565AAB8
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

#nullable disable
namespace PigeonProject.Objects
{
  public interface IGameObject
  {
    Vector2 Position { get; set; }

    bool Delete();

    void Update(GameTime gameTime);

    void Draw(SpriteBatch spriteBatch, GameTime gameTime);
  }
}
