using System;
using PerfectWorldSurvivor.Draw;
using PerfectWorldSurvivor.Utils;

namespace PerfectWorldSurvivor.Model
{
    public class Camera
    {
        public Camera(float fov, float viewWidth, float viewHeight)
        {
            _transform = new Transform();
            _FrustumCulling = new FrustumCulling();
            if (MathUtils.IsZero(fov))
            {
                FieldOfView = _defaultFOV;
            }
            else
            {
                FieldOfView = fov;
            }
            if (MathUtils.IsZero(viewWidth))
            {
                _viewWidth = _defaultSize;
            }
            else
            {
                _viewWidth = viewWidth;
            }
            if (MathUtils.IsZero(viewHeight))
            {
                _viewHeight = _defaultSize;
            }
            else
            {
                _viewHeight = viewHeight;
            }
            Aspect = _viewWidth / _viewHeight;
            enableFrustumCulling = true;
            HFOVTan = (float)Math.Tan(0.5f * fov * Math.PI / 180);
        }
        public void InitProjectionMatrix()
        {
            Projection.SetToProjection(Math.Abs(near), Math.Abs(far), FieldOfView, Aspect);
        }
        public void ProjectToViewSpace(ref VertexShaderInputWithText vertexInput, ref VertexShaderOutput output,ref Matrix4x4 mvMatrix)
        {
            Vector3f vertexPosition = vertexInput.position;
            Vector4f pointw = vertexPosition.ProjectW(ref mvMatrix);
            output.svPosition = pointw;
        }
        public void CalculateWorldData(ref VertexShaderInputWithText vertexInput, ref VertexShaderOutput output, ref Matrix4x4 worldMatrix)
        {
            Vector3f vertexPosition = vertexInput.position;
            Vector4f pointw = vertexPosition.ProjectW(ref worldMatrix);
            output.worldPosition = pointw.ToXYZ();
            output.worldNormal = vertexInput.normal.Project(ref worldMatrix);
        }
        public void ProjectToProjectionSpace(ref VertexShaderOutput vertexInput,ref Matrix4x4 pMatrix)
        {
            Vector4f vertexPosition = vertexInput.svPosition;
            Vector4f pointw = vertexPosition.Project(ref pMatrix);
            vertexInput.svPosition = pointw;
            float w = pointw.w;
            if (MathUtils.IsZero(w))
            {
                w = 1;
            }
            vertexInput.reciprocalZ = 1 / w;
        }
        public void TransformViewFrustumToScreen(ref VertexShaderOutput vertex)
        {
            float reciprocalZ = vertex.reciprocalZ;
            vertex.svPosition = vertex.svPosition * reciprocalZ;
            vertex.svPosition.w = 1;
            float x = (vertex.svPosition.x  + 1) * _viewWidth * 0.5f;
            float y =(1 - vertex.svPosition.y) * _viewHeight * 0.5f;
            vertex.svPosition.x = x;
            vertex.svPosition.y = y;
        }
        public bool CheckInFrustum(CubicBound cubicBound)
        {
            if (!enableFrustumCulling)
            {
                return true;
            }
            return _FrustumCulling.CheckBoundInFrustum(cubicBound);
        }
        public void Update()
        {
            View = Matrix4x4.SetToLookAt(Trans.position, Trans.position + Trans.Forward, Trans.Up);
            if (enableFrustumCulling)
            {
                Combined = Projection * View;
                _FrustumCulling.SetVPMatrix(Combined);
            }
        }
        public float Aspect { get; private set;}
      
        public float far;

        public float near;

        public bool enableFrustumCulling;
        public float FieldOfView { get; private set; }
        public Transform Trans { get { return _transform; } }

        //The projection matrix
        public Matrix4x4 Projection;
        //The view matrix
        public Matrix4x4 View;
        //The combined projection and view matrix
        public Matrix4x4 Combined;
        //The inverse combined projection and view matrix **/
        public Matrix4x4 InverseProjectionView;
        public float HFOVTan { get; private set; }

        private static readonly float _defaultSize = 100;

        private static readonly float _defaultFOV = 67;

        private Transform _transform;

        private float _viewWidth;

        private float _viewHeight;

        private FrustumCulling _FrustumCulling;
    }
}
