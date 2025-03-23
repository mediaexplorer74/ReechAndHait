
// Type: PigeonProject.Animation.EasingFunctions
// Assembly: PigeonProject, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ED64DA4-BF9A-4998-A25D-EAF7C565AAB8
// Modded by [M]edia[E]xplorer

using System;

#nullable disable
namespace PigeonProject.Animation
{
  public static class EasingFunctions
  {
    public static double EaseInSine(double x) => 1.0 - Math.Cos(x * Math.PI / 2.0);

    public static double EaseInCubic(double x) => x * x * x;

    public static double EaseInQuint(double x) => x * x * x * x * x;

    public static double EaseInCirc(double x) => 1.0 - Math.Sqrt(1.0 - Math.Pow(x, 2.0));

    public static double EaseInElastic(double x)
    {
      if (x == 0.0)
        return 0.0;
      return x != 1.0 ? -Math.Pow(2.0, 10.0 * x - 10.0) * Math.Sin((x * 10.0 - 10.75) * (2.0 * Math.PI / 3.0)) : 1.0;
    }

    public static double EaseInQuad(double x) => x * x;

    public static double EaseInQuart(double x) => x * x * x * x;

    public static double EaseInExpo(double x) => x != 0.0 ? Math.Pow(2.0, 10.0 * x - 10.0) : 0.0;

    public static double EaseInBack(double x) => 2.70158 * x * x * x - 1.70158 * x * x;

    public static double EaseInBoucne(double x) => 1.0 - EasingFunctions.EaseOutBounce(1.0 - x);

    public static double EaseOutSine(double x) => Math.Sin(x * Math.PI / 2.0);

    public static double EaseOutCubic(double x) => 1.0 - Math.Pow(1.0 - x, 3.0);

    public static double EaseOutQuint(double x) => 1.0 - Math.Pow(1.0 - x, 5.0);

    public static double EaseOutCirc(double x) => Math.Sqrt(1.0 - Math.Pow(x - 1.0, 2.0));

    public static double EaseOutElastic(double x)
    {
      if (x == 0.0)
        return 0.0;
      return x != 1.0 ? Math.Pow(2.0, -10.0 * x) * Math.Sin((x * 10.0 - 0.75) * (2.0 * Math.PI / 3.0)) + 1.0 : 1.0;
    }

    public static double EaseOutQuad(double x) => 1.0 - (1.0 - x) * (1.0 - x);

    public static double EaseOutQuart(double x) => 1.0 - Math.Pow(1.0 - x, 4.0);

    public static double EaseOutExpo(double x) => x != 1.0 ? 1.0 - Math.Pow(2.0, -10.0 * x) : 1.0;

    public static double EaseOutBack(double x)
    {
      return 1.0 + 2.70158 * Math.Pow(x - 1.0, 3.0) + 1.70158 * Math.Pow(x - 1.0, 2.0);
    }

    public static double EaseOutBounce(double x)
    {
      if (x < 4.0 / 11.0)
        return 121.0 / 16.0 * x * x;
      if (x < 8.0 / 11.0)
        return 121.0 / 16.0 * (x -= 6.0 / 11.0) * x + 0.75;
      return x < 10.0 / 11.0 ? 121.0 / 16.0 * (x -= 9.0 / 11.0) * x + 15.0 / 16.0 : 121.0 / 16.0 * (x -= 21.0 / 22.0) * x + 63.0 / 64.0;
    }

    public static double EaseInOutSine(double x) => -(Math.Cos(Math.PI * x) - 1.0) / 2.0;

    public static double EaseInOutCubic(double x)
    {
      return x >= 0.5 ? 1.0 - Math.Pow(-2.0 * x + 2.0, 3.0) / 2.0 : 4.0 * x * x * x;
    }

    public static double EaseInOutQuint(double x)
    {
      return x >= 0.5 ? 1.0 - Math.Pow(-2.0 * x + 2.0, 5.0) / 2.0 : 16.0 * x * x * x * x * x;
    }

    public static double EaseInOutCirc(double x)
    {
      return x >= 0.5 ? (Math.Sqrt(1.0 - Math.Pow(-2.0 * x + 2.0, 2.0)) + 1.0) / 2.0 : (1.0 - Math.Sqrt(1.0 - Math.Pow(2.0 * x, 2.0))) / 2.0;
    }

    public static double EaseInOutElastic(double x)
    {
      if (x == 0.0)
        return 0.0;
      if (x == 1.0)
        return 1.0;
      return x >= 0.5 ? Math.Pow(2.0, -20.0 * x + 10.0) * Math.Sin((20.0 * x - 11.125) * (4.0 * Math.PI / 9.0)) / 2.0 + 1.0 : -(Math.Pow(2.0, 20.0 * x - 10.0) * Math.Sin((20.0 * x - 11.125) * (4.0 * Math.PI / 9.0))) / 2.0;
    }

    public static double EaseInOutQuad(double x)
    {
      return x >= 0.5 ? 1.0 - Math.Pow(-2.0 * x + 2.0, 2.0) / 2.0 : 2.0 * x * x;
    }

    public static double EaseInOutQuart(double x)
    {
      return x >= 0.5 ? 1.0 - Math.Pow(-2.0 * x + 2.0, 4.0) / 2.0 : 8.0 * x * x * x * x;
    }

    public static double EaseInOutExpo(double x)
    {
      if (x == 0.0)
        return 0.0;
      if (x == 1.0)
        return 1.0;
      return x >= 0.5 ? (2.0 - Math.Pow(2.0, -20.0 * x + 10.0)) / 2.0 : Math.Pow(2.0, 20.0 * x - 10.0) / 2.0;
    }

    public static double EaseInOutBack(double x)
    {
      return x >= 0.5 ? (Math.Pow(2.0 * x - 2.0, 2.0) * (3.5949095 * (x * 2.0 - 2.0) + 2.5949095) + 2.0) / 2.0 : Math.Pow(2.0 * x, 2.0) * (7.189819 * x - 2.5949095) / 2.0;
    }

    public static double EaseInOutBounce(double x)
    {
      return x >= 0.5 ? (1.0 + EasingFunctions.EaseOutBounce(2.0 * x - 1.0)) / 2.0 : (1.0 - EasingFunctions.EaseOutBounce(1.0 - 2.0 * x)) / 2.0;
    }
  }
}
