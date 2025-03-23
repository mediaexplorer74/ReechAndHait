
// Type: PigeonProject.Utility.MathFunctions
// Assembly: PigeonProject, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ED64DA4-BF9A-4998-A25D-EAF7C565AAB8
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework;
using System;

#nullable disable
namespace PigeonProject.Utility
{
  public static class MathFunctions
  {
    public static int WrapInteger(int number, int lowerBoundary, int upperBoundary)
    {
      if (number < lowerBoundary)
        return upperBoundary;
      return number > upperBoundary ? lowerBoundary : number;
    }

    public static int WrapInteger(
      ref int number,
      int lowerBoundary,
      int upperBoundary,
      int incrementor = 1)
    {
      number = MathFunctions.WrapInteger(number + incrementor, lowerBoundary, upperBoundary);
      return number;
    }

    public static float CrossProduct2D(Vector2 vector1, Vector2 vector2)
    {
      return (float) ((double) vector1.X * (double) vector2.Y - (double) vector1.Y * (double) vector2.X);
    }

    public static float AngleBetweenVector2(Vector2 vector1, Vector2 vector2)
    {
      vector1.Normalize();
      vector2.Normalize();
      double num = Math.Acos((double) Vector2.Dot(vector1, vector2));
      if ((double) vector1.X < (double) vector2.X)
        num = 2.0 * Math.PI - num;
      return (float) num;
    }

    public static float RadiansToDegree(float angleInRadians) => angleInRadians * 57.29578f;

    public static float DistancePointToStraight(
      Vector2 straightNormal,
      Vector2 pointOnStraight,
      Vector2 targetPoint)
    {
      Vector2 targetPointToPointOnStraight = pointOnStraight - targetPoint;
      return MathFunctions.DistancePointToStraight(straightNormal, targetPointToPointOnStraight);
    }

    public static float DistancePointToStraight(
      Vector2 straightNormal,
      Vector2 targetPointToPointOnStraight)
    {
      return Math.Abs(Vector2.Dot(straightNormal, targetPointToPointOnStraight));
    }

    public static Vector2 VectorNormal(Vector2 vec, bool flip = false)
    {
      return !flip ? new Vector2(-vec.Y, vec.X) : new Vector2(vec.Y, -vec.X);
    }

    public static void Swap<T>(ref T a, ref T b)
    {
      T obj = a;
      a = b;
      b = obj;
    }
  }
}
