
// Type: ReachHigh.Shared.JoinUi
// Assembly: ReachHigh.Windows, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E7AAE051-E6AD-4505-A6E0-970B36D3CBD7
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PigeonProject.Animation;
using PigeonProject.ContentManagement;
using PigeonProject.Objects;
using PigeonProject.UserInterface;

#nullable disable
namespace ReachHigh.Shared
{
  internal class JoinUi(GameObjectManager manager) : UiElement(JoinUi.POSITION, ContentHandler.GetAsset<Texture2D>("titlescreen_gecko_login"), Main.screenWidth, Main.screenHeight, manager)
  {
    private int currentIndex;
    private static readonly Vector2 POSITION = new Vector2((float) (Main.screenWidth / 2), (float) (Main.screenHeight / 2));
    private static readonly string[] ASSET_NAMES = new string[6]
    {
      "titlescreen_gecko_login",
      "titlescreen_gecko_controller_check",
      "titlescreen_ready_checked1",
      "titlescreen_gecko_keyboard_check",
      "titlescreen_ready_checked2",
      "titlescreen_controller_check_both2"
    };
    private const double FADE_OUT_DURATION = 1.0;

    public void MoveForward()
    {
      switch (this.currentIndex)
      {
        case 0:
          this.currentIndex += RhPlayerSettings.Get<InputMode>(Players.Gecko, "InputMode") 
                        == InputMode.Gamepad ? 1 : 3;
          break;
        case 1:
          if (RhPlayerSettings.Get<InputMode>(Players.Monkey, "InputMode") == InputMode.Gamepad)
          {
            this.currentIndex += 4;
            break;
          }
          goto default;
        default:
          ++this.currentIndex;
          break;
      }
      this.UpdateSprite();
    }

    public void FadeOut()
    {
      ObjectAnimation objectAnimation = 
                new ObjectAnimation((object) this, this.GameObjectManager.ObjectAnimationManager, 
                "Opacity", 1f, 0.0f, easingFunction: new EasingFunction(EasingFunctions.EaseOutSine));
    }

    public void Reset()
    {
      this.currentIndex = 0;
      this.UpdateSprite();
    }

    private void UpdateSprite()
    {
      if (this.currentIndex < 0 || this.currentIndex >= JoinUi.ASSET_NAMES.Length)
        return;
      this.Sprite = ContentHandler.GetAsset<Texture2D>(JoinUi.ASSET_NAMES[this.currentIndex]);
    }
  }
}
