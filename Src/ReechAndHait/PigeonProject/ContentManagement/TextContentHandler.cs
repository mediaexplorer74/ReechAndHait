
// Type: PigeonProject.ContentManagement.TextContentHandler
// Assembly: PigeonProject, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ED64DA4-BF9A-4998-A25D-EAF7C565AAB8
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

#nullable disable
namespace PigeonProject.ContentManagement
{
  public static class TextContentHandler
  {
    private static readonly List<string> languages = new List<string>();
    private static int currentLanguageIndex = 0;
    private static IDictionary<string, string> textCollection = (IDictionary<string, string>) new Dictionary<string, string>();
    private static string path = "./Content/Text/language-";

    public static bool LoadLanguage(string language)
    {
      if (!TextContentHandler.RegisterLanguage(language))
        return false;
      IDictionary<string, string> dictionary = (IDictionary<string, string>) new Dictionary<string, string>();
      try
      {
        using (StreamReader streamReader = new StreamReader(File.OpenRead(TextContentHandler.path + language + ".txt")))
        {
          string str1;
          while ((str1 = streamReader.ReadLine()) != null)
          {
            if (str1.Length > 0 && !str1.StartsWith("#"))
            {
              string key = str1;
              string str2 = streamReader.ReadLine();
              if (str2.Length > 0 && !str2.StartsWith("#"))
              {
                StringBuilder stringBuilder = new StringBuilder();
                bool flag = false;
                foreach (char ch in str2)
                {
                  switch (ch)
                  {
                    case '{':
                      flag = true;
                      break;
                    case '}':
                      flag = false;
                      break;
                    default:
                      if (!flag)
                      {
                        stringBuilder.Append(ch);
                        break;
                      }
                      break;
                  }
                }
                string str3 = stringBuilder.ToString();
                dictionary.Add(key, str3);
              }
            }
          }
        }
      }
      catch (Exception ex)
      {
        Debug.WriteLine("ERROR with loading language " + language + ": " + ex.Message);
        return false;
      }
      TextContentHandler.textCollection = dictionary;
      return true;
    }

    public static bool RegisterLanguage(string language)
    {
      if (TextContentHandler.languages.Contains(language))
      {
        Debug.WriteLine("WARNING: " + language + " is already a registered language!");
        return false;
      }
      TextContentHandler.languages.Add(language);
      return true;
    }

    public static void ChangePath(string newPath) => TextContentHandler.path = newPath;

    public static string GetString(string key)
    {
      return !TextContentHandler.textCollection.ContainsKey(key)
                ? key + " text not found" 
                : TextContentHandler.textCollection[key];
    }

    public static void ChangeLanguage()
    {
      TextContentHandler.currentLanguageIndex =
                TextContentHandler.currentLanguageIndex >= TextContentHandler.languages.Count - 1
                ? 0
                : TextContentHandler.currentLanguageIndex + 1;

      if (TextContentHandler.LoadLanguage(TextContentHandler.languages[TextContentHandler.currentLanguageIndex]))
        return;

      TextContentHandler.ChangeLanguage();
    }

    public static bool ChangeToLanguage(string language)
    {
      int index = TextContentHandler.languages.IndexOf(language);
      if (index < 0 || !TextContentHandler.LoadLanguage(TextContentHandler.languages[index]))
        return false;
      TextContentHandler.currentLanguageIndex = index;
      return true;
    }

    public static bool ReloadCurrentLanguage()
    {
      return TextContentHandler.LoadLanguage(TextContentHandler.languages[TextContentHandler.currentLanguageIndex]);
    }

    public static string WrapText(SpriteFont font, string text, float maxLineWidth)
    {
      string[] strArray = text.Split(' ');
      StringBuilder stringBuilder = new StringBuilder();
      float num = 0.0f;
      float x = font.MeasureString(" ").X;
      foreach (string text1 in strArray)
      {
        Vector2 vector2 = font.MeasureString(text1);
        if ((double) num + (double) vector2.X < (double) maxLineWidth)
        {
          stringBuilder.Append(text1 + " ");
          num += vector2.X + x;
        }
        else if ((double) vector2.X > (double) maxLineWidth)
        {
          if (stringBuilder.ToString() == "")
            stringBuilder.Append(TextContentHandler.WrapText(font, text1.Insert(text1.Length / 2, " ") + " ", maxLineWidth));
          else
            stringBuilder.Append("\n" + TextContentHandler.WrapText(font, text1.Insert(text1.Length / 2, " ")
                + " ", maxLineWidth));
        }
        else
        {
          stringBuilder.Append("\n" + text1 + " ");
          num = vector2.X + x;
        }
      }
      return stringBuilder.ToString();
    }
  }
}
