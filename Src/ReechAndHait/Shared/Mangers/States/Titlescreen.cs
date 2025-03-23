
// Type: ReachHigh.Shared.Titlescreen
// Assembly: ReachHigh.Windows, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E7AAE051-E6AD-4505-A6E0-970B36D3CBD7
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using PigeonProject.Animation;
using PigeonProject.Objects;
using PigeonProject.UserInterface;
using PigeonProject.Utility;
using System;
using System.Diagnostics;
using System.IO;
using System.Xml.Linq;
using Windows.Storage;

#nullable disable
namespace ReachHigh.Shared
{
  public class Titlescreen : State
  {
    private const Buttons GAMEPAD_JOIN_BUTTON = Buttons.A;
    private const Keys KEYBOARD_JOIN_KEY = Keys.Enter;
    private const double SPRITE_FADE_DURATION = 0.5;
    private const double SPRITE_DISPLAY_DURATION = 5.0;
    private int index;
    private bool newGame;
    private bool gameStart;
    private bool newGameSequence;
    private bool checkController;
    private bool settings;
    private Sprite[] menu;
    private Sprite[] animation;
    private Sprite titlefont;
    private int saveData;
    private float timer;
    private float animTimer;
    private float animStartTimer;
    private const float _animTimer = 0.1f;
    private int currentFrame;
    public static SoundEffectInstance treeBreak;
    private Titlescreen.PlayerJoinState currentJoinState;
    private bool keyboardAlreadyJoined;
    private PlayerIndex geckoGamepadIndex;
    private readonly InputController menuController = new InputController();
    private readonly GameObjectManager gameObjectManager = 
            new GameObjectManager((PigeonProject.StateMachine.StateManager) null, registerObjectAnimationManager: true);
    private readonly JoinUi joinUi;
    private readonly Debounce inputDelay = new Debounce(1.0);
    private readonly UiElement[] credits;
    private bool startSequence;
    private bool creditsEnded = false;
    private float creditsTimer;


    public Titlescreen(StateManager STATEMANAGER, bool STARTSEQUENCE = false)
      : base(STATEMANAGER, false)
    {
      this.startSequence = STARTSEQUENCE;
      this.creditsTimer = 44f;
      this.credits = new UiElement[7];
      for (int index1 = 0; index1 < 7; ++index1)
      {
        Vector2 relativePosition = new Vector2((float) (Main.screenWidth / 2), (float) (Main.screenHeight / 2));
        Texture2D sprite = Globals.content.Load<Texture2D>("UI\\Credits\\credit" + index1.ToString());
        UiElement[] credits = this.credits;
        int index2 = index1;
        UiElement uiElement = new UiElement(relativePosition, sprite, sprite.Width, sprite.Height, this.gameObjectManager);
        uiElement.Opacity = 0.0f;
        credits[index2] = uiElement;
      }
      this.menu = new Sprite[11];
      this.menu[0] = new Sprite("UI\\Titlescreens\\titlescreen_continue", Vector2.Zero, new Vector2((float) Main.screenWidth, (float) Main.screenHeight));
      this.menu[1] = new Sprite("UI\\Titlescreens\\titlescreen_newgame", Vector2.Zero, new Vector2((float) Main.screenWidth, (float) Main.screenHeight));
      this.menu[2] = new Sprite("UI\\Titlescreens\\titlescreen_settings", Vector2.Zero, new Vector2((float) Main.screenWidth, (float) Main.screenHeight));
      this.menu[3] = new Sprite("UI\\Titlescreens\\titlescreen_exit", Vector2.Zero, new Vector2((float) Main.screenWidth, (float) Main.screenHeight));
      this.menu[4] = new Sprite("UI\\Titlescreens\\titlescreen_plain", Vector2.Zero, new Vector2((float) Main.screenWidth, (float) Main.screenHeight));
      this.menu[5] = new Sprite("UI\\Titlescreens\\start_titlescreen_continue", Vector2.Zero, new Vector2((float) Main.screenWidth, (float) Main.screenHeight));
      this.menu[6] = new Sprite("UI\\Titlescreens\\start_titlescreen_newgame", Vector2.Zero, new Vector2((float) Main.screenWidth, (float) Main.screenHeight));
      this.menu[7] = new Sprite("UI\\Titlescreens\\start_titlescreen_settings", Vector2.Zero, new Vector2((float) Main.screenWidth, (float) Main.screenHeight));
      this.menu[8] = new Sprite("UI\\Titlescreens\\start_titlescreen_exit", Vector2.Zero, new Vector2((float) Main.screenWidth, (float) Main.screenHeight));
      this.menu[9] = new Sprite("UI\\Titlescreens\\start_titlescreen_plain", Vector2.Zero, new Vector2((float) Main.screenWidth, (float) Main.screenHeight));
      this.menu[10] = new Sprite("UI\\Titlescreens\\titlescreen_character", Vector2.Zero, new Vector2((float) Main.screenWidth, (float) Main.screenHeight));
      this.animation = new Sprite[25];
      for (int index = 0; index < this.animation.Length; ++index)
        this.animation[index] = new Sprite("UI\\TreeAnimation\\titlescreen_F" + (index + 1).ToString(), Vector2.Zero, new Vector2((float) Main.screenWidth, (float) Main.screenHeight));
      this.animTimer = 0.1f;
      this.animStartTimer = 6f;
      this.titlefont = new Sprite("UI\\Titlefont", new Vector2((float) (Main.screenWidth / 3), (float) (Main.screenHeight / 9)), new Vector2((float) (Main.screenWidth / 3), (float) Main.screenHeight / 2.5f));
      this.timer = 3f;

      if (this.startSequence)
        Globals.audioManager.PlayTheme(Globals.audioManager.songs["MainTheme"]);
      else
        Globals.audioManager.PlayTheme(Globals.audioManager.songs["TitleTheme"], true);

      OperateSaveData();

      Titlescreen.treeBreak = Globals.audioManager.sounds["Tree_Break"].CreateInstance();

      JoinUi joinUi = new JoinUi(this.gameObjectManager);

      joinUi.IsVisible = false;
      this.joinUi = joinUi;
      this.index = this.saveData != -1 ? 0 : 1;

      if (!this.startSequence)
        return;
      this.ChainSpriteAnimations(this.credits);
    }//

        private async void OperateSaveData()
        {
            StorageFolder AppFolder = ApplicationData.Current.LocalFolder;

            if (!File.Exists(AppFolder.Path + "/"+ "SaveData.txt") || this.startSequence)
            {
                await AppFolder.CreateFileAsync("SaveData.txt", CreationCollisionOption.ReplaceExisting);
                
                //StorageFile SaveData = await AppFolder.CreateFileAsync("SaveData.txt",
                //                                CreationCollisionOption.ReplaceExisting);
                // запись в файл
                //await FileIO.WriteTextAsync(SaveData, "-1");

                using (FileStream AF = File.OpenWrite(AppFolder.Path + "/" +"SaveData.txt"))
                {
                    using (StreamWriter streamWriter = new StreamWriter(AF))
                    {
                        streamWriter.Write(-1);
                    }
                }
            }

            using (FileStream AF = File.OpenRead(AppFolder.Path + "/" +"SaveData.txt"))
            {
                using (StreamReader streamReader = new StreamReader(AF))
                {
                    this.saveData = int.Parse(streamReader.ReadLine());
                }
            }
            //StorageFile ReadData = await AppFolder.GetFileAsync("SaveData.txt");
            //string text = await FileIO.ReadTextAsync(ReadData);
            //this.saveData = int.Parse(text);
        }


    public override void Initialize()
    {
      base.Initialize();
      this.inputDelay.Start();
      TutorialPrompt.ResetManager();
      GateAnimation.ResetManager();
    }

    public override void Update(float deltaSeconds)
    {
      this.gameObjectManager.Update(Globals.UpdateTime);

      if (this.startSequence)
      {
        if ((double) this.creditsTimer > 0.0)
        {
          this.creditsTimer -= deltaSeconds;
          if ((double) this.creditsTimer < 2.0)
            MediaPlayer.Volume = this.creditsTimer / 4f;
        }
        else
        {
          this.startSequence = false;
          MediaPlayer.Volume = 0.5f;
          Globals.audioManager.PlayTheme(Globals.audioManager.songs["TitleTheme"]);
        }
      }
      else if (this.gameStart)
      {
        if ((double) this.timer > 0.0)
          this.timer -= deltaSeconds;
        else if (this.newGameSequence)
        {
          if ((double) this.timer > -4.0)
            this.timer -= deltaSeconds;
          else
            this.stateManager.StartState((State) new Tutorial(this.stateManager, true), AUDIOFADE: false);
        }
        else
        {
          switch (this.saveData)
          {
            case 0:
              this.stateManager.StartState((State) new Tutorial(this.stateManager, false));
              break;
            case 1:
            case 2:
            case 3:
            case 4:
            case 5:
              this.stateManager.StartState((State) new Cave(this.stateManager, false));
              break;
            case 6:
              this.stateManager.StartState((State) new Cave(this.stateManager, false));
              Globals.audioManager.PlayTheme(Globals.audioManager.songs["ExitCave"]);
              break;
          }
        }
      }
      else if (this.checkController)
      {
        this.joinUi.IsVisible = true;
        switch (this.currentJoinState)
        {
          case Titlescreen.PlayerJoinState.JoinGecko:
            this.TryJoinPlayer(Players.Gecko);
            break;
          case Titlescreen.PlayerJoinState.JoinMonkey:
            this.TryJoinPlayer(Players.Monkey);
            break;
          case Titlescreen.PlayerJoinState.Done:
            this.TryContinue();
            break;
        }
        if (this.menuController.TryMenuReturn())
          this.CancelJoinState();
      }
      else if (this.settings)
      {
        if (this.menuController.TryMenuConfirm())
        {
          if (Globals.graphics.IsFullScreen)
          {
            Globals.graphics.IsFullScreen = false;
            Globals.graphics.ApplyChanges();
          }
          else
          {
            Globals.graphics.IsFullScreen = true;
            Globals.graphics.ApplyChanges();
          }
        }
        else if (this.menuController.TryMenuReturn())
        {
          this.settings = false;
          Globals.audioManager.sounds["Menu_Undo"].Play();
        }
      }
      else if (this.menuController.TryMenuReturn())
      {
        this.settings = false;
        Globals.audioManager.sounds["Menu_Undo"].Play();
      }
      if (this.inputDelay.IsRunning)
        return;
      if (this.menuController.TryMenuConfirm())
      {
        switch (this.index)
        {
          case 0:
            if (this.saveData != -1)
            {
              this.checkController = true;
              this.newGame = false;
              Globals.audioManager.sounds["Menu_Confirm"].Play();
              break;
            }
            break;
          case 1:
            this.checkController = true;
            this.newGame = true;
            Globals.audioManager.sounds["Menu_Confirm"].Play();
            break;
          case 2:
            this.settings = true;
            Globals.audioManager.sounds["Menu_Confirm"].Play();
            break;
          case 3:
            Main.game.Exit();
            Globals.audioManager.sounds["Menu_Undo"].Play();
            break;
        }
      }
      if (this.index == 0 && this.menuController.TryMenuSwitch(false))
      {
        this.index = 1;
        Globals.audioManager.sounds["Menu_Switch"].Play();
      }
      else if (this.saveData != -1 && this.index == 1 && this.menuController.TryMenuSwitch())
      {
        this.index = 0;
        Globals.audioManager.sounds["Menu_Switch"].Play();
      }
      else if (this.index == 1 && this.menuController.TryMenuSwitch(false))
      {
        this.index = 2;
        Globals.audioManager.sounds["Menu_Switch"].Play();
      }
      else if (this.index == 2 && this.menuController.TryMenuSwitch())
      {
        this.index = 1;
        Globals.audioManager.sounds["Menu_Switch"].Play();
      }
      else if (this.index == 2 && this.menuController.TryMenuSwitch(false))
      {
        this.index = 3;
        Globals.audioManager.sounds["Menu_Switch"].Play();
      }
      else if (this.index == 3 && this.menuController.TryMenuSwitch())
      {
        this.index = 2;
        Globals.audioManager.sounds["Menu_Switch"].Play();
      }
      base.Update(deltaSeconds);
    }

    private void TreeAnimation()
    {
      if ((double) this.animTimer <= 0.0)
      {
        if (this.currentFrame < this.animation.Length - 1)
          ++this.currentFrame;
        this.animTimer = 0.1f;
      }
      else
        this.animTimer -= Globals.deltaTime;
      this.animation[this.currentFrame].Draw();
    }

    public override void Draw()
    {
      if (this.startSequence)
      {
        this.menu[10].Draw();
        if ((double) this.creditsTimer < 2.0)
        {
          this.menu[5].Transparancy = Math.Abs((float) (((double) this.creditsTimer - 2.0) / 2.0));
          this.menu[5].Draw();
        }
      }
      else if (this.gameStart)
      {
        if ((double) this.animStartTimer < 0.0)
        {
          this.TreeAnimation();
        }
        else
        {
          this.animStartTimer -= Globals.deltaTime;
          if (this.newGameSequence)
            this.menu[9].Draw();
          else
            this.menu[4].Draw();
        }
      }
      else if (this.checkController)
      {
        if (this.saveData == -1)
          this.menu[9].Draw();
        else
          this.menu[4].Draw();
      }
      else if (this.settings)
      {
        if (this.saveData == -1)
          this.menu[9].Draw();
        else
          this.menu[4].Draw();
        this.titlefont.Draw();
        Globals.spriteBatch.DrawString(Globals.menuFont, "Toggle Fullscreen ", new Vector2((float) (Main.screenWidth / 2), (float) Main.screenHeight / 1.3f), Color.White, 0.0f, Globals.menuFont.MeasureString("Toggle Fullscreen") / 2f, 1f, SpriteEffects.None, 0.0f);
        Globals.spriteBatch.DrawString(Globals.menuFont, Globals.graphics.IsFullScreen ? "Off" : "On", new Vector2((float) (Main.screenWidth / 2), (float) ((double) Main.screenHeight / 1.2999999523162842 + 60.0)), Color.White, 0.0f, Globals.menuFont.MeasureString(Globals.graphics.IsFullScreen ? "Off" : "On") / 2f, 1f, SpriteEffects.None, 0.0f);
      }
      else
      {
        if (this.saveData == -1)
          this.menu[this.index + 5].Draw();
        else
          this.menu[this.index].Draw();
        this.titlefont.Draw();
      }
      Globals.spriteBatch.Draw(Globals.rect, new Rectangle((int) Camera.mid.X - Main.screenWidth * 5, (int) Camera.mid.Y - Main.screenHeight * 5, Main.screenWidth * 10, Main.screenHeight * 10), new Rectangle?(), Color.Black * (float) ((3.0 - (double) this.timer) / 3.0), 0.0f, Vector2.Zero, SpriteEffects.None, 0.0f);
      base.Draw();
      this.gameObjectManager.Draw(Globals.spriteBatch, Globals.DrawTime);
    }

    private void TryJoinPlayer(Players player)
    {
      if (this.CheckKeyboardJoinInput())
      {
        RhPlayerSettings.Set(player, "InputMode", (object) InputMode.Keyboard);
        RhPlayerSettings.Set(player, "GamepadIndex", (object) PlayerIndex.One);

        // RnD / Dev / Hack: double player mode "imitation" :) 
        this.keyboardAlreadyJoined = true;
        this.ContinueJoinState();
      }
      else
      {
        PlayerIndex gamepadIndex;
        if (!this.CheckGamepadJoinInput(out gamepadIndex) 
                    || player == Players.Monkey
                    && !this.keyboardAlreadyJoined 
                    && gamepadIndex == this.geckoGamepadIndex)
          return;
        RhPlayerSettings.Set(player, "InputMode", (object) InputMode.Gamepad);
        RhPlayerSettings.Set(player, "GamepadIndex", (object) gamepadIndex);
        this.ContinueJoinState();
        if (player != Players.Gecko)
          return;
        this.geckoGamepadIndex = gamepadIndex;
      }
    }

    private void ContinueJoinState()
    {
      ++this.currentJoinState;
      this.joinUi.MoveForward();
    }

    private void TryContinue()
    {
      StorageFolder AppFolder = ApplicationData.Current.LocalFolder;
      if (!this.menuController.TryMenuConfirm())
        return;
      Globals.audioManager.StopTheme();
      this.checkController = false;
      this.gameStart = true;
      Globals.audioManager.sounds["Menu_Start_Game"].Play();
      if (this.newGame)
      {
        this.newGameSequence = true;
        this.timer = 8f;
        Titlescreen.treeBreak.Play();

        using (StreamWriter streamWriter = new StreamWriter(File.OpenWrite(AppFolder.Path + "/" + "SaveData.txt")))
          streamWriter.Write(0);
      }
      this.joinUi.FadeOut();
    }

    private void CancelJoinState()
    {
      this.currentJoinState = Titlescreen.PlayerJoinState.JoinGecko;
      this.keyboardAlreadyJoined = false;
      this.geckoGamepadIndex = PlayerIndex.One;
      this.joinUi.IsVisible = false;
      this.joinUi.Reset();
      this.checkController = false;
    }

    private bool CheckKeyboardJoinInput()
    {
      return !this.keyboardAlreadyJoined && this.menuController.TryKeyboardInput(Keys.Enter);
    }

    private bool CheckGamepadJoinInput(out PlayerIndex gamepadIndex)
    {
      foreach (PlayerIndex playerIndex in Enum.GetValues(typeof (PlayerIndex)))
      {
        if (this.menuController.TryGamePadInput(Buttons.A, new PlayerIndex?(playerIndex)))
        {
          gamepadIndex = playerIndex;
          return true;
        }
      }
      gamepadIndex = PlayerIndex.One;
      return false;
    }

    private void ChainSpriteAnimations(UiElement[] elements)
    {
      ObjectAnimation inAnimation;
      ObjectAnimation outAnimation1;
      this.DisplaySprite(elements[0], out inAnimation, out outAnimation1);
      for (int index = 1; index < elements.Length; ++index)
      {
        ObjectAnimation outAnimation2;
        ObjectAnimation newInAnimation;
        this.DisplaySprite(elements[index], out newInAnimation, out outAnimation2);
        outAnimation1.TerminateEffect = (PigeonProject.Animation.TerminateEffect) (() => newInAnimation.Start());
        outAnimation1 = outAnimation2;
      }
      inAnimation.Start();
      outAnimation1.TerminateEffect = (PigeonProject.Animation.TerminateEffect) (() => this.creditsEnded = true);
    }

    private void DisplaySprite(
      UiElement element,
      out ObjectAnimation inAnimation,
      out ObjectAnimation outAnimation)
    {
      inAnimation = this.FadeSprite(element);
      ObjectAnimation localOutAnimation = this.FadeSprite(element, false);
      Debounce timer = new Debounce(5.0, (PigeonProject.Utility.TerminateEffect) (() => localOutAnimation.Start()));
      inAnimation.TerminateEffect = (PigeonProject.Animation.TerminateEffect) (() => timer.Start());
      outAnimation = localOutAnimation;
    }

    private ObjectAnimation FadeSprite(UiElement sprite, bool fadeIn = true)
    {
      float startValue = 0.0f;
      float endValue = 1f;
      if (!fadeIn)
      {
        startValue = endValue;
        endValue = 0.0f;
      }
      return new ObjectAnimation((object) sprite, this.gameObjectManager.ObjectAnimationManager, "Opacity", startValue, endValue, 0.5, easingFunction: new EasingFunction(EasingFunctions.EaseInSine), startImmediately: false);
    }

    private enum PlayerJoinState
    {
      JoinGecko,
      JoinMonkey,
      Done,
    }
  }
}
