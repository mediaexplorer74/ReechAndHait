
// Type: ReachHigh.Shared.Bar
// Assembly: ReachHigh.Windows, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E7AAE051-E6AD-4505-A6E0-970B36D3CBD7
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;

#nullable disable
namespace ReachHigh.Shared
{
  public class Bar : Sprite
  {
    public bool getPulled;
    private bool previousGetPulled;
    private Vector2 ogPos;
    private float speed;
    private float bSpeed;
    private Vector2 previousVelocity;
    private static SoundEffectInstance moveInstance;

    public Bar(string PATH, Vector2 POS, Vector2 DIMS, string WAY, float SPEED = 50f, float BSPEED = 0.0f)
      : base(PATH, POS, DIMS)
    {
      this.type = WAY;
      this.ogPos = this.pos;
      this.speed = SPEED;
      this.bSpeed = (double) BSPEED != 0.0 ? BSPEED : this.speed;
      Bar.moveInstance = Globals.audioManager.sounds["Bar_Move"].CreateInstance();
    }

    public override void Update(float deltaSeconds)
    {
      this.velocity = Vector2.Zero;
      switch (this.type)
      {
        case "up":
          if (this.getPulled && (double) this.pos.Y > (double) this.ogPos.Y - (double) this.dims.Y)
          {
            this.velocity = new Vector2(0.0f, -this.speed);
            break;
          }
          if (!this.getPulled && (double) this.ogPos.Y > (double) this.pos.Y)
          {
            this.velocity = new Vector2(0.0f, this.bSpeed);
            break;
          }
          break;
        case "down":
          if (this.getPulled && (double) this.pos.Y < (double) this.ogPos.Y + (double) this.dims.Y)
          {
            this.velocity = new Vector2(0.0f, -this.speed);
            break;
          }
          if (!this.getPulled && (double) this.ogPos.Y < (double) this.pos.Y)
          {
            this.velocity = new Vector2(0.0f, this.bSpeed);
            break;
          }
          break;
        case "right":
          if (this.getPulled && (double) this.pos.X < (double) this.ogPos.X + (double) this.dims.X)
          {
            this.velocity = new Vector2(this.speed, 0.0f);
            break;
          }
          if (!this.getPulled && (double) this.ogPos.X < (double) this.pos.X)
          {
            this.velocity = new Vector2(-this.bSpeed, 0.0f);
            break;
          }
          break;
        case "left":
          if (this.getPulled && (double) this.pos.X > (double) this.ogPos.X - (double) this.dims.X)
          {
            this.velocity = new Vector2(-this.speed, 0.0f);
            break;
          }
          if (!this.getPulled && (double) this.ogPos.X > (double) this.pos.X)
          {
            this.velocity = new Vector2(this.bSpeed, 0.0f);
            break;
          }
          break;
      }
      if (this.velocity == Vector2.Zero && this.previousVelocity != Vector2.Zero)
      {
        Bar.moveInstance.Stop();
        Globals.audioManager.sounds["Bar_Stop"].Play();
      }
      else if (this.velocity != Vector2.Zero)
        Bar.moveInstance.Play();
      this.previousVelocity = this.velocity;
      this.pos = this.pos + this.velocity * deltaSeconds;
      if (this.getPulled && !this.previousGetPulled)
        Globals.audioManager.sounds["Lever"].Play();
      else if (!this.getPulled && this.previousGetPulled)
        Globals.audioManager.sounds["Lever"].Play();
      this.previousGetPulled = this.getPulled;
      this.getPulled = false;
      base.Update(deltaSeconds);
    }
  }
}
