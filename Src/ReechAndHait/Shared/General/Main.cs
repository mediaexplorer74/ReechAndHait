
// Type: ReachHigh.Shared.Main
// Assembly: ReachHigh.Windows, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E7AAE051-E6AD-4505-A6E0-970B36D3CBD7
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using PigeonProject.Utility;
using ReachHigh.Shared.Source.General;
using System.Collections.Generic;

#nullable disable
namespace ReachHigh.Shared
{
  public class Main : Game
  {
    private GraphicsDeviceManager graphics;
    private StateManager stateManager;
    private AudioManager audioManager;
    private float deltaSeconds;
    public List<Texture2D> grid;
    public static int screenWidth;
    public static int screenHeight;
    public static Main game;
    private Dictionary<string, SoundEffect> sounds;
    private Dictionary<string, Song> songs;
    private Dictionary<string, SoundSong> soundSongs;

    public Main()
    {
      this.graphics = new GraphicsDeviceManager((Game) this);
      this.Content.RootDirectory = "Content";
      Main.game = this;
    }

    protected override void Initialize()
    {
      this.graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
      this.graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
      this.graphics.IsFullScreen = false;//true;
      this.graphics.ApplyChanges();
      Main.screenWidth = this.graphics.PreferredBackBufferWidth;
      Main.screenHeight = this.graphics.PreferredBackBufferHeight;
      base.Initialize();
    }

    protected override void LoadContent()
    {
      Globals.content = this.Content;
      Globals.spriteBatch = new SpriteBatch(this.GraphicsDevice);
      Globals.graphics = this.graphics;
      Globals.LoadContent();
      ContentManagement.LoadContent(this.Content);
      this.SetSoundEffects();
      this.SetSongs();
      this.audioManager = new AudioManager(this.sounds, this.songs, this.soundSongs);
      Globals.audioManager = this.audioManager;
      this.stateManager = new StateManager();
    }

    private void SetSoundEffects()
    {
      this.sounds = new Dictionary<string, SoundEffect>();
      this.sounds["Gate_Open"] = Globals.content.Load<SoundEffect>("Audio\\4 SFX\\3 Door\\SFX_Wood_Door_Open");
      this.sounds["Gate_Close"] = Globals.content.Load<SoundEffect>("Audio\\4 SFX\\3 Door\\SFX_Wood_Door_Close");
      this.sounds["Gate_Activation_Monkey"] = Globals.content.Load<SoundEffect>("Audio\\4 SFX\\2 Char Activate Door\\SFX_M_Activation");
      this.sounds["Gate_Activation_Gecko"] = Globals.content.Load<SoundEffect>("Audio\\4 SFX\\2 Char Activate Door\\SFX_G_Activation");
      this.sounds["Lever"] = Globals.content.Load<SoundEffect>("Audio\\4 SFX\\4 Lever\\SFX_Lever");
      this.sounds["Gate_Unlock"] = Globals.content.Load<SoundEffect>("Audio\\4 SFX\\5 Lever Door Unlocked\\SFX_Lever_Door_Unlock");
      this.sounds["Bar_Move"] = Globals.content.Load<SoundEffect>("Audio\\4 SFX\\7 Big Plattforms\\SFX_Big_Plattforms_Move_Loop");
      this.sounds["Bar_Stop"] = Globals.content.Load<SoundEffect>("Audio\\4 SFX\\7 Big Plattforms\\SFX_Big_Plattforms_Stop");
      this.sounds["Box_Move"] = Globals.content.Load<SoundEffect>("Audio\\4 SFX\\6 Box\\SFX_Box_Move_Loop");
      this.sounds["Box_Stop"] = Globals.content.Load<SoundEffect>("Audio\\4 SFX\\6 Box\\SFX_Box_Stop");
      this.sounds["Menu_Confirm"] = Globals.content.Load<SoundEffect>("Audio\\4 SFX\\1 Menu Select Sounds\\1 confirm\\1 confirm");
      this.sounds["Menu_Undo"] = Globals.content.Load<SoundEffect>("Audio\\4 SFX\\1 Menu Select Sounds\\2 undo\\2 undo_1");
      this.sounds["Menu_Start_Game"] = Globals.content.Load<SoundEffect>("Audio\\4 SFX\\1 Menu Select Sounds\\3 Start Game\\3 Start Game");
      this.sounds["Menu_Switch"] = Globals.content.Load<SoundEffect>("Audio\\4 SFX\\1 Menu Select Sounds\\4 Switch\\switch_2");
      this.sounds["Falling"] = Globals.content.Load<SoundEffect>("Audio\\5 Sequences\\2 Falling into Cave\\Falling into Cave");
      this.sounds["Tree_Break"] = Globals.content.Load<SoundEffect>("Audio\\5 Sequences\\1 Breaking Tree\\Tree break_shortened");
      this.sounds["Birds"] = Globals.content.Load<SoundEffect>("Audio\\2 Ambiences\\1 Tutorial\\Birds_From_Above");
      this.sounds["Water_Run"] = Globals.content.Load<SoundEffect>("Audio\\2 Ambiences\\2 Cave\\Water_before_small");
      this.sounds["Waterfall_Small"] = Globals.content.Load<SoundEffect>("Audio\\2 Ambiences\\2 Cave\\Water_before_stream_background");
      this.sounds["Waterfall_Mid"] = Globals.content.Load<SoundEffect>("Audio\\2 Ambiences\\2 Cave\\Waterfal_Mid");
      this.sounds["Waterfall_Ground_1"] = Globals.content.Load<SoundEffect>("Audio\\2 Ambiences\\2 Cave\\Waterfal_Ground_close");
      this.sounds["Waterfall_Ground_2"] = Globals.content.Load<SoundEffect>("Audio\\2 Ambiences\\2 Cave\\Waterfal_Ground_Far");
      this.sounds["Waterfall_Top"] = Globals.content.Load<SoundEffect>("Audio\\2 Ambiences\\2 Cave\\Waterfal_Top");
      this.sounds["Radial_1"] = Globals.content.Load<SoundEffect>("Audio\\2 Ambiences\\2 Cave\\Radial_1");
      this.sounds["Radial_2"] = Globals.content.Load<SoundEffect>("Audio\\2 Ambiences\\2 Cave\\Radial_2");
      this.sounds["Radial_3"] = Globals.content.Load<SoundEffect>("Audio\\2 Ambiences\\2 Cave\\Radial_3");
    }

    private void SetSongs()
    {
      this.songs = new Dictionary<string, Song>();
      this.songs["TitleTheme"] = Globals.content.Load<Song>("Audio\\3 Music\\1 Titlescreen\\1 Title Screen");
      this.songs["MainTheme"] = Globals.content.Load<Song>("Audio\\3 Music\\1 Titlescreen\\2 Main Menu Theme");
      this.songs["Timer1"] = Globals.content.Load<Song>("Audio\\3 Music\\4 Action Climb\\4 Action Climb_1");
      this.songs["Timer2"] = Globals.content.Load<Song>("Audio\\3 Music\\4 Action Climb\\4 Action Climb_2");
      this.songs["Timer3"] = Globals.content.Load<Song>("Audio\\3 Music\\4 Action Climb\\4 Action Climb_3");
      this.songs["Timer4"] = Globals.content.Load<Song>("Audio\\3 Music\\4 Action Climb\\4 Action Climb_4");
      this.songs["Timer5"] = Globals.content.Load<Song>("Audio\\3 Music\\4 Action Climb\\4 Action Climb_5");
      this.songs["ExitCave"] = Globals.content.Load<Song>("Audio\\3 Music\\5 Exiting The Cave\\5 Exiting The Cave");
      this.soundSongs = new Dictionary<string, SoundSong>();
      for (int index = 1; index <= 4; ++index)
      {
        this.soundSongs[string.Format("CaveTheme_{0}", (object) index)] = new SoundSong(Globals.content.Load<SoundEffect>(string.Format("Audio\\3 Music\\3 The Cave\\3 The Cave_{0}", (object) index)));
        this.soundSongs[string.Format("CaveTheme_{0}_Loop", (object) index)] = new SoundSong(Globals.content.Load<SoundEffect>(string.Format("Audio\\3 Music\\3 The Cave\\3 The Cave_{0}_Loop", (object) index)));
      }
      this.soundSongs["TutorialTheme"] = new SoundSong(Globals.content.Load<SoundEffect>("Audio\\3 Music\\2 Tutorial\\2 Tutorial Level"));
      this.soundSongs["TutorialTheme_Loop"] = new SoundSong(Globals.content.Load<SoundEffect>("Audio\\3 Music\\2 Tutorial\\2 Tutorial Level_Loop"));
    }

    protected override void UnloadContent()
    {
    }

    protected override void Update(GameTime gameTime)
    {
      this.deltaSeconds = (float) gameTime.ElapsedGameTime.TotalSeconds;
      Globals.deltaTime = this.deltaSeconds;
      Globals.keyboard = Keyboard.GetState();
      Globals.mouse = Mouse.GetState();
      Globals.gamepad = GamePad.GetState(Globals.playerIndex);
      if (Globals.keyboard.IsKeyDown(Keys.F12))
        this.Exit();
      if (Globals.keyboard.IsKeyDown(Keys.F11) && !Globals.previousKeyboard.IsKeyDown(Keys.F11))
      {
        this.graphics.IsFullScreen = !this.graphics.IsFullScreen;
        this.graphics.ApplyChanges();
      }
      InputControllerManager.Update(gameTime);
      this.stateManager.Update(this.deltaSeconds);
      Globals.previousKeyboard = Globals.keyboard;
      Globals.previousMouse = Globals.mouse;
      Globals.previousGamepad = Globals.gamepad;
      Globals.UpdateTime = gameTime;
      TutorialPrompt.UpdateManager(gameTime);
      GateAnimation.UpdateManager(gameTime);
      DebounceManager.Update(gameTime);
      base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
      this.deltaSeconds = (float) gameTime.ElapsedGameTime.TotalSeconds;
      Globals.DrawTime = gameTime;
      Globals.deltaTime = this.deltaSeconds;
      this.GraphicsDevice.Clear(Color.Black);

      Globals.spriteBatch.Begin(blendState: BlendState.AlphaBlend, 
          transformMatrix: new Matrix?(Globals.camera.transform));
      this.stateManager.Draw();
      Globals.spriteBatch.End();

      base.Draw(gameTime);
    }
  }
}
