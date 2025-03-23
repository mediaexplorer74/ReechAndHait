
// Type: ReachHigh.Shared.AudioManager
// Assembly: ReachHigh.Windows, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E7AAE051-E6AD-4505-A6E0-970B36D3CBD7
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Diagnostics;

#nullable disable
namespace ReachHigh.Shared
{
  public class AudioManager
  {
    public Dictionary<string, SoundEffect> sounds;
    public Dictionary<string, Song> songs;
    public Dictionary<string, SoundSong> soundSongs;
    private SoundSong nextSoundSong;
    private SoundSong currentSoundSong;
    private SoundSong nextNextSoundSong;
    private SoundSong previousSoundSong;
    private SoundSong nextNextNextSoundSong;
    private SoundIteration soundIteration;
    private int count;
    private float iterationTimer;
    private float songTimer;
    private float fadeTimer;
    private float _fadeTimer;

    public AudioManager(
      Dictionary<string, SoundEffect> SOUNDS,
      Dictionary<string, Song> SONGS = null,
      Dictionary<string, SoundSong> SOUNDSONGS = null)
    {
      this.sounds = SOUNDS;
      this.songs = SONGS;
      this.soundSongs = SOUNDSONGS;
      MediaPlayer.Volume = 0.5f;
    }

    public void Update(float deltaSeconds)
    {
      if (this.currentSoundSong == null)
        return;
      if ((double) this.songTimer <= 1.0 && !this.currentSoundSong.loop && this.nextSoundSong != null)
      {
        this.previousSoundSong = this.currentSoundSong;
        this.PlaySoundSong(this.nextSoundSong, this.nextNextSoundSong);
        this.nextNextSoundSong = this.nextNextNextSoundSong;
        this.nextNextNextSoundSong = (SoundSong) null;
      }
      else if ((double) this.songTimer > 1.0)
      {
        this.songTimer -= deltaSeconds;
        if (this.currentSoundSong.loop && (double) this.songTimer <= 1.0)
          this.PlaySoundSong(this.currentSoundSong, this.nextSoundSong);
        if (this.previousSoundSong != null && (double) this.currentSoundSong.GetDuration() - 1.0 > (double) this.songTimer)
        {
          this.previousSoundSong.Stop();
          this.previousSoundSong = (SoundSong) null;
        }
      }
      if ((double) this.fadeTimer <= 0.0)
        return;

      //Debug.WriteLine((float) (1.0 - ((double) this._fadeTimer - (double) this.fadeTimer) / (double) this._fadeTimer));
      
      this.currentSoundSong.song.Volume = (float) (1.0 - ((double) this._fadeTimer - (double) this.fadeTimer) / (double) this._fadeTimer);
      this.currentSoundSong.songHelp.Volume = (float) (1.0 - ((double) this._fadeTimer - (double) this.fadeTimer) / (double) this._fadeTimer);
      if (this.nextSoundSong != null)
      {
        this.nextSoundSong.song.Volume = (float) (1.0 - ((double) this._fadeTimer - (double) this.fadeTimer) / (double) this._fadeTimer);
        this.nextSoundSong.songHelp.Volume = (float) (1.0 - ((double) this._fadeTimer - (double) this.fadeTimer) / (double) this._fadeTimer);
      }
      this.fadeTimer -= deltaSeconds;
    }

    public void PlayTheme(Song song, bool LOOP = false)
    {
      MediaPlayer.Play(song);
      MediaPlayer.IsRepeating = LOOP;
    }

    public void PlaySoundSong(SoundSong soundSong, SoundSong NEXTSOUNDSONG = null)
    {
      soundSong.Play();
      this.currentSoundSong = soundSong;
      this.songTimer = this.currentSoundSong.GetDuration();
      this.nextSoundSong = NEXTSOUNDSONG;
    }

    public void Transition(SoundSong next, SoundSong nextNext)
    {
      if (this.nextSoundSong != null)
      {
        this.nextNextSoundSong = next;
        this.nextNextNextSoundSong = nextNext;
        this.currentSoundSong.loop = false;
        this.nextSoundSong.loop = false;
      }
      else
      {
        this.nextSoundSong = next;
        this.nextNextSoundSong = nextNext;
        this.currentSoundSong.loop = false;
      }
    }

    public void FadeOutSoundSong(float seconds)
    {
      if ((double) this.fadeTimer > 0.0)
        return;
      this._fadeTimer = seconds;
      this.fadeTimer = seconds;
    }

    public void StopSoundSong()
    {
      if (this.currentSoundSong != null)
      {
        this.currentSoundSong.Stop();
        this.currentSoundSong = (SoundSong) null;
      }
      if (this.nextSoundSong != null)
      {
        this.nextSoundSong.Stop();
        this.nextSoundSong = (SoundSong) null;
      }
      if (this.nextNextSoundSong != null)
      {
        this.nextNextSoundSong.Stop();
        this.nextNextSoundSong = (SoundSong) null;
      }
      if (this.nextNextNextSoundSong == null)
        return;
      this.nextNextNextSoundSong.Stop();
      this.nextNextNextSoundSong = (SoundSong) null;
    }

    public void StopTheme() => MediaPlayer.Stop();

    public void PlaySoundEffectIteration(SoundIteration SOUNDITERATION)
    {
      if (this.soundIteration != SOUNDITERATION)
      {
        this.soundIteration = SOUNDITERATION;
        this.count = 0;
        this.iterationTimer = 0.4f;
      }
      else
      {
        if (this.soundIteration == null)
          return;
        this.iterationTimer += Globals.deltaTime;
        if ((double) this.iterationTimer <= (double) this.soundIteration.iterationSpeed)
          return;
        this.soundIteration.activeSound.Play();
        ++this.count;
        this.iterationTimer = 0.0f;
        if (this.count < this.soundIteration.maxIterations)
          return;
        this.count = 0;
      }
    }
  }
}
