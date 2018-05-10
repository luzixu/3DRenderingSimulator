using System.Text;

namespace PerfectWorldSurvivor.Model
{
    public struct Vector4f
    {
        public float x;
        public float y;
        public float z;
        public float w;

        public Vector4f(float x, float y, float z,float w)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            this.w = w;
        }

        public Vector4f(Vector4f other)
        {
            this.x = other.x;
            this.y = other.y;
            this.z = other.z;
            this.w = other.w;
        }

        public Vector4f Set(float x, float y, float z,float w)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            this.w = w;
            return this;
        }

        public Vector4f Set(Vector4f other)
        {
            this.x = other.x;
            this.y = other.y;
            this.z = other.z;
            this.w = other.w;
            return this;
        }

        public Vector4f Project(ref Matrix4x4 matrix)
        {
            Vector4f vec = new Vector4f();
            vec.x = x * matrix.m00 + y * matrix.m01 + z * matrix.m02 + w * matrix.m03;
            vec.y = x * matrix.m10 + y * matrix.m11 + z * matrix.m12 + w * matrix.m13;
            vec.z = x * matrix.m20 + y * matrix.m21 + z * matrix.m22 + w * matrix.m23;
            vec.w = x * matrix.m30 + y * matrix.m31 + z * matrix.m32 + w * matrix.m33;
            return vec;
        }

        public Vector3f ToXYZ()
        {
            return new Vector3f(x, y, z);
        }

        public static Vector4f operator -(Vector4f a)
        {
            Vector4f result = new Vector4f();
            result.x = -a.x;
            result.y = -a.y;
            result.z = -a.z;
            result.w = -a.w;
            return result;
        }


        public static Vector4f operator -(Vector4f a, Vector4f b)
        {
            Vector4f result = new Vector4f();
            result.x = a.x - b.x;
            result.y = a.y - b.y;
            result.z = a.z - b.z;
            result.w = a.w - b.w;
            return result;
        }

        public static Vector4f operator *(float d, Vector4f a)
        {
            Vector4f result = new Vector4f();
            result.x = a.x * d;
            result.y = a.y * d;
            result.z = a.z * d;
            result.w = a.w * d;
            return result;
        }
        public static Vector4f operator *(Vector4f a, float d)
        {
            Vector4f result = new Vector4f();
            result.x = a.x * d;
            result.y = a.y * d;
            result.z = a.z * d;
            result.w = a.w * d;
            return result;
        }
  
        public static Vector4f operator +(Vector4f a, Vector4f b)
        {
            Vector4f result = new Vector4f();
            result.x = a.x + b.x;
            result.y = a.y + b.y;
            result.z = a.z + b.z;
            result.w = a.w + b.w;
            return result;
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
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
            stringBuilder.Append(" w: ");
            stringBuilder.Append(w);
            return stringBuilder.ToString();
        }
    }
}
