
// Type: ReachHigh.Shared.SoundSong
// Assembly: ReachHigh.Windows, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E7AAE051-E6AD-4505-A6E0-970B36D3CBD7
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework.Audio;

#nullable disable
namespace ReachHigh.Shared
{
  public class SoundSong
  {
    public SoundEffect sound;
    public SoundEffectInstance song;
    public SoundEffectInstance songHelp;
    public string name;
    public bool loop;

    public SoundSong(SoundEffect SOUND)
    {
      this.sound = SOUND;
      this.song = this.sound.CreateInstance();
      this.name = this.sound.Name.ToString();
      this.songHelp = this.sound.CreateInstance();
      this.loop = this.name.Contains("Loop");
    }

    public void Play()
    {
      if (this.song.State.ToString() == "Playing")
        this.songHelp.Play();
      else
        this.song.Play();
    }

    public void Stop()
    {
      this.song.Stop();
      this.songHelp.Stop();
      this.loop = false;
    }

    public float GetDuration() => (float) this.sound.Duration.TotalSeconds;
  }
}
