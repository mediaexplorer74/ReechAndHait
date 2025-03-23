
// Type: PigeonProject.InputManagement.InputController
// Assembly: PigeonProject, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ED64DA4-BF9A-4998-A25D-EAF7C565AAB8
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using PigeonProject.Utility;
using System;
using System.Collections.Generic;

#nullable disable
namespace PigeonProject.InputManagement
{
  public class InputController
  {
    private readonly Debounce inputDebounce = new Debounce(1.0);
    public const float ANALOGUE_STICK_DEADZONE = 0.1f;
    private const double INPUT_DEBOUNCE_DURATION = 1.0;

    public PlayerIndex ControllerIndex { get; private set; }

    public GamePadState State => GamePad.GetState(this.ControllerIndex, GamePadDeadZone.Circular);

    public IPlayer Player { get; set; }

    public bool IsInputDelayed
    {
      get => this.inputDebounce.IsRunning;
      set
      {
        if (value)
          this.inputDebounce.Start();
        else
          this.inputDebounce.Terminate();
      }
    }

    public bool IsInputLocked { get; set; }

    public List<Buttons> DelayButtons { get; } = new List<Buttons>();

    public InputController(PlayerIndex controllerIndex) => this.ControllerIndex = controllerIndex;

    public InputController(PlayerIndex controllerIndex, IPlayer player)
      : this(controllerIndex)
    {
      this.Player = player;
    }

    public InputController(PlayerIndex controllerIndex, InputManager manager)
      : this(controllerIndex)
    {
      manager.RegisterController(this);
    }

    public InputController(PlayerIndex controllerIndex, IPlayer player, InputManager manager)
      : this(controllerIndex, player)
    {
      manager.RegisterController(this);
    }

    public void Update(GameTime gameTime)
    {
      if (this.Player == null)
        return;
      if (!this.IsInputLocked)
        this.Player.ProcessInput(this.State, gameTime, this);
      bool flag = false;
      foreach (Buttons delayButton in this.DelayButtons)
      {
        if (this.State.IsButtonDown(delayButton))
        {
          flag = true;
          break;
        }
      }
      if (this.IsInputDelayed)
      {
        if (flag)
          return;
        this.IsInputDelayed = false;
      }
      else
      {
        if (!flag)
          return;
        this.IsInputDelayed = true;
      }
    }

    public IPlayer RegisterPlayer(IPlayer player)
    {
      this.Player = player;
      return this.Player;
    }

    public void RegisterDelayButtons()
    {
      this.DelayButtons.Clear();
      this.DelayButtons.AddRange((IEnumerable<Buttons>) Enum.GetValues(typeof (Buttons)));
    }

    public void RegisterDelayButtons(Buttons[] buttons)
    {
      foreach (Buttons button in buttons)
      {
        if (!this.DelayButtons.Contains(button))
          this.DelayButtons.Add(button);
      }
    }

    public void RegisterDelayButtons(Buttons button)
    {
      this.RegisterDelayButtons(new Buttons[1]{ button });
    }
  }
}
