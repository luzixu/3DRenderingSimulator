using System.Text;

namespace PerfectWorldSurvivor.Model
{
    public struct Vector2i
    {
        public int x;
        public int y;
        public Vector2i(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public Vector2i(Vector2i other)
        {
            this.x = other.x;
            this.y = other.y;
        }

        public Vector2i Set(int x, int y)
        {
            this.x = x;
            this.y = y;
          
            return this;
        }

        public Vector2i Set(Vector2i other)
        {
            this.x = other.x;
            this.y = other.y;
            return this;
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
