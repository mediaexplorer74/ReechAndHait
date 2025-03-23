
// Type: PigeonProject.ContentManagement.SpriteSheet
// Assembly: PigeonProject, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ED64DA4-BF9A-4998-A25D-EAF7C565AAB8
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

#nullable disable
namespace PigeonProject.ContentManagement
{
  public class SpriteSheet
  {
    public Texture2D Texture { get; private set; }

    public int Columns { get; private set; }

    public int Rows { get; private set; }

    public int Count => this.Columns * this.Rows;

    public int FrameWidth { get; private set; }

    public int FrameHeight { get; private set; }

    public SpritesheetIterationMode IterationMode { get; set; }

    public bool IterateBackwards { get; set; }

    public SpriteSheet(Texture2D texture, int columns, int rows)
    {
      this.Texture = texture;
      this.Columns = columns;
      this.Rows = rows;
      this.FrameWidth = texture.Width / this.Columns;
      this.FrameHeight = texture.Height / this.Rows;
    }

    public Rectangle GetSourceRectangle(int index)
    {
      if (this.IterateBackwards)
        index = this.Count - index - 1;
      if (index < 0 || index >= this.Count)
        return Rectangle.Empty;
      int x;
      int y;
      if (this.IterationMode == SpritesheetIterationMode.LeftToRight)
      {
        x = index % this.Columns * this.FrameWidth;
        y = (int) Math.Floor((Decimal) index / (Decimal) this.Columns) * this.FrameHeight;
      }
      else
      {
        y = index % this.Rows * this.FrameHeight;
        x = (int) Math.Floor((Decimal) index / (Decimal) this.Rows) * this.FrameWidth;
      }
      return new Rectangle(x, y, this.FrameWidth, this.FrameHeight);
    }

    public Rectangle GetSourceRectangle(Point position)
    {
      return this.GetSourceRectangle(position.Y * this.Columns + position.X);
    }

    public void Draw(
      SpriteBatch spriteBatch,
      int index,
      Point position,
      int width,
      int height,
      Color color,
      float rotation,
      SpriteEffects effects,
      float layerDepth)
    {
      Rectangle sourceRectangle = this.GetSourceRectangle(index);
      spriteBatch.Draw(this.Texture, new Rectangle(position.X, position.Y, width, height), new Rectangle?(sourceRectangle), color, rotation, new Vector2((float) (sourceRectangle.Width / 2), (float) (sourceRectangle.Height / 2)), effects, layerDepth);
    }
  }
}
