
// Type: PigeonProject.UserInterface.UiElement
// Assembly: PigeonProject, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ED64DA4-BF9A-4998-A25D-EAF7C565AAB8
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PigeonProject.Objects;

#nullable disable
namespace PigeonProject.UserInterface
{
  public class UiElement : GameObject
  {
    private IUiCollection _uiCollection;
    private Vector2 _relativePosition;

    public IUiCollection UiCollection
    {
      get
      {
        return this._uiCollection == null ? (IUiCollection) new PigeonProject.UserInterface.UiCollection(Vector2.Zero) : this._uiCollection;
      }
      set
      {
        this._uiCollection = value;
        this.RelativePosition = this._relativePosition;
      }
    }

    public virtual Vector2 RelativePosition
    {
      get => this._relativePosition;
      set
      {
        this._relativePosition = value;
        this.Position = this.UiCollection.Position + this.RelativePosition;
      }
    }

    public override float LayerDepth
    {
      get => this._layerDepth * 2f;
      set => this._layerDepth = MathHelper.Clamp(value, 0.0f, 1f) * 0.5f;
    }

    public UiElement(
      Vector2 relativePosition,
      Texture2D sprite,
      int width,
      int height,
      GameObjectManager uiElementManager)
      : base(Vector2.Zero, width, height, sprite, uiElementManager)
    {
      this.RelativePosition = relativePosition;
      this._layerDepth = 0.25f;
    }

    public UiElement(
      Vector2 relativePosition,
      Texture2D sprite,
      int width,
      int height,
      string animationKey,
      string collectionKey,
      GameObjectManager uiElementManager,
      bool transitionIn = false)
      : base(Vector2.Zero, width, height, sprite, uiElementManager, animationKey, collectionKey)
    {
      this.RelativePosition = relativePosition;
      this._layerDepth = 0.25f;
    }

    public UiElement(
      Vector2 relativePosition,
      Texture2D sprite,
      int width,
      int height,
      IUiCollection uiCollection,
      GameObjectManager uiElementManager)
      : base(Vector2.Zero, width, height, sprite, uiElementManager)
    {
      this.UiCollection = uiCollection;
      this.RelativePosition = relativePosition;
      this._layerDepth = 0.25f;
    }

    public virtual void ExecuteTransition(bool transitionIn = true)
    {
    }
  }
}
