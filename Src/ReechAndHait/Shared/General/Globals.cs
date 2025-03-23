
// Type: ReachHigh.Shared.Globals
// Assembly: ReachHigh.Windows, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E7AAE051-E6AD-4505-A6E0-970B36D3CBD7
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

#nullable disable
namespace ReachHigh.Shared
{
  internal class Globals
  {
    public static float gravitation = 1500f;
    public static ContentManager content;
    public static SpriteBatch spriteBatch;
    public static GraphicsDeviceManager graphics;
    public static KeyboardState keyboard;
    public static KeyboardState previousKeyboard;
    public static GamePadState gamepad;
    public static GamePadState previousGamepad;
    public static MouseState mouse;
    public static MouseState previousMouse;
    //public static MouseState previousPreviousMouse;
    public static float deltaTime;
    public static Vector2 geckoPos;
    public static Vector2 monkeyPos;
    public static Vector2 bumerangPos;
    public static Matrix transform;
    public static Gate nextGate;
    public static PlayerIndex playerIndex = new PlayerIndex();
    public static AudioManager audioManager;
    public static SpriteFont menuFont;
    public static SpriteFont headfont;
    public static Texture2D rect;
    public static Camera camera;
    public static bool debugMode;
    private static Color[] data;

    public static GameTime UpdateTime { get; set; }

    public static GameTime DrawTime { get; set; }

    public static bool IsSequenceActive { get; set; }

    public static Vector2 Normalize(Vector2 vector)
    {
      if ((double) vector.Length() <= 0.0)
        return Vector2.Zero;
      vector.Normalize();
      return vector;
    }

    public static float GetDistance(Vector2 one, Vector2 other)
    {
      Vector2 vector2 = other - one;
      return (float) Math.Sqrt(Math.Pow((double) vector2.X, 2.0) + Math.Pow((double) vector2.Y, 2.0));
    }

    public static void LoadContent()
    {
      Globals.headfont = Globals.content.Load<SpriteFont>("Jungle Roar");
      Globals.menuFont = Globals.content.Load<SpriteFont>("Kefa");
      Globals.rect = new Texture2D(Globals.graphics.GraphicsDevice, 80, 30);
      Globals.data = new Color[2400];
      for (int index = 0; index < Globals.data.Length; ++index)
        Globals.data[index] = Color.White;
      Globals.rect.SetData<Color>(Globals.data);
    }
  }
}
