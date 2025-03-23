
// Type: ReachHigh.Shared.Player
// Assembly: ReachHigh.Windows, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E7AAE051-E6AD-4505-A6E0-970B36D3CBD7
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using System;
using System.Collections.Generic;
using System.Diagnostics;

#nullable disable
namespace ReachHigh.Shared
{
  public class Player : Sprite
  {
    public float jumpTimer;
    public float speed;
    protected bool collisionBottom;
    protected bool collisionTop;
    protected bool collisionLeft;
    protected bool collisionRight;
    public bool previousCollisionBottom;
    protected bool specialCollisionBottom;
    protected bool specialCollisionTop;
    protected bool specialCollisionLeft;
    protected bool specialCollisionRight;
    protected bool bumerangPossiblility;
    protected bool climbRangeUp;
    protected bool climbRangeDown;
    protected bool stickRange;
    protected bool landing;
    public bool jumpAvailable;
    public bool inJump;
    public int jumpBoost;
    public bool jumpInterrupt;
    public bool stunned;
    public bool sequence;
    protected float jumpForce;
    protected bool inputActivate;
    protected bool debugFly;
    protected float accelGrav;
    protected const float standardAccelGrav = 0.5f;
    protected float acceljumpTimer;
    private List<Sprite> collidingEnvironment;
    private List<Lever> levers;
    protected List<Bar> bars;
    public Dictionary<string, Animation> animations;
    protected bool pullingLever;
    public AnimationManager animationManager;
    public AudioManager audioManager;
    protected Vector2 previousVelocity;
    protected SoundEffectInstance landInstance;
    private string _currentSurfaceType;

    public Dictionary<string, SoundEffect> sounds { get; protected set; }

    public Dictionary<string, SoundIteration> soundIterations { get; protected set; }

    public string CurrentSurfaceType
    {
      get
      {
        return this._currentSurfaceType != null && this._currentSurfaceType.Length > 0 ? this._currentSurfaceType : "Stone";
      }
      private set => this._currentSurfaceType = value;
    }

    public InputController InputController { get; protected set; }

    public Player(
      string PATH,
      Vector2 POS,
      Vector2 DIMS,
      List<Sprite> COLLIDIING_ENVIRONMENT,
      List<Lever> LEVERS,
      List<Bar> BARS)
      : base(PATH, POS, DIMS)
    {
      this.collidingEnvironment = COLLIDIING_ENVIRONMENT;
      this.levers = LEVERS;
      this.bars = BARS;
      this.speed = 600f;
      this.accelGrav = 0.5f;
      this.stunned = false;
      this.previousVelocity = Vector2.Zero;
    }

    public override void Update(float deltaSeconds)
    {
      this.velocity.Y += Globals.gravitation * this.accelGrav;
      this.accelGrav += deltaSeconds;
      this.pullingLever = false;
      foreach (Lever lever in this.levers)
      {
        if (this.BoxCollider().Intersects(lever.BoxCollider()) && this.inputActivate)
        {
          lever.ActivateLever();
          switch (this)
          {
            case Monkey _ when lever.type == "Pulllever":
              lever.pullActivation = true;
              break;
            case Gecko _ when lever.type == "Button":
              lever.pullActivation = true;
              break;
          }
          this.pullingLever = true;
        }
      }
      this.collisionBottom = false;
      this.collisionTop = false;
      this.collisionLeft = false;
      this.collisionRight = false;
      this.specialCollisionBottom = false;
      this.specialCollisionTop = false;
      this.specialCollisionLeft = false;
      this.specialCollisionRight = false;
      this.inputActivate = false;
      this.debugFly = false;
      this.jumpAvailable = false;
      this.bumerangPossiblility = true;
      this.climbRangeUp = false;
      this.climbRangeDown = false;
      this.stickRange = false;
      if (this.inJump)
        this.velocity.Y -= (float) ((double) this.jumpForce * (double) this.jumpTimer + (double) this.jumpBoost * (double) this.jumpForce * (double) this.jumpTimer);
      if ((double) this.jumpTimer > 0.0)
        this.jumpTimer -= deltaSeconds;
      else
        this.inJump = false;
      this.CheckCollision();
      base.Update(deltaSeconds);
    }

    public override void Draw() => base.Draw();

    protected virtual void CheckCollision()
    {
      foreach (Sprite other in this.collidingEnvironment)
      {
        if (this.isCollidingBottom(other) && !(other is Player))
        {
          this.collisionBottom = true;
          this.jumpAvailable = true;
          this.CurrentSurfaceType = other.SurfaceType;
        }
        if (this.isCollidingTop(other))
        {
          switch (other)
          {
            case Floaty _:
            case Player _:
              break;
            default:
              this.collisionTop = true;
              if (other is SpecialSprite && this is Gecko && this.isCollidingTop(other))
              {
                this.specialCollisionTop = true;
                break;
              }
              break;
          }
        }
        if (this.isCollidingLeft(other))
        {
          switch (other)
          {
            case Floaty _:
            case Player _:
              break;
            default:
              this.collisionLeft = true;
              if (other is SpecialSprite && other.type == "climb" && this is Monkey && (this.isCollidingLeft(other, "climb-up") || this.isCollidingLeft(other, "climb-down")))
              {
                this.specialCollisionLeft = true;
                break;
              }
              break;
          }
        }
        if (this.isCollidingRight(other))
        {
          switch (other)
          {
            case Floaty _:
            case Player _:
              break;
            default:
              this.collisionRight = true;
              if (other is SpecialSprite && other.type == "climb" && this is Monkey && (this.isCollidingRight(other, "climb-up") || this.isCollidingRight(other, "climb-down")))
              {
                this.specialCollisionRight = true;
                break;
              }
              break;
          }
        }
        Rectangle rectangle;
        if (other is SpecialSprite)
        {
          rectangle = this.BoxCollider("climb-up");
          if (rectangle.Intersects(other.BoxCollider()))
            this.climbRangeUp = true;
        }
        if (other is SpecialSprite)
        {
          rectangle = this.BoxCollider("climb-down");
          if (rectangle.Intersects(other.BoxCollider()))
            this.climbRangeDown = true;
        }
        if (other is SpecialSprite && other.type == "sticky")
        {
          rectangle = this.BoxCollider("stick");
          if (rectangle.Intersects(other.BoxCollider()))
            this.stickRange = true;
        }
        if (this is Gecko)
        {
          this.pos.Y -= 60f;
          if (this.isCollidingTop(other) && !(other is Player))
            this.bumerangPossiblility = false;
          this.pos.Y += 60f;
        }
      }
    }

    protected void PlayWalkSound(string playerType)
    {
      string key = playerType + "_Walk_" + this.CurrentSurfaceType;
      if (!this.soundIterations.ContainsKey(key))
        return;
      this.audioManager.PlaySoundEffectIteration(this.soundIterations[key]);
    }

    protected void PlayActionSound(string player, string action)
    {
      this.sounds[player + "_" + action + "_" + this.CurrentSurfaceType].Play();
    }

    protected SoundEffectInstance GetActionSound(string player, string action)
    {
      return this.sounds[player + "_" + action + "_" + this.CurrentSurfaceType].CreateInstance();
    }

    protected void LoadActionSound(
      string player,
      int folderIndex,
      string[] actionsWithIndex,
      string[] surfaces)
    {
      this.sounds["Gecko_Jump"] = Globals.content.Load<SoundEffect>("Audio\\1 Character Sounds\\2 Gecko\\2 Jump\\G_Jump_grass");
      char ch = player[0];
      foreach (string str1 in actionsWithIndex)
      {
        string str2 = str1.Substring(2);
        foreach (string surface in surfaces)
        {
          try
          {
            this.sounds[player + "_" + str2 + "_" + surface] = Globals.content.Load<SoundEffect>(string.Format("Audio\\1 Character Sounds\\{0} {1}\\{2}\\{3}_{4}_{5}", (object) folderIndex, (object) player, (object) str1, (object) ch, (object) str2, (object) surface));
          }
          catch (Exception ex)
          {
            Debug.WriteLine(string.Format("Error loading the action sound: {0}_{1}_{2}: {3}", (object) ch, (object) str1, (object) surface, (object) ex.Message));
          }
        }
      }
    }
  }
}
