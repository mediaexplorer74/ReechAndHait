
// Type: ReachHigh.Shared.SoundIteration
// Assembly: ReachHigh.Windows, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E7AAE051-E6AD-4505-A6E0-970B36D3CBD7
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework.Audio;
using System.Collections.Generic;

#nullable disable
namespace ReachHigh.Shared
{
  public class SoundIteration
  {
    private List<SoundEffect> soundEffects;
    public SoundEffect activeSound;
    public int maxIterations;
    public float iterationSpeed;

    public SoundIteration(List<SoundEffect> SOUNDEFFECTS, int MAXITERATIONS, float ITERATIONSPEED)
    {
      this.soundEffects = SOUNDEFFECTS;
      this.maxIterations = MAXITERATIONS;
      this.iterationSpeed = ITERATIONSPEED;
      this.activeSound = this.soundEffects[0];
    }
  }
}
