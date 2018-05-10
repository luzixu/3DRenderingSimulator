using System;
using System.Text;
using PerfectWorldSurvivor.Utils;

namespace PerfectWorldSurvivor.Model
{

    /// <summary>
    /// Row first
    /// </summary>
    public struct Matrix4x4
    {
        public float m00;
        public float m01;
        public float m02;
        public float m03;
        public float m10;
        public float m11;
        public float m12;
        public float m13;
        public float m20;
        public float m21;
        public float m22;
        public float m23;
        public float m30;
        public float m31;
        public float m32;
        public float m33;

        public float this[int index]
        {
            get
            {
                float result = 0;
                switch (index)
                {
                    case IndexM00:
                        result = m00;
                        break;
                    case IndexM01:
                        result = m01;
                        break;
                    case IndexM02:
                        result = m02;
                        break;
                    case IndexM03:
                        result = m03;
                        break;
                    case IndexM10:
                        result = m10;
                        break;
                    case IndexM11:
                        result = m11;
                        break;
                    case IndexM12:
                        result = m12;
                        break;
                    case IndexM13:
                        result = m13;
                        break;
                    case IndexM20:
                        result = m20;
                        break;
                    case IndexM21:
                        result = m21;
                        break;
                    case IndexM22:
                        result = m22;
                        break;
                    case IndexM23:
                        result = m23;
                        break;
                    case IndexM30:
                        result = m30;
                        break;
                    case IndexM31:
                        result = m31;
                        break;
                    case IndexM32:
                        result = m32;
                        break;
                    case IndexM33:
                        result = m33;
                        break;
                }
                return result;
            }
            set
            {
                switch (index)
                {
                    case IndexM00:
                        m00 = value;
                        break;
                    case IndexM01:
                        m01 = value;
                        break;
                    case IndexM02:
                        m02 = value;
                        break;
                    case IndexM03:
                        m03 = value;
                        break;
                    case IndexM10:
                        m10 = value;
                        break;
                    case IndexM11:
                        m11 = value;
                        break;
                    case IndexM12:
                        m12 = value;
                        break;
                    case IndexM13:
                        m13 = value;
                        break;
                    case IndexM20:
                        m20 = value;
                        break;
                    case IndexM21:
                        m21 = value;
                        break;
                    case IndexM22:
                        m22 = value;
                        break;
                    case IndexM23:
                        m23 = value;
                        break;
                    case IndexM30:
                        m30 = value;
                        break;
                    case IndexM31:
                        m31 = value;
                        break;
                    case IndexM32:
                        m32 = value;
                        break;
                    case IndexM33:
                        m33 = value;
                        break;
                }
            }
        }

        public float this[int row, int column]
        {
            get
            {
                int index = column * 4 + row;
                return this[index];
            }
            set
            {
                int index = column * 4 + row;
                this[index] = value;
            }

        }

        public static Matrix4x4 operator *(Matrix4x4 lhs, Matrix4x4 rhs)
        {
            Matrix4x4 result = new Matrix4x4();
            result.m00 = lhs.m00 * rhs.m00 + lhs.m01 * rhs.m10 + lhs.m02 * rhs.m20 + lhs.m03 * rhs.m30;
            result.m01 = lhs.m00 * rhs.m01 + lhs.m01 * rhs.m11 + lhs.m02 * rhs.m21 + lhs.m03 * rhs.m31;
            result.m02 = lhs.m00 * rhs.m02 + lhs.m01 * rhs.m12 + lhs.m02 * rhs.m22 + lhs.m03 * rhs.m32;
            result.m03 = lhs.m00 * rhs.m03 + lhs.m01 * rhs.m13 + lhs.m02 * rhs.m23 + lhs.m03 * rhs.m33;

            result.m10 = lhs.m10 * rhs.m00 + lhs.m11 * rhs.m10 + lhs.m12 * rhs.m20 + lhs.m13 * rhs.m30;
            result.m11 = lhs.m10 * rhs.m01 + lhs.m11 * rhs.m11 + lhs.m12 * rhs.m21 + lhs.m13 * rhs.m31;
            result.m12 = lhs.m10 * rhs.m02 + lhs.m11 * rhs.m12 + lhs.m12 * rhs.m22 + lhs.m13 * rhs.m32;
            result.m13 = lhs.m10 * rhs.m03 + lhs.m11 * rhs.m13 + lhs.m12 * rhs.m23 + lhs.m13 * rhs.m33;

            result.m20 = lhs.m20 * rhs.m00 + lhs.m21 * rhs.m10 + lhs.m22 * rhs.m20 + lhs.m23 * rhs.m30;
            result.m21 = lhs.m20 * rhs.m01 + lhs.m21 * rhs.m11 + lhs.m22 * rhs.m21 + lhs.m23 * rhs.m31;
            result.m22 = lhs.m20 * rhs.m02 + lhs.m21 * rhs.m12 + lhs.m22 * rhs.m22 + lhs.m23 * rhs.m32;
            result.m23 = lhs.m20 * rhs.m03 + lhs.m21 * rhs.m13 + lhs.m22 * rhs.m23 + lhs.m23 * rhs.m33;

            result.m30 = lhs.m30 * rhs.m00 + lhs.m31 * rhs.m10 + lhs.m32 * rhs.m20 + lhs.m33 * rhs.m30;
            result.m31 = lhs.m30 * rhs.m01 + lhs.m31 * rhs.m11 + lhs.m32 * rhs.m21 + lhs.m33 * rhs.m31;
            result.m32 = lhs.m30 * rhs.m02 + lhs.m31 * rhs.m12 + lhs.m32 * rhs.m22 + lhs.m33 * rhs.m32;
            result.m33 = lhs.m30 * rhs.m03 + lhs.m31 * rhs.m13 + lhs.m32 * rhs.m23 + lhs.m33 * rhs.m33;
            return result;
        }

        public static Vector3f operator *(Matrix4x4 lhs, Vector3f vec)
        {
            float x = vec.x * lhs.m00 + vec.y * lhs.m01 + vec.z * lhs.m02 + lhs.m03;
            float y = vec.x * lhs.m10 + vec.y * lhs.m11 + vec.z * lhs.m12 + lhs.m13;
            float z = vec.x * lhs.m20 + vec.y * lhs.m21 + vec.z * lhs.m22 + lhs.m23;
            vec.x = x;
            vec.y = y;
            vec.z = z;
            return vec;
        }

        /// <summary>
        /// Set this matrix to identity matrix.
        /// </summary>
        /// <returns></returns>
        public void SetToIdentity()
        {
            m00 = 1;
            m01 = 0;
            m02 = 0;
            m03 = 0;
            m10 = 0;
            m11 = 1;
            m12 = 0;
            m13 = 0;
            m20 = 0;
            m21 = 0;
            m22 = 1;
            m23 = 0;
            m30 = 0;
            m31 = 0;
            m32 = 0;
            m33 = 1;
        }

        public Matrix4x4 Transpose()
        {
            Matrix4x4 result = new Matrix4x4();
            result.Set(this);
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    float tmp = result[i, j];
                    result[i, j] = result[j, i];
                    result[j, i] = tmp;
                }
            }
            return result;
        }

        public void SetToProjection(float near, float far, float fovy, float aspectRatio)
        {
            SetToIdentity();
            if (near <= 0 || far <= 0 || near >= far)
            {
                return;
            }
            if (fovy <= 0 || fovy > 180)
            {
                return;
            }
            if (aspectRatio <= 0)
            {
                return;
            }
            float l_fd = (float)(1.0 / Math.Tan((fovy * (Math.PI / 180)) * 0.5f));
            float l_a1 = (far + near) / (near - far);
            float l_a2 = (2 *  far * near) / (near - far);
            m00 = l_fd / aspectRatio;
            m10 = 0;
            m20 = 0;
            m30 = 0;
            m01 = 0;
            m11 = l_fd;
            m21 = 0;
            m31 = 0;
            m02 = 0;
            m12 = 0;
            m22 = l_a1;
            m32 = -1;
            m03 = 0;
            m13 = 0;
            m23 = l_a2;
            m33 = 0;
        }

        public Matrix4x4 SetToTranslation(float x, float y, float z)
        {
            SetToIdentity();
            m03 = x;
            m13 = y;
            m23 = z;
            return this;
        }

        public Matrix4x4 SetToTranslation(Vector3f translation)
        {
            SetToIdentity();
            m03 = translation.x;
            m13 = translation.y;
            m23 = translation.z;
            return this;
        }

        public static Matrix4x4  SetToLookAt(Vector3f position, Vector3f target, Vector3f up)
        {
            Vector3f dir = target - position;
            Matrix4x4 translationMatrix = Matrix4x4.identity.SetToTranslation(-position.x, -position.y, -position.z);
            return  SetToLookAt(dir, up) * translationMatrix;;
        }

        public static Matrix4x4 SetToLookAt(Vector3f direction, Vector3f up)
        {
            Matrix4x4 result = new Matrix4x4();
            Vector3f l_vez = direction.Normalized;
            Vector3f l_vex = new Vector3f(l_vez);
            l_vex = (l_vex.Cross(up)).Normalized;
            Vector3f l_vey = (l_vex.Cross(l_vez)).Normalized;
            result.SetToIdentity();

            //right
            result.m00 = l_vex.x;
            result.m01 = l_vex.y;
            result.m02 = l_vex.z;
            //up
            result.m10 = l_vey.x;
            result.m11 = l_vey.y;
            result.m12 = l_vey.z;
            //forward
            result.m20 = -l_vez.x;
            result.m21 = -l_vez.y;
            result.m22 = -l_vez.z;
            return result;
        }

        public void SetToRotation(Vector3f axis, float degrees)
        {
            if (MathUtils.IsZero(degrees))
            {
                SetToIdentity();
                return;
            }
            Quaternion quaternion = Quaternion.Identity;
            quaternion.Set(axis, degrees);
            Set(quaternion);
        }
     
        public static Matrix4x4 identity
        {
            get
            {
                Matrix4x4 matrix = new Matrix4x4();
                matrix.m00 = 1;
                matrix.m11 = 1;
                matrix.m22 = 1;
                matrix.m33 = 1;
                return matrix;
            }
        }

        public void Set(Matrix4x4 matrix)
        {
            m00 = matrix.m00;
            m01 = matrix.m01;
            m02 = matrix.m02;
            m03 = matrix.m03;
            m10 = matrix.m10;
            m11 = matrix.m11;
            m12 = matrix.m12;
            m13 = matrix.m13;
            m20 = matrix.m20;
            m21 = matrix.m21;
            m22 = matrix.m22;
            m23 = matrix.m23;
            m30 = matrix.m30;
            m31 = matrix.m31;
            m32 = matrix.m32;
            m33 = matrix.m33;
        }

        public void Set(Quaternion quaternion)
        {
            Set(0f, 0f, 0f, quaternion.x, quaternion.y, quaternion.z, quaternion.w); ;
        }

        public void Set(float translationX, float translationY, float translationZ, float quaternionX, float quaternionY,
            float quaternionZ, float quaternionW)
        {
            float xs = quaternionX * 2f, ys = quaternionY * 2f, zs = quaternionZ * 2f;
            float wx = quaternionW * xs, wy = quaternionW * ys, wz = quaternionW * zs;
            float xx = quaternionX * xs, xy = quaternionX * ys, xz = quaternionX * zs;
            float yy = quaternionY * ys, yz = quaternionY * zs, zz = quaternionZ * zs;

            m00 = (1.0f - (yy + zz));
            m01 = (xy - wz);
            m02 = (xz + wy);
            m03 = translationX;

            m10 = (xy + wz);
            m11 = (1.0f - (xx + zz));
            m12 = (yz - wx);
            m13 = translationY;

            m20 = (xz - wy);
            m21 = (yz + wx);
            m22 = (1.0f - (xx + yy));
            m23 = translationZ;

            m30 = 0;
            m31 = 0;
            m32 = 0;
            m33 = 1;
        }

        public bool InverseMatrix()
        {
            float l_det = _Det();
            if (MathUtils.IsZero(l_det))
            {
                return false;
            }
            Matrix4x4 inverse = new Matrix4x4();
            inverse.m00 = m12 * m23 * m31 - m13 * m22 * m31 + m13 * m21 * m32 - m11 * m23 * m32 - m12 * m21 * m33 + m11 * m22 * m33;
            inverse.m01 = m03 * m22 * m31 - m02 * m23 * m31 - m03 * m21 * m32 + m01 * m23 * m32 + m02 * m21 * m33 - m01 * m22 * m33;
            inverse.m02 = m02 * m13 * m31 - m03 * m12 * m31 + m03 * m11 * m32 - m01 * m13 * m32 - m02 * m11 * m33 + m01 * m12 * m33;
            inverse.m03 = m03 * m12 * m21 - m02 * m13 * m21 - m03 * m11 * m22 + m01 * m13 * m22 + m02 * m11 * m23 - m01 * m12 * m23;
            inverse.m10 = m13 * m22 * m30 - m12 * m23 * m30 - m13 * m20 * m32 + m10 * m23 * m32 + m12 * m20 * m33 - m10 * m22 * m33;
            inverse.m11 = m02 * m23 * m30 - m03 * m22 * m30 + m03 * m20 * m32 - m00 * m23 * m32 - m02 * m20 * m33 + m00 * m22 * m33;
            inverse.m12 = m03 * m12 * m30 - m02 * m13 * m30 - m03 * m10 * m32 + m00 * m13 * m32 + m02 * m10 * m33 - m00 * m12 * m33;
            inverse.m13 = m02 * m13 * m20 - m03 * m12 * m20 + m03 * m10 * m22 - m00 * m13 * m22 - m02 * m10 * m23 + m00 * m12 * m23;
            inverse.m20 = m11 * m23 * m30 - m13 * m21 * m30 + m13 * m20 * m31 - m10 * m23 * m31 - m11 * m20 * m33 + m10 * m21 * m33;
            inverse.m21 = m03 * m21 * m30 - m01 * m23 * m30 - m03 * m20 * m31 + m00 * m23 * m31 + m01 * m20 * m33 - m00 * m21 * m33;
            inverse.m22 = m01 * m13 * m30 - m03 * m11 * m30 + m03 * m10 * m31 - m00 * m13 * m31 - m01 * m10 * m33 + m00 * m11 * m33;
            inverse.m23 = m03 * m11 * m20 - m01 * m13 * m20 - m03 * m10 * m21 + m00 * m13 * m21 + m01 * m10 * m23 - m00 * m11 * m23;
            inverse.m30 = m12 * m21 * m30 - m11 * m22 * m30 - m12 * m20 * m31 + m10 * m22 * m31 + m11 * m20 * m32 - m10 * m21 * m32;
            inverse.m31 = m01 * m22 * m30 - m02 * m21 * m30 + m02 * m20 * m31 - m00 * m22 * m31 - m01 * m20 * m32 + m00 * m21 * m32;
            inverse.m32 = m02 * m11 * m30 - m01 * m12 * m30 - m02 * m10 * m31 + m00 * m12 * m31 + m01 * m10 * m32 - m00 * m11 * m32;
            inverse.m33 = m01 * m12 * m20 - m02 * m11 * m20 + m02 * m10 * m21 - m00 * m12 * m21 - m01 * m10 * m22 + m00 * m11 * m22;
            float inv_det = 1.0f / l_det;
            m00 = inverse.m00 * inv_det;
            m01 = inverse.m01 * inv_det;
            m02 = inverse.m02 * inv_det;
            m03 = inverse.m03 * inv_det;
            m10 = inverse.m10 * inv_det;
            m11 = inverse.m11 * inv_det;
            m12 = inverse.m12 * inv_det;
            m13 = inverse.m13 * inv_det;
            m20 = inverse.m20 * inv_det;
            m21 = inverse.m21 * inv_det;
            m22 = inverse.m22 * inv_det;
            m23 = inverse.m23 * inv_det;
            m30 = inverse.m30 * inv_det;
            m31 = inverse.m31 * inv_det;
            m32 = inverse.m32 * inv_det;
            m33 = inverse.m33 * inv_det;
            return true;
        }

        public override string ToString()
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("\n");
            stringBuilder.Append(m00);
            stringBuilder.Append(" ");
            stringBuilder.Append(m01);
            stringBuilder.Append(" ");
            stringBuilder.Append(m02);
            stringBuilder.Append(" ");
            stringBuilder.Append(m03);
            stringBuilder.Append("\n");

            stringBuilder.Append(m10);
            stringBuilder.Append(" ");
            stringBuilder.Append(m11);
            stringBuilder.Append(" ");
            stringBuilder.Append(m12);
            stringBuilder.Append(" ");
            stringBuilder.Append(m13);
            stringBuilder.Append("\n");

            stringBuilder.Append(m20);
            stringBuilder.Append(" ");
            stringBuilder.Append(m21);
            stringBuilder.Append(" ");
            stringBuilder.Append(m22);
            stringBuilder.Append(" ");
            stringBuilder.Append(m23);
            stringBuilder.Append("\n");

            stringBuilder.Append(m30);
            stringBuilder.Append(" ");
            stringBuilder.Append(m31);
            stringBuilder.Append(" ");
            stringBuilder.Append(m32);
            stringBuilder.Append(" ");
            stringBuilder.Append(m33);
            stringBuilder.Append("\n");

            return stringBuilder.ToString();
        }
        private float _Det()
        {
            return
              m30 * m21 * m12 * m03 - m20 * m31 * m12 * m03 - m30 * m11 * m22 * m03 + m10 * m31 * m22 * m03 + m20 * m11 * m32 * m03 - m10 * m21 * m32 * m03
            - m30 * m21 * m02 * m13 + m20 * m31 * m02 * m13 + m30 * m01 * m22 * m13 - m00 * m31 * m22 * m13 - m20 * m01 * m32 * m13 + m00 * m21 * m32 * m13
            + m30 * m11 * m02 * m23 - m10 * m31 * m02 * m23 - m30 * m01 * m12 * m23 + m00 * m31 * m12 * m23 + m10 * m01 * m32 * m23 - m00 * m11 * m32 * m23
            - m20 * m11 * m02 * m33 + m10 * m21 * m02 * m33 + m20 * m01 * m12 * m33 - m00 * m21 * m12 * m33 - m10 * m01 * m22 * m33 + m00 * m11 * m22 * m33;
        }

        public const int IndexM00 = 0;
        public const int IndexM01 = 4;
        public const int IndexM02 = 8;
        public const int IndexM03 = 12;
        public const int IndexM10 = 1;
        public const int IndexM11 = 5;
        public const int IndexM12 = 9;
        public const int IndexM13 = 13;
        public const int IndexM20 = 2;
        public const int IndexM21 = 6;
        public const int IndexM22 = 10;
        public const int IndexM23 = 14;
        public const int IndexM30 = 3;
        public const int IndexM31 = 7;
        public const int IndexM32 = 11;
        public const int IndexM33 = 15;
    }
}
