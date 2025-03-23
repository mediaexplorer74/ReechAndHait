
// Type: PigeonProject.Animation.ObjectAnimationManager
// Assembly: PigeonProject, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ED64DA4-BF9A-4998-A25D-EAF7C565AAB8
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework;
using PigeonProject.Objects;
using PigeonProject.StateMachine;
using System;
using System.Collections.Generic;
using System.Diagnostics;

#nullable disable
namespace PigeonProject.Animation
{
  public class ObjectAnimationManager : IManager
  {
    private readonly List<IObjectAnimation> register = new List<IObjectAnimation>();

    public StateManager StateManager { get; private set; }

    public ObjectAnimationManager(StateManager stateManager) => this.StateManager = stateManager;

    public IObjectAnimation Register(IObjectAnimation animation)
    {
      this.register.Add(animation);
      return animation;
    }

    public bool UnRegister(IObjectAnimation animation)
    {
      if (!this.register.Contains(animation))
        return false;
      this.register.Remove(animation);
      return true;
    }

    public void Reset() => this.register.Clear();

    public void Update(GameTime gameTime)
    {
      try
      {
        for (int index = this.register.Count - 1; index >= 0; --index)
          this.register[index].Update(gameTime);
      }
      catch (Exception ex)
      {
        Debug.WriteLine("NOTE: Error with iterating the Object Animation register: " + ex.ToString());
      }
    }
  }
}
