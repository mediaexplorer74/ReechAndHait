
// Type: PigeonProject.Animation.FrameAnimation
// Assembly: PigeonProject, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ED64DA4-BF9A-4998-A25D-EAF7C565AAB8
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PigeonProject.ContentManagement;
using System;

#nullable disable
namespace PigeonProject.Animation
{
  public class FrameAnimation : IFrameAnimation
  {
    private float _animationSpeed = 24f;
    private double timer;

    public FrameAnimationManager FrameAnimationManager { get; private set; }

    public float AnimationSpeed
    {
      get => this._animationSpeed;
      set => this._animationSpeed = (double) value > 0.0 ? value : 1f;
    }

    public float AnimationDuration
    {
      get => (float) this.FrameCount / this.AnimationSpeed;
      set => this.AnimationSpeed = (float) this.FrameCount / value;
    }

    public int CurrentFrame { get; private set; }

    public bool IsLooping { get; set; }

    public int FrameCount => this.EndIndex - this.StartIndex + 1;

    public bool PlayBackward { get; set; }

    public TerminateEffect TerminateEffect { get; set; }

    private string[] FrameKeys { get; }

    private SpriteSheet SpriteSheet { get; }

    private int StartIndex { get; }

    private int EndIndex { get; }

    public FrameAnimation(string[] frameKeys, FrameAnimationManager manager)
    {
      if (frameKeys != null)
      {
        this.FrameKeys = frameKeys;
        this.StartIndex = 0;
        this.EndIndex = this.FrameKeys.Length - 1;
        this.CurrentFrame = this.StartIndex;
      }
      this.FrameAnimationManager = manager ?? throw new Exception("FrameAnimationManager not registered!");
    }

    public FrameAnimation(string collectionKey, FrameAnimationManager manager)
      : this(ContentHandler.GetCollectionKeys<Texture2D>(collectionKey).ToArray(), manager)
    {
    }

    public FrameAnimation(
      SpriteSheet spriteSheet,
      int startIndex,
      int endIndex,
      FrameAnimationManager manager)
      : this((string[]) null, manager)
    {
      this.SpriteSheet = spriteSheet;
      this.StartIndex = Math.Min(startIndex, endIndex);
      this.EndIndex = Math.Max(startIndex, endIndex);
      this.CurrentFrame = this.StartIndex;
    }

    public FrameAnimation(
      string spriteSheetKey,
      int startIndex,
      int endIndex,
      FrameAnimationManager manager)
      : this(ContentHandler.GetAsset<SpriteSheet>(spriteSheetKey), startIndex, endIndex, manager)
    {
    }

    public void Update(GameTime gameTime)
    {
      if (this.StartIndex == this.EndIndex)
        return;
      this.timer += gameTime.ElapsedGameTime.TotalSeconds;
      if (this.timer < 1.0 / (double) this.AnimationSpeed)
        return;
      if (!this.PlayBackward)
      {
        if (++this.CurrentFrame > this.EndIndex)
        {
          this.CurrentFrame = this.StartIndex;
          if (!this.IsLooping)
            this.Stop(true);
        }
      }
      else if (--this.CurrentFrame < this.StartIndex)
      {
        this.CurrentFrame = this.EndIndex;
        if (!this.IsLooping)
          this.Stop(true);
      }
      this.timer = 0.0;
    }

    public bool Play()
    {
      this.CurrentFrame = this.PlayBackward ? this.EndIndex : this.StartIndex;
      return this.FrameAnimationManager.Register((IFrameAnimation) this);
    }

    public bool Play(TerminateEffect terminateEffect)
    {
      if (!this.Play())
        return false;
      this.TerminateEffect = terminateEffect;
      return true;
    }

    public bool Stop(bool invokeTerminateEffect = false)
    {
      if (!this.FrameAnimationManager.UnRegister((IFrameAnimation) this))
        return false;
      if (invokeTerminateEffect)
      {
        TerminateEffect terminateEffect = this.TerminateEffect;
        if (terminateEffect != null)
          terminateEffect();
      }
      return true;
    }

    public string GetCurrentFrameKey()
    {
      return this.FrameKeys == null ? "" : this.FrameKeys[this.CurrentFrame];
    }

    public Texture2D GetCurrentFrameImage()
    {
      if (this.FrameKeys != null)
        return ContentHandler.GetAsset<Texture2D>(this.GetCurrentFrameKey());
      return this.SpriteSheet != null ? this.SpriteSheet.Texture : (Texture2D) null;
    }

    public Texture2D GetCurrentFrameImage(out Rectangle sourceRectangle)
    {
      sourceRectangle = this.SpriteSheet != null ? this.SpriteSheet.GetSourceRectangle(this.CurrentFrame) : Rectangle.Empty;
      return this.GetCurrentFrameImage();
    }
  }
}
