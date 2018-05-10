using System.Text;

namespace PerfectWorldSurvivor.Model
{
    public struct Colorf
    {
        public float a;
        public float b;
        public float g;
        public float r;

        public Colorf(float r, float g, float b)
        {
            this.r = r;
            this.g = g;
            this.b = b;
            this.a = 1;
        }

        public Colorf(float r, float g, float b, float a)
        {
            this.r = r;
            this.g = g;
            this.b = b;
            this.a = a;
        }

        public Colorf(Colorf color)
        {
            this.r = color.r;
            this.g = color.g;
            this.b = color.b;
            this.a = color.a;
        }

        public void Set(Colorf color)
        {
            this.a = color.a;
            this.b = color.b;
            this.g = color.g;
            this.r = color.r;
        }

        public void Set(float r, float g, float b, float a)
        {
            this.r = r;
            this.g = g;
            this.b = b;
            this.a = a;
        }
        public override string ToString()
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("r : ");
            stringBuilder.Append(r);
            stringBuilder.Append(" g : ");
            stringBuilder.Append(g);
            stringBuilder.Append(" b : ");
            stringBuilder.Append(b);
            stringBuilder.Append(" a : ");
            stringBuilder.Append(a);
            return stringBuilder.ToString();
        }

        public static Colorf operator + (Colorf a,Colorf b)
        {
            Colorf result = new Colorf();
            result.b = a.b + b.b;
            result.g = a.g + b.g;
            result.r = a.r + b.r;
            return result;
        }

        public static Colorf operator -(Colorf a, Colorf b)
        {
            Colorf result = new Colorf();
            result.b = a.b - b.b;
            result.g = a.g - b.g;
            result.r = a.r - b.r;
            return result;
        }

        public static Colorf operator *(Colorf a, float b)
        {
            Colorf result = new Colorf();
            result.b = a.b * b;
            result.g = a.g * b;
            result.r = a.r * b;
            return result;
        }

        public static Colorf operator *(Colorf a, Colorf b)
        {
            Colorf result = new Colorf();
            result.b = a.b * b.b;
            result.g = a.g * b.g;
            result.r = a.r * b.r;
            return result;
        }

        public static readonly Colorf CLEAR = new Colorf(0, 0, 0, 0);
        public static readonly Colorf WHITE = new Colorf(1, 1, 1, 1);
        public static readonly Colorf BLACK = new Colorf(0, 0, 0, 1);
        public static readonly Colorf RED = new Colorf(1, 0, 0, 1);
        public static readonly Colorf GREEN = new Colorf(0, 1, 0, 1);
        public static readonly Colorf BLUE = new Colorf(0, 0, 1, 1);
        public static readonly Colorf LIGHT_GRAY = new Colorf(0.75f, 0.75f, 0.75f, 1);
        public static readonly Colorf GRAY = new Colorf(0.5f, 0.5f, 0.5f, 1);
        public static readonly Colorf DARK_GRAY = new Colorf(0.25f, 0.25f, 0.25f, 1);
        public static readonly Colorf PINK = new Colorf(1, 0.68f, 0.68f, 1);
        public static readonly Colorf ORANGE = new Colorf(1, 0.78f, 0, 1);
        public static readonly Colorf YELLOW = new Colorf(1, 1, 0, 1);
        public static readonly Colorf MAGENTA = new Colorf(1, 0, 1, 1);
        public static readonly Colorf CYAN = new Colorf(0, 1, 1, 1);
    }
}
