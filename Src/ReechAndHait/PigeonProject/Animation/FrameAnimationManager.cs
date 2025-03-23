
// Type: PigeonProject.Animation.FrameAnimationManager
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
  public class FrameAnimationManager : IManager
  {
    private readonly List<IFrameAnimation> register = new List<IFrameAnimation>();

    public StateManager StateManager { get; private set; }

    public FrameAnimationManager(StateManager stateManager) => this.StateManager = stateManager;

    public void Update(GameTime gameTime)
    {
      try
      {
        for (int index = this.register.Count - 1; index >= 0; --index)
          this.register[index].Update(gameTime);
      }
      catch (Exception ex)
      {
        Debug.WriteLine("NOTE: Error with iterating the Frame Animation register: " + ex.Message);
      }
    }

    public bool Register(IFrameAnimation animation)
    {
      if (this.register.Contains(animation))
        return false;
      this.register.Add(animation);
      return true;
    }

    public bool UnRegister(IFrameAnimation animation) => this.register.Remove(animation);

    public void Reset() => this.register.Clear();
  }
}
