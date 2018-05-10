using PerfectWorldSurvivor.Model;

namespace PerfectWorldSurvivor.Draw
{
    public struct VertexShaderInput
    {
        public Vector3f position;

        public Vector3f normal;

        public Colorf color;
    }

    public struct VertexShaderInputWithText
    {
        public Vector3f position;

        public Vector3f normal;

        public Vector2f textCoord;

        public Colorf color;
    }
}
