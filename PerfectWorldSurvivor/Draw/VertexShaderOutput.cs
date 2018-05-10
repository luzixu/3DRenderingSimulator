using PerfectWorldSurvivor.Model;

namespace PerfectWorldSurvivor.Draw
{
    public struct VertexShaderOutput
    {
        public Vector4f svPosition;

        public Vector3f worldPosition;

        public Vector3f worldNormal;

        public Vector2f uv;

        public float reciprocalZ;

        public Colorf color;

        public Colorf fragColor; //Blinn-Phong

    }
}
