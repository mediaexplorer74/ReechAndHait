
// Type: ReachHigh.Shared.Pause
// Assembly: ReachHigh.Windows, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E7AAE051-E6AD-4505-A6E0-970B36D3CBD7
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using PigeonProject.ContentManagement;
using PigeonProject.Objects;
using PigeonProject.UserInterface;

#nullable disable
namespace ReachHigh.Shared
{
  public class Pause : State
  {
    private byte index;
    private bool controlpanel;
    private Texture2D controlTexture;
    private readonly GameObjectManager gameObjectManager = new GameObjectManager((PigeonProject.StateMachine.StateManager) null);
    private readonly UiElement controlUi;
    private UiElement[] pauseScreens;
    private int CONTROL_UI_WIDTH = (int) ((double) Main.screenWidth / 1.5);
    private int CONTROL_UI_HEIGHT = (int) ((double) Main.screenHeight / 1.5);

    public Pause(StateManager STATEMANAGER)
      : base(STATEMANAGER, NEWPOS: false)
    {
      this.index = (byte) 0;
      this.controlTexture = Globals.content.Load<Texture2D>("UI\\control_UI");
      Globals.audioManager.sounds["Menu_Confirm"].Play();
      UiElement uiElement1 = new UiElement(Camera.mid, ContentHandler.GetAsset<Texture2D>(this.GetControlUiAssetKey()), this.CONTROL_UI_WIDTH, this.CONTROL_UI_HEIGHT, this.gameObjectManager);
      uiElement1.IsVisible = false;
      this.controlUi = uiElement1;
      UiElement[] uiElementArray = new UiElement[4];
      UiElement uiElement2 = new UiElement(Camera.mid, ContentHandler.GetAsset<Texture2D>("pausescreen_continue"), this.CONTROL_UI_WIDTH, this.CONTROL_UI_HEIGHT, this.gameObjectManager);
      uiElement2.IsVisible = false;
      uiElementArray[0] = uiElement2;
      UiElement uiElement3 = new UiElement(Camera.mid, ContentHandler.GetAsset<Texture2D>("pausescreen_controls"), this.CONTROL_UI_WIDTH, this.CONTROL_UI_HEIGHT, this.gameObjectManager);
      uiElement3.IsVisible = false;
      uiElementArray[1] = uiElement3;
      UiElement uiElement4 = new UiElement(Camera.mid, ContentHandler.GetAsset<Texture2D>("pausescreen_reset"), this.CONTROL_UI_WIDTH, this.CONTROL_UI_HEIGHT, this.gameObjectManager);
      uiElement4.IsVisible = false;
      uiElementArray[2] = uiElement4;
      UiElement uiElement5 = new UiElement(Camera.mid, ContentHandler.GetAsset<Texture2D>("pausescreen_quit"), this.CONTROL_UI_WIDTH, this.CONTROL_UI_HEIGHT, this.gameObjectManager);
      uiElement5.IsVisible = false;
      uiElementArray[3] = uiElement5;
      this.pauseScreens = uiElementArray;
      Camera.stupid = true;
    }

    public override void Update(float deltaSeconds)
    {
      Camera.scale = 1f;

      if (this.index == (byte) 0 
                && (Globals.keyboard.IsKeyDown(Keys.S) 
                && !Globals.previousKeyboard.IsKeyDown(Keys.S) 
                ||
                Globals.keyboard.IsKeyDown(Keys.Down)
                && !Globals.previousKeyboard.IsKeyDown(Keys.Down)
                ||
                Globals.gamepad.IsButtonDown(Buttons.DPadDown)
                && !Globals.previousGamepad.IsButtonDown(Buttons.DPadDown)
                || 
                (double) Globals.gamepad.ThumbSticks.Left.Y < -0.5
                && (double) Globals.previousGamepad.ThumbSticks.Left.Y >= -0.5))
      {
        this.index = (byte) 1;
        Globals.audioManager.sounds["Menu_Switch"].Play();
      }
      else if (this.index == (byte) 1 
                && (Globals.keyboard.IsKeyDown(Keys.W) 
                && !Globals.previousKeyboard.IsKeyDown(Keys.W) 
                ||
                Globals.keyboard.IsKeyDown(Keys.Up)
                && !Globals.previousKeyboard.IsKeyDown(Keys.Up)
                ||
                Globals.gamepad.IsButtonDown(Buttons.DPadUp)
                && !Globals.previousGamepad.IsButtonDown(Buttons.DPadUp) 
                || 
                (double) Globals.gamepad.ThumbSticks.Left.Y > 0.5 
                && (double) Globals.previousGamepad.ThumbSticks.Left.Y <= 0.5))
      {
        this.index = (byte) 0;
        Globals.audioManager.sounds["Menu_Switch"].Play();
      }
      else if (this.index == (byte) 1
                && (Globals.keyboard.IsKeyDown(Keys.S)
                && !Globals.previousKeyboard.IsKeyDown(Keys.S)
                ||
                Globals.keyboard.IsKeyDown(Keys.Down)
                && !Globals.previousKeyboard.IsKeyDown(Keys.Down)
                ||
                Globals.gamepad.IsButtonDown(Buttons.DPadDown) 
                && !Globals.previousGamepad.IsButtonDown(Buttons.DPadDown) 
                ||
                (double) Globals.gamepad.ThumbSticks.Left.Y < -0.5
                && (double) Globals.previousGamepad.ThumbSticks.Left.Y >= -0.5))
      {
        this.index = (byte) 2;
        Globals.audioManager.sounds["Menu_Switch"].Play();
      }
      else if (this.index == (byte) 2 
                && (Globals.keyboard.IsKeyDown(Keys.W)
                && !Globals.previousKeyboard.IsKeyDown(Keys.W) 
                ||
                Globals.keyboard.IsKeyDown(Keys.Up)
                && !Globals.previousKeyboard.IsKeyDown(Keys.Up)
                ||
                Globals.gamepad.IsButtonDown(Buttons.DPadUp) 
                && !Globals.previousGamepad.IsButtonDown(Buttons.DPadUp) 
                || 
                (double) Globals.gamepad.ThumbSticks.Left.Y > 0.5 
                && (double) Globals.previousGamepad.ThumbSticks.Left.Y <= 0.5))
      {
        this.index = (byte) 1;
        Globals.audioManager.sounds["Menu_Switch"].Play();
      }
      else if (this.index == (byte) 2 
                && (Globals.keyboard.IsKeyDown(Keys.S)
                && !Globals.previousKeyboard.IsKeyDown(Keys.S)
                ||
                Globals.keyboard.IsKeyDown(Keys.Down)
                && !Globals.previousKeyboard.IsKeyDown(Keys.Down)
                ||
                Globals.gamepad.IsButtonDown(Buttons.DPadDown) 
                && !Globals.previousGamepad.IsButtonDown(Buttons.DPadDown) 
                || 
                (double) Globals.gamepad.ThumbSticks.Left.Y < -0.5 
                && (double) Globals.previousGamepad.ThumbSticks.Left.Y >= -0.5))
      {
        this.index = (byte) 3;
        Globals.audioManager.sounds["Menu_Switch"].Play();
      }
      else if (this.index == (byte) 3 
                && (Globals.keyboard.IsKeyDown(Keys.W) 
                && !Globals.previousKeyboard.IsKeyDown(Keys.W) 
                ||
                Globals.keyboard.IsKeyDown(Keys.Up)
                && !Globals.previousKeyboard.IsKeyDown(Keys.Up)
                ||
                Globals.gamepad.IsButtonDown(Buttons.DPadUp) 
                && !Globals.previousGamepad.IsButtonDown(Buttons.DPadUp) 
                || 
                (double) Globals.gamepad.ThumbSticks.Left.Y > 0.5 
                && (double) Globals.previousGamepad.ThumbSticks.Left.Y <= 0.5))
      {
        this.index = (byte) 2;
        Globals.audioManager.sounds["Menu_Switch"].Play();
      }


      if (!this.controlpanel)
      {
        if ((Globals.keyboard.IsKeyDown(Keys.Enter) 
                    && !Globals.previousKeyboard.IsKeyDown(Keys.Enter)
                    || Globals.gamepad.IsButtonDown(Buttons.A)
                    && !Globals.previousGamepad.IsButtonDown(Buttons.A)) 
                    && this.index == (byte) 0)
        {
          Camera.stupid = false;
          this.stateManager.StartState(this.stateManager.previousState, false);
          Globals.audioManager.sounds["Menu_Confirm"].Play();
        }
        else if ((Globals.keyboard.IsKeyDown(Keys.Enter) 
                    && !Globals.previousKeyboard.IsKeyDown(Keys.Enter)
                    || 
                    Globals.gamepad.IsButtonDown(Buttons.A)
                    && !Globals.previousGamepad.IsButtonDown(Buttons.A))
                    && this.index == (byte) 1)
        {
          this.controlpanel = true;
          Globals.audioManager.sounds["Menu_Confirm"].Play();
        }
        else if ((Globals.keyboard.IsKeyDown(Keys.Enter) 
                    && !Globals.previousKeyboard.IsKeyDown(Keys.Enter) 
                    || 
                    Globals.gamepad.IsButtonDown(Buttons.A) 
                    && !Globals.previousGamepad.IsButtonDown(Buttons.A)) 
                    && this.index == (byte) 2)
        {
          Globals.audioManager.sounds["Menu_Confirm"].Play();
          if (this.stateManager.previousState is Cave)
          {
            Globals.audioManager.StopSoundSong();
            Camera.stupid = false;
            this.stateManager.StartState((State) new Cave(this.stateManager, false));
          }
          else if (this.stateManager.previousState is Tutorial)
          {
            Globals.audioManager.StopSoundSong();
            Camera.stupid = false;
            this.stateManager.StartState((State) new Tutorial(this.stateManager, false));
          }
        }
        else if ((Globals.keyboard.IsKeyDown(Keys.Enter)
                    && !Globals.previousKeyboard.IsKeyDown(Keys.Enter) 
                    || 
                    Globals.gamepad.IsButtonDown(Buttons.A) 
                    && !Globals.previousGamepad.IsButtonDown(Buttons.A))
                    && this.index == (byte) 3)
        {
          Globals.audioManager.sounds["Menu_Confirm"].Play();
          Globals.audioManager.StopSoundSong();
          Camera.stupid = false;
          this.stateManager.StartState((State) new Titlescreen(this.stateManager));
        }
      }
      else if (Globals.keyboard.IsKeyDown(Keys.Enter) 
                && !Globals.previousKeyboard.IsKeyDown(Keys.Enter) 
                || 
                Globals.gamepad.IsButtonDown(Buttons.A) 
                && !Globals.previousGamepad.IsButtonDown(Buttons.A)
                ||
                Globals.keyboard.IsKeyDown(Keys.Escape) 
                && !Globals.previousKeyboard.IsKeyDown(Keys.Escape) 
                || 
                Globals.gamepad.IsButtonDown(Buttons.B)
                && !Globals.previousGamepad.IsButtonDown(Buttons.B))
      {
        this.index = (byte) 0;
        Globals.audioManager.sounds["Menu_Undo"].Play();
        this.controlpanel = false;
      }
      else
        this.index = (byte) 10;
      base.Update(deltaSeconds);
      if (this.controlpanel && !this.controlUi.IsVisible)
        this.controlUi.IsVisible = true;
      else if (!this.controlpanel && this.controlUi.IsVisible)
        this.controlUi.IsVisible = false;
      this.gameObjectManager.Update(Globals.UpdateTime);
    }

    public override void Draw()
    {
      this.stateManager.previousState.Draw();
      GateAnimation.DrawManager(Globals.spriteBatch, Globals.DrawTime);
      TutorialPrompt.DrawManager(Globals.spriteBatch, Globals.DrawTime);
      for (int index = 0; index < this.pauseScreens.Length; ++index)
      {
        if ((int) this.index == index)
          this.pauseScreens[index].IsVisible = true;
        else
          this.pauseScreens[index].IsVisible = false;
      }
      base.Draw();
      this.gameObjectManager.Draw(Globals.spriteBatch, Globals.DrawTime);
    }

    private string GetControlUiAssetKey()
    {
      int num = (int) RhPlayerSettings.Get<InputMode>(Players.Gecko, "InputMode");
      InputMode inputMode = RhPlayerSettings.Get<InputMode>(Players.Monkey, "InputMode");
      if (num == 1)
        return "controlls2";
      return inputMode == InputMode.Keyboard ? "controlls1" : "controlls3";
    }
  }
}
