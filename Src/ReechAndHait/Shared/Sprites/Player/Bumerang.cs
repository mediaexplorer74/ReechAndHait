
// Type: ReachHigh.Shared.Bumerang
// Assembly: ReachHigh.Windows, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E7AAE051-E6AD-4505-A6E0-970B36D3CBD7
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;

#nullable disable
namespace ReachHigh.Shared
{
  public class Bumerang : Sprite
  {
    public float speed;
    public static bool loose;
    private bool previousLoose;
    private float count;
    private Gecko thrower;
    private bool toTarget;
    private List<Sprite> collidingEnvironment;
    private SoundEffectInstance bumerangInstance;
    public AnimationManager animationManager;
    private Animation animation;

    public Bumerang(
      string PATH,
      Vector2 POS,
      Vector2 DIMS,
      Gecko gecko,
      List<Sprite> COLLIDIING_ENVIRONMENT)
      : base(PATH, POS, DIMS)
    {
      this.collidingEnvironment = COLLIDIING_ENVIRONMENT;
      this.origin = new Vector2((float) (this.sprite.Width / 2), (float) (this.sprite.Height / 2));
      this.direction = Globals.Normalize(Target.mousePos - this.pos);
      this.thrower = gecko;
      this.bumerangInstance = this.thrower.sounds["Gecko_Bumerang_Fly"].CreateInstance();
      this.animation = new Animation(this.sprite, 4, 0.05f, new Vector2(1f, 1.2f));
      this.animationManager = new AnimationManager(this.animation);
      this.animationManager.pos = this.pos;
    }

    public override void Update(float deltaSeconds)
    {
      this.animationManager.pos = this.pos;
      this.animationManager.PlayAnimation(this.animation, SpriteEffects.None);
      this.animationManager.Update(deltaSeconds);
      this.velocity = Vector2.Zero;
      if ((double) this.thrower.catapultTimer > 0.0)
        this.count = 3f;
      if (Bumerang.loose && (double) this.count < 3.0)
      {
        this.speed = (float) ((3.0 - (double) this.count) * 300.0);
        this.velocity = this.direction * this.speed;
        this.count += deltaSeconds * 2f;
        this.toTarget = true;
        this.bumerangInstance.Play();
      }
      else if (Bumerang.loose)
      {
        this.speed = (float) (((double) this.count - 3.0) * 200.0);
        this.count += deltaSeconds * 3f;
        this.velocity = Globals.Normalize(Globals.geckoPos - this.pos) * this.speed;
        this.toTarget = false;
      }
      else
      {
        if (this.previousLoose)
        {
          this.bumerangInstance.Stop();
          this.thrower.sounds["Gecko_Bumerang_Catch"].Play();
        }
        this.pos = Globals.geckoPos;
        this.count = 0.0f;
        this.direction = Globals.Normalize(Target.mousePos - this.pos);
        if ((double) this.direction.X < 1.0 / Math.Sqrt(2.0) && (double) this.direction.X > 0.0 && (double) this.direction.Y > 0.0)
          this.direction = new Vector2(1f / (float) Math.Sqrt(2.0), 1f / (float) Math.Sqrt(2.0));
        else if ((double) this.direction.X > 1.0 / -Math.Sqrt(2.0) && (double) this.direction.X < 0.0 && (double) this.direction.Y > 0.0)
          this.direction = new Vector2(1f / (float) -Math.Sqrt(2.0), 1f / (float) Math.Sqrt(2.0));
      }
      this.previousLoose = Bumerang.loose;
      if ((double) this.count >= 1.0 && (double) Globals.GetDistance(this.pos, Globals.geckoPos) < 50.0)
        Bumerang.loose = false;
      if (this.toTarget)
      {
        foreach (Sprite other in this.collidingEnvironment)
        {
          if (this.isCollidingBottom(other) && (double) this.velocity.Y > 0.0 && !(other is Player))
          {
            this.velocity.Y = 0.0f;
            this.count = 3f;
            //Debug.WriteLine(other.path);
            this.thrower.sounds["Gecko_Bumerang_Collision"].Play();
          }
          if (this.isCollidingTop(other) && (double) this.velocity.Y < 0.0 && !(other is Player))
          {
            this.velocity.Y = 0.0f;
            this.count = 3f;
            //Debug.WriteLine(other.path);
            this.thrower.sounds["Gecko_Bumerang_Collision"].Play();
          }
          if (this.isCollidingLeft(other) && (double) this.velocity.X < 0.0 && !(other is Player))
          {
            this.velocity.X = 0.0f;
            this.count = 3f;
            //Debug.WriteLine(other.path);
            this.thrower.sounds["Gecko_Bumerang_Collision"].Play();
          }
          if (this.isCollidingRight(other) && (double) this.velocity.X > 0.0 && !(other is Player))
          {
            this.velocity.X = 0.0f;
            this.count = 3f;
            //Debug.WriteLine(other.path);
            this.thrower.sounds["Gecko_Bumerang_Collision"].Play();
          }
        }
      }
      this.pos = this.pos + this.velocity * deltaSeconds;
      Globals.bumerangPos = this.pos;
      base.Update(deltaSeconds);
    }

    public override void Draw() => this.animationManager.Draw();

    public override Rectangle BoxCollider(string special = "none")
    {
      return new Rectangle((int) this.pos.X, (int) this.pos.Y, (int) this.dims.X / 2, (int) this.dims.Y / 2);
    }
  }
}
