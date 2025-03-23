
// Type: ReachHigh.Shared.RhPlayerSettings
// Assembly: ReachHigh.Windows, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E7AAE051-E6AD-4505-A6E0-970B36D3CBD7
// Modded by [M]edia[E]xplorer

using System.Collections.Generic;

#nullable disable
namespace ReachHigh.Shared
{
  public class RhPlayerSettings
  {
    private static readonly IDictionary<Players, IDictionary<string, object>> settings = (IDictionary<Players, IDictionary<string, object>>) new Dictionary<Players, IDictionary<string, object>>();

    public static void Set(Players index, string key, object saveObject)
    {
      if (!RhPlayerSettings.settings.ContainsKey(index))
        RhPlayerSettings.settings.Add(index, (IDictionary<string, object>) new Dictionary<string, object>());
      IDictionary<string, object> setting = RhPlayerSettings.settings[index];
      if (!setting.ContainsKey(key))
        setting.Add(key, saveObject);
      else
        setting[key] = saveObject;
    }

    public static object Get(Players index, string key)
    {
      if (!RhPlayerSettings.settings.ContainsKey(index))
        return (object) null;
      IDictionary<string, object> setting = RhPlayerSettings.settings[index];
      return !setting.ContainsKey(key) ? (object) null : setting[key];
    }

    public static T Get<T>(Players index, string key)
    {
      object obj = RhPlayerSettings.Get(index, key);
      try
      {
        return obj != null ? (T) obj : default (T);
      }
      catch
      {
        return default (T);
      }
    }
  }
}
