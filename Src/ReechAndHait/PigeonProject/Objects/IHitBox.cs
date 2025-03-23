
// Type: PigeonProject.Objects.IHitBox
// Assembly: PigeonProject, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ED64DA4-BF9A-4998-A25D-EAF7C565AAB8
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework;
using System.Collections.Generic;

#nullable disable
namespace PigeonProject.Objects
{
  public interface IHitBox
  {
    List<IHitBox> CurrentCollisions { get; }

    Vector2 Position { get; }

    int Radius { get; }

    bool IsStatic { get; }

    void InvokeCollisionEnter(IHitBox other, Vector2 collisionDirection);

    void InvokeCollisionExit(IHitBox other, Vector2 collisionDirection);

    void InvokeCollision(IHitBox other, Vector2 collisionDirection);
  }
}
