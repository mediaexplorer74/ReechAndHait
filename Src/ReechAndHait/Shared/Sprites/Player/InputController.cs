
// Type: ReachHigh.Shared.InputController
// Assembly: ReachHigh.Windows, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E7AAE051-E6AD-4505-A6E0-970B36D3CBD7
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

#nullable disable
namespace ReachHigh.Shared
{
  public class InputController : IInputController
  {
    private readonly List<Buttons> _pressedButtons = new List<Buttons>();
    private readonly List<Keys> _pressedKeys = new List<Keys>();
    private const float GAMEPAD_AIM_OFFSET = 300f;

    public InputMode InputMode { get; private set; } = InputMode.None;

    public PlayerIndex GamepadIndex { get; private set; }

    public InputController(InputMode inputMode = InputMode.Gamepad, PlayerIndex gameadIndex = PlayerIndex.One)
    {
      this.InputMode = inputMode;
      this.GamepadIndex = gameadIndex;
      InputControllerManager.RegisterInputController(this);
    }

    public Vector2 Move()
    {
      switch (this.InputMode)
      {
        case InputMode.Gamepad:
          return this.GetGamepadMoveInput();
        case InputMode.Keyboard:
          return this.GetKeyboardMoveInput();
        default:
          return Vector2.Zero;
      }
    }

    public bool Jump()
    {
      switch (this.InputMode)
      {
        case InputMode.Gamepad:
          return this.CheckButtonInput(Buttons.A, false);
        case InputMode.Keyboard:
          return this.CheckKeyInput(Keys.Space, false);
        default:
          return false;
      }
    }

    public bool Jump(out bool isFirstInput)
    {
      isFirstInput = !this._pressedButtons.Contains(Buttons.A) 
                && !this._pressedKeys.Contains(Keys.Space);
      return this.Jump();
    }

    public bool DialogueContinue()
    {
      switch (this.InputMode)
      {
        case InputMode.Gamepad:
          return this.CheckButtonInput(Buttons.A);
        case InputMode.Keyboard:
          return this.CheckKeyInput(Keys.Enter);
        default:
          return false;
      }
    }

    public bool TogglePause()
    {
      switch (this.InputMode)
      {
        case InputMode.Gamepad:
          return this.CheckButtonInput(Buttons.Start);
        case InputMode.Keyboard:
          return this.CheckKeyInput(Keys.Escape);
        default:
          return false;
      }
    }

    public bool MonkeyInteract(bool onlyOnPress = true)
    {
      switch (this.InputMode)
      {
        case InputMode.Gamepad:
          return this.CheckButtonInput(Buttons.RightTrigger, onlyOnPress);
        case InputMode.Keyboard:
          return this.CheckKeyInput(Keys.Enter, onlyOnPress);
        default:
          return false;
      }
    }

    public bool MonkeyThrowRide(bool onlyOnPress = true)
    {
      switch (this.InputMode)
      {
        case InputMode.Gamepad:
          return this.CheckButtonInput(Buttons.X, onlyOnPress);
        case InputMode.Keyboard:
          return this.CheckKeyInput(Keys.F, onlyOnPress);
        default:
          return false;
      }
    }

    public bool GeckoInteract(bool onlyOnPress = true)
    {
      switch (this.InputMode)
      {
        case InputMode.Gamepad:
          return this.CheckButtonInput(Buttons.X, onlyOnPress);
        case InputMode.Keyboard:
          return this.CheckKeyInput(Keys.F, onlyOnPress);
        default:
          return false;
      }
    }

    public ButtonState GeckoBumerangStance()
    {
      switch (this.InputMode)
      {
        case InputMode.Gamepad:
          return this.CheckBumerangStanceGamepad();
        case InputMode.Keyboard:
          return Mouse.GetState().RightButton;
        default:
          return ButtonState.Released;
      }
    }

    private ButtonState CheckBumerangStanceGamepad()
    {
      return GamePad.GetState(this.GamepadIndex).IsButtonDown(Buttons.LeftTrigger) 
                ? ButtonState.Pressed : ButtonState.Released;
    }

    public bool GeckoBumerangShoot()
    {
      switch (this.InputMode)
      {
        case InputMode.Gamepad:
          return this.CheckButtonInput(Buttons.RightTrigger);
        case InputMode.Keyboard:
          return Mouse.GetState().LeftButton == ButtonState.Pressed;
        default:
          return false;
      }
    }

    public Vector2 GeckoAimPosition(Vector2 geckoPosition)
    {
      switch (this.InputMode)
      {
        case InputMode.Gamepad:
            // RnD / Dev / Hack aim position :)
            //return this.GetGamepadAimPosition(geckoPosition);
            return Vector2.Transform(Mouse.GetState().Position.ToVector2(), Matrix.Invert(Globals.transform));
        case InputMode.Keyboard:
          return Vector2.Transform(Mouse.GetState().Position.ToVector2(), Matrix.Invert(Globals.transform));
        default:
          return Vector2.Zero;
      }
    }

    public bool DebugFly() => false;

    public bool TryKeyboardInput(Keys key) => this.CheckKeyInput(key);

    public bool TryGamePadInput(Buttons button, PlayerIndex? gamepadIndex = null)
    {
      PlayerIndex gamepadIndex1 = this.GamepadIndex;
      if (gamepadIndex.HasValue)
        this.GamepadIndex = gamepadIndex.Value;
      int num = this.CheckButtonInput(button) ? 1 : 0;
      this.GamepadIndex = gamepadIndex1;
      return num != 0;
    }

    public bool TryMenuConfirm()
    {
      return this.TryKeyboardInput(Keys.Enter) || this.TryGamePadInput(Buttons.A);
    }

    public bool TryMenuReturn()
    {
      return this.TryKeyboardInput(Keys.Escape) || this.TryGamePadInput(Buttons.B);
    }

    public bool TryMenuSwitch(bool up = true)
    {
      if (this.TryKeyboardInput(up ? Keys.W : Keys.S) 
                || this.TryKeyboardInput(up ? Keys.Up : Keys.Down) 
                || this.TryGamePadInput(up ? Buttons.DPadUp : Buttons.DPadDown))
        return true;
      int num;
      if (!up)
      {
        GamePadThumbSticks thumbSticks = GamePad.GetState(this.GamepadIndex).ThumbSticks;
        if ((double) thumbSticks.Left.Y < -0.5)
        {
          thumbSticks = Globals.previousGamepad.ThumbSticks;
          num = (double) thumbSticks.Left.Y >= -0.5 ? 1 : 0;
        }
        else
          num = 0;
      }
      else
      {
        GamePadThumbSticks thumbSticks = GamePad.GetState(this.GamepadIndex).ThumbSticks;
        if ((double) thumbSticks.Left.Y > 0.5)
        {
          thumbSticks = Globals.previousGamepad.ThumbSticks;
          num = (double) thumbSticks.Left.Y <= 0.5 ? 1 : 0;
        }
        else
          num = 0;
      }
      return num != 0;
    }

    public void Update(GameTime gameTime)
    {
      foreach (Buttons button in Enum.GetValues(typeof (Buttons)))
      {
        if (this._pressedButtons.Contains(button) && GamePad.GetState(this.GamepadIndex).IsButtonUp(button))
          this._pressedButtons.Remove(button);
      }
      foreach (Keys key in Enum.GetValues(typeof (Keys)))
      {
        if (this._pressedKeys.Contains(key) && Keyboard.GetState().IsKeyUp(key))
          this._pressedKeys.Remove(key);
      }
    }

    private Vector2 GetKeyboardMoveInput()
    {
            /*Vector2 zero = Vector2.Zero;
            KeyboardState state = Keyboard.GetState();
            if (state.IsKeyDown(Keys.A) || state.IsKeyDown(Keys.Left))
              --zero.X;
            if (state.IsKeyDown(Keys.D) || state.IsKeyDown(Keys.Right))
              ++zero.X;
            if (state.IsKeyDown(Keys.W) || state.IsKeyDown(Keys.Up))
              ++zero.Y;
            if (state.IsKeyDown(Keys.S) || state.IsKeyDown(Keys.Down))
              --zero.Y;
            return zero;*/
            Vector2 zero = Vector2.Zero;
            KeyboardState state = Keyboard.GetState();
            if (state.IsKeyDown(Keys.Left))
                --zero.X;
            if (state.IsKeyDown(Keys.Right))
                ++zero.X;
            if (state.IsKeyDown(Keys.Up))
                ++zero.Y;
            if (state.IsKeyDown(Keys.Down))
                --zero.Y;
            return zero;
        }

    private Vector2 GetGamepadMoveInput()
    {
            /*GamePadState state = GamePad.GetState(this.GamepadIndex);
            Vector2 left = state.ThumbSticks.Left;
            if ((double) left.Length() <= 0.1)
            {
              if (state.IsButtonDown(Buttons.DPadLeft))
                --left.X;
              if (state.IsButtonDown(Buttons.DPadRight))
                ++left.X;
              if (state.IsButtonDown(Buttons.DPadUp))
                ++left.Y;
              if (state.IsButtonDown(Buttons.DPadDown))
                --left.Y;
            }
            return left;*/
            Vector2 zero = Vector2.Zero;
            KeyboardState state = Keyboard.GetState();
            if (state.IsKeyDown(Keys.A))
                --zero.X;
            if (state.IsKeyDown(Keys.D))
                ++zero.X;
            if (state.IsKeyDown(Keys.W))
                ++zero.Y;
            if (state.IsKeyDown(Keys.S))
                --zero.Y;
            return zero;
        }

    private bool CheckButtonInput(Buttons button, bool onlyOnButtonPress = true)
    {
            /*if (GamePad.GetState(this.GamepadIndex).IsButtonDown(button))
            {
              if (!this._pressedButtons.Contains(button))
              {
                this._pressedButtons.Add(button);
                return true;
              }
              if (!onlyOnButtonPress)
                return true;
            }
            return false;*/

            Keys key = Keys.X; // "default button"

            if (button == Buttons.A)
                key = Keys.X; // button A (interact)
            if (button == Buttons.B)
                key = Keys.B; // button B
            if (button == Buttons.X)
                key = Keys.H; // hold
            if (button == Buttons.RightTrigger)
                key = Keys.R; // RT ("rich transit"))) 
            if (button == Buttons.Start)
                key = Keys.P; // pause

            if (Keyboard.GetState().IsKeyDown(key))
            {
                if (!this._pressedKeys.Contains(key))
                {
                    this._pressedKeys.Add(key);
                    return true;
                }
                if (!onlyOnButtonPress)
                    return true;
            }
            return false;
        }

    private bool CheckKeyInput(Keys key, bool onlyOnKeyPress = true)
    {
      if (Keyboard.GetState().IsKeyDown(key))
      {
        if (!this._pressedKeys.Contains(key))
        {
          this._pressedKeys.Add(key);
          return true;
        }
        if (!onlyOnKeyPress)
          return true;
      }
      return false;
    }

    private Vector2 GetGamepadAimPosition(Vector2 geckoPosition)
    {
      Vector2 left = GamePad.GetState(this.GamepadIndex).ThumbSticks.Left;
      left.Y = -left.Y;
      return geckoPosition + left * 300f;
    }
  }
}
