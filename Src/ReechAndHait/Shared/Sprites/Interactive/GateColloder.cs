
// Type: ReachHigh.Shared.GateCollider
// Assembly: ReachHigh.Windows, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E7AAE051-E6AD-4505-A6E0-970B36D3CBD7
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework;

#nullable disable
namespace ReachHigh.Shared
{
  public class GateCollider : Sprite
  {
    private Lever lever;

    public GateCollider(string PATH, Vector2 POS, Vector2 DIMS, Lever LEVER)
      : base(PATH, POS, DIMS)
    {
      this.lever = LEVER;
    }

    public override void Update(float deltaSeconds)
    {
      if (this.lever.activated)
        this.pos = new Vector2(float.MaxValue, float.MaxValue);
      base.Update(deltaSeconds);
    }
  }
}
