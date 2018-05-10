using System;
using System.Text;
using PerfectWorldSurvivor.Utils;

namespace PerfectWorldSurvivor.Model
{
    public struct Quaternion
    {
        public float w;

        public float x;

        public float y;

        public float z;

        public Quaternion(float x, float y, float z, float w)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            this.w = w;
        }

        public Quaternion(Quaternion quaternion)
        {
            this.x = quaternion.x;
            this.y = quaternion.y;
            this.z = quaternion.z;
            this.w = quaternion.w;
        }

        public static Quaternion operator *(Quaternion lhs, Quaternion rhs)
        {
            float newX = lhs.w * rhs.x + lhs.x * rhs.w + lhs.y * rhs.z - lhs.z * rhs.y;
            float newY = lhs.w * rhs.y + lhs.y * rhs.w + lhs.z * rhs.x - lhs.x * rhs.z;
            float newZ = lhs.w * rhs.z + lhs.z * rhs.w + lhs.x * rhs.y - lhs.y * rhs.x;
            float newW = lhs.w * rhs.w - lhs.x * rhs.x - lhs.y * rhs.y - lhs.z * rhs.z;
            lhs.x = newX;
            lhs.y = newY;
            lhs.z = newZ;
            lhs.w = newW;
            return lhs;
        }

        public void Set(Quaternion quaternion)
        {
            this.x = quaternion.x;
            this.y = quaternion.y;
            this.z = quaternion.z;
            this.w = quaternion.w;
        }
        public void Set(float x, float y, float z, float w)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            this.w = w;
        }

        public void Set(Vector3f axis, float angle)
        {
            SetFromAngleAxis(axis.x, axis.y, axis.z, angle);
        }

        public void SetFromAngleAxis(Vector3f axis, float degrees)
        {
            SetFromAngleAxis(axis.x, axis.y, axis.z, degrees);
        }

        public void SetFromAngleAxis(float x, float y, float z, float degrees)
        {
            SetFromAngleAxisRad(x,y,z,degrees * MathUtils.degreesToRadians);
        }

        public void SetFromAngleAxisRad(float x, float y, float z, float radians)
        {
            float d = Vector3f.Len(x, y, z);
            if (MathUtils.IsZero(d))
            {
                return;
            }
            d = 1f / d;
            float ang = radians < 0 ? MathUtils.pi2 - (-radians % MathUtils.pi2) : radians % MathUtils.pi2;//Ensure ang in [0,360];
            float sin = (float)Math.Sin(ang / 2);
            float cos = (float)Math.Cos(ang / 2);
            Set(d * x * sin, d * y * sin, d * z * sin, cos);
        }
        public void SetFromEulerAngles(Vector3f eulerAngles)
        {
            SetFromEulerAngles(eulerAngles.x, eulerAngles.y, eulerAngles.z);
        }

        public void SetFromEulerAngles(float xDegree,float yDegree,float zDegree)
        {
            SetFromEulerAnglesRadian(xDegree * MathUtils.degreesToRadians, yDegree * MathUtils.degreesToRadians, zDegree * MathUtils.degreesToRadians);
        }

        public void SetFromEulerAnglesRadian(float x, float y, float z)
        {
            float halfX = x * 0.5f;
            float sinHalfX = (float)(Math.Sin(halfX));
            float cosHalfX = (float)(Math.Cos(halfX));
            float halfY = y * 0.5f;
            float sinHalfY = (float)(Math.Sin(halfY));
            float cosHalfY = (float)(Math.Cos(halfY));
            float halfZ = z * 0.5f;
            float sinHalfZ = (float)(Math.Sin(halfZ));
            float cosHalfZ = (float)(Math.Cos(halfZ));
            float cosHalfY_sinHalfX = cosHalfY * sinHalfX;
            float sinHalfY_cosHalfX = sinHalfY * cosHalfX;
            float cosHalfY_cosHalfX = cosHalfY * cosHalfX;
            float sinHalfY_sinHalfX = sinHalfY * sinHalfX;
            this.x = (cosHalfY_sinHalfX * cosHalfZ) + (sinHalfY_cosHalfX * sinHalfZ);
            this.y = (sinHalfY_cosHalfX * cosHalfZ) - (cosHalfY_sinHalfX * sinHalfZ);
            this.z = (cosHalfY_cosHalfX * sinHalfZ) - (sinHalfY_sinHalfX * cosHalfZ);
            this.w = (cosHalfY_cosHalfX * cosHalfZ) + (sinHalfY_sinHalfX * sinHalfZ);
        }

        public Matrix4x4 ToMatrix()
        {
            Matrix4x4 result = new Matrix4x4();
            float xx = x * x;
            float xy = x * y;
            float xz = x * z;
            float xw = x * w;
            float yy = y * y;
            float yz = y * z;
            float yw = y * w;
            float zz = z * z;
            float zw = z * w;
            result.m00 = 1 - 2 * (yy + zz);
            result.m01 = 2 * (xy - zw);
            result.m02 = 2 * (xz + yw);
            result.m03 = 0;
            result.m10 = 2 * (xy + zw);
            result.m11 = 1 - 2 * (xx + zz);
            result.m12 = 2 * (yz - xw);
            result.m13 = 0;
            result.m20 = 2 * (xz - yw);
            result.m21 = 2 * (yz + xw);
            result.m22 = 1 - 2 * (xx + yy);
            result.m23 = 0;
            result.m30 = 0;
            result.m31 = 0;
            result.m32 = 0;
            result.m33 = 1;
            return result;
        }

        public void SetToIdentity()
        {
            Set(0, 0, 0, 1);
        }

        public static Quaternion Identity
        {
            get
            {
                Quaternion result = new Quaternion();
                result.SetToIdentity();
                return result;
            }
        }

        public override string ToString()
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("x: ");
            stringBuilder.Append(x);
            stringBuilder.Append("y: ");
            stringBuilder.Append(y);
            stringBuilder.Append("z: ");
            stringBuilder.Append(z);
            stringBuilder.Append("w: ");
            stringBuilder.Append(w);
            return stringBuilder.ToString();
        }
    }
}
