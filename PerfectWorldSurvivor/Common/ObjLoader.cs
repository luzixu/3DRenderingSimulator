using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using PerfectWorldSurvivor.Model;
using PerfectWorldSurvivor.Utils;

namespace PerfectWorldSurvivor.Common
{
    public class ObjLoader
    {
        public static Mesh LoadMesh(string objFilePath)
        {
            _Reset();
            if (StringUtils.IsNullOrEmpty(objFilePath))
            {
                return null;
            }
            _LoadModelData(objFilePath);
            if (_finishLoading)
            {
                return _CreateMesh();
            }
            return null;
        }

        private static void _LoadModelData(string objFilePath)
        {
            String line;
            char firstChar;
            FileStream fileStream = null;
            StreamReader streamReader = null;
            try
            {
                fileStream = new FileStream(objFilePath, FileMode.Open);
                streamReader = new StreamReader(fileStream);

                List<CharBuffer> charBuffers = new List<CharBuffer>();

                while ((line = streamReader.ReadLine()) != null)
                {
                    if (line.Length < 1)
                    {
                        continue;
                    }
                    charBuffers.Clear();
                    CharBuffer.Parse(line.ToCharArray(0, line.Length), 0, line.Length - 1, charBuffers);

                    if (charBuffers == null || charBuffers.Count <= 1)
                    {
                        continue;
                    }
                    firstChar = charBuffers[0].GetIndex(0);
                    if (firstChar == '#')
                    {
                        continue;
                    }
                    else if (firstChar == 'v')
                    {
                        CharBuffer bufferToken = charBuffers[0];
                        if (bufferToken.Length() == 1)
                        {
                            _verts.Add(NumberConverter.ConvertPerfectCharBufferToFloat(charBuffers[1]));
                            _verts.Add(NumberConverter.ConvertPerfectCharBufferToFloat(charBuffers[2]));
                            _verts.Add(NumberConverter.ConvertPerfectCharBufferToFloat(charBuffers[3]));
                        }
                        else if (bufferToken.GetIndex(1) == 'n')
                        {
                            _norms.Add(NumberConverter.ConvertPerfectCharBufferToFloat(charBuffers[1]));
                            _norms.Add(NumberConverter.ConvertPerfectCharBufferToFloat(charBuffers[2]));
                            _norms.Add(NumberConverter.ConvertPerfectCharBufferToFloat(charBuffers[3]));
                        }
                        else if (bufferToken.GetIndex(1) == 't')
                        {
                            float first = NumberConverter.ConvertPerfectCharBufferToFloat(charBuffers[1]);
                            float second = NumberConverter.ConvertPerfectCharBufferToFloat(charBuffers[2]);
                            _uvs.Add(first);
                            _uvs.Add(_flipUV ? 1 - second : second);
                        }
                    }
                    else if (firstChar == 'f')
                    {
                        int end = charBuffers.Count - 2;
                        for (int i = 1; i < end; i--)
                        {
                            CharBuffer toke1 = charBuffers[1];
                            List<CharBuffer> parts = CharBuffer.Split('/', toke1.charArray, toke1.startPosition, toke1.endPosition);
                            Triangle triangle = new Triangle();
                            triangle.x = _GetIndex(parts[0], _verts.Count);
                            triangle.y = _GetIndex(parts[1], _uvs.Count);
                            triangle.z = _GetIndex(parts[2], _norms.Count);
                            _triangles.Add(triangle);
                            CharBuffer tmp = charBuffers[++i];
                            parts = CharBuffer.Split('/', tmp.charArray, tmp.startPosition, tmp.endPosition);
                            triangle = new Triangle();
                            triangle.x = _GetIndex(parts[0], _verts.Count);
                            triangle.y = _GetIndex(parts[1], _uvs.Count);
                            triangle.z = _GetIndex(parts[2], _norms.Count);
                            _triangles.Add(triangle);
                            tmp = charBuffers[++i];
                            parts = CharBuffer.Split('/', tmp.charArray, tmp.startPosition, tmp.endPosition);
                            triangle = new Triangle();
                            triangle.x = _GetIndex(parts[0], _verts.Count);
                            triangle.y = _GetIndex(parts[1], _uvs.Count);
                            triangle.z = _GetIndex(parts[2], _norms.Count);
                            _triangles.Add(triangle);
                        }
                    }
                }
                _finishLoading = true;
            }
            catch (Exception exception)
            {
                Logger.Log(exception.ToString());
            }
            finally
            {
                if (fileStream != null)
                {
                    fileStream.Close();
                }

                if (streamReader != null)
                {
                    streamReader.Close();
                }
            }
        }

        private static Mesh _CreateMesh()
        {
            int vertexCount = _verts.Count / 3;
           
            Mesh result = new Mesh(vertexCount);
            Vector3f[] vertices = new Vector3f[vertexCount];
            for (int i = 0; i < vertexCount; i++)
            {
                int index = i * 3;
                vertices[i] = new Vector3f(_verts[index], _verts[index + 1], _verts[index + 2]);
            }
            result.vertices = vertices;
            int normalCount = _norms.Count / 3;
            Vector3f[] norms = new Vector3f[normalCount];
            for (int i = 0; i < normalCount; i++)
            {
                int index = i * 3;
                norms[i] = new Vector3f(_norms[index], _norms[index + 1], _norms[index + 2]);
            }
            result.normals = norms;
            int uvCount = _uvs.Count / 2;
            Vector2f[] uvs = new Vector2f[uvCount];
            for (int i = 0; i < uvCount; i++)
            {
                int index = i * 2;
                uvs[i] = new Vector2f(_uvs[index], _uvs[index + 1]);
            }
            result.uv1 = uvs;
            int faceCount = _triangles.Count / 3;
            Face[] faces = new Face[faceCount];
            for (int i = 0; i < faceCount; i++)
            {
                int index = i * 3;
                Face face = new Face();
                Triangle t1 = _triangles[index];
                Triangle t2 = _triangles[index + 1];
                Triangle t3 = _triangles[index + 2];
                face.vertIndices = new Vector3i(t1.x, t2.x, t3.x);
                face.uvIndices = new Vector3i(t1.y, t2.y, t3.y);
                face.normalIndices = new Vector3i(t1.z, t2.z, t3.z);
                faces[i] = face;
            }
            result.faces = faces;
            return result;
        }

        private static int _GetIndex(CharBuffer charBuffer, int size)
        {
            if (charBuffer.Length() == 0)
                return 0;
            int idx = NumberConverter.ConvertCharArrayToInt(charBuffer.charArray, charBuffer.startPosition, charBuffer.endPosition);
            if (idx < 0)
            {
                return size + idx;
            }
            return idx - 1;
        }

        private static void _Reset()
        {
            _verts.Clear();
            _norms.Clear();
            _uvs.Clear();
            _triangles.Clear();
            _flipUV = true;
            _finishLoading = false;
        }

        private static List<float> _verts = new List<float>();

        private static List<float> _norms = new List<float>();

        private static List<float> _uvs = new List<float>();

        private static List<Triangle> _triangles = new List<Triangle>();

        private static bool _flipUV = true;

        private static bool _finishLoading = false;
    }
}
