
// Type: PigeonProject.Animation.FrameAnimationHandler
// Assembly: PigeonProject, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ED64DA4-BF9A-4998-A25D-EAF7C565AAB8
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PigeonProject.Utility;
using System;
using System.Collections.Generic;
using System.Diagnostics;

#nullable disable
namespace PigeonProject.Animation
{
  public class FrameAnimationHandler
  {
    private readonly Dictionary<string, FrameAnimation> animations = new Dictionary<string, FrameAnimation>();

    public string DefaultAnimationKey { get; private set; }

    public FrameAnimation DefaultAnimation => this.animations[this.DefaultAnimationKey];

    public string ActiveAnimationKey { get; private set; }

    public FrameAnimation ActiveAnimation => this.animations[this.ActiveAnimationKey];

    public FrameAnimationHandler(string animationKey, FrameAnimation animation)
    {
      this.RegisterAnimation(animationKey, animation, isDefault: true);
    }

    public FrameAnimationHandler(
      string animationKey,
      string collectionKey,
      FrameAnimationManager manager)
    {
      this.RegisterAnimation(animationKey, new FrameAnimation(collectionKey, manager), isDefault: true);
    }

    public FrameAnimationHandler(
      string animationKey,
      string spriteSheetKey,
      int startIndex,
      int endIndex,
      FrameAnimationManager manager)
    {
      this.RegisterAnimation(animationKey, spriteSheetKey, startIndex, endIndex, manager, isDefault: true);
    }

    public FrameAnimation RegisterAnimation(
      string animationKey,
      FrameAnimation animation,
      bool isLooping = false,
      bool isDefault = false)
    {
      if (this.animations.ContainsKey(animationKey))
        return (FrameAnimation) null;
      if (this.animations.ContainsValue(animation))
        return (FrameAnimation) null;
      this.animations.Add(animationKey, animation);
      if (isDefault)
      {
        animation.IsLooping = true;
        this.DefaultAnimationKey = animationKey;
        this.ActiveAnimationKey = animationKey;
        animation.Play();
        return animation;
      }
      if (isLooping)
        animation.IsLooping = true;
      return animation;
    }

    public FrameAnimation RegisterAnimation(
      string animationKey,
      string collectionKey,
      FrameAnimationManager manager,
      bool isLooping = false,
      bool isDefault = false)
    {
      FrameAnimation animation = new FrameAnimation(collectionKey, manager);
      return animation == null ? (FrameAnimation) null : this.RegisterAnimation(animationKey, animation, isLooping, isDefault);
    }

    public FrameAnimation RegisterAnimation(
      string animationKey,
      string spritesheetKey,
      int startIndex,
      int endIndex,
      FrameAnimationManager manager,
      bool isLooping = false,
      bool isDefault = false)
    {
      FrameAnimation animation = new FrameAnimation(spritesheetKey, startIndex, endIndex, manager);
      return animation == null ? (FrameAnimation) null : this.RegisterAnimation(animationKey, animation, isLooping, isDefault);
    }

    public bool UnRegisterAnimation(string animationKey)
    {
      if (!this.animations.ContainsKey(animationKey))
        return false;
      this.animations.Remove(animationKey);
      return true;
    }

    public bool SetDefaultAnimation(string animationKey, bool switchImmediately = true)
    {
      if (!this.animations.ContainsKey(animationKey))
        return false;
      this.DefaultAnimationKey = animationKey;
      if (switchImmediately)
        this.ReturnToDefaultAnimation();
      return true;
    }

    public FrameAnimation GetAnimation(string animationKey)
    {
      return !this.animations.ContainsKey(animationKey) ? (FrameAnimation) null : this.animations[animationKey];
    }

    public string GetCurrentFrameKey() => this.ActiveAnimation.GetCurrentFrameKey();

    public Texture2D GetCurrentFrameImage(out Rectangle sourceRectangle)
    {
      try
      {
        return this.ActiveAnimation.GetCurrentFrameImage(out sourceRectangle);
      }
      catch (Exception ex)
      {
        Debug.WriteLine("Error with loading animation frame: " + ex.Message);
        sourceRectangle = Rectangle.Empty;
        return (Texture2D) null;
      }
    }

    public bool SwitchAnimation(
      string animationKey,
      TerminateEffect terminateEffect = null,
      double duration = -1.0)
    {
      if (!this.animations.ContainsKey(animationKey) || this.ActiveAnimationKey == animationKey)
        return false;
      this.ActiveAnimation.Stop();
      this.ActiveAnimationKey = animationKey;
      if (duration < 0.0)
      {
        this.ActiveAnimation.Play(terminateEffect ?? new TerminateEffect(this.ReturnToDefaultAnimation));
      }
      else
      {
        this.ActiveAnimation.Play();
        Debounce debounce = new Debounce(duration, (PigeonProject.Utility.TerminateEffect) (() =>
        {
          this.ReturnToDefaultAnimation();
          TerminateEffect terminateEffect1 = terminateEffect;
          if (terminateEffect1 == null)
            return;
          terminateEffect1();
        }), true);
      }
      return true;
    }

    public void ReturnToDefaultAnimation() => this.SwitchAnimation(this.DefaultAnimationKey);
  }
}
