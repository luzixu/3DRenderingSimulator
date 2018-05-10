
namespace PerfectWorldSurvivor.Model
{
    /// <summary>
    /// 1.We can check point in view space.
    ///   Transform point into view-space point.
    ///      -w <= x <= w
    ///      -w <= y <= w
    ///      -w <= z <= w
    /// 2.We can check point in world space 
    ///   Transform clipping box into world space
    ///    
    /// 
    /// </summary>
    public class FrustumCulling
    {
        public void SetVPMatrix(Matrix4x4 vp)
        {
            //Transform clipping box into world space.
            //inverse matrix vp to calculate M-1
            vp.InverseMatrix();
            int pointArrayLen = _planePoints.Length;
            for (int i = 0; i < pointArrayLen; i++)
            {
                _planePoints[i] = _originalPlanePoints[i].Project(ref vp);
            }
            planes[0].Set(_planePoints[1], _planePoints[0], _planePoints[2]); //Near
            planes[1].Set(_planePoints[4], _planePoints[5], _planePoints[7]); //Far
            planes[2].Set(_planePoints[0], _planePoints[4], _planePoints[3]); //Left
            planes[3].Set(_planePoints[5], _planePoints[1], _planePoints[6]); //Right
            planes[4].Set(_planePoints[2], _planePoints[3], _planePoints[6]); //Top
            planes[5].Set(_planePoints[4], _planePoints[0], _planePoints[1]); //Bottom

            //Or
            //Left: 0 =( Mcol3 + Mcol0)
            //Right: 0 = (Mcol3 - Mcol0)
            //NearZ: 0 = Mcol2
            //FarZ: 0 = (Mcol3 - Mcol2)
            //Top: 0 = (Mcol3 - Mcol1)
            //Bottom:  0 = (Mcol3 + Mcol1)
            //Normalize this Vectors 

        }

        public bool CheckBoundInFrustum(CubicBound bound)
        {
            Vector3f[] corners = bound.corners;
            int cornersLen = corners.Length;
            int planesLen = planes.Length;
            for (int i = 0; i < planesLen; i++)
            {
                int backCount = 0;
                for (int ii = 0; ii < cornersLen; ii++)
                {
                    if (planes[i].GetPointSide(corners[ii]) == Plane.Side.Back)
                    {
                        backCount++;
                    }
                }
                if (backCount == cornersLen)
                {
                    return false;
                }
            }
            return true;
        }

        public readonly Plane[] planes = new Plane[6];

        private readonly Vector3f[] _planePoints = new Vector3f[8];

        private static readonly float _scale = 1f;

        private static readonly Vector3f[] _originalPlanePoints = new Vector3f[8] {
            new Vector3f(-1,-1,-1) * _scale,
            new Vector3f(1,-1,-1) * _scale,
            new Vector3f(1,1,-1) * _scale,
            new Vector3f(-1,1,-1) * _scale,

            new Vector3f(-1,-1,1) * _scale,
            new Vector3f(1,-1,1) * _scale,
            new Vector3f(1,1,1) * _scale,
            new Vector3f(-1,1,1) * _scale
        };
    }
}
