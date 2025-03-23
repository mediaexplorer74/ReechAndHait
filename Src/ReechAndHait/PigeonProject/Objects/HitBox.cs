
// Type: PigeonProject.Objects.HitBox
// Assembly: PigeonProject, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ED64DA4-BF9A-4998-A25D-EAF7C565AAB8
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PigeonProject.ContentManagement;
using System;
using System.Collections.Generic;

#nullable disable
namespace PigeonProject.Objects
{
  public class HitBox : IHitBox
  {
    public HitBoxManager HitBoxManager { get; private set; }

    public GameObject Parent { get; private set; }

    public Vector2 Position => this.Parent.Position;

    public int Radius { get; set; }

    public bool IsStatic { get; private set; }

    public CollisionFunction CollisionFunction { get; set; }

    public CollisionFunction EnterCollisionFunction { get; set; }

    public CollisionFunction ExitCollisionFunction { get; set; }

    public List<IHitBox> CurrentCollisions { get; } = new List<IHitBox>();

    public HitBox(GameObject parent, int radius, HitBoxManager manager, bool isStatic = false)
    {
      this.Parent = parent;
      this.Radius = radius;
      this.HitBoxManager = manager ?? throw new Exception("Trying to instantiate a HitBox with no valid HitBoxManager registered!");
      this.IsStatic = isStatic;
      this.HitBoxManager.RegisterHitBox((IHitBox) this, this.IsStatic);
      this.CollisionFunction = new CollisionFunction(this.Parent.OnCollision);
      this.EnterCollisionFunction = new CollisionFunction(this.Parent.OnCollisionEnter);
      this.ExitCollisionFunction = new CollisionFunction(this.Parent.OnCollisionExit);
    }

    public bool Delete()
    {
      if (!this.HitBoxManager.RemoveHitBox((IHitBox) this))
        return false;
      foreach (HitBox currentCollision in this.CurrentCollisions)
        this.InvokeCollisionExit((IHitBox) currentCollision, currentCollision.Position - this.Position);
      return true;
    }

    public void InvokeCollisionEnter(IHitBox other, Vector2 collisionDirection)
    {
      this.EnterCollisionFunction(other is HitBox hitBox ? hitBox.Parent : (GameObject) null, collisionDirection);
    }

    public void InvokeCollision(IHitBox other, Vector2 collisionDirection)
    {
      this.CollisionFunction(other is HitBox hitBox ? hitBox.Parent : (GameObject) null, collisionDirection);
    }

    public void InvokeCollisionExit(IHitBox other, Vector2 collisionDirection)
    {
      this.ExitCollisionFunction(other is HitBox hitBox ? hitBox.Parent : (GameObject) null, collisionDirection);
    }

    public static bool DrawHitboxes { get; set; }

    public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
    {
      if (!HitBox.DrawHitboxes)
        return;
      Texture2D asset = ContentHandler.GetAsset<Texture2D>("light-circle");
      if (asset == null)
        return;
      Rectangle destinationRectangle = new Rectangle((this.Position - new Vector2((float) this.Radius)).ToPoint(), new Point(this.Radius * 2));
      Color color = (this.IsStatic ? Color.Blue : Color.Red) with
      {
        A = 128
      };
      spriteBatch.Draw(asset, destinationRectangle, new Rectangle?(), color, 0.0f, new Vector2((float) (destinationRectangle.Width / 2), (float) (destinationRectangle.Height / 2)), SpriteEffects.None, 0.0f);
    }
  }
}
