using System.Text;

namespace PerfectWorldSurvivor.Model
{
    public struct Face
    {
        public Vector3i vertIndices;

        public Vector3i normalIndices;

        public Vector3i uvIndices;

        public override string ToString()
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("vertex index : ");
            stringBuilder.Append(vertIndices.ToString());
            stringBuilder.Append("\nuv index: ");
            stringBuilder.Append(uvIndices.ToString());
            stringBuilder.Append("\nnormal index: ");
            stringBuilder.Append(normalIndices.ToString());
            return stringBuilder.ToString();
        }
    }
}
