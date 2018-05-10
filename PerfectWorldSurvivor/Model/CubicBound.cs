using System;
using System.Text;

namespace PerfectWorldSurvivor.Model
{
    public class CubicBound
    {
        public CubicBound()
        {
            _LocalCorners = new Vector3f[8];
            corners = new Vector3f[8];
        }
        public void Set(Vector3f[] points)
        {
            if (points == null)
            {
                return;
            }
            _Reset();
            int len = points.Length;
            for (int i = 0; i < len; i++)
            {
                _Extend(points[i]);
            }
            _UpdateCorners();
        }

        public void Update(ref Matrix4x4 worldMat)
        {
            int len = _LocalCorners.Length;
            for (int i = 0; i < len; i++)
            {
                corners[i] = _LocalCorners[i].Project(ref worldMat);
            }
        }

        public override string ToString()
        {
            StringBuilder stringBuilder = new StringBuilder();
            for (int i = 0; i < _LocalCorners.Length; i++)
            {
                stringBuilder.Append("Corner");
                stringBuilder.Append(i);
                stringBuilder.Append(" : ");
                stringBuilder.Append(corners[i].ToString());
                stringBuilder.Append("\n");
            }
            return stringBuilder.ToString();
        }
        private void _Reset()
        {
            _min.Set(float.MaxValue, float.MaxValue, float.MaxValue);
            _max.Set(float.MinValue, float.MinValue, float.MinValue);
        }
        private void _Extend(Vector3f point)
        {
            _Set(
                _min.Set(Math.Min(_min.x, point.x), Math.Min(_min.y, point.y), Math.Min(_min.z, point.z)),
                _max.Set(Math.Max(_max.x, point.x), Math.Max(_max.y, point.y), Math.Max(_max.z, point.z))
                );
        }
        private void _Set(Vector3f min, Vector3f max)
        {
            min.Set(min.x < max.x ? min.x : max.x, min.y < max.y ? min.y : max.y,
            min.z < max.z ? min.z : max.z);
            max.Set(min.x > max.x ? min.x : max.x, min.y > max.y ? min.y : max.y,
                min.z > max.z ? min.z : max.z);
        }
        private void _UpdateCorners()
        {
            _LocalCorners[0].Set(_min.x, _min.y, _min.z);
            _LocalCorners[1].Set(_max.x, _min.y, _min.z);
            _LocalCorners[2].Set(_max.x, _max.y, _min.z);
            _LocalCorners[3].Set(_min.x, _max.y, _min.z);
            _LocalCorners[4].Set(_min.x, _min.y, _max.z);
            _LocalCorners[5].Set(_max.x, _min.y, _max.z);
            _LocalCorners[6].Set(_max.x, _max.y, _max.z);
            _LocalCorners[7].Set(_min.x, _max.y, _max.z);
        }

        public readonly Vector3f[] corners;

        private Vector3f _min;

        private Vector3f _max;

        private Vector3f[] _LocalCorners;
    }
}
