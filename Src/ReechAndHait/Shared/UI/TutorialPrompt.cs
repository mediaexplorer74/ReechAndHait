
// Type: ReachHigh.Shared.TutorialPrompt
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
  public class TutorialPrompt : UiElementCollection
  {
    private static readonly GameObjectManager manager 
            = new GameObjectManager((PigeonProject.StateMachine.StateManager) null, 
                registerObjectAnimationManager: true);

    private readonly Rectangle area;
        
    //RnD
    private readonly bool checkGecko = true;
    private readonly bool checkMonkey = true;
    
    private bool geckoActive;
    private bool monkeyActive;
    
    private ObjectAnimation geckoAnimation;
    private ObjectAnimation monkeyAnimation;
    public const int WIDTH = 120;
    public const int HEIGHT = 120;
    private static readonly Vector2 OFFSET = new Vector2(0.0f, -100f);
    private const double FADE_DURATION = 0.2;

    public TutorialPrompt(Rectangle area, TutorialPrompts promptType, Players targetPlayer = Players.None)
      : base(Vector2.Zero, (Texture2D) null, 0, 0, TutorialPrompt.manager)
    {
      this.area = area;
      switch (targetPlayer)
      {
        case Players.Gecko:
          this.checkMonkey = false;
          break;
        case Players.Monkey:
          this.checkGecko = false;
          break;
      }
      TutorialPromptDescriptor descriptor;

      if (!TutorialPromptSettings.TryGetDescriptor(promptType, out descriptor))
        return;

      if (descriptor.GeckoPromptAssetName.Length > 0)
      {
        string str = RhPlayerSettings.Get<InputMode>(Players.Gecko, "InputMode") 
                    == InputMode.Gamepad ? "_controller" : "_mouse";
        this.SetupPromptElement("gecko", descriptor.GeckoPromptAssetName + str, descriptor.Dimensions);
      }
      else
        this.checkGecko = false;

      if (descriptor.MonkeyPromptAssetName.Length > 0)
      {
        string str = RhPlayerSettings.Get<InputMode>(Players.Monkey, "InputMode") 
                    == InputMode.Gamepad ? "_controller" : "_mouse";
        this.SetupPromptElement("monkey", descriptor.MonkeyPromptAssetName + str, descriptor.Dimensions);
      }
      else
        this.checkMonkey = false;
    }

    public static void UpdateManager(GameTime gameTime)
    {
      if (Globals.IsSequenceActive)
        return;
      TutorialPrompt.manager.Update(gameTime);
    }

    public static void DrawManager(SpriteBatch spriteBatch, GameTime gameTime)
    {
      if (Globals.IsSequenceActive)
        return;
      TutorialPrompt.manager.Draw(spriteBatch, gameTime);
    }

    public static void ResetManager() => TutorialPrompt.manager.Reset();

    public override void Update(GameTime gameTime)
    {
      base.Update(gameTime);

      if (this.checkGecko)
        this.UpdatePromptElement("gecko", Globals.geckoPos, ref this.geckoAnimation, ref this.geckoActive);

      if (!this.checkMonkey)
        return;

      this.UpdatePromptElement("monkey", Globals.monkeyPos, ref this.monkeyAnimation, ref this.monkeyActive);
    }

    private void SetupPromptElement(string elementKey, string assetKey, Vector2 dimensions)
    {
      string key = elementKey;
      UiElement element = new UiElement(Vector2.Zero, ContentHandler.GetAsset<Texture2D>(assetKey), 
          (int) dimensions.X, (int) dimensions.Y, TutorialPrompt.manager);
      element.Opacity = 0.0f;
      this.RegisterUiElement(key, element);
    }

    private void UpdatePromptElement(
      string elementKey,
      Vector2 playerPosition,
      ref ObjectAnimation fadeAnimation,
      ref bool isActive)
    {
      UiElement uiElement = this.GetUiElement(elementKey);
      if (Globals.IsSequenceActive)
        return;
      if (this.area.Contains(playerPosition))
      {
        if (!isActive)
        {
          this.ExecuteFadeAnimation(ref fadeAnimation, (GameObject) uiElement);
          isActive = true;
        }
      }
      else if (isActive)
      {
        this.ExecuteFadeAnimation(ref fadeAnimation, (GameObject) uiElement, false);
        isActive = false;
      }
      uiElement.RelativePosition = playerPosition + TutorialPrompt.OFFSET;
    }

    private void ExecuteFadeAnimation(
      ref ObjectAnimation animation,
      GameObject animatedObject,
      bool fadeIn = true)
    {
      this.GameObjectManager.ObjectAnimationManager.UnRegister((IObjectAnimation) animation);
      float opacity = animatedObject.Opacity;
      float endValue = 1f;
      if (!fadeIn)
        endValue = 0.0f;
      animation = new ObjectAnimation((object) animatedObject, 
          this.GameObjectManager.ObjectAnimationManager, "Opacity", opacity, endValue, 0.2, 
          easingFunction: new EasingFunction(EasingFunctions.EaseInSine));
    }
  }
}
