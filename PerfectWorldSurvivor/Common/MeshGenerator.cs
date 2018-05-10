
namespace PerfectWorldSurvivor.Model
{
    public static class MeshGenerator
    {
        public static Mesh CreateLineCube()
        {
            Mesh mesh = _CreateCube();
            Colorf[] vertexColors = new Colorf[mesh.faces.Length];
            Colorf color = new Colorf(0.5f, 0.4f, 0.1f, 1);
            vertexColors[0] = Colorf.GREEN;
            vertexColors[1] = Colorf.RED;
            vertexColors[2] = Colorf.PINK;
            vertexColors[3] = Colorf.YELLOW;
            vertexColors[4] = Colorf.GREEN;
            vertexColors[5] = Colorf.RED;
            vertexColors[6] = Colorf.ORANGE;
            vertexColors[7] = Colorf.WHITE;
            vertexColors[8] = Colorf.RED;
            vertexColors[9] = Colorf.YELLOW;
            vertexColors[10] = Colorf.WHITE;
            vertexColors[11] = Colorf.ORANGE;
            mesh.colors = vertexColors;
            return mesh;
        }

        public static Mesh CreateFaceCube()
        {
            Mesh mesh = _CreateCube();
            Colorf[] vertexColors = new Colorf[mesh.faces.Length];
            vertexColors[0] = Colorf.RED;
            vertexColors[1] = Colorf.RED;
            vertexColors[2] = Colorf.BLUE;
            vertexColors[3] = Colorf.BLUE;
            vertexColors[4] = Colorf.YELLOW;
            vertexColors[5] = Colorf.YELLOW;
            vertexColors[6] = Colorf.GRAY;
            vertexColors[7] = Colorf.GRAY;
            vertexColors[8] = Colorf.ORANGE;
            vertexColors[9] = Colorf.ORANGE;
            vertexColors[10] = Colorf.GREEN;
            vertexColors[11] = Colorf.GREEN;
            mesh.colors = vertexColors;
            return mesh;
        }

        private static Mesh _CreateCube()
        {
            int vertexCount = 8;
            Mesh mesh = new Mesh(vertexCount);
            float plane = 1;
            Vector3f[] vertices = mesh.vertices;
            vertices[0] = new Vector3f(-plane, 1, plane);
            vertices[1] = new Vector3f(plane, 1, plane);
            vertices[2] = new Vector3f(-plane, -1, plane);
            vertices[3] = new Vector3f(plane, -1, plane);
            vertices[4] = new Vector3f(-plane, 1, -plane);
            vertices[5] = new Vector3f(plane, 1, -plane);
            vertices[6] = new Vector3f(plane, -1, -plane);
            vertices[7] = new Vector3f(-plane, -1, -plane);
            Face[] faces = new Face[]
            {
                new Face(),
                new Face(),
                new Face(),
                new Face(),
                new Face(),
                new Face(),
                new Face(),
                new Face(),
                new Face(),
                new Face(),
                new Face(),
                new Face()
            };
            faces[0].vertIndices = new Vector3i(0, 2, 1);
            faces[1].vertIndices = new Vector3i(2, 3, 1);
            faces[2].vertIndices = new Vector3i(1, 3, 6);
            faces[3].vertIndices = new Vector3i(1, 6, 5);
            faces[4].vertIndices = new Vector3i(0, 1, 4);
            faces[5].vertIndices = new Vector3i(1, 5, 4);
            faces[6].vertIndices = new Vector3i(2, 7, 3);
            faces[7].vertIndices = new Vector3i(3, 7, 6);
            faces[8].vertIndices = new Vector3i(0, 7, 2);
            faces[9].vertIndices = new Vector3i(0, 4, 7);
            faces[10].vertIndices = new Vector3i(4, 5, 6);
            faces[11].vertIndices = new Vector3i(4, 6, 7);
            mesh.faces = faces;
            mesh.CalNormals();
            return mesh;
        }
    }
}
