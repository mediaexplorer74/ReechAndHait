
// Type: ReachHigh.Shared.Tutorial
// Assembly: ReachHigh.Windows, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E7AAE051-E6AD-4505-A6E0-970B36D3CBD7
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.IO;
using Windows.Storage;

#nullable disable
namespace ReachHigh.Shared
{
  public class Tutorial : State
  {
    public Monkey monkey;
    public Bumerang bumerang;
    public Target target;
    public List<Sprite> collidingEnvironment;
    public List<Sprite> specialEnvironment;
    public List<Sprite> environment;
    public List<Sprite> foreEnvironment;
    public List<Box> boxes;
    public List<Lever> levers;
    public List<Bar> bars;
    public List<RadialSound> radialSounds;
    private SoundEffectInstance gateOpenInstance;
    private SoundEffectInstance gateCloseInstance;
    public static SoundEffectInstance fallingInstance;
    private float sequenceTimer;
    private DialogManager dialogManager;
    private string[,,] worldTexturePath;
    private string[,,] worldTextureTransPath;
    private WorldTexture worldTexture;
    private WorldTexture worldTextureTrans;
    public List<Rectangle> debugPrompts;

    public Gecko gecko { get; set; }

    public Tutorial(StateManager STATEMANAGER, bool STARTSEQUENCE)
      : base(STATEMANAGER)
    {
      this.collidingEnvironment = new List<Sprite>();
      this.specialEnvironment = new List<Sprite>();
      this.environment = new List<Sprite>();
      this.foreEnvironment = new List<Sprite>();
      this.boxes = new List<Box>();
      this.levers = new List<Lever>();
      this.bars = new List<Bar>();
      this.radialSounds = new List<RadialSound>();
      this.debugPrompts = new List<Rectangle>();
      this.gateOpenInstance = Globals.audioManager.sounds["Gate_Open"].CreateInstance();
      this.gateCloseInstance = Globals.audioManager.sounds["Gate_Close"].CreateInstance();
      Tutorial.fallingInstance = Globals.audioManager.sounds["Falling"].CreateInstance();
      this.StartSequence = STARTSEQUENCE;
      this.SetRadialSounds();
      this.SetWorldTextures();
      Worldbuilding.CollidingEnvironment = this.collidingEnvironment;
      Worldbuilding.DrawEnvironment = this.environment;
      Worldbuilding.CreateCollider(new Vector2(0.0f, 900f), 1200f, 1680f, "floor 1", "Grass");
      Worldbuilding.CreateCollider(new Vector2(1680f, 780f), 1320f, 1470f, "floor 2", "Grass");
      Worldbuilding.CreateCollider(new Vector2(3240f, 900f), 1200f, 360f, "floor 3", "");
      Worldbuilding.CreateCollider(new Vector2(3600f, 1160f), 960f, 420f, "floor 4", "");
      Worldbuilding.CreateCollider(4020f, 980f, 1000f, 140f, "floor 8", "");
      Worldbuilding.CreateCollider(new Vector2(4160f, 980f), 1120f, 1900f, "floor 5", "Grass");
      Worldbuilding.CreateCollider(new Vector2(5900f, 680f), 1440f, 1140f, "floor 6", "Grass");
      Worldbuilding.CreateCollider(3150f, 780f, 1000f, 90f, "floor 7", "");
      Worldbuilding.CreateCollider(new Vector2(2870f, -140f), 800f, 370f, "platform 2", "Grass");
      Worldbuilding.CreateCollider(new Vector2(3170f, -180f), 780f, 180f, "platform 3", "Grass");
      Worldbuilding.CreateCollider(3350f, -180f, 780f, 190f, "platform 7", "");
      Worldbuilding.CreateCollider(new Vector2(3540f, 180f), 420f, 360f, "platform 4", "");
      Worldbuilding.CreateCollider(new Vector2(3900f, 0.0f), 600f, 130f, "platform 5", "");
      Worldbuilding.CreateCollider(4030f, 0.0f, 600f, 240f, "platform 6", "Grass");
      Worldbuilding.CreateClimbWall(new Vector2(2870f, 240f), 360f, "climb 1");
      Worldbuilding.CreateStickyCeiling(new Vector2(2920f, 600f), 1280f, "sticky 1");
      Worldbuilding.CreateCollider(new Vector2(-1210f, -120f), 2220f, 1210f, "ceiling 1", "");
      Worldbuilding.CreateCollider(new Vector2(0.0f, -120f), 240f, 240f, "ceiling 2", "");
      Worldbuilding.CreateCollider(new Vector2(930f, -120f), 150f, 600f, "ceiling 3", "");
      Worldbuilding.CreateCollider(new Vector2(1500f, -1080f), 1140f, 780f, "ceiling 4", "");
      Worldbuilding.CreateCollider(new Vector2(2220f, -1080f), 1470f, 330f, "ceiling 5", "");
      Worldbuilding.CreateCollider(new Vector2(2520f, -1080f), 480f, 2160f, "ceiling 7", "");
      Worldbuilding.CreateCollider(4680f, -1080f, 960f, 360f, "ceiling 6", "");
      Worldbuilding.CreateCollider(new Vector2(5040f, -1080f), 1380f, 3120f, "ceiling 8", "");
      Worldbuilding.CreateCollider(new Vector2(6420f, 280f), 60f, 1200f, "ceiling 9", "");
      Worldbuilding.CreateClimbWall(new Vector2(2490f, -180f), 360f, "climb 2");
      Worldbuilding.CreateStickyCeiling(new Vector2(5440f, 280f), 980f, "sticky 2");
      TutorialPrompt.ResetManager();
      this.debugPrompts.Add(new Rectangle(1180, 780, 500, 120));
      TutorialPrompt tutorialPrompt1 = new TutorialPrompt(new Rectangle(1180, 780, 500, 120), TutorialPrompts.Jump);
      this.debugPrompts.Add(new Rectangle(0, 780, 1120, 120));
      TutorialPrompt tutorialPrompt2 = new TutorialPrompt(new Rectangle(0, 780, 1120, 120), TutorialPrompts.Walk);
      this.debugPrompts.Add(new Rectangle(3000, 1000, 1120, 120));
      TutorialPrompt tutorialPrompt3 = new TutorialPrompt(new Rectangle(3000, 1000, 1120, 120), TutorialPrompts.ThrowGecko);
      this.debugPrompts.Add(new Rectangle(5360, 850, 1120, 120));
      TutorialPrompt tutorialPrompt4 = new TutorialPrompt(new Rectangle(5360, 850, 1120, 120), TutorialPrompts.ThrowGecko);
      this.debugPrompts.Add(new Rectangle(5900, 540, 300, 120));
      TutorialPrompt tutorialPrompt5 = new TutorialPrompt(new Rectangle(5900, 540, 300, 120), TutorialPrompts.Boomerang);
      this.debugPrompts.Add(new Rectangle(6420, 540, 120, 120));
      TutorialPrompt tutorialPrompt6 = new TutorialPrompt(new Rectangle(6420, 540, 120, 120), TutorialPrompts.Interact);
      GateAnimation.ResetManager();
      Globals.nextGate = new Gate("Tileset\\Gate", new Vector2(6540f, 120f), new Vector2(180f, 600f), "right", seconds: 8f);
      this.sequenceTimer = 10f;
      Camera.scale = 1.35f;
      this.monkey = new Monkey("Animations\\Monkey\\monkey_idle", 
          new Vector2(350f, 550f), new Vector2(100f, 160f), this.collidingEnvironment, this.boxes, this.levers, this.bars);
      this.gecko = new Gecko("Animations\\Gecko\\gecko_idle",
          new Vector2(850f, 600f), new Vector2(80f, 100f), this.collidingEnvironment, this.levers, this.bars);
      this.bumerang = new Bumerang("Animations\\Gecko\\bumerang_idle", 
          this.gecko.pos, new Vector2(40f, 40f), this.gecko, this.collidingEnvironment);
      this.target = new Target("Tileset\\Cursor", new Vector2(450f, 100f), new Vector2(30f, 30f));
      this.dialogManager = new DialogManager(this.gecko, this.monkey);
      this.monkey.partner = this.gecko;
      this.gecko.partner = this.monkey;
      this.monkey.bumerang = this.bumerang;
      foreach (Sprite sprite in this.specialEnvironment)
        this.collidingEnvironment.Add(sprite);
      this.collidingEnvironment.Add((Sprite) this.gecko);
      this.collidingEnvironment.Add((Sprite) this.monkey);
      this.foreEnvironment.Add((Sprite) this.gecko);
      this.foreEnvironment.Add((Sprite) this.monkey);
      this.collidingEnvironment.Add((Sprite) Globals.nextGate);
      if (this.StartSequence)
        return;
      Globals.audioManager.PlaySoundSong(Globals.audioManager.soundSongs["TutorialTheme"], Globals.audioManager.soundSongs["TutorialTheme_Loop"]);
    }

    private void SetRadialSounds()
    {
      this.radialSounds.Add(new RadialSound(Globals.audioManager.sounds["Birds"], new Vector2(550f, -320f), 7500f));
    }

    private void SetWorldTextures()
    {
      this.worldTexturePath = new string[4, 2, 3]
      {
        {
          {
            "Stage\\Stage1\\stage_1_0_(0,0,0)",
            "Stage\\Stage1\\stage_1_0_(0,0,1)",
            "Stage\\Stage1\\stage_1_0_(0,1,0)"
          },
          {
            "Stage\\Stage1\\stage_1_0_(0,1,1)",
            "Stage\\Stage1\\stage_1_0_(1,0,0)",
            "Stage\\Stage1\\stage_1_0_(1,0,1)"
          }
        },
        {
          {
            "Stage\\Stage1\\stage_1_1_(0,0,0)",
            "Stage\\Stage1\\stage_1_1_(0,0,1)",
            "Stage\\Stage1\\stage_1_1_(0,1,0)"
          },
          {
            "Stage\\Stage1\\stage_1_1_(0,1,1)",
            "Stage\\Stage1\\stage_1_1_(1,0,0)",
            "Stage\\Stage1\\stage_1_1_(1,0,1)"
          }
        },
        {
          {
            "Stage\\Stage1\\stage_1_2_(0,0,0)",
            "Stage\\Stage1\\stage_1_2_(0,0,1)",
            "Stage\\Stage1\\stage_1_2_(0,1,0)"
          },
          {
            "Stage\\Stage1\\stage_1_2_(0,1,1)",
            "Stage\\Stage1\\stage_1_2_(1,0,0)",
            "Stage\\Stage1\\stage_1_2_(1,0,1)"
          }
        },
        {
          {
            "Stage\\Stage1\\stage_1_3_(0,0,0)",
            "Stage\\Stage1\\stage_1_3_(0,0,1)",
            "Stage\\Stage1\\stage_1_3_(0,1,0)"
          },
          {
            "Stage\\Stage1\\stage_1_3_(0,1,1)",
            "Stage\\Stage1\\stage_1_3_(1,0,0)",
            "Stage\\Stage1\\stage_1_3_(1,0,1)"
          }
        }
      };
      this.worldTextureTransPath = new string[4, 6, 1]
      {
        {
          {
            "Stage\\Stage1\\fall_0_(0,0,0)"
          },
          {
            "Stage\\Stage1\\fall_0_(0,0,1)"
          },
          {
            "Stage\\Stage1\\fall_0_(0,1,0)"
          },
          {
            "Stage\\Stage1\\fall_0_(0,1,1)"
          },
          {
            "Stage\\Stage1\\fall_0_(1,0,0)"
          },
          {
            "Stage\\Stage1\\fall_0_(1,0,1)"
          }
        },
        {
          {
            "Stage\\Stage1\\fall_1_(0,0,0)"
          },
          {
            "Stage\\Stage1\\fall_1_(0,0,1)"
          },
          {
            "Stage\\Stage1\\fall_1_(0,1,0)"
          },
          {
            "Stage\\Stage1\\fall_1_(0,1,1)"
          },
          {
            "Stage\\Stage1\\fall_1_(1,0,0)"
          },
          {
            "Stage\\Stage1\\fall_1_(1,0,1)"
          }
        },
        {
          {
            "Stage\\Stage1\\fall_2_(0,0,0)"
          },
          {
            "Stage\\Stage1\\fall_2_(0,0,1)"
          },
          {
            "Stage\\Stage1\\fall_2_(0,1,0)"
          },
          {
            "Stage\\Stage1\\fall_2_(0,1,1)"
          },
          {
            "Stage\\Stage1\\fall_2_(1,0,0)"
          },
          {
            "Stage\\Stage1\\fall_2_(1,0,1)"
          }
        },
        {
          {
            "Stage\\Stage1\\fall_3_(0,0,0)"
          },
          {
            "Stage\\Stage1\\fall_3_(0,0,1)"
          },
          {
            "Stage\\Stage1\\fall_3_(0,1,0)"
          },
          {
            "Stage\\Stage1\\fall_3_(0,1,1)"
          },
          {
            "Stage\\Stage1\\fall_3_(1,0,0)"
          },
          {
            "Stage\\Stage1\\fall_3_(1,0,1)"
          }
        }
      };
      this.worldTexture = new WorldTexture(this.worldTexturePath, new Vector2(-463f, -2084f), new Vector2(8346f, 3774f));
      this.worldTextureTrans = new WorldTexture(this.worldTextureTransPath, 
          new Vector2(0.0f, 0.0f), new Vector2(0.0f, 0.0f));
      this.worldTextureTrans.offpos = new Vector2(5973f, 779f);
      this.worldTextureTrans.offdims = new Vector2(3880f, 12073f);
    }

    public override void Update(float deltaSeconds)
    {
      StorageFolder AppFolder = ApplicationData.Current.LocalFolder;

      if (this.StartSequence)
      {
        Camera.locked = false;
        this.monkey.stunned = true;
        this.gecko.stunned = true;
        Globals.IsSequenceActive = true;
        if ((double) this.sequenceTimer > -2.0)
          this.sequenceTimer -= deltaSeconds;
        if ((double) this.sequenceTimer < 6.0 && (double) this.sequenceTimer > 0.0)
          Camera.pos = -new Vector2(550f, (float) (820.0 - (double) this.sequenceTimer * 190.0));
        else if ((double) this.sequenceTimer > 6.0)
        {
          Camera.pos = -new Vector2(550f, -320f);
          this.monkey.sequence = true;
          this.monkey.animationManager.PlayAnimation(this.monkey.animations["Throw"], 
              SpriteEffects.None, false, 1, true);
          Titlescreen.treeBreak.Volume = (float) (((double) this.sequenceTimer - 6.0) / 8.0);
        }
        else if ((double) this.sequenceTimer < 0.0 && (double) this.sequenceTimer > -1.0)
        {
          this.gecko.sequence = true;
          this.gecko.pos.X -= 200f * deltaSeconds;
          this.gecko.animationManager.PlayAnimation(this.gecko.animations["Run"], SpriteEffects.FlipHorizontally);
          this.monkey.animationManager.PlayAnimation(this.monkey.animations["Idle"], SpriteEffects.None, false, 6, true);
          this.monkey.animationManager.PlayAnimation(this.monkey.animations["Throw"], 
              SpriteEffects.None, false, ONEFRAME: true);
        }
        else
        {
          this.gecko.sequence = false;
          this.monkey.sequence = false;
        }
        if ((double) this.sequenceTimer <= -2.0)
        {
          if (!this.dialogManager.hasEnded)
            this.dialogManager.Update(deltaSeconds, "Tutorial_Intro");
          if (this.dialogManager.hasEnded)
          {
            this.dialogManager.Reset();
            this.StartSequence = false;
            this.gecko.stunned = false;
            this.monkey.stunned = false;
            Camera.locked = true;
            Globals.audioManager.PlaySoundSong(Globals.audioManager.soundSongs["TutorialTheme"],
                Globals.audioManager.soundSongs["TutorialTheme_Loop"]);
            Globals.IsSequenceActive = false;
          }
        }
      }
      this.gecko.Update(deltaSeconds);
      this.monkey.Update(deltaSeconds);
      this.target.Update(deltaSeconds);
      this.bumerang.Update(deltaSeconds);
      foreach (Sprite box in this.boxes)
        box.Update(deltaSeconds);
      foreach (RadialSound radialSound in this.radialSounds)
        radialSound.Update();
      if ((double) Globals.nextGate.timer > 0.0)
        Globals.nextGate.Update(deltaSeconds);
      if (Globals.nextGate.activated)
      {
        this.gecko.sequence = false;
        this.monkey.sequence = false;
        if ((double) Globals.nextGate.timer < 8.0 && (double) Globals.nextGate.timer > 7.6999998092651367)
        {
          this.gateOpenInstance.Play();
          Globals.nextGate.OpenGate();
        }
        if (Globals.nextGate.type == "right" && (double) Globals.nextGate.timer < 6.5 
                    && (double) Globals.nextGate.timer > 4.7)
        {
          this.gecko.pos.X += 350f * deltaSeconds;
          this.gecko.animationManager.PlayAnimation(this.gecko.animations["Run"], SpriteEffects.None);
          this.gecko.sequence = true;
          this.monkey.pos.X += 360f * deltaSeconds;
          this.monkey.animationManager.PlayAnimation(this.monkey.animations["Run"], SpriteEffects.None);
          this.monkey.sequence = true;
        }
        if ((double) Globals.nextGate.timer < 4.6999998092651367 && (double) Globals.nextGate.timer > 4.0)
        {
          Tutorial.fallingInstance.Play();
          Globals.audioManager.FadeOutSoundSong(4f);
        }
        this.gecko.stunned = true;
        this.monkey.stunned = true;
        if ((double) Globals.nextGate.timer <= 0.0)
        {
          this.gecko.stunned = false;
          this.monkey.stunned = false;
          this.stateManager.StartState((State) new Cave(this.stateManager, true), AUDIOFADE: false);
          Globals.audioManager.StopSoundSong();

          using (StreamWriter streamWriter = new StreamWriter(File.OpenRead(AppFolder.Path + "/" + "SaveData.txt")))
            streamWriter.Write(1);
        }
      }
      if (!Globals.IsSequenceActive && (this.gecko.InputController.TogglePause() 
                || this.monkey.InputController.TogglePause()))
        this.stateManager.StartState((State) new Pause(this.stateManager), false, false);
      base.Update(deltaSeconds);
    }

    public override void Draw()
    {
      for (int layer = 0; layer < 2; ++layer)
        this.worldTextureTrans.Draw(layer);
      for (int layer = 0; layer < 2; ++layer)
        this.worldTexture.Draw(layer);
      foreach (Sprite sprite in this.environment)
        sprite.Draw();
      foreach (Sprite sprite in this.collidingEnvironment)
      {
        switch (sprite)
        {
          case Monkey _:
          case Gecko _:
            continue;
          default:
            sprite.Draw();
            continue;
        }
      }
      this.worldTextureTrans.Draw(2);
      this.worldTexture.Draw(2);
      this.monkey.Draw();
      this.gecko.Draw();
      this.worldTextureTrans.Draw(3);
      this.worldTexture.Draw(3);
      if (Target.active)
        this.target.Draw();
      if (Bumerang.loose && !this.monkey.holdOn)
        this.bumerang.Draw();
      if ((double) Globals.nextGate.timer < 5.0)
        Globals.spriteBatch.Draw(Globals.rect, new Rectangle((int) Camera.mid.X - Main.screenWidth * 5, (int) Camera.mid.Y - Main.screenHeight * 5, Main.screenWidth * 10, Main.screenHeight * 10), new Rectangle?(), Color.Black * (float) ((5.0 - (double) Globals.nextGate.timer) / 3.0), 0.0f, Vector2.Zero, SpriteEffects.None, 0.0f);
      if (this.StartSequence)
        Globals.spriteBatch.Draw(Globals.rect, new Rectangle((int) Camera.mid.X - Main.screenWidth * 5, (int) Camera.mid.Y - Main.screenHeight * 5, Main.screenWidth * 10, Main.screenHeight * 10), new Rectangle?(), Color.Black * (float) (((double) this.sequenceTimer - 6.0) / 4.0), 0.0f, Vector2.Zero, SpriteEffects.None, 0.0f);
      if (this.dialogManager.active)
        this.dialogManager.Draw(this.gecko.pos + this.gecko.dims / 2f, this.monkey.pos + this.monkey.dims / 2f);
      base.Draw();
      GateAnimation.DrawManager(Globals.spriteBatch, Globals.DrawTime);
      TutorialPrompt.DrawManager(Globals.spriteBatch, Globals.DrawTime);
    }
  }
}
