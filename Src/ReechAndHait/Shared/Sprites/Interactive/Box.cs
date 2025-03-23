
// Type: ReachHigh.Shared.Box
// Assembly: ReachHigh.Windows, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E7AAE051-E6AD-4505-A6E0-970B36D3CBD7
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using System.Collections.Generic;

#nullable disable
namespace ReachHigh.Shared
{
  public class Box : Sprite
  {
    private List<Sprite> collidingEnvironment;
    private Monkey monkey;
    private Vector2 previousVelocity;
    private SoundEffectInstance moveInstance;

    public Box(
      string PATH,
      Vector2 POS,
      Vector2 DIMS,
      List<Sprite> COLLIDIING_ENVIRONMENT,
      Monkey MONKEY)
      : base(PATH, POS, DIMS)
    {
      this.collidingEnvironment = COLLIDIING_ENVIRONMENT;
      this.monkey = MONKEY;
      this.moveInstance = Globals.audioManager.sounds["Box_Move"].CreateInstance();
    }

    public override void Update(float deltaSeconds)
    {
      this.velocity.Y += Globals.gravitation / 3f;
      foreach (Sprite other in this.collidingEnvironment)
      {
        if (this.isCollidingBottom(other) && !(other is Floaty))
          this.velocity.Y = 0.0f;
        if (this.isCollidingTop(other) && !(other is Floaty))
          this.velocity.Y = 0.0f;
        if (this.isCollidingLeft(other) && !(other is Floaty))
          this.velocity.X = 0.0f;
        if (this.isCollidingRight(other) && !(other is Floaty))
          this.velocity.X = 0.0f;
      }
      if (this.velocity == Vector2.Zero && this.previousVelocity != Vector2.Zero)
      {
        this.moveInstance.Stop();
        Globals.audioManager.sounds["Box_Stop"].Play();
      }
      else if (this.velocity != Vector2.Zero && this.previousVelocity == Vector2.Zero)
        this.moveInstance.Play();
      this.previousVelocity = this.velocity;
      this.pos = this.pos + this.velocity * deltaSeconds;
      base.Update(deltaSeconds);
    }
  }
}
