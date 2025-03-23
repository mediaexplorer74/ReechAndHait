
// Type: PigeonProject.StateMachine.StateManager
// Assembly: PigeonProject, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ED64DA4-BF9A-4998-A25D-EAF7C565AAB8
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PigeonProject.InputManagement;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

#nullable disable
namespace PigeonProject.StateMachine
{
  public class StateManager
  {
    private Dictionary<string, IState> states;
    private IState activeState;
    private IState titleState;
    private readonly Dictionary<string, object> safe;

    public Game GameInstance { get; private set; }

    public InputManager InputManager { get; private set; }

    public string ActiveState
    {
      get
      {
        return this.states.FirstOrDefault<KeyValuePair<string, IState>>
                    ((Func<KeyValuePair<string, IState>, bool>) (x => x.Value == this.activeState)).Key;
      }
    }

    public StateManager(Game gameInstance)
    {
      this.GameInstance = gameInstance;
      this.InputManager = new InputManager(this);
      this.states = new Dictionary<string, IState>();
      this.safe = new Dictionary<string, object>();
    }

    public bool RegisterState<T>(string stateKey, bool isTitleState = false)
    {
      if (this.states.ContainsKey(stateKey))
        return false;
      if (!((T) Activator.CreateInstance(typeof (T), (object) this) is IState instance))
        return false;
      this.states.Add(stateKey, instance);
      if (isTitleState)
        this.titleState = instance;
      return true;
    }

    public void ReloadStates()
    {
      Dictionary<string, IState> dictionary = new Dictionary<string, IState>();
      foreach (KeyValuePair<string, IState> state in this.states)
      {
        Type type = state.Value.GetType();
        dictionary.Add(state.Key, (IState) Activator.CreateInstance(type, (object) this));
      }
      this.states = dictionary;
    }

    public void Initialize()
    {
      this.activeState = this.titleState;
      this.activeState.Initialize();
    }

    public void Update(GameTime gameTime)
    {
      if (this.activeState == null)
        return;
      this.activeState.Update(gameTime);
    }

    public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
    {
      if (this.activeState == null)
        return;
      this.activeState.Draw(spriteBatch, gameTime);
    }

    public bool ChangeToState(string stateKey, bool terminate = true, bool initialize = true)
    {
      if (this.states.ContainsKey(stateKey))
        return this.ChangeToState(this.states[stateKey], terminate, initialize);
      Debug.WriteLine("WARNING: State '" + stateKey + "' is not registered!");
      return false;
    }

    public bool ChangeToState(IState state, bool terminate = true, bool initialize = true)
    {
      if (terminate)
        this.activeState.Terminate();
      this.activeState = state;
      if (initialize)
        this.activeState.Initialize();
      this.activeState.IsInputDelayed = true;
      return true;
    }

    public IState GetState(string stateKey)
    {
      if (this.states.ContainsKey(stateKey))
        return this.states[stateKey];
      Debug.WriteLine("WARNING: " + stateKey + " is not a registered state!");
      return (IState) null;
    }

    public void Save(string saveKey, object savedObject)
    {
      if (this.safe.ContainsKey(saveKey))
        this.safe[saveKey] = savedObject;
      else
        this.safe.Add(saveKey, savedObject);
    }

    public object GetSavedObject(string saveKey)
    {
      return !this.safe.ContainsKey(saveKey) ? (object) null : this.safe[saveKey];
    }

    public bool DeleteSavedObject(string saveKey)
    {
      if (!this.safe.ContainsKey(saveKey))
        return false;
      this.safe.Remove(saveKey);
      return true;
    }

    public void ClearSave() => this.safe.Clear();
  }
}
