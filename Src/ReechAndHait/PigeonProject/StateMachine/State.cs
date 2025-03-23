
// Type: PigeonProject.StateMachine.State
// Assembly: PigeonProject, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ED64DA4-BF9A-4998-A25D-EAF7C565AAB8
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using PigeonProject.Utility;

#nullable disable
namespace PigeonProject.StateMachine
{
  public class State : IState
  {
    private readonly Debounce buttonSwitchDebounce = new Debounce(0.5);
    private readonly Debounce inputDebounce = new Debounce(1.0);
    public const double INPUT_DEBOUNCE_DURATION = 1.0;
    public const float BUTTON_SWITCH_DEBOUNCE_DURATION = 0.5f;
    public const float STICK_THRESHOLD = 0.3f;

    public StateManager StateManager { get; private set; }

    public bool IsButtonSwitchLocked
    {
      get => this.buttonSwitchDebounce.IsRunning;
      set
      {
        if (!value)
          return;
        this.buttonSwitchDebounce.Start();
      }
    }

    public bool IsInputDelayed
    {
      get => this.inputDebounce.IsRunning;
      set
      {
        if (!value)
          return;
        this.inputDebounce.Start();
      }
    }

    public bool IsInputLocked { get; set; }

    protected GamePadState Player1 { get; private set; }

    public State(StateManager stateManager) => this.StateManager = stateManager;

    public virtual void Initialize()
    {
    }

    public virtual void Terminate()
    {
    }

    public virtual void Update(GameTime gameTime)
    {
      this.Player1 = GamePad.GetState(PlayerIndex.One);
      if (!this.buttonSwitchDebounce.IsRunning && !this.inputDebounce.IsRunning 
                || State.DetectInput(this.Player1))
        return;
      this.buttonSwitchDebounce.Terminate();
      this.inputDebounce.Terminate();
    }

    public virtual void Draw(SpriteBatch spriteBatch, GameTime gameTime)
    {
    }

    public static bool DetectInput(GamePadState state)
    {
      return state.DPad.Down != ButtonState.Released || state.DPad.Up != ButtonState.Released || state.DPad.Right != ButtonState.Released || state.DPad.Left != ButtonState.Released || (double) state.ThumbSticks.Left.Length() > 0.30000001192092896 || (double) state.ThumbSticks.Right.Length() > 0.30000001192092896 || (double) state.Triggers.Left > 0.30000001192092896 || (double) state.Triggers.Right > 0.30000001192092896 || !state.IsButtonUp(Buttons.A) || !state.IsButtonUp(Buttons.B) || !state.IsButtonUp(Buttons.X) || !state.IsButtonUp(Buttons.Y) || !state.IsButtonUp(Buttons.Start) || !state.IsButtonUp(Buttons.Back) || !state.IsButtonUp(Buttons.LeftShoulder) || !state.IsButtonUp(Buttons.RightShoulder);
    }

    public static bool DetectInput(PlayerIndex player)
    {
      return State.DetectInput(GamePad.GetState(player));
    }
  }
}
