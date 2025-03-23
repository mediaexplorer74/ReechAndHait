
// Type: PigeonProject.Objects.GameObjectManager
// Assembly: PigeonProject, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ED64DA4-BF9A-4998-A25D-EAF7C565AAB8
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PigeonProject.Animation;
using PigeonProject.StateMachine;
using System;
using System.Collections.Generic;
using System.Diagnostics;

#nullable disable
namespace PigeonProject.Objects
{
  public class GameObjectManager
  {
    private readonly List<IGameObject> register = new List<IGameObject>();
    private readonly IDictionary<Type, IManager> managers = (IDictionary<Type, IManager>) new Dictionary<Type, IManager>();

    public StateManager StateManager { get; private set; }

    public HitBoxManager HitBoxManager => this.GetManager<HitBoxManager>();

    public PhysicsObjectManager PhysicsObjectManager => this.GetManager<PhysicsObjectManager>();

    public FrameAnimationManager FrameAnimationManager => this.GetManager<FrameAnimationManager>();

    public ObjectAnimationManager ObjectAnimationManager
    {
      get => this.GetManager<ObjectAnimationManager>();
    }

    public Rectangle Deathzone { get; set; } = Rectangle.Empty;

    public bool IterateRegisterBackwards { get; set; }

    public GameObjectManager(
      StateManager stateManager,
      bool registerHitBoxManager = false,
      bool registerPhysicsObjectManager = false,
      bool registerFrameAnimationManager = false,
      bool registerObjectAnimationManager = false)
    {
      this.StateManager = stateManager;
      if (registerHitBoxManager)
        this.managers.Add(typeof (HitBoxManager), (IManager) new HitBoxManager(this.StateManager));
      if (registerPhysicsObjectManager)
        this.managers.Add(typeof (PhysicsObjectManager), (IManager) new PhysicsObjectManager(this.StateManager));
      if (registerFrameAnimationManager)
        this.managers.Add(typeof (FrameAnimationManager), (IManager) new FrameAnimationManager(this.StateManager));
      if (!registerObjectAnimationManager)
        return;
      this.managers.Add(typeof (ObjectAnimationManager), (IManager) new ObjectAnimationManager(this.StateManager));
    }

    public void Update(GameTime gameTime)
    {
      foreach (IManager manager in (IEnumerable<IManager>) this.managers.Values)
        manager.Update(gameTime);
      try
      {
        for (int index = this.register.Count - 1; index >= 0; --index)
        {
          IGameObject gameObject = this.register[index];
          if (!this.Deathzone.IsEmpty && !this.Deathzone.Contains(gameObject.Position))
            gameObject.Delete();
          else
            gameObject.Update(gameTime);
        }
      }
      catch (Exception ex)
      {
        Debug.WriteLine("Error with iterating the Game Object register: " + ex.Message);
      }
    }

    public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
    {
      if (this.IterateRegisterBackwards)
      {
        for (int index = this.register.Count - 1; index >= 0; --index)
          this.register[index].Draw(spriteBatch, gameTime);
      }
      else
      {
        foreach (IGameObject gameObject in this.register)
          gameObject.Draw(spriteBatch, gameTime);
      }
    }

    public bool RegisterGameObject(IGameObject gameObject)
    {
      if (!this.register.Contains(gameObject))
      {
        this.register.Add(gameObject);
        return true;
      }
      Debug.WriteLine("WARNING: Trying to register IGameObject that is already registered to this IGameObjectManager!");
      return false;
    }

    public bool RemoveGameObject(IGameObject gameObject) => this.register.Remove(gameObject);

    public bool Contains(IGameObject gameObject) => this.register.Contains(gameObject);

    public T GetManager<T>()
    {
      return this.managers.ContainsKey(typeof (T)) ? (T) this.managers[typeof (T)] : default (T);
    }

    public void Reset(bool deleteItems = false)
    {
      foreach (IManager manager in (IEnumerable<IManager>) this.managers.Values)
        manager.Reset();
      if (!deleteItems)
      {
        this.register.Clear();
      }
      else
      {
        for (int index = this.register.Count - 1; index >= 0; --index)
          this.register[index].Delete();
      }
    }

    public IGameObject[] GetRegisterAsArray()
    {
      IGameObject[] array = new IGameObject[this.register.Count];
      this.register.CopyTo(array);
      return array;
    }
  }
}
