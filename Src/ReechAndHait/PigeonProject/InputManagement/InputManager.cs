
// Type: PigeonProject.InputManagement.InputManager
// Assembly: PigeonProject, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ED64DA4-BF9A-4998-A25D-EAF7C565AAB8
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework;
using PigeonProject.StateMachine;
using System;
using System.Collections.Generic;
using System.Diagnostics;

#nullable disable
namespace PigeonProject.InputManagement
{
  public class InputManager
  {
    private readonly List<InputController> register = new List<InputController>();

    public StateManager StateManager { get; private set; }

    public int PlayerCount => this.register.Count;

    public bool IsAllInputLocked { get; set; }

    public InputManager(StateManager stateManager) => this.StateManager = stateManager;

    public void Update(GameTime gameTime)
    {
      if (this.IsAllInputLocked)
        return;
      try
      {
        foreach (InputController inputController in this.register)
          inputController.Update(gameTime);
      }
      catch (Exception ex)
      {
        Debug.WriteLine("NOTE: Error with iterating the Input Controller register: " + ex.Message);
      }
    }

    public InputController RegisterController(InputController controller)
    {
      if (this.register.Contains(controller))
      {
        Debug.WriteLine("WARNING: Trying to register InputController that is already registered!");
        return (InputController) null;
      }
      this.register.Add(controller);
      return controller;
    }

    public bool UnRegisterController(InputController controller)
    {
      if (!this.register.Contains(controller))
      {
        Debug.WriteLine("WARNING: Trying to remove InputController that is not registered to this InputManager!");
        return false;
      }
      this.register.Remove(controller);
      return true;
    }

    public List<InputController> GetRegister() => this.register;

    public InputController GetControllerByIndex(PlayerIndex controllerIndex)
    {
      foreach (InputController controllerByIndex in this.register)
      {
        if (controllerByIndex.ControllerIndex == controllerIndex)
          return controllerByIndex;
      }
      return (InputController) null;
    }

    public bool IsControllerRegistered(PlayerIndex controllerIndex)
    {
      return this.GetControllerByIndex(controllerIndex) != null;
    }

    public void Reset() => this.register.Clear();

    public void ResetPlayers()
    {
      foreach (InputController inputController in this.register)
        inputController.Player = (IPlayer) null;
    }
  }
}
