using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PerfectWorldSurvivor.graphicEngine.model;

namespace PerfectWorldSurvivor.utils
{
    public class PerfectMath
    {
        public const float degreesToRadians =(float) (Math.PI / 180);

        public const float radiansToDegree = (float) (180 / Math.PI);

        public const float pi2 = (float)(Math.PI * 2f);

        public static float Clamp(float value, float min = 0, float max = 1)
        {
            return Math.Max(min, Math.Min(value, max));
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
