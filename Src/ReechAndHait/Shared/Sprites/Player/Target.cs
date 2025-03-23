
// Type: ReachHigh.Shared.Target
// Assembly: ReachHigh.Windows, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E7AAE051-E6AD-4505-A6E0-970B36D3CBD7
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

#nullable disable
namespace ReachHigh.Shared
{
  public class Target : Sprite
  {
    public static Vector2 mousePos;
    public static bool active;
    private readonly InputController geckoInputController;

    public Target(string PATH, Vector2 POS, Vector2 DIMS)
      : base(PATH, POS, DIMS)
    {
      Target.active = false;
      this.origin = new Vector2(this.dims.X / 2f, this.dims.Y / 2f);
      this.geckoInputController = RhPlayerSettings.Get<InputController>(Players.Gecko, "InputController");
    }

    public override void Update(float deltaSeconds)
    {
      this.rotation -= 0.1f;
      this.pos.X = (float) Globals.mouse.X;
      this.pos.Y = (float) Globals.mouse.Y;
      if (Target.active && this.geckoInputController.GeckoBumerangShoot())
        Bumerang.loose = true;
      Target.mousePos = this.geckoInputController.GeckoAimPosition(Globals.geckoPos);
      base.Update(deltaSeconds);
    }

    public override void Draw()
    {
      if (this.sprite == null)
        return;
      Globals.spriteBatch.Draw(this.sprite, new Rectangle((int) Target.mousePos.X, 
          (int) Target.mousePos.Y, (int) this.dims.X, (int) this.dims.Y), new Rectangle?(),
          Color.White, this.rotation, this.origin, SpriteEffects.None, 0.0f);
    }
  }
}
