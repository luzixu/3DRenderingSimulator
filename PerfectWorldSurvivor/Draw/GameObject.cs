using PerfectWorldSurvivor.Common;
using PerfectWorldSurvivor.Model;
using System;

namespace PerfectWorldSurvivor.Draw
{
    public class GameObject : IDisposable
    {
        public GameObject()
        {
            _transform = new Transform();
            _cubicBound = new CubicBound();
        }

        public Mesh GetMesh()
        {
            return _mesh;
        }

        public void SetMesh(Mesh mesh)
        {
            _mesh = mesh;
            _cubicBound.Set(_mesh.vertices);
        }

        public void SetTexture(Texture texture)
        {
            _texture = texture;
        }

        public Texture GetTexture()
        {
            return _texture;
        }

        public void Dispose()
        {
            if (_texture != null)
            {
                _texture.Dispose();
            }
        }

        public Transform Trans { get { return _transform; } }

        public CubicBound Bound { get { return _cubicBound; } }

        private Transform _transform;

        private Mesh _mesh;

        private Texture _texture;

        private CubicBound _cubicBound;
    }
}
