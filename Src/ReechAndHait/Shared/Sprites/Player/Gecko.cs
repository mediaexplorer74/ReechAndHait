
// Type: ReachHigh.Shared.Gecko
// Assembly: ReachHigh.Windows, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E7AAE051-E6AD-4505-A6E0-970B36D3CBD7
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

#nullable disable
namespace ReachHigh.Shared
{
  public class Gecko : Player
  {
    private bool stick;
    private bool previousStick;
    public Monkey partner;
    public float catapultTimer;
    private SoundEffectInstance tossInstance;

    public Gecko(
      string PATH,
      Vector2 POS,
      Vector2 DIMS,
      List<Sprite> COLLIDIING_ENVIRONMENT,
      List<Lever> LEVERS,
      List<Bar> BARS)
      : base(PATH, POS, DIMS, COLLIDIING_ENVIRONMENT, LEVERS, BARS)
    {
      this.jumpForce = 2050f;
      this.partner = (Monkey) null;
      this.SetAnimations();
      this.animationManager = new AnimationManager(this.animations["Idle"]);
      this.animationManager.pos = this.pos;
      this.SetSoundEffects();
      this.audioManager = new AudioManager(this.sounds);
      this.landInstance = this.sounds["Gecko_Land_Stone"].CreateInstance();
      this.InputController = new InputController(RhPlayerSettings.Get<InputMode>(Players.Gecko, "InputMode"), RhPlayerSettings.Get<PlayerIndex>(Players.Gecko, "GamepadIndex"));
      RhPlayerSettings.Set(Players.Gecko, "InputController", (object) this.InputController);
    }

    private void SetAnimations()
    {
      this.animations = new Dictionary<string, Animation>();
      this.animations["Idle"] = new Animation(this.sprite, 6, 0.12f, new Vector2(1f, 1.2f), 40);
      this.animations["Run"] = new Animation(Globals.content.Load<Texture2D>("Animations\\Gecko\\gecko_run"), 5, 0.12f, new Vector2(1f, 1.2f), 40);
      this.animations["Jump"] = new Animation(Globals.content.Load<Texture2D>("Animations\\Gecko\\gecko_jump"), 3, 0.04f, new Vector2(1f, 1.2f));
      this.animations["Fall"] = new Animation(Globals.content.Load<Texture2D>("Animations\\Gecko\\gecko_fall"), 3, 0.1f, new Vector2(1f, 1.2f));
      this.animations["Stick"] = new Animation(Globals.content.Load<Texture2D>("Animations\\Gecko\\gecko_climb"), 6, 0.12f, new Vector2(1f, 1.2f));
      this.animations["Bumerang_Throw"] = new Animation(Globals.content.Load<Texture2D>("Animations\\Gecko\\gecko_boomerang_throw"), 6, 0.06f, new Vector2(1f, 1.2f), 40);
      this.animations["Bumerang_Idle"] = new Animation(Globals.content.Load<Texture2D>("Animations\\Gecko\\gecko_boomerang_idle"), 4, 0.12f, new Vector2(1f, 1.2f), 40);
      this.animations["Airborne"] = new Animation(Globals.content.Load<Texture2D>("Animations\\Gecko\\gecko_airborne"), 5, 0.1f, new Vector2(1f, 1.2f));
      this.animations["Land"] = new Animation(Globals.content.Load<Texture2D>("Animations\\Gecko\\gecko_landing"), 6, 0.15f, new Vector2(1f, 1.2f), 40);
      this.animations["Toss"] = new Animation(Globals.content.Load<Texture2D>("Animations\\Gecko\\gecko_toss"), 3, 0.1f, new Vector2(1f, 1.2f));
      this.animations["Toss_T"] = new Animation(Globals.content.Load<Texture2D>("Animations\\Gecko\\gecko_toss_transition"), 2, 0.1f, new Vector2(1f, 1.2f));
    }

    private void SetSoundEffects()
    {
      this.sounds = new Dictionary<string, SoundEffect>();
      this.LoadActionSound(nameof (Gecko), 2, new string[2]
      {
        "2 Jump",
        "3 Land"
      }, new string[3]{ "Stone", "Grass", "Water" });
      this.sounds["Gecko_Land_Ceiling"] = Globals.content.Load<SoundEffect>("Audio\\1 Character Sounds\\2 Gecko\\3 Land\\G_Land_Ceiling");
      this.sounds["Gecko_Bumerang_Fly"] = Globals.content.Load<SoundEffect>("Audio\\1 Character Sounds\\2 Gecko\\4 Boomerang\\G_Boomerang_Flying_Loop");
      this.sounds["Gecko_Bumerang_Collision"] = Globals.content.Load<SoundEffect>("Audio\\1 Character Sounds\\2 Gecko\\4 Boomerang\\G_Boomerang_Collision");
      this.sounds["Gecko_Bumerang_Catch"] = Globals.content.Load<SoundEffect>("Audio\\1 Character Sounds\\2 Gecko\\4 Boomerang\\G_Boomerang_Catch");
      this.tossInstance = Globals.content.Load<SoundEffect>("Audio\\1 Character Sounds\\2 Gecko\\5 Toss\\G_Toss").CreateInstance();
      List<SoundEffect> SOUNDEFFECTS1 = new List<SoundEffect>();
      List<SoundEffect> SOUNDEFFECTS2 = new List<SoundEffect>();
      List<SoundEffect> SOUNDEFFECTS3 = new List<SoundEffect>();
      List<SoundEffect> SOUNDEFFECTS4 = new List<SoundEffect>();
      for (int index = 1; index <= 12; ++index)
      {
        SOUNDEFFECTS2.Add(Globals.content.Load<SoundEffect>(string.Format("Audio\\1 Character Sounds\\2 Gecko\\1 Walk\\1 Grass\\G_Walk_Grass_{0}", (object) index)));
        SOUNDEFFECTS1.Add(Globals.content.Load<SoundEffect>(string.Format("Audio\\1 Character Sounds\\2 Gecko\\1 Walk\\2 Stone\\G_Walk_Stone_{0}", (object) index)));
        SOUNDEFFECTS3.Add(Globals.content.Load<SoundEffect>(string.Format("Audio\\1 Character Sounds\\2 Gecko\\1 Walk\\4 Water\\G_Walk_Water_{0}", (object) index)));
        SOUNDEFFECTS4.Add(Globals.content.Load<SoundEffect>(string.Format("Audio\\1 Character Sounds\\2 Gecko\\1 Walk\\3 Ceiling\\G_Walk_Ceiling_{0}", (object) index)));
      }
      this.soundIterations = new Dictionary<string, SoundIteration>();
      this.soundIterations["Gecko_Walk_Stone"] = new SoundIteration(SOUNDEFFECTS1, 12, 0.3f);
      this.soundIterations["Gecko_Walk_Grass"] = new SoundIteration(SOUNDEFFECTS2, 12, 0.3f);
      this.soundIterations["Gecko_Walk_Water"] = new SoundIteration(SOUNDEFFECTS3, 12, 0.3f);
      this.soundIterations["Gecko_Walk_Ceiling"] = new SoundIteration(SOUNDEFFECTS4, 12, 0.3f);
    }

    public override void Update(float deltaSeconds)
    {
      this.velocity = Vector2.Zero;
      if (this.sequence)
        this.stunned = true;
      if (!this.stunned)
      {
        if ((double) this.InputController.Move().X < -0.5)
          this.velocity.X = -this.speed;
        if ((double) this.InputController.Move().X > 0.5)
          this.velocity.X = this.speed;
        bool isFirstInput;
        if (this.InputController.Jump(out isFirstInput) & isFirstInput && this.jumpAvailable && (double) this.catapultTimer <= 0.0)
        {
          this.inJump = true;
          this.jumpAvailable = false;
          this.jumpInterrupt = false;
          this.jumpTimer = 0.5f;
          this.PlayActionSound(nameof (Gecko), "Jump");
          this.landInstance = this.GetActionSound(nameof (Gecko), "Land");
        }
        else if (this.InputController.Jump() && this.inJump && !this.jumpInterrupt)
          this.jumpBoost = 1;
        else if (this.inJump)
        {
          this.jumpBoost = 0;
          this.jumpInterrupt = true;
        }
      }
      if (this.InputController.GeckoInteract(false))
        this.inputActivate = true;
      if (this.InputController.DebugFly())
        this.debugFly = true;
      if ((double) this.InputController.Move().Y < -0.5)
        this.stick = false;
      if (this.stick)
        this.velocity.Y -= 1f * Globals.gravitation;
      if ((double) this.catapultTimer > 0.0)
        this.catapultTimer -= deltaSeconds;
      this.velocity.Y -= 1350f * this.catapultTimer;
      base.Update(deltaSeconds);
      if (this.InputController.GeckoBumerangStance() == ButtonState.Pressed && !Bumerang.loose && this.collisionBottom && this.bumerangPossiblility && !this.stunned)
      {
        Target.active = true;
        this.stunned = true;
      }
      else if (this.InputController.GeckoBumerangStance() == ButtonState.Released && !Bumerang.loose)
      {
        Target.active = false;
        this.stunned = false;
      }
      if (this.InputController.GeckoInteract(false) && this.BoxCollider().Intersects(Globals.nextGate.BoxCollider("gate")) && !Globals.nextGate.geckoTrigger)
      {
        Globals.nextGate.geckoTrigger = true;
        Globals.audioManager.sounds["Gate_Activation_Gecko"].Play();
      }
      else if (!this.InputController.GeckoInteract(false) && Globals.nextGate.geckoTrigger)
        Globals.nextGate.geckoTrigger = false;
      if (this.collisionBottom)
      {
        if ((double) this.velocity.Y > 0.0)
          this.velocity.Y = 0.0f;
        this.jumpAvailable = true;
        this.catapultTimer = 0.0f;
      }
      if (this.collisionTop)
      {
        this.velocity.Y = 0.0f;
        this.inJump = false;
      }
      if (!this.stickRange)
        this.stick = false;
      if (this.collisionLeft)
        this.velocity.X = 0.0f;
      if (this.collisionRight)
        this.velocity.X = 0.0f;
      if (this.specialCollisionTop && this.stickRange)
      {
        this.stick = true;
        this.catapultTimer = 0.0f;
      }
      foreach (Bar bar in this.bars)
      {
        if (this.isCollidingBarBottom((Sprite) bar) || this.isCollidingBarTop((Sprite) bar))
          this.velocity.Y += bar.velocity.Y;
        if (this.isCollidingBarLeft((Sprite) bar) || this.isCollidingBarRight((Sprite) bar))
          this.velocity.X += bar.velocity.X;
      }
      if (this.collisionBottom || this.stick)
      {
        this.accelGrav = 0.5f;
        if (this.tossInstance.State == SoundState.Playing)
          this.tossInstance.Stop();
      }
      if ((double) this.catapultTimer > 0.0)
        this.velocity.X = 0.0f;
      this.pos = this.pos + this.velocity * deltaSeconds;
      if (this.velocity != Vector2.Zero)
        Globals.nextGate.geckoTrigger = false;
      Globals.geckoPos = new Vector2(this.pos.X + this.dims.X / 2f, this.pos.Y + this.dims.Y / 2f);
      if (this.collisionBottom && !this.previousCollisionBottom)
      {
        this.landInstance.Play();
        this.landing = true;
      }
      else if (this.stick && !this.previousStick)
        this.sounds["Gecko_Land_Ceiling"].Play();
      if (Bumerang.loose && (double) Target.mousePos.X > (double) this.pos.X)
      {
        if (this.animations["Bumerang_Throw"].count - 1 == this.animations["Bumerang_Throw"].activeFrame)
          this.animationManager.PlayAnimation(this.animations["Bumerang_Idle"], SpriteEffects.None);
        else
          this.animationManager.PlayAnimation(this.animations["Bumerang_Throw"], SpriteEffects.None, false);
      }
      else if (Bumerang.loose && (double) Target.mousePos.X < (double) this.pos.X)
      {
        if (this.animations["Bumerang_Throw"].count - 1 == this.animations["Bumerang_Throw"].activeFrame)
          this.animationManager.PlayAnimation(this.animations["Bumerang_Idle"], SpriteEffects.FlipHorizontally);
        else
          this.animationManager.PlayAnimation(this.animations["Bumerang_Throw"], SpriteEffects.FlipHorizontally, false);
      }
      else if (Target.active && (double) Target.mousePos.X > (double) this.pos.X)
        this.animationManager.PlayAnimation(this.animations["Bumerang_Throw"], SpriteEffects.None, false, ONEFRAME: true);
      else if (Target.active && (double) Target.mousePos.X < (double) this.pos.X)
        this.animationManager.PlayAnimation(this.animations["Bumerang_Throw"], SpriteEffects.FlipHorizontally, false, ONEFRAME: true);
      else if (this.stick && (double) this.velocity.X > 0.0)
      {
        this.animationManager.PlayAnimation(this.animations["Stick"], SpriteEffects.None);
        this.audioManager.PlaySoundEffectIteration(this.soundIterations["Gecko_Walk_Ceiling"]);
      }
      else if (this.stick && (double) this.velocity.X < 0.0)
      {
        this.animationManager.PlayAnimation(this.animations["Stick"], SpriteEffects.FlipHorizontally);
        this.audioManager.PlaySoundEffectIteration(this.soundIterations["Gecko_Walk_Ceiling"]);
      }
      else if (this.stick && (double) this.velocity.X == 0.0 && (double) this.previousVelocity.X > 0.0)
        this.animationManager.PlayAnimation(this.animations["Stick"], SpriteEffects.None, false, 5);
      else if (this.stick && (double) this.velocity.X == 0.0 && (double) this.previousVelocity.X < 0.0)
        this.animationManager.PlayAnimation(this.animations["Stick"], SpriteEffects.FlipHorizontally, false, 5);
      else if ((double) this.velocity.X > 0.0 && this.collisionBottom)
      {
        this.animationManager.PlayAnimation(this.animations["Run"], SpriteEffects.None);
        this.PlayWalkSound(nameof (Gecko));
        this.landing = false;
      }
      else if ((double) this.velocity.X < 0.0 && this.collisionBottom)
      {
        this.animationManager.PlayAnimation(this.animations["Run"], SpriteEffects.FlipHorizontally);
        this.PlayWalkSound(nameof (Gecko));
        this.landing = false;
      }
      else if ((double) this.catapultTimer > 0.0)
      {
        this.animationManager.PlayAnimation(this.animations["Toss"], SpriteEffects.None);
        this.tossInstance.Play();
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
      else if ((double) this.previousVelocity.X < 0.0 && Globals.nextGate.geckoTrigger)
        this.animationManager.PlayAnimation(this.animations["Idle"], SpriteEffects.FlipHorizontally, false, 5);
      else if ((double) this.previousVelocity.X > 0.0 && Globals.nextGate.geckoTrigger)
        this.animationManager.PlayAnimation(this.animations["Idle"], SpriteEffects.None, false, 5);
      else if ((double) this.previousVelocity.X < 0.0 && !this.sequence)
        this.animationManager.PlayAnimation(this.animations["Idle"], SpriteEffects.FlipHorizontally);
      else if ((double) this.previousVelocity.X > 0.0 && !this.sequence)
        this.animationManager.PlayAnimation(this.animations["Idle"], SpriteEffects.None);
      else if (!this.sequence)
        this.animationManager.PlayAnimation(this.animations["Idle"], SpriteEffects.FlipHorizontally);
      this.animationManager.pos = this.pos;
      this.animationManager.Update(deltaSeconds);
      if ((double) this.velocity.X < 0.0)
        this.previousVelocity.X = -1f;
      else if ((double) this.velocity.X > 0.0)
        this.previousVelocity.X = 1f;
      this.previousCollisionBottom = this.collisionBottom;
      this.previousStick = this.stick;
    }

    public override void Draw()
    {
      if (!Globals.debugMode)
      {
        this.animationManager.Draw();
      }
      else
      {
        Globals.spriteBatch.Draw(Globals.rect, this.BoxCollider("stick"), Color.LightBlue);
        Globals.spriteBatch.Draw(Globals.rect, this.BoxCollider(), Color.Blue);
      }
    }
  }
}
