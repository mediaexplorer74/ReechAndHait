
// Type: PigeonProject.Utility.Debounce
// Assembly: PigeonProject, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ED64DA4-BF9A-4998-A25D-EAF7C565AAB8
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework;
using System;
using System.Diagnostics;

#nullable disable
namespace PigeonProject.Utility
{
  public class Debounce
  {
    public const int FLAG_RESET_TIME = -1;
    public const int FLAG_RUN_PERMANENTLY = -7;

    public TerminateEffect TerminateEffect { get; set; }

    public UpdateEffect UpdateEffect { get; set; }

    public double RemainingTime { get; private set; } = -1.0;

    public double Time { get; set; } = -1.0;

    public bool IsRunning => this.RemainingTime > 0.0;

    public Debounce(double time = -1.0, TerminateEffect function = null, bool startImmediately = false)
    {
      this.TerminateEffect = function;
      this.Time = time;
      if (!startImmediately)
        return;
      this.Start();
    }

    public void Start(TerminateEffect function)
    {
      this.TerminateEffect = function;
      this.Start();
    }

    public void Start(double time, TerminateEffect function)
    {
      this.TerminateEffect = function;
      this.Start(time);
    }

    public void Start()
    {
      if (this.Time != -1.0)
        this.Start(this.Time);
      else
        Debug.WriteLine("WARNING: Trying ot start debounce without a time specified!");
    }

    public void Start(double time)
    {
      this.RemainingTime = time;
      DebounceManager.Register(this);
    }

    public void Update(GameTime gameTime)
    {
      UpdateEffect updateEffect = this.UpdateEffect;
      if (updateEffect != null)
        updateEffect(gameTime);
      if (this.RemainingTime == -7.0)
        return;
      this.RemainingTime -= gameTime.ElapsedGameTime.TotalSeconds;
      if (this.RemainingTime > 0.0)
        return;
      this.Terminate();
    }

    public void Terminate(bool invokeTerminateEffect = true)
    {
      DebounceManager.UnRegister(this);
      this.RemainingTime = -1.0;
      if (!invokeTerminateEffect)
        return;
      TerminateEffect terminateEffect = this.TerminateEffect;
      if (terminateEffect == null)
        return;
      terminateEffect();
    }

    public void SetTerminateEffect(TerminateEffect function) => this.TerminateEffect = function;

    public void RemoveTerminateEffect() => this.TerminateEffect = (TerminateEffect) null;
  }
}
