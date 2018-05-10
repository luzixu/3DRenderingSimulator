using PerfectWorldSurvivor.Model;

namespace PerfectWorldSurvivor.Draw
{
    public class Transform
    {
        public Transform()
        {
            Up = new Vector3f(0, 1, 0);
            Forward = new Vector3f(0, 0, 1);
            _worldMatrix = Matrix4x4.identity;
            _worldToLocalMatrix = Matrix4x4.identity;
            _localToWorldMatrix = Matrix4x4.identity;
            _rotation = Matrix4x4.identity;
        }
        public void LookAt(float x,float y,float z)
        {
            Forward = (new Vector3f(x,y,z) - position).Normalized;
            _NormalizeUp();
        }
        public void Rotate(Vector3f axis , float angle)
        {
            Matrix4x4 matrix = Matrix4x4.identity;
            matrix.SetToRotation(axis, angle);
            Forward *= matrix;
            Up *= matrix;
            Right *= matrix;
            _rotation *= matrix;
        }
        public void Translate(Vector3f vec)
        {
            position = position + vec;
        }
        private void _NormalizeUp()
        {
            Vector3f forwardVec = Forward;
            Right = (forwardVec.Cross(Up)).Normalized;
            Up = (Right.Cross(Forward)).Normalized;
          
        }
        public Vector3f Forward { get; private set; }
        public Vector3f Right { get; private set; }
        public Matrix4x4 MatrixRotation { get { return _rotation; } }
        public Vector3f Up { get; private set; }

        public Vector3f position;

        private Matrix4x4 _worldToLocalMatrix;

        private Matrix4x4 _worldMatrix;

        private Matrix4x4 _rotation;

        private Matrix4x4 _localToWorldMatrix;
    }
}
