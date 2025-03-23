
// Type: PigeonProject.Utility.PlayerSettings
// Assembly: PigeonProject, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ED64DA4-BF9A-4998-A25D-EAF7C565AAB8
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework;
using System.Collections.Generic;

#nullable disable
namespace PigeonProject.Utility
{
  public static class PlayerSettings
  {
    private static readonly IDictionary<PlayerIndex, IDictionary<string, object>> settings = (IDictionary<PlayerIndex, IDictionary<string, object>>) new Dictionary<PlayerIndex, IDictionary<string, object>>();

    public static void Set(PlayerIndex index, string key, object saveObject)
    {
      if (!PlayerSettings.settings.ContainsKey(index))
        PlayerSettings.settings.Add(index, (IDictionary<string, object>) new Dictionary<string, object>());
      IDictionary<string, object> setting = PlayerSettings.settings[index];
      if (!setting.ContainsKey(key))
        setting.Add(key, saveObject);
      else
        setting[key] = saveObject;
    }

    public static object Get(PlayerIndex index, string key)
    {
      if (!PlayerSettings.settings.ContainsKey(index))
        return (object) null;
      IDictionary<string, object> setting = PlayerSettings.settings[index];
      return !setting.ContainsKey(key) ? (object) null : setting[key];
    }

    public static T Get<T>(PlayerIndex index, string key)
    {
      object obj = PlayerSettings.Get(index, key);
      try
      {
        return (T) obj;
      }
      catch
      {
        return default (T);
      }
    }
  }
}
