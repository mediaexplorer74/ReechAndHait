
// Type: ReachHigh.Shared.TutorialPromptDescriptor
// Assembly: ReachHigh.Windows, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E7AAE051-E6AD-4505-A6E0-970B36D3CBD7
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework;

#nullable disable
namespace ReachHigh.Shared
{
  internal struct TutorialPromptDescriptor(string monkeyAsset = "", string geckoAsset = "")
  {
    public string MonkeyPromptAssetName { get; } = monkeyAsset;

    public string GeckoPromptAssetName { get; } = geckoAsset;

    public Vector2 Dimensions { get; } = new Vector2(120f, 120f);

    public TutorialPromptDescriptor(Vector2 dimensions, string monkeyAsset = "", string geckoAsset = "")
      : this(monkeyAsset, geckoAsset)
    {
      this.Dimensions = dimensions;
    }
  }
}
