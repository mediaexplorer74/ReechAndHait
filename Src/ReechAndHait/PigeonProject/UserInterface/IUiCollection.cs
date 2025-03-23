
// Type: PigeonProject.UserInterface.IUiCollection
// Assembly: PigeonProject, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ED64DA4-BF9A-4998-A25D-EAF7C565AAB8
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework;

#nullable disable
namespace PigeonProject.UserInterface
{
  public interface IUiCollection
  {
    Vector2 Position { get; set; }

    bool RegisterUiElement(string key, UiElement element);

    bool AddUiElement(UiElement element, string key = "");

    bool UnRegisterUiElement(string key);

    UiElement GetUiElement(string key);
  }
}
