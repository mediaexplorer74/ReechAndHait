
// Type: ReachHigh.Shared.Gate
// Assembly: ReachHigh.Windows, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E7AAE051-E6AD-4505-A6E0-970B36D3CBD7
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework;

#nullable disable
namespace ReachHigh.Shared
{
  public class Gate : Sprite
  {
    public bool geckoTrigger;
    public bool monkeyTrigger;
    public bool closed;
    public bool activated;
    public float timer;
    public float _timer;
    public GateAnimation gateAnimation;

    public Gate(string PATH, Vector2 POS, Vector2 DIMS, string TYPE, bool CLOSED = false, float seconds = 5f)
      : base(PATH, POS, DIMS)
    {
      this.closed = CLOSED;
      this.type = TYPE;
      this.timer = seconds;
      this._timer = this.timer;
      this.gateAnimation = new GateAnimation(new Vector2(this.pos.X + 120f, this.pos.Y + 380f));
    }

    public override void Update(float deltaSeconds)
    {
      if (this.geckoTrigger && this.monkeyTrigger && !this.closed)
        this.activated = true;
      if (this.activated)
      {
        this.timer -= deltaSeconds;
        if ((double) this.timer > (double) this._timer - 1.5)
          this.pos.Y -= 200f * deltaSeconds;
        else if ((double) this.timer < 1.5)
          this.pos.Y += 200f * deltaSeconds;
      }
      base.Update(deltaSeconds);
    }

    public override void Draw()
    {
    }

    public void OpenGate() => this.gateAnimation.Open();

    public void CloseGate() => this.gateAnimation.Close();

    public void UnlockGate()
    {
      if (!this.closed)
        return;
      this.closed = false;
      Globals.audioManager.sounds["Gate_Unlock"].Play();
    }
  }
}
