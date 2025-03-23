
// Type: PigeonProject.Objects.IPhysicsObject
// Assembly: PigeonProject, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ED64DA4-BF9A-4998-A25D-EAF7C565AAB8
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework;

#nullable disable
namespace PigeonProject.Objects
{
  public interface IPhysicsObject
  {
    bool UseFriction { get; }

    Vector2 Position { get; set; }

    Vector2 Velocity { get; set; }

    float RotationVelocity { get; set; }
  }
}
