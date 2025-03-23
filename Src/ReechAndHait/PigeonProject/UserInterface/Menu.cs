
// Type: PigeonProject.UserInterface.Menu
// Assembly: PigeonProject, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ED64DA4-BF9A-4998-A25D-EAF7C565AAB8
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using PigeonProject.Utility;
using System.Collections.Generic;

#nullable disable
namespace PigeonProject.UserInterface
{
  public class Menu : UiCollection
  {
    private readonly IDictionary<Buttons, bool> IterateInteractions = (IDictionary<Buttons, bool>) new Dictionary<Buttons, bool>();
    private readonly Debounce iterateDebounce = new Debounce(0.5);
    private int _activeMenuItemIndex;

    public List<PlayerIndex> ControllingPlayers { get; }

    public List<IMenuItem> MenuItems { get; } = new List<IMenuItem>();

    public IMenuItem ActiveMenuItem => this.MenuItems[this._activeMenuItemIndex];

    public Menu(PlayerIndex[] controllingPlayers, Vector2 position)
      : base(position)
    {
      this.ControllingPlayers = new List<PlayerIndex>((IEnumerable<PlayerIndex>) controllingPlayers);
    }

    public Menu(PlayerIndex[] controllingPlayers, Vector2 position, IMenuItem[] menuItems)
      : this(controllingPlayers, position)
    {
      this.RegisterMenuItem(menuItems);
    }

    public Menu(PlayerIndex controllingPlayer, Vector2 position)
      : this(new PlayerIndex[1]{ controllingPlayer }, position)
    {
    }

    public void UpdateMenu()
    {
      foreach (PlayerIndex controllingPlayer in this.ControllingPlayers)
      {
        GamePadState state = GamePad.GetState(controllingPlayer);
        foreach (KeyValuePair<Buttons, bool> iterateInteraction in (IEnumerable<KeyValuePair<Buttons, bool>>) this.IterateInteractions)
        {
          if (state.IsButtonDown(iterateInteraction.Key))
            this.IterateMenuItems(iterateInteraction.Value);
        }
        this.ActiveMenuItem.ProcessInput(state);
      }
    }

    public void RegisterMenuItem(IMenuItem menuItem, string key = "")
    {
      if (this.MenuItems.Contains(menuItem) || !this.AddUiElement(menuItem as UiElement, key))
        return;
      this.MenuItems.Add(menuItem);
      this.ActiveMenuItem.IsActive = true;
    }

    public void RegisterMenuItem(IMenuItem[] menuItems)
    {
      foreach (IMenuItem menuItem in menuItems)
        this.RegisterMenuItem(menuItem);
    }

    public void IterateMenuItems(bool iterateForward = true)
    {
      if (this.iterateDebounce.IsRunning)
        return;
      this.ActiveMenuItem.IsActive = false;
      this._activeMenuItemIndex = MathFunctions.WrapInteger(iterateForward ? this._activeMenuItemIndex + 1 : this._activeMenuItemIndex - 1, 0, this.MenuItems.Count - 1);
      this.ActiveMenuItem.IsActive = true;
      this.iterateDebounce.Start();
    }

    public void RegisterIterateInteraction(Buttons button, bool iterateForward = true)
    {
      if (this.IterateInteractions.ContainsKey(button))
        return;
      this.IterateInteractions.Add(button, iterateForward);
    }

    public void RegisterControllingPlayer(PlayerIndex player)
    {
      if (this.ControllingPlayers.Contains(player))
        return;
      this.ControllingPlayers.Add(player);
    }

    public void RegisterControllingPlayer(PlayerIndex[] players)
    {
      foreach (PlayerIndex player in players)
        this.RegisterControllingPlayer(player);
    }

    public IMenuItem GetMenuItem(string key) => this.GetUiElement(key) as IMenuItem;
  }
}
