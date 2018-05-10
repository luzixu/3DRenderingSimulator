using System.Text;

namespace PerfectWorldSurvivor.Model
{

    public struct Plane
    {
        public enum Side
        {
            On,
            Back,
            Front
        }

        public void Set(Vector3f normal, float d)
        {
            this.normal = normal;
            this.distanceToOrigin = d;
        }

        public void Set(Vector3f p1, Vector3f p2, Vector3f p3)
        {
            Vector3f dir1 = p1 - p2;
            Vector3f dir2 = p2 - p3;
            this.normal = (dir1.Cross(dir2)).Normalized;
            this.distanceToOrigin = (-p1).Dot(normal);
        }

        public Side GetPointSide(Vector3f p)
        {
            float relative = normal.Dot(p) + distanceToOrigin;
            if (relative == 0)
            {
                return Side.On;
            }
            else if (relative < 0)
            {
                return Side.Back;
            }
            else
            {
                return Side.Front;
            }
        }

        public override string ToString()
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("normal-->");
            stringBuilder.Append(normal.ToString());
            stringBuilder.Append("  d-->");
            stringBuilder.Append(distanceToOrigin);
            return stringBuilder.ToString();
        }

        public Vector3f normal;

        public float distanceToOrigin;
    }
}
