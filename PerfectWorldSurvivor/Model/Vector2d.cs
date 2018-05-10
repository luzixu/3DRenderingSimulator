using System.Text;

namespace PerfectWorldSurvivor.Model
{
    public struct Vector2d
    {
        public double x;
        public double y;

        public Vector2d(float x, float y)
        {
            this.x = x;
            this.y = y;
        }

        public Vector2d(Vector2d other)
        {
            this.x = other.x;
            this.y = other.y;
        }

        public Vector2d(Vector2f other)
        {
            this.x = other.x;
            this.y = other.y;
        }

        public Vector2d Set(double x, double y)
        {
            this.x = x;
            this.y = y;

            return this;
        }

        public Vector2d Set(Vector2d other)
        {
            this.x = other.x;
            this.y = other.y;

            return this;
        }

        public static Vector2d operator *(Vector2d a, double d)
        {
            Vector2d result = new Vector2d();
            result.x = a.x * d;
            result.y = a.y * d;
            return result;
        }
        public override string ToString()
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("x: ");
            stringBuilder.Append(x);
            stringBuilder.Append(" y: ");
            stringBuilder.Append(y);
            return stringBuilder.ToString();
        }
    }
}
