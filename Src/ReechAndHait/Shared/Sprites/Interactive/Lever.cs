
// Type: ReachHigh.Shared.Lever
// Assembly: ReachHigh.Windows, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E7AAE051-E6AD-4505-A6E0-970B36D3CBD7
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Diagnostics;

#nullable disable
namespace ReachHigh.Shared
{
  public class Lever : Sprite
  {
    public bool activated;
    public bool previousActivated;
    public bool pullActivation;
    public bool timerActivation;
    public bool previousTimerActivation;
    private float timer;
    private float previousTimer;
    private int songCount;

    public Lever(string PATH, Vector2 POS, Vector2 DIMS, string TYPE)
      : base(PATH, POS, DIMS)
    {
      this.type = TYPE;
      this.timer = 64.2f;
      this.previousTimer = this.timer;
      this.songCount = 1;
    }

    public override void Update(float deltaSeconds)
    {
      if (this.timerActivation)
      {
        if ((double) this.timer == (double) this.previousTimer)
        {
          if (this.songCount < 6)
            Globals.audioManager.PlayTheme(Globals.audioManager.songs[string.Format("Timer{0}", (object) this.songCount)]);
          else
            Globals.audioManager.PlayTheme(Globals.audioManager.songs["Timer5"]);
        }
        this.timer -= deltaSeconds;
      }
      if ((double) this.timer <= 0.0)
      {
        this.timer = this.songCount >= 5 ? this.previousTimer : this.previousTimer + 15.1f;
        ++this.songCount;
        this.previousTimer = this.timer;
        this.timerActivation = false;
      }
      this.previousActivated = this.activated;
      this.previousTimerActivation = this.timerActivation;
      base.Update(deltaSeconds);
    }

    public override void Draw()
    {
      if (this.activated && this.type == nameof (Lever))
        Globals.spriteBatch.Draw(this.sprite, new Rectangle((int) this.pos.X, (int) this.pos.Y, (int) this.dims.X, (int) this.dims.Y), new Rectangle?(), Color.White, this.rotation, this.origin, SpriteEffects.FlipHorizontally, 0.0f);
      else if (this.timerActivation && this.type == "Timer")
        Globals.spriteBatch.Draw(this.sprite, new Rectangle((int) this.pos.X, (int) this.pos.Y, (int) this.dims.X, (int) this.dims.Y), new Rectangle?(), Color.White, this.rotation, this.origin, SpriteEffects.FlipHorizontally, 0.0f);
      else
        base.Draw();
    }

    public void ActivateLever()
    {
      if (this.type == "Timer" && !this.timerActivation)
      {
        this.timerActivation = true;
        Debug.WriteLine("timer activated");
      }
      else
      {
        if (this.activated || !(this.type != "Timer"))
          return;
        this.activated = true;
        Globals.audioManager.sounds[nameof (Lever)].Play();
      }
    }
  }
}
