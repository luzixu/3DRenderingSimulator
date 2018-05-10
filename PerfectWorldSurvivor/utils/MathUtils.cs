using System;
using PerfectWorldSurvivor.Model;

namespace PerfectWorldSurvivor.Utils
{
    public class MathUtils
    {
        public static readonly float degreesToRadians =(float) (Math.PI / 180);

        public static readonly float radiansToDegree = (float)(180 / Math.PI);

        public static readonly float pi2 = (float)(Math.PI * 2f);

        public static readonly float floatPrecision = 0.000001f;

        public static bool IsZero(float f)
        {
            if (Math.Abs(f) <= floatPrecision)
            {
                return true;
            }
            return false;
        }

        public static float Clamp(float value, float min = 0, float max = 1)
        {
            if (value >= max)
            {
                return max;
            }
            if (value <= min)
            {
                return min;
            }
            return value;
        }

        public static float Interpolate(float gradient, float min, float max)
        {
            return min + (max - min) * Clamp(gradient);
        }

        public static double Interpolate(float gradient, double min, double max)
        {
            return min + (max - min) * Clamp(gradient);
        }


        public static Vector3f Interpolate(float gradient, Vector3f min, Vector3f max)
        {
            return min + (max - min) * Clamp(gradient);
        }

        public static Colorf Interpolate(float gradient, Colorf min, Colorf max)
        {
            return min + (max - min) * Clamp(gradient);
        }

        public static float Pow(float x,float y)
        {
            return (float)Math.Pow(x, y);
        }
    }
}
