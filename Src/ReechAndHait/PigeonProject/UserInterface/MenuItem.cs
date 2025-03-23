
// Type: PigeonProject.UserInterface.MenuItem
// Assembly: PigeonProject, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ED64DA4-BF9A-4998-A25D-EAF7C565AAB8
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

#nullable disable
namespace PigeonProject.UserInterface
{
  public class MenuItem : UiElementCollection, IMenuItem
  {
    private bool _isActive;

    private IDictionary<Buttons, Interact> Interactions { get; } = (IDictionary<Buttons, Interact>) new Dictionary<Buttons, Interact>();

    public bool IsActive
    {
      get => this._isActive;
      set
      {
        this._isActive = value;
        if (this.IsActive)
        {
          this.GetUiElement("Active").IsVisible = true;
          this.GetUiElement("InActive").IsVisible = false;
        }
        else
        {
          this.GetUiElement("Active").IsVisible = false;
          this.GetUiElement("InActive").IsVisible = true;
        }
      }
    }

    public MenuItem(
      Vector2 relativePosition,
      UiElement inActive,
      UiElement active,
      bool transitionIn = false)
      : base(relativePosition, (Texture2D) null, 0, 0, inActive.GameObjectManager)
    {
      this.RegisterUiElement("InActive", inActive);
      this.RegisterUiElement("Active", active);
      this.IsActive = false;
    }

    public MenuItem(
      Vector2 relativePosition,
      UiElement inActive,
      UiElement active,
      UiElement inner)
      : this(relativePosition, inActive, active)
    {
      this.RegisterUiElement("Inner", inner);
    }

    public void RegisterInteraction(Buttons button, Interact interaction)
    {
      if (this.Interactions.ContainsKey(button))
        return;
      this.Interactions.Add(button, interaction);
    }

    public virtual void ProcessInput(GamePadState state)
    {
      foreach (KeyValuePair<Buttons, Interact> interaction in (IEnumerable<KeyValuePair<Buttons, Interact>>) this.Interactions)
      {
        if (state.IsButtonDown(interaction.Key))
          interaction.Value();
      }
    }
  }
}
