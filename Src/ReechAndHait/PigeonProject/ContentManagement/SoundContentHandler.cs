
// Type: PigeonProject.ContentManagement.SoundContentHandler
// Assembly: PigeonProject, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ED64DA4-BF9A-4998-A25D-EAF7C565AAB8
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

#nullable disable
namespace PigeonProject.ContentManagement
{
  public static class SoundContentHandler
  {
    private static readonly Random random = new Random();
    private static readonly IDictionary<string, List<string>> soundCollections = (IDictionary<string, List<string>>) new Dictionary<string, List<string>>();

    public static bool LoadSoundCollection(
      ContentManager content,
      string directoryPath,
      string collectionName = "")
    {
      try
      {
        DirectoryInfo directoryInfo = new DirectoryInfo(content.RootDirectory + "/" + directoryPath);
        FileInfo[] files = directoryInfo.GetFiles();
        List<string> stringList = new List<string>();
        foreach (FileSystemInfo fileSystemInfo in files)
        {
          string withoutExtension = Path.GetFileNameWithoutExtension(fileSystemInfo.Name);
          stringList.Add(withoutExtension);
          ContentHandler.Load<SoundEffect>(content, withoutExtension, directoryPath + "/" + withoutExtension);
        }
        SoundContentHandler.AddSoundCollection(collectionName.Length > 0 ? collectionName : directoryInfo.Name, stringList.ToArray());
      }
      catch (Exception ex)
      {
        Debug.WriteLine("Error loading sound collection from directory " + directoryPath + ": " + ex.Message);
        return false;
      }
      return true;
    }

    public static void AddSoundCollection(string collectionKey, string[] soundeffectKeys)
    {
      if (soundeffectKeys.Length < 1)
        return;
      if (SoundContentHandler.soundCollections.ContainsKey(collectionKey))
        SoundContentHandler.soundCollections[collectionKey].AddRange((IEnumerable<string>) soundeffectKeys);
      else
        SoundContentHandler.soundCollections.Add(collectionKey, new List<string>((IEnumerable<string>) soundeffectKeys));
    }

    public static void AddSoundCollection(string collectionKey, string soundeffectKey)
    {
      SoundContentHandler.AddSoundCollection(collectionKey, new string[1]
      {
        soundeffectKey
      });
    }

    public static bool PlaySound(string key)
    {
      SoundEffect asset = ContentHandler.GetAsset<SoundEffect>(key);
      if (asset == null)
      {
        Debug.WriteLine("WARNING The soundeffect " + key + " does not exist!");
        return false;
      }
      asset.Play();
      return true;
    }

    public static bool PlaySoundFromCollection(string collectionKey)
    {
      if (!SoundContentHandler.soundCollections.ContainsKey(collectionKey))
      {
        Debug.WriteLine("WARNING: SoundEffectCollection " + collectionKey + " does not exist!");
        return false;
      }
      int index = SoundContentHandler.random.Next(SoundContentHandler.soundCollections[collectionKey].Count);
      return SoundContentHandler.PlaySound(SoundContentHandler.soundCollections[collectionKey][index]);
    }

    public static SoundEffectInstance GetSoundEffectInstance(
      string key,
      bool playImmediately = false,
      bool isLooped = true)
    {
      SoundEffect asset = ContentHandler.GetAsset<SoundEffect>(key);
      if (asset == null)
      {
        Debug.WriteLine("WARNING The soundeffect " + key + " does not exist!");
        return (SoundEffectInstance) null;
      }
      SoundEffectInstance instance = asset.CreateInstance();
      instance.IsLooped = isLooped;
      if (playImmediately)
        instance.Play();
      return instance;
    }
  }
}
