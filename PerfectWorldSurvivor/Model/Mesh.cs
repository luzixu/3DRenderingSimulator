using System.Text;

namespace PerfectWorldSurvivor.Model
{
    public class Mesh
    {
        public Mesh(int verticesCount)
        {
            this._verticesCount = verticesCount;
            vertices = new Vector3f[verticesCount];
            colors = new Colorf[verticesCount];
        }
        public void CalNormals()
        {
            Vector3f[] trianglesNormals;
            int facesLen = faces.Length;
            trianglesNormals = new Vector3f[facesLen];
            normals = new Vector3f[VerticesCount];
            //First,calculate each triangle's normal.
            for (int i = 0; i < facesLen; i++)
            {
                Face face = faces[i];
                Vector3f p1 = vertices[face.vertIndices.x];
                Vector3f p2 = vertices[face.vertIndices.y];
                Vector3f p3 = vertices[face.vertIndices.z];
                Vector3f dir1 = p1 - p2;
                Vector3f dir2 = p2 - p3;
                trianglesNormals[i] = dir2.Cross(dir1);
            }
            normals = trianglesNormals;
            for (int i = 0; i < facesLen; i++)
            {
                Face face = faces[i];
                face.normalIndices = new Vector3i(i, i, i);
                face.uvIndices = face.vertIndices;
                faces[i] = face;
            }
        }

        public override string ToString()
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("vertex:\n");
            for (int i = 0; i < vertices.Length; i++)
            {
                stringBuilder.Append(vertices[i].ToString());
                stringBuilder.Append("\n");
            }
            stringBuilder.Append("uv:\n");
            for (int i = 0; i < uv1.Length; i++)
            {
                stringBuilder.Append(uv1[i].ToString());
                stringBuilder.Append("\n");
            }
            stringBuilder.Append("normal:\n");
            for (int i = 0; i < normals.Length; i++)
            {
                stringBuilder.Append(normals[i].ToString());
                stringBuilder.Append("\n");
            }
            stringBuilder.Append("faces:\n");
            for (int i = 0; i < faces.Length; i++)
            {
                stringBuilder.Append(faces[i].ToString());
                stringBuilder.Append("\n");
            }
            return stringBuilder.ToString();
        }

        public Vector3f[] vertices;

        public Vector2f[] uv1;

        public Face[] faces;

        public Vector3f[] normals;

        public Vector2f[] uv2;

        public Vector2f[] uv3;

        public Colorf[] colors;
        public int VerticesCount { get { return _verticesCount; } }

        private int _verticesCount;
    }
}
