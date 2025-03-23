
// Type: PigeonProject.Objects.HitBoxManager
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
  public class HitBoxManager : IManager
  {
    private readonly List<IHitBox> dynamicRegister = new List<IHitBox>();
    private readonly List<IHitBox> staticRegister = new List<IHitBox>();

    public StateManager StateManager { get; private set; }

    public HitBoxManager(StateManager stateManager) => this.StateManager = stateManager;

    public void Update(GameTime gameTime)
    {
      try
      {
        for (int index = 0; index < this.dynamicRegister.Count; ++index)
        {
          IHitBox parentHitBox = this.dynamicRegister[index];
          this.CheckDynamicRegister(parentHitBox, index + 1);
          this.CheckStaticRegister(parentHitBox);
        }
      }
      catch (Exception ex)
      {
        Debug.WriteLine("NOTE: Error with iterating the IHitBox register: " + ex.Message);
      }
    }

    private void CheckDynamicRegister(IHitBox parentHitBox, int startingIndex)
    {
      for (int index = startingIndex; index < this.dynamicRegister.Count; ++index)
      {
        IHitBox targetHitBox = this.dynamicRegister[index];
        this.HandleRegisterIteration(parentHitBox, targetHitBox);
      }
    }

    private void CheckStaticRegister(IHitBox parentHitBox)
    {
      try
      {
        foreach (IHitBox targetHitBox in this.staticRegister)
          this.HandleRegisterIteration(parentHitBox, targetHitBox);
      }
      catch (InvalidOperationException ex)
      {
        Debug.WriteLine("NOTICE: static IHitBox register got reset! Error message: " + ex.Message);
      }
    }

    private void HandleRegisterIteration(IHitBox parentHitBox, IHitBox targetHitBox)
    {
      Vector2 collisionDirection;
      if (this.CheckForCollision(parentHitBox, targetHitBox, out collisionDirection))
      {
        if (!parentHitBox.CurrentCollisions.Contains(targetHitBox))
          HitBoxManager.HandleCollisionEnter(parentHitBox, targetHitBox, collisionDirection);
        HitBoxManager.HandleCollisionOngoing(parentHitBox, targetHitBox, collisionDirection);
      }
      else
      {
        if (!parentHitBox.CurrentCollisions.Contains(targetHitBox))
          return;
        HitBoxManager.HandleCollisionExit(parentHitBox, targetHitBox, collisionDirection);
      }
    }

    private bool CheckForCollision(
      IHitBox parentHitBox,
      IHitBox targetHitBox,
      out Vector2 collisionDirection)
    {
      collisionDirection = targetHitBox.Position - parentHitBox.Position;
      return (double) collisionDirection.Length() < (double) (targetHitBox.Radius + parentHitBox.Radius);
    }

    private static void HandleCollisionEnter(
      IHitBox parentIHitBox,
      IHitBox targetIHitBox,
      Vector2 collisionDirection)
    {
      parentIHitBox.InvokeCollisionEnter(targetIHitBox, collisionDirection);
      targetIHitBox.InvokeCollisionEnter(parentIHitBox, -collisionDirection);
      parentIHitBox.CurrentCollisions.Add(targetIHitBox);
      targetIHitBox.CurrentCollisions.Add(parentIHitBox);
    }

    private static void HandleCollisionOngoing(
      IHitBox parentIHitBox,
      IHitBox targetIHitBox,
      Vector2 collisionDirection)
    {
      parentIHitBox.InvokeCollision(targetIHitBox, collisionDirection);
      targetIHitBox.InvokeCollision(parentIHitBox, -collisionDirection);
    }

    private static void HandleCollisionExit(
      IHitBox parentIHitBox,
      IHitBox targetIHitBox,
      Vector2 collisionDirection)
    {
      parentIHitBox.InvokeCollisionExit(targetIHitBox, collisionDirection);
      targetIHitBox.InvokeCollisionExit(parentIHitBox, -collisionDirection);
      parentIHitBox.CurrentCollisions.Remove(targetIHitBox);
      targetIHitBox.CurrentCollisions.Remove(parentIHitBox);
    }

    public void Reset()
    {
      this.dynamicRegister.Clear();
      this.staticRegister.Clear();
    }

    public bool RegisterHitBox(IHitBox IHitBox, bool isStatic = false)
    {
      if (this.dynamicRegister.Contains(IHitBox) || this.staticRegister.Contains(IHitBox))
      {
        Debug.WriteLine("WARNING: Trying to register IHitBox that is already registered!");
        return false;
      }
      if (isStatic)
        this.staticRegister.Add(IHitBox);
      else
        this.dynamicRegister.Add(IHitBox);
      return true;
    }

    public bool RemoveHitBox(IHitBox IHitBox)
    {
      return IHitBox.IsStatic ? this.staticRegister.Remove(IHitBox) : this.dynamicRegister.Remove(IHitBox);
    }
  }
}
