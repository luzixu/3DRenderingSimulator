using System;
using System.Text;
using PerfectWorldSurvivor.Utils;

namespace PerfectWorldSurvivor.Model 
{
    public struct Vector3f
    {
        public float x;
        public float y;
        public float z;

        public Vector3f(float x, float y, float z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public Vector3f(Vector3f other)
        {
            this.x = other.x;
            this.y = other.y;
            this.z = other.z;
        }

        public Vector3f Set(float x, float y, float z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            return this;
        }

        public Vector3f Set(Vector3f other)
        {
            this.x = other.x;
            this.y = other.y;
            this.z = other.z;
            return this;
        }

        public void Add(float dx, float dy, float dz)
        {
            this.x += dx;
            this.y += dy;
            this.z += dz;
        }

        public void Rotate(Matrix4x4 mat)
        {
            Set(x * mat.m00 + y * mat.m01 + z * mat.m02,
                x * mat.m10 + y * mat.m11 + z * mat.m12,
                x * mat.m20 + y * mat.m21 + z * mat.m22
                );
        }

        public void Rotate(Vector3f axis, float degrees)
        {
            Matrix4x4 matrix = Matrix4x4.identity;
            matrix.SetToRotation(axis, degrees);
            this = this * matrix;
        }

        public Vector4f ToXYZW()
        {
            return new Vector4f(x, y, z, 1);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
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
        public Vector3f Cross(Vector3f other)
        {
            return new Vector3f(y * other.z - z * other.y, z * other.x - x * other.z, x * other.y - y * other.x);
        }
        public float Dot(Vector3f other)
        {
            float result = x * other.x + y * other.y + z * other.z;
            return result;
        }
        public static Vector3f operator -(Vector3f a)
        {
            Vector3f result = new Vector3f();
            result.x = -a.x;
            result.y = -a.y;
            result.z = -a.z;
            return result;
        }
        public static Vector3f operator -(Vector3f a, Vector3f b)
        {
            Vector3f result = new Vector3f();
            result.x = a.x - b.x;
            result.y = a.y - b.y;
            result.z = a.z - b.z;
            return result;
        }

        public static Vector3f operator *(float d, Vector3f a)
        {
            Vector3f result = new Vector3f();
            result.x = a.x * d;
            result.y = a.y * d;
            result.z = a.z * d;
            return result;
        }
        public static Vector3f operator *(Vector3f a, float d)
        {
            Vector3f result = new Vector3f();
            result.x = a.x * d;
            result.y = a.y * d;
            result.z = a.z * d;
            return result;
        }

        public static Vector3f operator *(Vector3f a, Matrix4x4 matrix)
        {
            Vector3f result = new Vector3f();
            float x = a.x;
            float y = a.y;
            float z = a.z;
            result.Set(
                x * matrix.m00 + y * matrix.m01 + z * matrix.m02 + matrix.m03,
                x * matrix.m10 + y * matrix.m11 + z * matrix.m12 + matrix.m13,
                x * matrix.m20 + y * matrix.m21 + z * matrix.m22 + matrix.m23
                );
            return result;
        }
     
        public static Vector3f operator +(Vector3f a, Vector3f b)
        {
            Vector3f result = new Vector3f();
            result.x = a.x + b.x;
            result.y = a.y + b.y;
            result.z = a.z + b.z;
            return result;
        }
      
        //Returns the length of this vector (Read Only).
        public float Magnitude
        {
            get { return (float)Math.Sqrt(x * x + y * y + z * z); }
        }
        //Returns this vector with a magnitude of 1 (Read Only).
        public Vector3f Normalized
        {
            get
            {
                float len = SqrMagnitude;
                if (len == 0f || len == 1f)
                {
                    return this;
                }
                return this * (1f / ((float)Math.Sqrt(SqrMagnitude)));
            }
        }
        
        //Returns the squared length of this vector (Read Only).
        public float SqrMagnitude
        {
            get { return x * x + y * y + z * z; }
        }
      

        public Vector3f Project(ref Matrix4x4 matrix)
        {
            float w = x * matrix.m30 + y * matrix.m31 + z * matrix.m32 + matrix.m33;
            if (MathUtils.IsZero(w))
            {
                w = 1;
            }
            float l_w = 1f / w;
            return new Vector3f(
                (x * matrix.m00 + y * matrix.m01 + z * matrix.m02 + matrix.m03) * l_w,
                (x * matrix.m10 + y * matrix.m11 + z * matrix.m12 + matrix.m13) * l_w,
                (x * matrix.m20 + y * matrix.m21 + z * matrix.m22 + matrix.m23) * l_w);
        }

        public Vector4f ProjectW(ref Matrix4x4 matrix)
        {
            Vector4f vec = new Vector4f();
            vec.x = x * matrix.m00 + y * matrix.m01 + z * matrix.m02 + matrix.m03;
            vec.y = x * matrix.m10 + y * matrix.m11 + z * matrix.m12 + matrix.m13;
            vec.z = x * matrix.m20 + y * matrix.m21 + z * matrix.m22 + matrix.m23;
            vec.w = x * matrix.m30 + y * matrix.m31 + z * matrix.m32 + matrix.m33;
            return vec;
        }

        //Shorthand for writing Vector3(0, 1, 0).
        public static Vector3f Up
        {
            get { return new Vector3f(0, 1, 0); }
        }
        //Shorthand for writing Vector3(0, 0, 0).
        public static Vector3f Zero
        {
            get { return new Vector3f(0, 0, 0); }
        }

        //Shorthand for writing Vector3(1, 1, 1).
        public static Vector3f One
        {
            get { return new Vector3f(1, 1, 1); }
        }
        //Shorthand for writing Vector3(1, 0, 0).
        public static Vector3f Right
        {
            get { return new Vector3f(1, 0, 0); }
        }
        //Shorthand for writing Vector3(0, 0, -1).
        public static Vector3f Back
        {
            get { return new Vector3f(0, 0, -1); }
        }
        //Shorthand for writing Vector3(0, -1, 0).
        public static Vector3f Down
        {
            get { return new Vector3f(0, -1, 0); }
        }
        //Shorthand for writing Vector3(0, 0, 1).
        public static Vector3f Forward
        {
            get { return new Vector3f(0, 0, 1); }
        }
        public static float Len(float x, float y, float z)
        {
            return (float)Math.Sqrt(x * x + y * y + z * z);
        }
    }
}
