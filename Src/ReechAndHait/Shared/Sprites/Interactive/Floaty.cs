
// Type: ReachHigh.Shared.Floaty
// Assembly: ReachHigh.Windows, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E7AAE051-E6AD-4505-A6E0-970B36D3CBD7
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework;

#nullable disable
namespace ReachHigh.Shared
{
  public class Floaty(string PATH, Vector2 POS, Vector2 DIMS) : Sprite(PATH, POS, DIMS)
  {
    public override Rectangle BoxCollider(string special = "none")
    {
      return new Rectangle((int) this.pos.X, (int) this.pos.Y, (int) this.dims.X, 5);
    }
  }
}
