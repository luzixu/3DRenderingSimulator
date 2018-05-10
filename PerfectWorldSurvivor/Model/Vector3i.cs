using System.Text;

namespace PerfectWorldSurvivor.Model
{
    public struct Vector3i
    {
        public int x;
        public int y;
        public int z;
        public Vector3i(int x, int y, int z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public Vector3i(Vector3i other)
        {
            this.x = other.x;
            this.y = other.y;
            this.z = other.z;
        }

        public Vector3i Set(int x, int y, int z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            return this;
        }

        public Vector3i Set(Vector3i other)
        {
            this.x = other.x;
            this.y = other.y;
            this.z = other.z;
            return this;
        }

        public override string ToString()
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("x: ");
            stringBuilder.Append(x);
            stringBuilder.Append(" y: ");
            stringBuilder.Append(y);
            stringBuilder.Append(" z: ");
            stringBuilder.Append(z);
            return stringBuilder.ToString();
        }
    }
}
