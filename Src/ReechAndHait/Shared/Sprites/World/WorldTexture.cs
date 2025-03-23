
// Type: ReachHigh.Shared.WorldTexture
// Assembly: ReachHigh.Windows, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E7AAE051-E6AD-4505-A6E0-970B36D3CBD7
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

#nullable disable
namespace ReachHigh.Shared
{
  public class WorldTexture
  {
    private Vector2[,,] pos;
    private Vector2 dims;
    private Vector2 debugPos;
    private Texture2D[,,] textures;
    public Vector2 offpos;
    public Vector2 offdims;

    public WorldTexture(string[,,] texturePaths, Vector2 wholePos, Vector2 wholeDims)
    {
      this.debugPos = wholePos;
      this.dims = new Vector2(wholeDims.X / (float) texturePaths.GetLength(2), wholeDims.Y / (float) texturePaths.GetLength(1));
      this.textures = new Texture2D[texturePaths.GetLength(0), texturePaths.GetLength(1), texturePaths.GetLength(2)];
      this.pos = new Vector2[texturePaths.GetLength(0), texturePaths.GetLength(1), texturePaths.GetLength(2)];
      for (int index1 = 0; index1 < texturePaths.GetLength(0); ++index1)
      {
        for (int index2 = 0; index2 < texturePaths.GetLength(1); ++index2)
        {
          for (int index3 = 0; index3 < texturePaths.GetLength(2); ++index3)
          {
            this.textures[index1, index2, index3] = Globals.content.Load<Texture2D>(texturePaths[index1, index2, index3]);
            this.pos[index1, index2, index3] = new Vector2(wholePos.X + this.dims.X * (float) index3, wholePos.Y + this.dims.Y * (float) index2);
          }
        }
      }
    }

    public virtual void Update(float deltaSeconds)
    {
    }

    public virtual void Draw(int layer)
    {
      switch (layer)
      {
        case 0:
          for (int index1 = 0; index1 < this.textures.GetLength(1); ++index1)
          {
            for (int index2 = 0; index2 < this.textures.GetLength(2); ++index2)
              Globals.spriteBatch.Draw(this.textures[0, index1, index2], new Rectangle((int) ((double) this.debugPos.X + (double) this.offpos.X + ((double) this.dims.X + (double) this.offdims.X / 2.0) * (double) index2), (int) ((double) this.debugPos.Y + (double) this.offpos.Y + ((double) this.dims.Y + (double) this.offdims.Y / 2.0) * (double) index1), (int) ((double) this.dims.X + (double) this.offdims.X / 2.0), (int) ((double) this.dims.Y + (double) this.offdims.Y / 2.0)), new Rectangle?(), Color.White, 0.0f, Vector2.Zero, SpriteEffects.None, 0.0f);
          }
          break;
        case 1:
          for (int index3 = 0; index3 < this.textures.GetLength(1); ++index3)
          {
            for (int index4 = 0; index4 < this.textures.GetLength(2); ++index4)
              Globals.spriteBatch.Draw(this.textures[1, index3, index4], new Rectangle((int) ((double) this.debugPos.X + (double) this.offpos.X + ((double) this.dims.X + (double) this.offdims.X / 2.0) * (double) index4), (int) ((double) this.debugPos.Y + (double) this.offpos.Y + ((double) this.dims.Y + (double) this.offdims.Y / 2.0) * (double) index3), (int) ((double) this.dims.X + (double) this.offdims.X / 2.0), (int) ((double) this.dims.Y + (double) this.offdims.Y / 2.0)), new Rectangle?(), Color.White, 0.0f, Vector2.Zero, SpriteEffects.None, 0.0f);
          }
          break;
        case 2:
          for (int index5 = 0; index5 < this.textures.GetLength(1); ++index5)
          {
            for (int index6 = 0; index6 < this.textures.GetLength(2); ++index6)
              Globals.spriteBatch.Draw(this.textures[2, index5, index6], new Rectangle((int) ((double) this.debugPos.X + (double) this.offpos.X + ((double) this.dims.X + (double) this.offdims.X / 2.0) * (double) index6), (int) ((double) this.debugPos.Y + (double) this.offpos.Y + ((double) this.dims.Y + (double) this.offdims.Y / 2.0) * (double) index5), (int) ((double) this.dims.X + (double) this.offdims.X / 2.0), (int) ((double) this.dims.Y + (double) this.offdims.Y / 2.0)), new Rectangle?(), Color.White, 0.0f, Vector2.Zero, SpriteEffects.None, 0.0f);
          }
          break;
        case 3:
          for (int index7 = 0; index7 < this.textures.GetLength(1); ++index7)
          {
            for (int index8 = 0; index8 < this.textures.GetLength(2); ++index8)
              Globals.spriteBatch.Draw(this.textures[3, index7, index8], new Rectangle((int) ((double) this.debugPos.X + (double) this.offpos.X + ((double) this.dims.X + (double) this.offdims.X / 2.0) * (double) index8), (int) ((double) this.debugPos.Y + (double) this.offpos.Y + ((double) this.dims.Y + (double) this.offdims.Y / 2.0) * (double) index7), (int) ((double) this.dims.X + (double) this.offdims.X / 2.0), (int) ((double) this.dims.Y + (double) this.offdims.Y / 2.0)), new Rectangle?(), Color.White, 0.0f, Vector2.Zero, SpriteEffects.None, 0.0f);
          }
          break;
      }
    }
  }
}
