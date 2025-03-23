
// Type: PigeonProject.UserInterface.UiElementCollection
// Assembly: PigeonProject, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ED64DA4-BF9A-4998-A25D-EAF7C565AAB8
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PigeonProject.Objects;

#nullable disable
namespace PigeonProject.UserInterface
{
  public class UiElementCollection : UiElement, IUiCollection
  {
    private readonly PigeonProject.UserInterface.UiCollection collection;

    public override Vector2 RelativePosition
    {
      get => base.RelativePosition;
      set
      {
        base.RelativePosition = value;
        if (this.collection == null)
          return;
        this.collection.Position = this.Position;
      }
    }

    public override Vector2 Position
    {
      get => base.Position;
      set
      {
        base.Position = value;
        if (this.collection == null)
          return;
        this.collection.Position = value;
      }
    }

    public UiElementCollection(
      Vector2 relativePosition,
      Texture2D sprite,
      int width,
      int height,
      GameObjectManager uiElementManager)
      : base(relativePosition, sprite, width, height, uiElementManager)
    {
      this.collection = new PigeonProject.UserInterface.UiCollection(this.Position);
      this.collection.Position = this.Position;
    }

    public bool RegisterUiElement(string key, UiElement element)
    {
      return this.collection.AddUiElement(element, key);
    }

    public bool AddUiElement(UiElement element, string key = "")
    {
      return this.collection.AddUiElement(element, key);
    }

    public bool UnRegisterUiElement(string key) => this.collection.UnRegisterUiElement(key);

    public UiElement GetUiElement(string key) => this.collection.GetUiElement(key);

    public override void ExecuteTransition(bool transitionIn = true)
    {
      this.collection.ExecuteTransition(transitionIn);
    }
  }
}
