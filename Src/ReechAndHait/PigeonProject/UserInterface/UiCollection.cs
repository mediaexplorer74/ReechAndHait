
// Type: PigeonProject.UserInterface.UiCollection
// Assembly: PigeonProject, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ED64DA4-BF9A-4998-A25D-EAF7C565AAB8
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;

#nullable disable
namespace PigeonProject.UserInterface
{
  public class UiCollection : IUiCollection
  {
    private Vector2 _position;
    private readonly Dictionary<string, UiElement> namedElements = new Dictionary<string, UiElement>();
    private readonly List<UiElement> uiElements = new List<UiElement>();

    public Vector2 Position
    {
      get => this._position;
      set
      {
        this._position = value;
        foreach (UiElement uiElement in this.uiElements)
          uiElement.RelativePosition = uiElement.RelativePosition;
        foreach (UiElement uiElement in this.namedElements.Values)
          uiElement.RelativePosition = uiElement.RelativePosition;
      }
    }

    public UiCollection(Vector2 position) => this.Position = position;

    public bool RegisterUiElement(string key, UiElement element)
    {
      if (this.namedElements.ContainsValue(element) || this.namedElements.ContainsKey(key))
      {
        Debug.WriteLine("WARNING: Key or UiElement already exists in the Dictionary!");
        return false;
      }
      this.namedElements.Add(key, element);
      this.uiElements.Add(element);
      element.UiCollection = (IUiCollection) this;
      return true;
    }

    public bool AddUiElement(UiElement element, string key = "")
    {
      if (key.Length > 0)
      {
        if (this.namedElements.ContainsValue(element) || this.namedElements.ContainsKey(key))
        {
          Debug.WriteLine("WARNING: Key or UiElement already exists in the Dictionary!");
          return false;
        }
        this.namedElements.Add(key, element);
      }
      this.uiElements.Add(element);
      element.UiCollection = (IUiCollection) this;
      return true;
    }

    public bool UnRegisterUiElement(string key)
    {
      if (!this.namedElements.ContainsKey(key))
      {
        Debug.WriteLine("WARNING: Trying to remove key that is not registered to Dictionary!");
        return false;
      }
      this.namedElements.Remove(key);
      return true;
    }

    public UiElement GetUiElement(string key)
    {
      return !this.namedElements.ContainsKey(key) ? (UiElement) null : this.namedElements[key];
    }

    public void ExecuteTransition(bool transitionIn = true)
    {
      foreach (UiElement uiElement in this.uiElements)
        uiElement.ExecuteTransition(transitionIn);
    }
  }
}
