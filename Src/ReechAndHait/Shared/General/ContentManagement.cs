
// Type: ReachHigh.Shared.Source.General.ContentManagement
// Assembly: ReachHigh.Windows, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E7AAE051-E6AD-4505-A6E0-970B36D3CBD7
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using PigeonProject.ContentManagement;

#nullable disable
namespace ReachHigh.Shared.Source.General
{
  internal static class ContentManagement
  {
    public static void LoadContent(ContentManager content)
    {
      ContentHandler.LoadDirectory<Texture2D>(content, "PigeonProject", true);
      ContentHandler.LoadCollectionToDictionary<Texture2D>(content, "PigeonProject\\gate", "gate");
    }
  }
}
