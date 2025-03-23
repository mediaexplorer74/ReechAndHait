
// Type: ReachHigh.Shared.StateManager
// Assembly: ReachHigh.Windows, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E7AAE051-E6AD-4505-A6E0-970B36D3CBD7
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

#nullable disable
namespace ReachHigh.Shared
{
  public class StateManager
  {
    public Dictionary<string, State> states;
    public State currentState;
    public State previousState;
    private float fadeTimer;
    private bool switchDraw;
    private bool audioFade;

    public StateManager()
    {
      this.states = new Dictionary<string, State>();
      this.states.Add("Titlescreen", (State) new Titlescreen(this));
      this.currentState = this.states["Titlescreen"];
      this.fadeTimer = 3f;
    }

    public void StartState(State state, bool fade = true, bool AUDIOFADE = true)
    {
      this.previousState = this.currentState;
      this.currentState = state;
      this.audioFade = AUDIOFADE;
      if (fade)
      {
        this.fadeTimer = 3f;
        this.switchDraw = true;
      }
      this.previousState.Terminate();
      this.currentState.Initialize();
    }

    public virtual void Update(float deltaSeconds)
    {
      if ((double) this.fadeTimer > 0.0)
        this.fadeTimer -= deltaSeconds;
      if ((double) this.fadeTimer < 3.0 && (double) this.fadeTimer > 0.0 && this.audioFade)
        SoundEffect.MasterVolume = (float) ((3.0 - (double) this.fadeTimer) / 6.0);
      Globals.camera.Update(deltaSeconds);
      this.currentState.Update(deltaSeconds);
    }

    public virtual void Draw()
    {
      if (this.switchDraw)
        this.switchDraw = false;
      else
        this.currentState.Draw();
      Globals.spriteBatch.Draw(Globals.rect, new Rectangle((int) Camera.mid.X - Main.screenWidth * 5, (int) Camera.mid.Y - Main.screenHeight * 5, Main.screenWidth * 10, Main.screenHeight * 10), new Rectangle?(), Color.Black * (this.fadeTimer / 3f), 0.0f, Vector2.Zero, SpriteEffects.None, 0.0f);
    }
  }
}
