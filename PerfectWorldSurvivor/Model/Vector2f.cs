using System.Text;

namespace PerfectWorldSurvivor.Model
{
    public struct Vector2f
    {
        public float x;
        public float y;

        public Vector2f(float x, float y)
        {
            this.x = x;
            this.y = y;
        }

        public Vector2f(Vector2f other)
        {
            this.x = other.x;
            this.y = other.y;
        }

        public Vector2f Set(float x, float y)
        {
            this.x = x;
            this.y = y;
           
            return this;
        }

        public Vector2f Set(Vector2f other)
        {
            this.x = other.x;
            this.y = other.y;
    
            return this;
        }

        public static Vector2f operator *(Vector2f a, float d)
        {
            Vector2f result = new Vector2f();
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
