
// Type: PigeonProject.ContentManagement.SpriteDescriptor
// Assembly: PigeonProject, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ED64DA4-BF9A-4998-A25D-EAF7C565AAB8
// Modded by [M]edia[E]xplorer

#nullable disable
namespace PigeonProject.ContentManagement
{
  public class SpriteDescriptor
  {
    public SpriteSheet SpriteSheet { get; set; }

    public int Index { get; set; }

    public SpriteDescriptor(SpriteSheet spriteSheet, int index)
    {
      this.SpriteSheet = spriteSheet;
      this.Index = index;
    }
  }
}
