
// Type: ReachHigh.Shared.GateAnimation
// Assembly: ReachHigh.Windows, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E7AAE051-E6AD-4505-A6E0-970B36D3CBD7
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PigeonProject.Animation;
using PigeonProject.Objects;
using PigeonProject.UserInterface;

#nullable disable
namespace ReachHigh.Shared
{
  public class GateAnimation : UiElement
  {
    private static readonly GameObjectManager manager = new GameObjectManager((PigeonProject.StateMachine.StateManager) null, registerFrameAnimationManager: true);
    private bool isClosed = true;
    private const int WIDTH = 568;
    private const int HEIGHT = 1000;
    private const float ANIMATION_DURATION = 1.5f;

    public GateAnimation(Vector2 position)
      : base(position, (Texture2D) null, 568, 1000, GateAnimation.manager)
    {
      this.FrameAnimationHandler = new FrameAnimationHandler("idle_close", new FrameAnimation(new string[1]
      {
        "gate23"
      }, GateAnimation.manager.FrameAnimationManager));
      this.FrameAnimationHandler.RegisterAnimation("idle_open", new FrameAnimation(new string[1]
      {
        "gate01"
      }, GateAnimation.manager.FrameAnimationManager));
      this.FrameAnimationHandler.RegisterAnimation("open", new FrameAnimation("gate", GateAnimation.manager.FrameAnimationManager)
      {
        PlayBackward = true,
        AnimationDuration = 1.5f
      });
      this.FrameAnimationHandler.RegisterAnimation("close", new FrameAnimation("gate", GateAnimation.manager.FrameAnimationManager)
      {
        AnimationDuration = 1.5f
      });
    }

    public static void UpdateManager(GameTime gameTime) => GateAnimation.manager.Update(gameTime);

    public static void DrawManager(SpriteBatch spriteBatch, GameTime gameTime)
    {
      GateAnimation.manager.Draw(spriteBatch, gameTime);
    }

    public static void ResetManager() => GateAnimation.manager.Reset();

    public void Open()
    {
      if (!this.isClosed)
        return;
      this.SwitchState();
    }

    public void Close()
    {
      if (this.isClosed)
        return;
      this.SwitchState();
    }

    private void SwitchState()
    {
      string key = this.isClosed ? "open" : "close";
      this.FrameAnimationHandler.SwitchAnimation(key, (TerminateEffect) (() =>
      {
        this.FrameAnimationHandler.SwitchAnimation("idle_" + key);
        this.isClosed = !this.isClosed;
      }));
    }
  }
}
