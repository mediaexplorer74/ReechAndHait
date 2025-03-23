
// Type: ReachHigh.Shared.Animation
// Assembly: ReachHigh.Windows, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E7AAE051-E6AD-4505-A6E0-970B36D3CBD7
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

#nullable disable
namespace ReachHigh.Shared
{
  public class Animation
  {
    public Texture2D texture;
    public int activeFrame;
    public int count;
    public float frameSpeed;
    public Vector2 scale;
    public int offsetTop;

    public int width => this.texture.Width / this.count;

    public int height => this.texture.Height;

    public Animation(
      Texture2D TEXTURE,
      int COUNT,
      float FRAMESPEED,
      Vector2 SCALE,
      int OFFSETTOP = 0)
    {
      this.texture = TEXTURE;
      this.count = COUNT;
      this.frameSpeed = FRAMESPEED;
      this.scale = SCALE;
      this.offsetTop = OFFSETTOP;
    }
  }
}
