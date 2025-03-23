
// Type: ReachHigh.Shared.Worldbuilding
// Assembly: ReachHigh.Windows, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E7AAE051-E6AD-4505-A6E0-970B36D3CBD7
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

#nullable disable
namespace ReachHigh.Shared
{
  internal static class Worldbuilding
  {
    private const float tileSize = 60f;
    private const float debugTransparency = 0.2f;

    public static List<Sprite> CollidingEnvironment { get; set; }

    public static List<Sprite> DrawEnvironment { get; set; }

        public static void CreateClimbWall(
          Vector2 position,
          float height,
          string debugId = "",
          string surfaceType = "")
        {
            Worldbuilding.CreateCollider(position, height, 60f, "climb", debugId, surfaceType);
        }

        public static void CreateStickyCeiling(
          Vector2 position,
          float width,
          string debugId = "",
          string surfaceType = "")
        {
            Worldbuilding.CreateCollider(position, 60f, width, "sticky", debugId, surfaceType);
        }

        public static void CreateCollider(
          Vector2 position,
          float height,
          float width,
          string debugId = "",
          string surfaceType = "")
        {
            Worldbuilding.CreateCollider(position, height, width, "default", debugId, surfaceType);
        }

        public static void CreateCollider(
          float positionX,
          float positionY,
          float height,
          float width,
          string debugId = "",
          string surfaceType = "")
        {
            Worldbuilding.CreateCollider(new Vector2(positionX, positionY), height, width, "default", debugId, surfaceType);
        }

    /*public static void CreateCollider(
      float positionX,
      float positionY,
      float height,
      float width,
      string debugId = "",
      string surfaceType = "")
    {
      Worldbuilding.CreateCollider(new Vector2(positionX, positionY), height, width, debugId, surfaceType);
    }*/

    public static void CreateCollider(
      Vector2 position,
      float height,
      float width,
      string type,
      string debugId = "",
      string surfaceType = "")
    {
      List<Sprite> collidingEnvironment = Worldbuilding.CollidingEnvironment;
      SpecialSprite specialSprite = new SpecialSprite("", position, new Vector2(width, height), type);
      specialSprite.SurfaceType = surfaceType;
      collidingEnvironment.Add((Sprite) specialSprite);
    }

    public static void CreateCollider(
      float positionX,
      float positionY,
      float height,
      float width,
      string type,
      string debugId = "",
      string surfaceType = "")
    {
      Worldbuilding.CreateCollider(new Vector2(positionX, positionY), height, width, type, debugId, surfaceType);
    }

    public static void CreateCollider(
      Vector2 position,
      float height,
      float width,
      Type colliderType,
      string debugId = "",
      string surfaceType = "")
    {
      Sprite instance = (Sprite) Activator.CreateInstance(colliderType, (object) "", (object) position, (object) new Vector2(width, 60f));
      instance.SurfaceType = surfaceType;
      Worldbuilding.CollidingEnvironment.Add(instance);
    }

    private static void CreateDebugCollider(
      Vector2 position,
      float height,
      float width,
      Color color,
      string debugId = "")
    {
      Worldbuilding.DrawEnvironment.Add(new Sprite("Tileset\\Debug_Tile", position, new Vector2(width, height), debugId)
      {
        Transparancy = 0.2f,
        Color = color
      });
    }
  }
}
