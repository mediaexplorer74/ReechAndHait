
// Type: ReachHigh.Shared.Credits
// Assembly: ReachHigh.Windows, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E7AAE051-E6AD-4505-A6E0-970B36D3CBD7
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

#nullable disable
namespace ReachHigh.Shared
{
  public class Credits : State
  {
    private float timer;
    private float _timer;
    private Texture2D creditsSheet;
    private Vector2 pos;

    public Credits(StateManager STATEMANAGER)
      : base(STATEMANAGER, false)
    {
      this.pos = Vector2.Zero;
      this.timer = 25f;
      this._timer = this.timer;
      this.creditsSheet = Globals.content.Load<Texture2D>("UI\\credits");
    }

    public override void Update(float deltaSeconds)
    {
      this.timer -= deltaSeconds;
      this.pos.Y -= (float) ((double) deltaSeconds * (double) this._timer * 3.0);
      if ((double) this.timer <= 0.0)
        Main.game.Exit();
      base.Update(deltaSeconds);
    }

    public override void Draw()
    {
      Globals.spriteBatch.Draw(this.creditsSheet, new Rectangle((int) this.pos.X, (int) this.pos.Y, Main.screenWidth, Main.screenHeight * 3), new Rectangle?(), Color.White, 0.0f, Vector2.Zero, SpriteEffects.None, 0.0f);
      base.Draw();
    }
  }
}
