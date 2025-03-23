
// Type: ReachHigh.Shared.RadialSound
// Assembly: ReachHigh.Windows, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E7AAE051-E6AD-4505-A6E0-970B36D3CBD7
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using System;
using System.Diagnostics;

#nullable disable
namespace ReachHigh.Shared
{
  public class RadialSound
  {
    private SoundEffectInstance instance;
    private Vector2 pos;
    private float radius;
    private float distance;

    public RadialSound(SoundEffect sound, Vector2 POS, float RADIUS)
    {
      this.instance = sound.CreateInstance();
      this.instance.IsLooped = true;
      this.pos = POS;
      this.radius = RADIUS;
    }

    public void Update()
    {
      this.distance = Globals.GetDistance(this.pos, Camera.mid);
      if ((double) this.distance < (double) this.radius)
      {
        this.instance.Volume = (float) Math.Log(((double) this.radius - (double) this.distance) / (double) this.radius + 1.0, 2.0);
        //Debug.WriteLine("Radial sound - volume: " + this.instance.Volume);
        this.instance.Play();
      }
      else
      {
        this.instance.Volume = 0.0f;
        this.instance.Stop();
      }
    }
  }
}
