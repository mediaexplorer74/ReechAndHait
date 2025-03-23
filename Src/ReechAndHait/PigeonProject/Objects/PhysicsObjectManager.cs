
// Type: PigeonProject.Objects.PhysicsObjectManager
// Assembly: PigeonProject, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ED64DA4-BF9A-4998-A25D-EAF7C565AAB8
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework;
using PigeonProject.StateMachine;
using System;
using System.Collections.Generic;
using System.Diagnostics;

#nullable disable
namespace PigeonProject.Objects
{
  public class PhysicsObjectManager : IManager
  {
    private readonly List<IPhysicsObject> register = new List<IPhysicsObject>();
    private const float FRICTION = 300f;
    private const float ROTATION_FRICTION = 1.57079637f;
    private const double ROTATION_DEADZONE = 0.1;

    public StateManager StateManager { get; private set; }

    public PhysicsObjectManager(StateManager stateManager) => this.StateManager = stateManager;

    public void Update(GameTime gameTime)
    {
      try
      {
        foreach (IPhysicsObject physicsObject in this.register)
        {
          if (physicsObject.UseFriction)
          {
            this.ApplyRotationFriction(physicsObject, gameTime);
            this.ApplyVelocityFriction(physicsObject, gameTime);
          }
          this.ApplyRotation(physicsObject, gameTime);
          this.ApplyVelocity(physicsObject, gameTime);
        }
      }
      catch (Exception ex)
      {
        Debug.WriteLine("NOTE: Error with iterating the Physics Object register: " + ex.Message);
      }
    }

    private void ApplyRotation(IPhysicsObject physicsObject, GameTime gameTime)
    {
      physicsObject.Velocity = Vector2.Transform(physicsObject.Velocity, Matrix.CreateRotationZ(physicsObject.RotationVelocity * (float) gameTime.ElapsedGameTime.TotalSeconds));
    }

    private void ApplyVelocity(IPhysicsObject physicsObject, GameTime gameTime)
    {
      physicsObject.Position += physicsObject.Velocity * (float) gameTime.ElapsedGameTime.TotalSeconds;
    }

    private void ApplyRotationFriction(IPhysicsObject physicsObject, GameTime gameTime)
    {
      double num = (double) physicsObject.RotationVelocity - 1.5707963705062866 * gameTime.ElapsedGameTime.TotalSeconds;
      if (Math.Abs(num) < 0.1)
        physicsObject.RotationVelocity = 0.0f;
      else if (num > 0.0)
        physicsObject.RotationVelocity -= (float) (1.5707963705062866 * gameTime.ElapsedGameTime.TotalSeconds);
      else
        physicsObject.RotationVelocity += (float) (1.5707963705062866 * gameTime.ElapsedGameTime.TotalSeconds);
    }

    private void ApplyVelocityFriction(IPhysicsObject physicsObject, GameTime gameTime)
    {
      if ((double) physicsObject.Velocity.Length() - 300.0 * gameTime.ElapsedGameTime.TotalSeconds > 0.0)
      {
        Vector2 vector2 = -physicsObject.Velocity;
        vector2.Normalize();
        physicsObject.Velocity += vector2 * (float) (300.0 * gameTime.ElapsedGameTime.TotalSeconds);
      }
      else
        physicsObject.Velocity = Vector2.Zero;
    }

    public bool RegisterPhysicsObject(IPhysicsObject physicsObject)
    {
      if (this.register.Contains(physicsObject))
      {
        Debug.WriteLine("WARNING: Trying to register IPhysicsObject that is already registered!");
        return false;
      }
      this.register.Add(physicsObject);
      return true;
    }

    public bool RemovePhysicsObject(IPhysicsObject physicsObject)
    {
      return this.register.Remove(physicsObject);
    }

    public void Reset() => this.register.Clear();
  }
}
