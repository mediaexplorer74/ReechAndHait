
// Type: PigeonProject.ContentManagement.ContentHandler
// Assembly: PigeonProject, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ED64DA4-BF9A-4998-A25D-EAF7C565AAB8
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;

#nullable disable
namespace PigeonProject.ContentManagement
{
  public static class ContentHandler
  {
    private static readonly IDictionary<string, object> assets 
            = (IDictionary<string, object>) new Dictionary<string, object>();
    private static readonly IDictionary<Type, IDictionary<string, List<string>>> collections 
            = (IDictionary<Type, IDictionary<string, List<string>>>)
                 new Dictionary<Type, IDictionary<string, List<string>>>();
    private const string SPRITESHEET_IDENTIFIER = "ss";
    private const string SPRITESHEET_TEXTURE_SUFFIX = "_texture";
    private static readonly Regex SpritesheetRegex =
            new Regex("(?:ss)(?<options>\\w*)(?:_)(?<columns>[0-9]+)(?:_)(?<rows>[0-9]+)(?:_)(?<name>.+)");

    public static bool Load<T>(Microsoft.Xna.Framework.Content.ContentManager content, string assetKey, string path)
    {
      T asset;
      try
      {
        int first = path.IndexOf("Content\\");

        if (first > 0)
        {
            first = first + "Content\\".Length;
            path = path.Substring(first);
        }

        asset = content.Load<T>(path);
        if (typeof (T) == typeof (Texture2D))
        {
          if (assetKey.StartsWith("ss"))
            assetKey = ContentHandler.LoadSpritesheet<T>(assetKey, asset);
        }
      }
      catch (Exception ex)
      {
        Debug.WriteLine("Error loading asset " + path + ": " + ex.Message);
        return false;
      }
      return ContentHandler.AddToAssetDictionary<T>(assetKey, (object) asset);
    }

    private static string LoadSpritesheet<T>(string assetKey, T asset)
    {
      foreach (Match match in ContentHandler.SpritesheetRegex.Matches(assetKey))
      {
        try
        {
          GroupCollection groups = match.Groups;
          string assetKey1 = groups["name"].Value;
          if (assetKey1.Length < 1)
            throw new Exception("No name found!");
          int result1;
          if (int.TryParse(groups["columns"].Value, out result1))
          {
            int result2;
            if (int.TryParse(groups["rows"].Value, out result2))
            {
              SpriteSheet asset1 = new SpriteSheet((object) asset as Texture2D, result1, result2);
              ContentHandler.AddToAssetDictionary<SpriteSheet>(assetKey1, (object) asset1);
              assetKey = assetKey1 + "_texture";
              string str1 = groups["options"].Value;
              while (str1.Length >= 2)
              {
                string str2 = str1.Substring(0, 2);
                str1 = str1.Remove(0, 2);
                switch (str2)
                {
                  case "lr":
                    asset1.IterationMode = SpritesheetIterationMode.LeftToRight;
                    continue;
                  case "tb":
                    asset1.IterationMode = SpritesheetIterationMode.TopToBottom;
                    continue;
                  case "ib":
                    asset1.IterateBackwards = true;
                    continue;
                  default:
                    continue;
                }
              }
            }
          }
        }
        catch (Exception ex)
        {
          Debug.WriteLine("Error with attempt to load spritesheet " + assetKey + ": " + ex.Message);
        }
      }
      return assetKey;
    }

    public static bool LoadDirectory<T>(
      ContentManager content,
      string directoryPath,
      bool loadSubdirectories = false)
    {
      try
      {
        DirectoryInfo directory = new DirectoryInfo(content.RootDirectory + "/" + directoryPath);
        ContentHandler.IterateFiles<T>(content, directoryPath, directory, loadSubdirectories);
      }
      catch (Exception ex)
      {
        Debug.WriteLine("Error loading directory " + directoryPath + ": " + ex.Message);
        return false;
      }
      return true;
    }

    private static void IterateFiles<T>
    (
      ContentManager content,
      string directoryPath,
      DirectoryInfo directory,
      bool loadSubdirectories
    )
    {
      foreach (FileSystemInfo file in directory.GetFiles())
      {
        string withoutExtension = Path.GetFileNameWithoutExtension(file.Name);
        ContentHandler.Load<T>(content, withoutExtension, directoryPath + "/" + withoutExtension);
      }
      
     if (!loadSubdirectories)
        return;
      
      DirectoryInfo[] directories = directory.GetDirectories();
      
      if (directories.Length == 0)
        return;

      foreach (DirectoryInfo directory1 in directories)
        ContentHandler.IterateFiles<T>(content, directory1.FullName, directory1, true);
    }

    public static void LoadCollectionsFromDirectory<T>(ContentManager content, string directoryPath)
    {
      try
      {
        foreach (DirectoryInfo directory in new DirectoryInfo(content.RootDirectory + "/" + directoryPath).GetDirectories())
          ContentHandler.LoadCollectionToDictionary<T>(content, directoryPath + "/" + directory.Name);
      }
      catch (Exception ex)
      {
        Debug.WriteLine("Error with loading collections from directory " + directoryPath + ": " + ex.Message);
      }
    }

    public static void LoadCollectionToDictionary<T>(
      ContentManager content,
      string directoryPath,
      string collectionName = "")
    {
      try
      {
        DirectoryInfo directoryInfo = new DirectoryInfo(content.RootDirectory + "/" + directoryPath);
        FileInfo[] files = directoryInfo.GetFiles();
        List<string> assetKeys = new List<string>(files.Length);
        foreach (FileSystemInfo fileSystemInfo in files)
        {
          string withoutExtension = Path.GetFileNameWithoutExtension(fileSystemInfo.Name);
          assetKeys.Add(withoutExtension);
          ContentHandler.Load<T>(content, withoutExtension, directoryPath + "/" + withoutExtension);
        }
        ContentHandler.AddCollection<T>(collectionName.Length > 0 ? collectionName : directoryInfo.Name, assetKeys);
      }
      catch (Exception ex)
      {
        Debug.WriteLine("Error loading collection from directory " + directoryPath + ": " + ex.Message);
      }
    }

    public static T GetAsset<T>(string assetKey)
    {
      if (!ContentHandler.assets.ContainsKey(assetKey))
      {
        Debug.WriteLine("WARNING: " + assetKey + " does not exist!");
        return default (T);
      }
      if (ContentHandler.assets[assetKey] is T asset)
        return asset;
      Debug.WriteLine(string.Format("WARNING: {0} is not of type {1}!", (object) assetKey, (object) typeof (T)));
      return default (T);
    }

    public static List<string> GetCollectionKeys<T>(string collectionKey)
    {
      Type key = typeof (T);
      if (ContentHandler.collections.ContainsKey(key))
      {
        IDictionary<string, List<string>> collection = ContentHandler.collections[key];
        if (collection.ContainsKey(collectionKey))
          return collection[collectionKey];
      }
      return (List<string>) null;
    }

    public static List<T> GetCollectionAssets<T>(string collectionKey)
    {
      List<string> collectionKeys = ContentHandler.GetCollectionKeys<T>(collectionKey);
      if (collectionKeys == null || collectionKeys.Count < 1)
        return (List<T>) null;
      List<T> collectionAssets = new List<T>();
      foreach (string assetKey in collectionKeys)
      {
        T asset = ContentHandler.GetAsset<T>(assetKey);
        if ((object) asset != null)
          collectionAssets.Add(asset);
      }
      return collectionAssets;
    }

    public static void AddCollection<T>(string collectionKey, List<string> assetKeys)
    {
      if (assetKeys.Count < 1)
        return;
      Type key = typeof (T);
      if (ContentHandler.collections.ContainsKey(key))
      {
        IDictionary<string, List<string>> collection = ContentHandler.collections[key];
        if (collection.ContainsKey(collectionKey))
          collection[collectionKey].AddRange((IEnumerable<string>) assetKeys);
        else
          collection.Add(collectionKey, assetKeys);
      }
      else
      {
        ContentHandler.collections.Add(key, (IDictionary<string, List<string>>) 
            new Dictionary<string, List<string>>());
        ContentHandler.collections[key].Add(collectionKey, assetKeys);
      }
    }

    private static bool AddToAssetDictionary<T>(string assetKey, object asset)
    {
      if (ContentHandler.assets.ContainsKey(assetKey))
      {
        //Debug.WriteLine(string.Format("WARNING: {0} already exists in the {1} collection!",
        //    (object) assetKey, (object) typeof (T)));
        return false;
      }
      ContentHandler.assets.Add(assetKey, asset);
      return true;
    }

    public static void DebugPrintRegister()
    {
      foreach (KeyValuePair<string, object> asset in (IEnumerable<KeyValuePair<string, object>>) ContentHandler.assets)
        Debug.WriteLine(string.Format("Key: {0}; Type: {1}", (object) asset.Key, 
            (object) asset.Value.GetType()));
    }
  }
}
