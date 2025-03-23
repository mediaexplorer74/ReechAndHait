
// Type: ReachHigh.Shared.Monkey
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
  public class Monkey : Player
  {
    private bool grip;
    private bool previousGrip;
    private bool push;
    private bool gripRight;
    public bool holdOn;
    public Gecko partner;
    public Bumerang bumerang;
    private List<Box> boxes;
    private Box contactedBox;
    private static int index;
    private static float timer;
    private float gripTimer;
    private List<SoundEffect> stoneWalks;
    private List<SoundEffect> grassWalks;
    private List<SoundEffect> waterWalks;
    private List<SoundEffect> climbVines;

    public Monkey(
      string PATH,
      Vector2 POS,
      Vector2 DIMS,
      List<Sprite> COLLIDIING_ENVIRONMENT,
      List<Box> BOXES,
      List<Lever> LEVERS,
      List<Bar> BARS)
      : base(PATH, POS, DIMS, COLLIDIING_ENVIRONMENT, LEVERS, BARS)
    {
      this.jumpForce = 2300f;
      this.gripTimer = 0.3f;
      this.partner = (Gecko) null;
      this.bumerang = (Bumerang) null;
      this.boxes = BOXES;
      this.SetAnimations();
      this.animationManager = new AnimationManager(this.animations["Idle"]);
      this.animationManager.pos = this.pos;
      this.SetSoundEffects();
      this.audioManager = new AudioManager(this.sounds);
      this.landInstance = this.sounds["Monkey_Land_Stone"].CreateInstance();
      this.InputController = new InputController(RhPlayerSettings.Get<InputMode>(Players.Monkey, "InputMode"), RhPlayerSettings.Get<PlayerIndex>(Players.Monkey, "GamepadIndex"));
    }

    private void SetAnimations()
    {
      this.animations = new Dictionary<string, Animation>();
      this.animations["Idle"] = new Animation(this.sprite, 7, 0.15f, new Vector2(1f, 1.1f), -10);
      this.animations["Run"] = new Animation(Globals.content.Load<Texture2D>("Animations\\Monkey\\monkey_run"), 5, 0.12f, new Vector2(1f, 1f));
      this.animations["Jump"] = new Animation(Globals.content.Load<Texture2D>("Animations\\Monkey\\monkey_jump"), 3, 0.04f, new Vector2(1f, 1f));
      this.animations["Fall"] = new Animation(Globals.content.Load<Texture2D>("Animations\\Monkey\\monkey_fall"), 3, 0.1f, new Vector2(1f, 1f));
      this.animations["Throw"] = new Animation(Globals.content.Load<Texture2D>("Animations\\Monkey\\monkey_throw"), 4, 0.04f, new Vector2(1f, 1f));
      this.animations["Push"] = new Animation(Globals.content.Load<Texture2D>("Animations\\Monkey\\monkey_push"), 4, 0.12f, new Vector2(1f, 1f));
      this.animations["Pull"] = new Animation(Globals.content.Load<Texture2D>("Animations\\Monkey\\monkey_pull"), 4, 0.12f, new Vector2(1f, 1f));
      this.animations["Climb"] = new Animation(Globals.content.Load<Texture2D>("Animations\\Monkey\\monkey_climb"), 4, 0.12f, new Vector2(1f, 1f));
      this.animations["Land"] = new Animation(Globals.content.Load<Texture2D>("Animations\\Monkey\\monkey_landing"), 3, 0.12f, new Vector2(1f, 1f));
      this.animations["Airborne"] = new Animation(Globals.content.Load<Texture2D>("Animations\\Monkey\\monkey_airborne"), 5, 0.12f, new Vector2(1f, 1f));
      this.animations["Bumerang_Travel"] = new Animation(Globals.content.Load<Texture2D>("Animations\\Monkey\\monkey_boomerang_travel"), 4, 0.12f, new Vector2(1f, 1f), -10);
    }

    private void SetSoundEffects()
    {
      this.sounds = new Dictionary<string, SoundEffect>();
      this.LoadActionSound(nameof (Monkey), 1, new string[2]
      {
        "2 Jump",
        "3 Land"
      }, new string[3]{ "Stone", "Grass", "Water" });
      this.sounds["Monkey_Land_Vines"] = Globals.content.Load<SoundEffect>("Audio\\1 Character Sounds\\1 Monkey\\3 Land\\M_Land_Vines");
      this.sounds["Monkey_Throw_Gecko"] = Globals.content.Load<SoundEffect>("Audio\\1 Character Sounds\\1 Monkey\\5 Throw Gecko\\M_Throw_Gecko");
      this.stoneWalks = new List<SoundEffect>();
      this.grassWalks = new List<SoundEffect>();
      this.waterWalks = new List<SoundEffect>();
      this.climbVines = new List<SoundEffect>();
      for (int index = 1; index <= 12; ++index)
      {
        this.grassWalks.Add(Globals.content.Load<SoundEffect>(string.Format("Audio\\1 Character Sounds\\1 Monkey\\1 Walk\\1 Grass\\M_Walk_Grass_{0}", (object) index)));
        this.stoneWalks.Add(Globals.content.Load<SoundEffect>(string.Format("Audio\\1 Character Sounds\\1 Monkey\\1 Walk\\2 Stone\\M_Walk_Stone_{0}", (object) index)));
        this.waterWalks.Add(Globals.content.Load<SoundEffect>(string.Format("Audio\\1 Character Sounds\\1 Monkey\\1 Walk\\3 Water\\M_Walk_Water_{0}", (object) index)));
        this.climbVines.Add(Globals.content.Load<SoundEffect>(string.Format("Audio\\1 Character Sounds\\1 Monkey\\4 Climb on Vines\\M_Climb_Vines_{0}", (object) index)));
      }
      this.soundIterations = new Dictionary<string, SoundIteration>();
      this.soundIterations["Monkey_Walk_Stone"] = new SoundIteration(this.stoneWalks, 12, 0.68f);
      this.soundIterations["Monkey_Walk_Grass"] = new SoundIteration(this.grassWalks, 12, 0.68f);
      this.soundIterations["Monkey_Walk_Water"] = new SoundIteration(this.waterWalks, 12, 0.68f);
      this.soundIterations["Monkey_Climb_Vines"] = new SoundIteration(this.climbVines, 12, 0.3f);
    }

    public override void Update(float deltaSeconds)
    {
      if ((double) this.gripTimer > 0.0)
        this.gripTimer -= deltaSeconds;
      this.velocity = Vector2.Zero;
      if (this.sequence)
        this.stunned = true;
      this.push = false;
      if (!this.stunned)
      {
        if ((double) this.InputController.Move().X < -0.5)
          this.velocity.X = -this.speed;
        if ((double) this.InputController.Move().X > 0.5)
          this.velocity.X = this.speed;
        bool isFirstInput;
        if (this.InputController.Jump(out isFirstInput) & isFirstInput && this.jumpAvailable)
        {
          this.inJump = true;
          this.jumpAvailable = false;
          this.jumpInterrupt = false;
          this.jumpTimer = 0.5f;
          Debug.WriteLine("jump");
          this.PlayActionSound(nameof (Monkey), "Jump");
          this.landInstance = this.GetActionSound(nameof (Monkey), "Land");
          if (this.grip)
          {
            this.grip = false;
            this.gripTimer = 0.3f;
          }
        }
        else if (this.InputController.Jump() && this.inJump && !this.jumpInterrupt)
          this.jumpBoost = 1;
        else if (this.inJump)
        {
          this.jumpBoost = 0;
          this.jumpInterrupt = true;
        }
      }
      if (this.InputController.MonkeyInteract(false))
        this.inputActivate = true;
      if (this.InputController.DebugFly())
        this.debugFly = true;
      if (this.specialCollisionRight && (double) this.gripTimer <= 0.0)
      {
        this.grip = true;
        this.gripRight = true;
      }
      else if (this.specialCollisionLeft && (double) this.gripTimer <= 0.0)
      {
        this.grip = true;
        this.gripRight = false;
      }
      if (this.InputController.MonkeyInteract(false) && this.contactedBox != null)
        this.push = true;
      else if (this.InputController.MonkeyThrowRide() && this.BoxCollider().Intersects(this.partner.BoxCollider()) && !this.holdOn && !this.stunned && this.collisionBottom && this.partner.jumpAvailable && !this.partner.inJump)
      {
        this.partner.catapultTimer = 2f;
        this.partner.pos = this.pos;
        Target.active = false;
        this.partner.stunned = false;
        Bumerang.loose = false;
        this.sounds["Monkey_Throw_Gecko"].Play();
      }
      else if (this.InputController.MonkeyThrowRide(false) && this.BoxCollider().Intersects(this.bumerang.BoxCollider("none")) && Bumerang.loose && !this.holdOn)
        this.holdOn = true;
      if (!Bumerang.loose)
        this.holdOn = false;
      if (this.holdOn)
        this.grip = false;
      base.Update(deltaSeconds);
      if (this.grip)
      {
        if (!this.jumpAvailable && !this.InputController.Jump())
          this.jumpAvailable = true;
        this.velocity = Vector2.Zero;
        if (this.climbRangeUp && ((double) this.InputController.Move().X > 0.5 && this.specialCollisionRight || (double) this.InputController.Move().X < -0.5 && this.specialCollisionLeft || (double) this.InputController.Move().Y > 0.5))
          this.velocity.Y = -200f;
        else if (this.climbRangeDown && (double) this.InputController.Move().Y < -0.5)
          this.velocity.Y = 200f;
      }
      if (this.push)
      {
        this.speed = 200f;
        if ((double) this.velocity.Y < 0.0)
          this.velocity.Y = 0.0f;
      }
      else
        this.speed = 600f;
      if (this.contactedBox != null)
        this.contactedBox.velocity = Vector2.Zero;
      if (!this.push)
        this.contactedBox = (Box) null;
      if (this.boxes != null)
      {
        foreach (Box box in this.boxes)
        {
          if (this.BoxCollider().Intersects(box.BoxCollider("box")))
            this.contactedBox = box;
        }
      }
      if (this.push && this.contactedBox != null)
      {
        if ((double) Globals.GetDistance(this.pos + this.dims / 2f, this.contactedBox.pos + this.contactedBox.dims / 2f) > 200.0)
          this.contactedBox = (Box) null;
        else
          this.contactedBox.velocity = this.velocity;
      }
      if (this.InputController.MonkeyInteract(false) && this.BoxCollider().Intersects(Globals.nextGate.BoxCollider("gate")) && !Globals.nextGate.monkeyTrigger)
      {
        Globals.nextGate.monkeyTrigger = true;
        Globals.audioManager.sounds["Gate_Activation_Monkey"].Play();
      }
      else if (!this.InputController.MonkeyInteract(false) && Globals.nextGate.monkeyTrigger)
        Globals.nextGate.monkeyTrigger = false;
      if (this.collisionBottom)
      {
        this.jumpAvailable = true;
        if ((double) this.velocity.Y > 0.0)
          this.velocity.Y = 0.0f;
      }
      if (this.collisionTop)
      {
        this.velocity.Y = 0.0f;
        this.inJump = false;
      }
      if (this.collisionLeft)
        this.velocity.X = 0.0f;
      if (this.collisionRight)
        this.velocity.X = 0.0f;
      foreach (Bar bar in this.bars)
      {
        if (this.isCollidingBarBottom((Sprite) bar) || this.isCollidingBarTop((Sprite) bar))
          this.velocity.Y += bar.velocity.Y;
        if (this.isCollidingBarLeft((Sprite) bar) || this.isCollidingBarRight((Sprite) bar))
          this.velocity.X += bar.velocity.X;
      }
      if (this.collisionBottom || this.grip)
        this.accelGrav = 0.5f;
      this.pos = this.pos + this.velocity * deltaSeconds;
      if (this.holdOn)
        this.pos = new Vector2(Globals.bumerangPos.X - this.dims.X / 3f, Globals.bumerangPos.Y - this.dims.Y / 1f);
      Globals.monkeyPos = new Vector2(this.pos.X + this.dims.X / 2f, this.pos.Y + this.dims.Y / 2f);
      if (this.collisionBottom && !this.previousCollisionBottom)
      {
        this.landInstance.Play();
        this.landing = true;
      }
      else if (this.grip && !this.previousGrip)
        this.sounds["Monkey_Land_Vines"].Play();
      if (this.holdOn)
        this.animationManager.PlayAnimation(this.animations["Bumerang_Travel"], SpriteEffects.None);
      else if ((double) this.velocity.Y == 0.0 && this.grip && this.gripRight)
        this.animationManager.PlayAnimation(this.animations["Climb"], SpriteEffects.None, false, 3);
      else if (this.grip && this.gripRight)
      {
        this.animationManager.PlayAnimation(this.animations["Climb"], SpriteEffects.None);
        this.audioManager.PlaySoundEffectIteration(this.soundIterations["Monkey_Climb_Vines"]);
      }
      else if ((double) this.velocity.Y == 0.0 && this.grip && !this.gripRight)
        this.animationManager.PlayAnimation(this.animations["Climb"], SpriteEffects.FlipHorizontally, false, 3);
      else if (this.grip && !this.gripRight)
      {
        this.animationManager.PlayAnimation(this.animations["Climb"], SpriteEffects.FlipHorizontally);
        this.audioManager.PlaySoundEffectIteration(this.soundIterations["Monkey_Climb_Vines"]);
      }
      else if ((double) this.partner.catapultTimer <= 2.0 && (double) this.partner.catapultTimer > 1.7000000476837158)
        this.animationManager.PlayAnimation(this.animations["Throw"], SpriteEffects.None, false);
      else if (this.contactedBox != null && this.push && (double) this.contactedBox.pos.X > (double) this.pos.X && (double) this.velocity.X > 0.0)
        this.animationManager.PlayAnimation(this.animations["Push"], SpriteEffects.None);
      else if (this.contactedBox != null && this.push && (double) this.contactedBox.pos.X > (double) this.pos.X && (double) this.velocity.X < 0.0)
        this.animationManager.PlayAnimation(this.animations["Pull"], SpriteEffects.None);
      else if (this.contactedBox != null && this.push && (double) this.contactedBox.pos.X < (double) this.pos.X && (double) this.velocity.X < 0.0)
        this.animationManager.PlayAnimation(this.animations["Push"], SpriteEffects.FlipHorizontally);
      else if (this.contactedBox != null && this.push && (double) this.contactedBox.pos.X < (double) this.pos.X && (double) this.velocity.X > 0.0)
        this.animationManager.PlayAnimation(this.animations["Pull"], SpriteEffects.FlipHorizontally);
      else if (this.contactedBox != null && this.push && (double) this.contactedBox.pos.X > (double) this.pos.X && (double) this.velocity.X == 0.0)
        this.animationManager.PlayAnimation(this.animations["Push"], SpriteEffects.None, false, 3);
      else if (this.contactedBox != null && this.push && (double) this.contactedBox.pos.X < (double) this.pos.X && (double) this.velocity.X == 0.0)
        this.animationManager.PlayAnimation(this.animations["Push"], SpriteEffects.FlipHorizontally, false, 3);
      else if (this.pullingLever)
        this.animationManager.PlayAnimation(this.animations["Pull"], SpriteEffects.None);
      else if ((double) this.velocity.X > 0.0 && this.collisionBottom)
      {
        this.animationManager.PlayAnimation(this.animations["Run"], SpriteEffects.None);
        this.PlayWalkSound(nameof (Monkey));
        this.landing = false;
      }
      else if ((double) this.velocity.X < 0.0 && this.collisionBottom)
      {
        this.animationManager.PlayAnimation(this.animations["Run"], SpriteEffects.FlipHorizontally);
        this.PlayWalkSound(nameof (Monkey));
        this.landing = false;
      }
      else if ((double) this.velocity.Y < 0.0 && !this.collisionBottom && (double) this.previousVelocity.X > 0.0)
        this.animationManager.PlayAnimation(this.animations["Jump"], SpriteEffects.None, false);
      else if ((double) this.velocity.Y < 0.0 && !this.collisionBottom && (double) this.previousVelocity.X < 0.0)
        this.animationManager.PlayAnimation(this.animations["Jump"], SpriteEffects.FlipHorizontally, false);
      else if ((double) this.velocity.Y > 0.0 && !this.collisionBottom && (double) this.previousVelocity.X > 0.0)
      {
        if (this.animations["Fall"].count - 1 == this.animations["Fall"].activeFrame)
          this.animationManager.PlayAnimation(this.animations["Airborne"], SpriteEffects.None);
        else
          this.animationManager.PlayAnimation(this.animations["Fall"], SpriteEffects.None, false);
      }
      else if ((double) this.velocity.Y > 0.0 && !this.collisionBottom && (double) this.previousVelocity.X < 0.0)
      {
        if (this.animations["Fall"].count - 1 == this.animations["Fall"].activeFrame)
          this.animationManager.PlayAnimation(this.animations["Airborne"], SpriteEffects.FlipHorizontally);
        else
          this.animationManager.PlayAnimation(this.animations["Fall"], SpriteEffects.FlipHorizontally, false);
      }
      else if ((double) this.velocity.Y > 0.0 && !this.collisionBottom && (double) this.previousVelocity.X == 0.0)
      {
        if (this.animations["Fall"].count - 1 == this.animations["Fall"].activeFrame)
          this.animationManager.PlayAnimation(this.animations["Airborne"], SpriteEffects.None);
        else
          this.animationManager.PlayAnimation(this.animations["Fall"], SpriteEffects.None, false);
      }
      else if (this.landInstance.State == SoundState.Playing && this.landing && (double) this.previousVelocity.X > 0.0)
        this.animationManager.PlayAnimation(this.animations["Land"], SpriteEffects.None, false);
      else if (this.landInstance.State == SoundState.Playing && this.landing && (double) this.previousVelocity.X < 0.0)
        this.animationManager.PlayAnimation(this.animations["Land"], SpriteEffects.FlipHorizontally, false);
      else if (this.landInstance.State == SoundState.Playing && this.landing && (double) this.previousVelocity.X == 0.0)
        this.animationManager.PlayAnimation(this.animations["Land"], SpriteEffects.None, false);
      else if ((double) this.previousVelocity.X < 0.0 && Globals.nextGate.monkeyTrigger)
        this.animationManager.PlayAnimation(this.animations["Idle"], SpriteEffects.FlipHorizontally, false, 6);
      else if ((double) this.previousVelocity.X > 0.0 && Globals.nextGate.monkeyTrigger)
        this.animationManager.PlayAnimation(this.animations["Idle"], SpriteEffects.None, false, 6);
      else if ((double) this.previousVelocity.X < 0.0 && !this.sequence)
      {
        this.animationManager.PlayAnimation(this.animations["Idle"], SpriteEffects.FlipHorizontally);
        this.audioManager.PlaySoundEffectIteration((SoundIteration) null);
      }
      else if (!this.sequence)
      {
        this.animationManager.PlayAnimation(this.animations["Idle"], SpriteEffects.None);
        this.audioManager.PlaySoundEffectIteration((SoundIteration) null);
      }
      this.animationManager.pos = this.pos;
      this.animationManager.Update(deltaSeconds);
      if ((double) this.velocity.X < 0.0)
        this.previousVelocity.X = -1f;
      else if ((double) this.velocity.X > 0.0)
        this.previousVelocity.X = 1f;
      this.previousCollisionBottom = this.collisionBottom;
      this.previousGrip = this.grip;
    }

    public override void Draw()
    {
      if (!Globals.debugMode)
      {
        this.animationManager.Draw();
      }
      else
      {
        Globals.spriteBatch.Draw(Globals.rect, this.BoxCollider("climb-down"), Color.LightPink);
        Globals.spriteBatch.Draw(Globals.rect, this.BoxCollider("climb-up"), Color.DeepPink);
        Globals.spriteBatch.Draw(Globals.rect, this.BoxCollider(), Color.Violet);
      }
    }

    public void AnimationRun(SpriteEffects spriteEffects)
    {
      if ((double) Monkey.timer < 0.25)
        Monkey.index = 0;
      else if ((double) Monkey.timer < 0.5)
        Monkey.index = 1;
      else if ((double) Monkey.timer < 0.75)
        Monkey.index = 2;
      else if ((double) Monkey.timer < 1.0)
        Monkey.index = 3;
      else if ((double) Monkey.timer < 1.25)
        Monkey.index = 4;
      Globals.spriteBatch.Draw(this.sprite, new Rectangle((int) this.pos.X, (int) ((double) this.pos.Y + 20.0), (int) this.dims.X, (int) this.dims.Y + 20), new Rectangle?(new Rectangle((int) ((double) this.dims.X * 2.0) * Monkey.index, 0, (int) ((double) this.dims.X * 2.0), (int) ((double) this.dims.Y * 1.5))), Color.White, this.rotation, this.origin, spriteEffects, 0.0f);
      Monkey.timer += Globals.deltaTime;
      if ((double) Monkey.timer < 1.25)
        return;
      Monkey.timer = 0.0f;
    }
  }
}
