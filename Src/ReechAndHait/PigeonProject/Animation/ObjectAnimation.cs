
// Type: PigeonProject.Animation.ObjectAnimation
// Assembly: PigeonProject, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ED64DA4-BF9A-4998-A25D-EAF7C565AAB8
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;

#nullable disable
namespace PigeonProject.Animation
{
  public class ObjectAnimation : IObjectAnimation
  {
    private EasingFunction _easingFunction;
    private readonly string[] propertyNames;
    private readonly float[] startValues;
    private readonly float[] endValues;

    public List<object> AnimatedObjects { get; protected set; } = new List<object>();

    public ObjectAnimationManager AnimationManager { get; protected set; }

    public double Duration { get; set; }

    public bool IsLooped { get; set; }

    public TerminateEffect TerminateEffect { private get; set; }

    public bool IsRunning => this.CurrentTime > 0.0;

    protected double CurrentTime { get; private set; }

    private EasingFunction EasingFunction
    {
      get => this._easingFunction ?? (EasingFunction) (x => x);
      set => this._easingFunction = value;
    }

    public ObjectAnimation(
      object animatedObject,
      ObjectAnimationManager animationManager,
      string[] propertyNames,
      float[] startValues,
      float[] endValues,
      double duration = 1.0,
      bool isLooped = false,
      EasingFunction easingFunction = null,
      bool startImmediately = true)
    {
      if (animatedObject != null)
        this.AnimatedObjects.Add(animatedObject);
      this.AnimationManager = animationManager ?? throw new Exception("ObjectAnimationManager not registered!");
      this.Duration = duration;
      this.IsLooped = isLooped;
      this.propertyNames = propertyNames;
      this.startValues = startValues;
      this.endValues = endValues;
      this.EasingFunction = easingFunction;
      if (!startImmediately)
        return;
      this.Start();
    }

    public ObjectAnimation(
      object animatedObject,
      ObjectAnimationManager animationManager,
      string[] propertyNames,
      float startValue,
      float endValue,
      double duration = 1.0,
      bool isLooped = false,
      EasingFunction easingFunction = null,
      bool startImmediately = true)
      : this(animatedObject, animationManager, propertyNames, new float[1]
      {
        startValue
      }, new float[1]{ endValue }, duration, (isLooped ? 1 : 0) != 0, easingFunction, (startImmediately ? 1 : 0) != 0)
    {
    }

    public ObjectAnimation(
      object animatedObject,
      ObjectAnimationManager animationManager,
      string propertyNames,
      float startValue,
      float endValue,
      double duration = 1.0,
      bool isLooped = false,
      EasingFunction easingFunction = null,
      bool startImmediately = true)
      : this(animatedObject, animationManager, new string[1]
      {
        propertyNames
      }, new float[1]{ startValue }, new float[1]
      {
        endValue
      }, duration, (isLooped ? 1 : 0) != 0, easingFunction, (startImmediately ? 1 : 0) != 0)
    {
    }

    public virtual void Initialize()
    {
      this.CurrentTime = 0.0;
      this.SetPropertyValues(this.startValues);
    }

    public virtual void Update(GameTime gameTime)
    {
      this.CurrentTime += gameTime.ElapsedGameTime.TotalSeconds;
      if (this.CurrentTime > this.Duration)
      {
        this.Terminate();
      }
      else
      {
        for (int val2 = 0; val2 < this.propertyNames.Length; ++val2)
        {
          int index1 = Math.Min(this.startValues.Length - 1, val2);
          int index2 = Math.Min(this.endValues.Length - 1, val2);
          float startValue = this.startValues[index1];
          float num = this.endValues[index2] - startValue;
          this.SetProperty(startValue + (float) this.EasingFunction(this.CurrentTime / this.Duration) * num, this.propertyNames[val2]);
        }
      }
    }

    protected virtual bool Terminate()
    {
      this.SetPropertyValues(this.endValues);
      TerminateEffect terminateEffect = this.TerminateEffect;
      if (terminateEffect != null)
        terminateEffect();
      if (this.IsLooped)
      {
        this.Initialize();
        return false;
      }
      this.AnimationManager.UnRegister((IObjectAnimation) this);
      return true;
    }

    private void SetPropertyValues(float value)
    {
      foreach (string propertyName in this.propertyNames)
      {
        try
        {
          this.SetProperty(value, propertyName);
        }
        catch (Exception ex)
        {
          Debug.WriteLine(string.Format("NOTE: Error with setting {0} property of object: {1}", (object) propertyName, (object) ex));
        }
      }
    }

    private void SetPropertyValues(float[] values)
    {
      for (int val2 = 0; val2 < this.propertyNames.Length; ++val2)
      {
        int index = Math.Min(values.Length - 1, val2);
        float num = values[index];
        string propertyName = this.propertyNames[val2];
        try
        {
          this.SetProperty(num, propertyName);
        }
        catch (Exception ex)
        {
          Debug.WriteLine(string.Format("NOTE: Error with setting {0} property of object: {1}", (object) propertyName, (object) ex));
        }
      }
    }

    private void SetProperty(float value, string property)
    {
      foreach (object animatedObject in this.AnimatedObjects)
      {
        PropertyInfo property1 = animatedObject.GetType().GetProperty(property);
        if (property1 == (PropertyInfo) null)
          throw new Exception(string.Format("Could not find property '{0} of object '{1}", (object) property, animatedObject));
        object obj = Convert.ChangeType((object) value, property1.PropertyType);
        property1.SetValue(animatedObject, obj);
      }
    }

    public bool RegisterAnimatedObject(object animatedObject)
    {
      if (this.AnimatedObjects.Contains(animatedObject))
        return false;
      this.AnimatedObjects.Add(animatedObject);
      return true;
    }

    public bool UnregisterAnimatedObject(object animatedObject)
    {
      return this.AnimatedObjects.Remove(animatedObject);
    }

    public void Start() => this.AnimationManager.Register((IObjectAnimation) this);
  }
}
