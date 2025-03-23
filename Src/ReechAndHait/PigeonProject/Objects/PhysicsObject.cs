
// Type: PigeonProject.Objects.PhysicsObject
// Assembly: PigeonProject, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ED64DA4-BF9A-4998-A25D-EAF7C565AAB8
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

#nullable disable
namespace PigeonProject.Objects
{
  public class PhysicsObject : GameObject, IPhysicsObject
  {
    private Vector2 _velocity;
    private const float BUMPERFORCE = 30f;

    public PhysicsObjectManager PhysicsObjectManager { get; private set; }

    public Vector2 Velocity
    {
      get => this._velocity;
      set
      {
        this._velocity = value;
        if ((double) this._velocity.Length() <= 0.0)
          return;
        this.Direction = this._velocity;
      }
    }

    public float RotationVelocity { get; set; }

    public bool UseFriction { get; set; }

    public bool IsTrigger { get; set; }

    public PhysicsObject(
      Rectangle transform,
      Texture2D sprite,
      GameObjectManager gameObjectManager)
      : base(transform, sprite, gameObjectManager)
    {
      this.PhysicsObjectManager = gameObjectManager.PhysicsObjectManager != null ? gameObjectManager.PhysicsObjectManager : throw new Exception("Trying to instantiate a PhysicsObject with no valid PhysicsObjectManager registered!");
      this.PhysicsObjectManager.RegisterPhysicsObject((IPhysicsObject) this);
    }

    public PhysicsObject(
      Rectangle transform,
      Texture2D sprite,
      GameObjectManager gameObjectManager,
      int radius,
      bool isStatic = false,
      bool useFriction = false,
      bool isTrigger = false)
      : this(transform, sprite, gameObjectManager)
    {
      this.RegisterHitBox(radius, isStatic);
      this.UseFriction = useFriction;
      this.IsTrigger = isTrigger;
    }

    public PhysicsObject(
      Vector2 position,
      int width,
      int height,
      Texture2D sprite,
      GameObjectManager gameObjectManager,
      int radius,
      bool isStatic = false,
      bool useFriction = false,
      bool isTrigger = false)
      : this(new Rectangle((int) ((double) position.X - (double) (width / 2)), (int) ((double) position.Y - (double) (height / 2)), width, height), sprite, gameObjectManager, radius, isStatic, useFriction, isTrigger)
    {
    }

    public override void OnCollision(GameObject other, Vector2 collisionDirection)
    {
      if (other is PhysicsObject)
      {
        PhysicsObject physicsObject = (PhysicsObject) other;
        if (!this.IsTrigger && !physicsObject.IsTrigger)
        {
          Vector2 vector2 = -collisionDirection;
          vector2.Normalize();
          this.AddForce(vector2 * 30f);
        }
      }
      base.OnCollision(other, collisionDirection);
    }

    public override bool Delete()
    {
      this.PhysicsObjectManager.RemovePhysicsObject((IPhysicsObject) this);
      return base.Delete();
    }

    public void AddForce(Vector2 force) => this.Velocity += force;
  }
}
