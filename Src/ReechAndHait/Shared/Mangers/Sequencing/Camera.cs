
// Type: ReachHigh.Shared.Camera
// Assembly: ReachHigh.Windows, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E7AAE051-E6AD-4505-A6E0-970B36D3CBD7
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;

#nullable disable
namespace ReachHigh.Shared
{
  public class Camera
  {
    public Matrix transform;
    private float scrollSpeed;
    public static Vector2 pos;
    public static Vector2 offset;
    public static bool locked;
    public static bool stupid;
    public static float scaleOffset;
    public static float scale;
    public static Vector2 mid;
    private bool active;
    private Rectangle innerRect;
    private Rectangle outerRect;
    private Point playerPoint;
    private static float highlightTimer;
    private static float highlightScale;
    private static Vector2 highlight;
    private static Vector2 highlightDirection;
    public static int ViewWidth = Main.screenWidth;
    public static int ViewHeight = Main.screenHeight;

    public Camera(bool ACTIVE = true, bool NEWPOS = true)
    {
      this.scrollSpeed = 1200f;
      if (NEWPOS)
      {
        Camera.pos = Vector2.Zero;
        Camera.scale = 1f;
        this.transform = Matrix.Identity;
      }
      Camera.locked = true;
      Camera.highlightTimer = -1f;
      this.active = ACTIVE;
    }

    public virtual void Update(float deltaSeconds)
    {
      Camera.mid = Vector2.Transform(new Vector2((float) (Camera.ViewWidth / 2), (float) (Camera.ViewHeight / 2)), Matrix.Invert(this.transform));
      this.playerPoint = new Point(Convert.ToInt32(Globals.geckoPos.X), Convert.ToInt32(Globals.geckoPos.Y));
      this.outerRect = new Rectangle((int) Camera.mid.X - (int) ((double) Camera.ViewWidth * (1.6 - (double) Camera.scale) / 0.85000002384185791 / 2.0), (int) Camera.mid.Y - (int) ((double) Camera.ViewHeight * (1.6 - (double) Camera.scale) / 0.89999997615814209 / 2.0), (int) ((double) Camera.ViewWidth * (1.6 - (double) Camera.scale) / 0.85000002384185791), (int) ((double) Camera.ViewHeight * (1.6 - (double) Camera.scale) / 0.89999997615814209));
      this.innerRect = new Rectangle((int) Camera.mid.X - (int) ((double) Camera.ViewWidth * (1.6 - (double) Camera.scale) / 1.2000000476837158 / 2.0), (int) Camera.mid.Y - (int) ((double) Camera.ViewHeight * (1.6 - (double) Camera.scale) / 1.2000000476837158 / 2.0), (int) ((double) Camera.ViewWidth * (1.6 - (double) Camera.scale) / 1.2000000476837158), (int) ((double) Camera.ViewHeight * (1.6 - (double) Camera.scale) / 1.2000000476837158));
      Camera.highlightDirection = Globals.Normalize(Camera.highlight - Camera.pos);
      if (!this.active)
        return;
      if ((double) Camera.highlightTimer >= 0.0)
      {
        if ((double) Globals.GetDistance(Camera.pos, Camera.highlight) > (double) deltaSeconds * (double) this.scrollSpeed)
          Camera.pos += Camera.highlightDirection * deltaSeconds * this.scrollSpeed;
        else
          Camera.highlightTimer -= deltaSeconds;
        if ((double) Camera.highlightScale < (double) Camera.scale)
          Camera.scale -= deltaSeconds / 4f;
        if ((double) Camera.highlightScale > (double) Camera.scale)
          Camera.scale += deltaSeconds / 4f;
      }
      else if (!Camera.locked)
      {
        if (Globals.keyboard.IsKeyDown(Keys.Up))
          Camera.pos.Y += this.scrollSpeed * deltaSeconds;
        if (Globals.keyboard.IsKeyDown(Keys.Down))
          Camera.pos.Y -= this.scrollSpeed * deltaSeconds;
        if (Globals.keyboard.IsKeyDown(Keys.Left))
          Camera.pos.X += this.scrollSpeed * deltaSeconds;
        if (Globals.keyboard.IsKeyDown(Keys.Right))
          Camera.pos.X -= this.scrollSpeed * deltaSeconds;
      }
      else
      {
        Camera.offset.X = (float) (Camera.ViewWidth / 2) / Camera.scale;
        Camera.offset.Y = (float) (Camera.ViewHeight / 2) / Camera.scale;
        Vector2 vector2_1 = (Globals.geckoPos + Globals.monkeyPos) / 2f;
        Vector2 vector2_2 = Camera.mid - vector2_1;
        Camera.pos = !Camera.stupid ? -Camera.mid + vector2_2 / 20f : -vector2_1;
        if (this.innerRect.Contains(this.playerPoint) && (double) Camera.scale < 1.0)
        {
          Camera.scale += deltaSeconds / 5f;
          Camera.pos -= new Vector2((float) (Main.screenWidth / 2), (float) (Main.screenHeight / 2)) * (deltaSeconds / 5f) * (1.6f - Camera.scale) * 1.6f;
        }
        else if (!this.outerRect.Contains(this.playerPoint) && (double) Camera.scale > 0.60000002384185791)
        {
          Camera.scale -= deltaSeconds / 5f;
          Camera.pos += new Vector2((float) (Main.screenWidth / 2), (float) (Main.screenHeight / 2)) * (deltaSeconds / 5f) * (1.6f - Camera.scale) * 1.6f;
        }
      }
      this.transform = Matrix.CreateTranslation(Camera.pos.X, Camera.pos.Y, 0.0f) * Matrix.CreateTranslation(Camera.offset.X, Camera.offset.Y, 0.0f) * Matrix.CreateScale(Camera.scale + Camera.scaleOffset);
      Globals.transform = this.transform;
    }

    public static void Highlight(Vector2 highlighted, float seconds, float highlightedScale = 1f)
    {
      Camera.highlightTimer = seconds;
      Camera.highlight = -highlighted;
      Camera.highlightScale = highlightedScale;
    }
  }
}
