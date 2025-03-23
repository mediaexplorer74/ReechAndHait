
// Type: ReachHigh.Shared.AnimationManager
// Assembly: ReachHigh.Windows, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E7AAE051-E6AD-4505-A6E0-970B36D3CBD7
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

#nullable disable
namespace ReachHigh.Shared
{
  public class AnimationManager
  {
    private Animation animation;
    private float timer;
    public Vector2 pos;
    private SpriteEffects spriteEffects;
    private bool loop;
    private bool oneFrame;

    public AnimationManager(Animation ANIMATION) => this.animation = ANIMATION;

    public void PlayAnimation(
      Animation ANIMATION,
      SpriteEffects SPRITEEFFECTS,
      bool LOOP = true,
      int STARTFRAME = 0,
      bool ONEFRAME = false)
    {
      if (this.animation == ANIMATION && this.spriteEffects == SPRITEEFFECTS && LOOP == this.loop && this.oneFrame == ONEFRAME)
        return;
      this.animation = ANIMATION;
      this.spriteEffects = SPRITEEFFECTS;
      this.loop = LOOP;
      this.oneFrame = ONEFRAME;
      this.animation.activeFrame = STARTFRAME;
      this.timer = 0.0f;
    }

    public void StopAnimation()
    {
      this.timer = 0.0f;
      this.animation.activeFrame = 0;
    }

    public void Update(float deltaSeconds)
    {
      this.timer += deltaSeconds;
      if ((double) this.timer <= (double) this.animation.frameSpeed)
        return;
      this.timer = 0.0f;
      ++this.animation.activeFrame;
      if (this.animation.activeFrame < this.animation.count && !this.oneFrame)
        return;
      if (this.loop)
        this.animation.activeFrame = 0;
      else
        --this.animation.activeFrame;
    }

    public void Draw()
    {
      Globals.spriteBatch.Draw(this.animation.texture, new Rectangle((int) ((double) this.pos.X - 50.0), (int) ((double) this.pos.Y - 20.0), this.animation.width, this.animation.height), new Rectangle?(new Rectangle(this.animation.activeFrame * this.animation.width, this.animation.offsetTop, (int) ((double) this.animation.width * (double) this.animation.scale.X), (int) ((double) this.animation.height * (double) this.animation.scale.Y))), Color.White, 0.0f, Vector2.Zero, this.spriteEffects, 0.0f);
    }
  }
}
