
// Type: ReachHigh.Shared.SpecialSprite
// Assembly: ReachHigh.Windows, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E7AAE051-E6AD-4505-A6E0-970B36D3CBD7
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework;

#nullable disable
namespace ReachHigh.Shared
{
  public class SpecialSprite : Sprite
  {
    public SpecialSprite(string PATH, Vector2 POS, Vector2 DIMS, string TYPE, string DEBUGID = "")
      : base(PATH, POS, DIMS, DEBUGID)
    {
      this.type = TYPE;
    }
  }
}
