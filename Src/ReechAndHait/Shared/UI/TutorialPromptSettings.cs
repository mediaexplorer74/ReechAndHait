
// Type: ReachHigh.Shared.TutorialPromptSettings
// Assembly: ReachHigh.Windows, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E7AAE051-E6AD-4505-A6E0-970B36D3CBD7
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework;
using System.Collections.Generic;

#nullable disable
namespace ReachHigh.Shared
{
  internal static class TutorialPromptSettings
  {
    private static readonly IDictionary<TutorialPrompts, TutorialPromptDescriptor> descriptors = (IDictionary<TutorialPrompts, TutorialPromptDescriptor>) new Dictionary<TutorialPrompts, TutorialPromptDescriptor>()
    {
      {
        TutorialPrompts.Jump,
        new TutorialPromptDescriptor("monkey_contact_prompts_jump", "gecko_contact_prompts_jump")
      },
      {
        TutorialPrompts.Interact,
        new TutorialPromptDescriptor("monkey_contact_prompts_interact", "gecko_contact_prompts_interact")
      },
      {
        TutorialPrompts.Walk,
        new TutorialPromptDescriptor("monkey_contact_prompts_move", "gecko_contact_prompts_move")
      },
      {
        TutorialPrompts.Boomerang,
        new TutorialPromptDescriptor(new Vector2(282f, 120f), geckoAsset: "gecko_contact_prompts_boomerang")
      },
      {
        TutorialPrompts.BoomerangRide,
        new TutorialPromptDescriptor("monkey_contact_prompts_throw")
      },
      {
        TutorialPrompts.ThrowGecko,
        new TutorialPromptDescriptor("monkey_contact_prompts_throw")
      }
    };

    public static TutorialPromptDescriptor GetDescriptor(TutorialPrompts promptType)
    {
      TutorialPromptDescriptor promptDescriptor;
      return TutorialPromptSettings.descriptors.TryGetValue(promptType, out promptDescriptor) 
                ? promptDescriptor : new TutorialPromptDescriptor();
    }

    public static bool TryGetDescriptor(
      TutorialPrompts promptType,
      out TutorialPromptDescriptor descriptor)
    {
      descriptor = TutorialPromptSettings.GetDescriptor(promptType);
      return !descriptor.Equals((object) new TutorialPromptDescriptor());
    }
  }
}
