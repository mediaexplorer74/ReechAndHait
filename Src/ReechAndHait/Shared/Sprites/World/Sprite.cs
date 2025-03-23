
// Type: ReachHigh.Shared.Sprite
// Assembly: ReachHigh.Windows, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E7AAE051-E6AD-4505-A6E0-970B36D3CBD7
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

#nullable disable
namespace ReachHigh.Shared
{
  public class Sprite
  {
    public Vector2 pos;
    public Vector2 dims;
    public Vector2 oldOffset;
    public Vector2 direction;
    public float rotation;
    public Vector2 velocity;
    protected Vector2 origin;
    public string path;

    public Texture2D sprite { get; set; }

    public string type { get; set; }

    public string SurfaceType { get; set; }

    public float Transparancy { get; set; }

    public Color Color { get; set; } = Color.White;

    public string DebugID { get; set; }

    public Sprite(string PATH, Vector2 POS, Vector2 DIMS, string DEBUGID = "")
    {
      this.pos = POS;
      this.path = PATH;
      if (this.path.Length > 0)
        this.sprite = Globals.content.Load<Texture2D>(PATH);
      this.Transparancy = 1f;
      this.dims = !(DIMS == Vector2.Zero) ? DIMS : new Vector2((float) this.sprite.Width, (float) this.sprite.Height);
      if (DEBUGID.Length <= 0)
        return;
      this.DebugID = DEBUGID;
    }

    public virtual void Update(float deltaSeconds)
    {
    }

    public virtual void Draw()
    {
      if (this.sprite == null)
        return;
      Globals.spriteBatch.Draw(this.sprite, new Rectangle((int) this.pos.X, (int) this.pos.Y, (int) this.dims.X, (int) this.dims.Y), new Rectangle?(), this.Color * this.Transparancy, this.rotation, this.origin, SpriteEffects.None, 0.0f);
    }

    public virtual Rectangle BoxCollider(string special = "none")
    {
      switch (special)
      {
        case "climb-up":
          return new Rectangle((int) ((double) this.pos.X - 20.0), (int) ((double) this.pos.Y + (double) this.dims.Y / 4.0 - 5.0), (int) this.dims.X + 40, 5);
        case "climb-down":
          return new Rectangle((int) ((double) this.pos.X - 20.0), (int) ((double) this.pos.Y + (double) this.dims.Y / 4.0), (int) this.dims.X + 40, 5);
        case "stick":
          return new Rectangle((int) ((double) this.pos.X + (double) this.dims.X / 2.0), (int) ((double) this.pos.Y - 30.0), 5, (int) this.dims.Y + 30);
        case "gate":
          return new Rectangle((int) ((double) this.pos.X - (double) this.dims.X / 2.0), (int) this.pos.Y, (int) this.dims.X * 2, (int) this.dims.Y);
        case "box":
          return new Rectangle((int) ((double) this.pos.X - 20.0), (int) this.pos.Y, (int) this.dims.X + 40, (int) this.dims.Y);
        default:
          return new Rectangle((int) this.pos.X, (int) this.pos.Y, (int) this.dims.X, (int) this.dims.Y);
      }
    }

    public bool isCollidingLeft(Sprite other, string special = "none")
    {
      Rectangle rectangle1 = this.BoxCollider(special);
      double num = (double) rectangle1.Left + (double) this.velocity.X * (double) Globals.deltaTime;
      rectangle1 = other.BoxCollider();
      double right1 = (double) rectangle1.Right;
      if (num < right1)
      {
        Rectangle rectangle2 = this.BoxCollider(special);
        int right2 = rectangle2.Right;
        rectangle2 = other.BoxCollider();
        int right3 = rectangle2.Right;
        if (right2 > right3)
        {
          rectangle2 = this.BoxCollider(special);
          int top1 = rectangle2.Top;
          rectangle2 = other.BoxCollider();
          int bottom1 = rectangle2.Bottom;
          if (top1 < bottom1)
          {
            rectangle2 = this.BoxCollider(special);
            int bottom2 = rectangle2.Bottom;
            rectangle2 = other.BoxCollider();
            int top2 = rectangle2.Top;
            return bottom2 > top2;
          }
        }
      }
      return false;
    }

    public bool isCollidingRight(Sprite other, string special = "none")
    {
      Rectangle rectangle1 = this.BoxCollider(special);
      double num = (double) rectangle1.Right + (double) this.velocity.X * (double) Globals.deltaTime;
      rectangle1 = other.BoxCollider();
      double left1 = (double) rectangle1.Left;
      if (num > left1)
      {
        Rectangle rectangle2 = this.BoxCollider(special);
        int left2 = rectangle2.Left;
        rectangle2 = other.BoxCollider();
        int left3 = rectangle2.Left;
        if (left2 < left3)
        {
          rectangle2 = this.BoxCollider(special);
          int top1 = rectangle2.Top;
          rectangle2 = other.BoxCollider();
          int bottom1 = rectangle2.Bottom;
          if (top1 < bottom1)
          {
            rectangle2 = this.BoxCollider(special);
            int bottom2 = rectangle2.Bottom;
            rectangle2 = other.BoxCollider();
            int top2 = rectangle2.Top;
            return bottom2 > top2;
          }
        }
      }
      return false;
    }

    public bool isCollidingTop(Sprite other, string special = "none")
    {
      Rectangle rectangle1 = this.BoxCollider(special);
      double num = (double) rectangle1.Top + (double) this.velocity.Y * (double) Globals.deltaTime;
      rectangle1 = other.BoxCollider();
      double bottom1 = (double) rectangle1.Bottom;
      if (num < bottom1)
      {
        Rectangle rectangle2 = this.BoxCollider(special);
        int bottom2 = rectangle2.Bottom;
        rectangle2 = other.BoxCollider();
        int bottom3 = rectangle2.Bottom;
        if (bottom2 > bottom3)
        {
          rectangle2 = this.BoxCollider(special);
          int left1 = rectangle2.Left;
          rectangle2 = other.BoxCollider();
          int right1 = rectangle2.Right;
          if (left1 < right1)
          {
            rectangle2 = this.BoxCollider(special);
            int right2 = rectangle2.Right;
            rectangle2 = other.BoxCollider();
            int left2 = rectangle2.Left;
            return right2 > left2;
          }
        }
      }
      return false;
    }

    public bool isCollidingBottom(Sprite other, string special = "none")
    {
      if (!(this is Bumerang) && other is Floaty)
      {
        if ((double) this.BoxCollider(special).Bottom + (double) this.velocity.Y * (double) Globals.deltaTime > (double) other.BoxCollider().Top && (double) this.BoxCollider(special).Top + (double) this.dims.Y - 5.0 < (double) other.BoxCollider().Top)
        {
          int left1 = this.BoxCollider(special).Left;
          Rectangle rectangle = other.BoxCollider();
          int right1 = rectangle.Right;
          if (left1 < right1)
          {
            rectangle = this.BoxCollider(special);
            int right2 = rectangle.Right;
            rectangle = other.BoxCollider();
            int left2 = rectangle.Left;
            return right2 > left2;
          }
        }
        return false;
      }
      if ((double) this.BoxCollider(special).Bottom + (double) this.velocity.Y * (double) Globals.deltaTime > (double) other.BoxCollider().Top)
      {
        int top1 = this.BoxCollider(special).Top;
        Rectangle rectangle = other.BoxCollider();
        int top2 = rectangle.Top;
        if (top1 < top2)
        {
          rectangle = this.BoxCollider(special);
          int left3 = rectangle.Left;
          rectangle = other.BoxCollider();
          int right3 = rectangle.Right;
          if (left3 < right3)
          {
            rectangle = this.BoxCollider(special);
            int right4 = rectangle.Right;
            rectangle = other.BoxCollider();
            int left4 = rectangle.Left;
            return right4 > left4;
          }
        }
      }
      return false;
    }

    public bool isCollidingBarLeft(Sprite other, string special = "none")
    {
      Rectangle rectangle1 = this.BoxCollider(special);
      double num1 = (double) rectangle1.Left + (double) this.velocity.X * (double) Globals.deltaTime;
      rectangle1 = other.BoxCollider();
      double num2 = (double) rectangle1.Right + (double) other.velocity.X * (double) Globals.deltaTime;
      if (num1 < num2)
      {
        Rectangle rectangle2 = this.BoxCollider(special);
        int right1 = rectangle2.Right;
        rectangle2 = other.BoxCollider();
        int right2 = rectangle2.Right;
        if (right1 > right2)
        {
          rectangle2 = this.BoxCollider(special);
          int top1 = rectangle2.Top;
          rectangle2 = other.BoxCollider();
          int bottom1 = rectangle2.Bottom;
          if (top1 < bottom1)
          {
            rectangle2 = this.BoxCollider(special);
            int bottom2 = rectangle2.Bottom;
            rectangle2 = other.BoxCollider();
            int top2 = rectangle2.Top;
            return bottom2 > top2;
          }
        }
      }
      return false;
    }

    public bool isCollidingBarRight(Sprite other, string special = "none")
    {
      Rectangle rectangle1 = this.BoxCollider(special);
      double num1 = (double) rectangle1.Right + (double) this.velocity.X * (double) Globals.deltaTime;
      rectangle1 = other.BoxCollider();
      double num2 = (double) rectangle1.Left + (double) other.velocity.X * (double) Globals.deltaTime;
      if (num1 > num2)
      {
        Rectangle rectangle2 = this.BoxCollider(special);
        int left1 = rectangle2.Left;
        rectangle2 = other.BoxCollider();
        int left2 = rectangle2.Left;
        if (left1 < left2)
        {
          rectangle2 = this.BoxCollider(special);
          int top1 = rectangle2.Top;
          rectangle2 = other.BoxCollider();
          int bottom1 = rectangle2.Bottom;
          if (top1 < bottom1)
          {
            rectangle2 = this.BoxCollider(special);
            int bottom2 = rectangle2.Bottom;
            rectangle2 = other.BoxCollider();
            int top2 = rectangle2.Top;
            return bottom2 > top2;
          }
        }
      }
      return false;
    }

    public bool isCollidingBarTop(Sprite other, string special = "none")
    {
      Rectangle rectangle1 = this.BoxCollider(special);
      double num1 = (double) rectangle1.Top + (double) this.velocity.Y * (double) Globals.deltaTime;
      rectangle1 = other.BoxCollider();
      double num2 = (double) rectangle1.Bottom + (double) other.velocity.Y * (double) Globals.deltaTime;
      if (num1 < num2)
      {
        Rectangle rectangle2 = this.BoxCollider(special);
        int bottom1 = rectangle2.Bottom;
        rectangle2 = other.BoxCollider();
        int bottom2 = rectangle2.Bottom;
        if (bottom1 > bottom2)
        {
          rectangle2 = this.BoxCollider(special);
          int left1 = rectangle2.Left;
          rectangle2 = other.BoxCollider();
          int right1 = rectangle2.Right;
          if (left1 < right1)
          {
            rectangle2 = this.BoxCollider(special);
            int right2 = rectangle2.Right;
            rectangle2 = other.BoxCollider();
            int left2 = rectangle2.Left;
            return right2 > left2;
          }
        }
      }
      return false;
    }

    public bool isCollidingBarBottom(Sprite other, string special = "none")
    {
      Rectangle rectangle1 = this.BoxCollider(special);
      double num1 = (double) rectangle1.Bottom + (double) this.velocity.Y * (double) Globals.deltaTime;
      rectangle1 = other.BoxCollider();
      double num2 = (double) rectangle1.Top + (double) other.velocity.Y * (double) Globals.deltaTime;
      if (num1 > num2)
      {
        Rectangle rectangle2 = this.BoxCollider(special);
        int top1 = rectangle2.Top;
        rectangle2 = other.BoxCollider();
        int top2 = rectangle2.Top;
        if (top1 < top2)
        {
          rectangle2 = this.BoxCollider(special);
          int left1 = rectangle2.Left;
          rectangle2 = other.BoxCollider();
          int right1 = rectangle2.Right;
          if (left1 < right1)
          {
            rectangle2 = this.BoxCollider(special);
            int right2 = rectangle2.Right;
            rectangle2 = other.BoxCollider();
            int left2 = rectangle2.Left;
            return right2 > left2;
          }
        }
      }
      return false;
    }
  }
}
