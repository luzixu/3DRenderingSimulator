using System.Drawing;
using System;
using System.Collections.Generic;
using PerfectWorldSurvivor.Model;
using PerfectWorldSurvivor.UI;
using PerfectWorldSurvivor.Common;
using PerfectWorldSurvivor.Utils;

namespace PerfectWorldSurvivor.Draw
{
    public class Batch : IDisposable
    {
        public Batch(Bitmap bitmap, World world)
        {
            if (bitmap == null || world == null)
            {
                throw new ArgumentException("Null value in Batch constructor!");
            }
            _world = world;
            _rasterization = new Rasterization(bitmap);
            _clipTriangle = new ClipTriangle();
        }

        public void Render()
        {
            _ResetRenderDisplayRecord();
            _Prepare();
            int len = _world.GameObjectCount;
            for (int i = 0; i < len; i++)
            {
                GameObject obj = _world.GetGameObject(i);
                if (obj != null)
                {
                    _DrawGameObject(obj);
                }
            }
            _rasterization.End();
            _SetRenderDisplayRecord();
        }

        public void Dispose()
        {
            _rasterization.Dispose();
        }

        private void _ResetRenderDisplayRecord()
        {
            _countBackCullingTriangles = 0;
            _countCullingObjects = 0;
            _renderTriangles = 0;
        }
        private void _Prepare()
        {
            _world.Update();
            _cachedCamera = _world.GetMainCamera();
            _clipTriangle.SetCamera(_cachedCamera);
            _clipTriangle.Update();

            _rasterization.Begin();
            _rasterization.Clear(Colorf.BLACK);
        }
        private void _DrawGameObject(GameObject obj)
        {
            Mesh mesh = obj.GetMesh();
            if (mesh == null)
            {
                return;
            }
            Matrix4x4 rotation = obj.Trans.MatrixRotation;
            Matrix4x4 meshWorldMatrix = rotation * (new Matrix4x4().SetToTranslation(obj.Trans.position));
            if (_CheckFrustumCulling(obj, ref meshWorldMatrix))
            {
                return;
            }
            Matrix4x4 mvMatrix = _cachedCamera.View * meshWorldMatrix;
            Matrix4x4 mvpMatrixInverse = _cachedCamera.Projection * mvMatrix;
            int verticesCount = mesh.VerticesCount;
            Vector3f[] vertices = mesh.vertices;
            Vector3f[] vertexNormals = mesh.normals;
            Colorf[] vertexColors = mesh.colors;
            Vector2f[] uvs = mesh.uv1;
            Face[] faces = mesh.faces;
            int faceLen = faces.Length;
            Texture texture = obj.GetTexture();
            LightsContainer lightsContainer = LightsContainer.GetInstance();
            bool lightsOn = lightsContainer.lightOn;
            for (int i = 0; i < faceLen; i++)
            {
                Face face = faces[i];
                VertexShaderInputWithText vertexXInput = new VertexShaderInputWithText();
                VertexShaderInputWithText vertexYInput = new VertexShaderInputWithText();
                VertexShaderInputWithText vertexZInput = new VertexShaderInputWithText();
                VertexShaderOutput vertexXOutput = new VertexShaderOutput();
                VertexShaderOutput vertexYOutput = new VertexShaderOutput();
                VertexShaderOutput vertexZOutput = new VertexShaderOutput();
                _ComposeVertexOutput(ref vertexXOutput, ref vertexYOutput, ref vertexZOutput, vertexColors, uvs, ref face);
                _ComposeVertexInput(ref vertexXInput, ref vertexYInput, ref vertexZInput, vertexNormals, vertices, ref face);
                //Project world point to view space
                _cachedCamera.ProjectToViewSpace(ref vertexXInput, ref vertexXOutput, ref mvMatrix);
                _cachedCamera.ProjectToViewSpace(ref vertexYInput, ref vertexYOutput, ref mvMatrix);
                _cachedCamera.ProjectToViewSpace(ref vertexZInput, ref vertexZOutput, ref mvMatrix);
                // Back face culling
                if (_backCulling && _CheckBackface(ref vertexXOutput, ref vertexYOutput, ref vertexZOutput))
                {
                    continue;
                }
                //Project local point to world space
                _cachedCamera.CalculateWorldData(ref vertexXInput, ref vertexXOutput, ref meshWorldMatrix);
                _cachedCamera.CalculateWorldData(ref vertexYInput, ref vertexYOutput, ref meshWorldMatrix);
                _cachedCamera.CalculateWorldData(ref vertexZInput, ref vertexZOutput, ref meshWorldMatrix);
                if (lightsOn)
                {
                    _CaculateVertexLightsColor(ref vertexXOutput, lightsContainer, lightsContainer.specular, lightsContainer.gross);
                }
                _clipTriangle.ClipInViewSpace(ref vertexXOutput, ref vertexYOutput, ref vertexZOutput,_cachedTriangles);
                while (_cachedTriangles.Count > 0)
                {
                    VertexShaderOutput out1 = _cachedTriangles.Dequeue();
                    VertexShaderOutput out2 = _cachedTriangles.Dequeue();
                    VertexShaderOutput out3 = _cachedTriangles.Dequeue();
                    _DrawTriangles(ref out1, ref out2, ref out3, texture);
                }
                _cachedTriangles.Clear();
            }
        }

        //Frustum Culling.
        //Cull object not in frustum
        private bool _CheckFrustumCulling(GameObject obj, ref Matrix4x4 meshWorldMatrix)
        {
            if (_cachedCamera.enableFrustumCulling)
            {
                obj.Bound.Update(ref meshWorldMatrix);
                bool inFrustum = _cachedCamera.CheckInFrustum(obj.Bound);
                if (!inFrustum)   //Discard
                {
                    _countCullingObjects++;
                    return true;
                }
            }
            return false;
        }
        private void _ComposeVertexInput(ref VertexShaderInputWithText vertexXInput, ref VertexShaderInputWithText vertexYInput, ref VertexShaderInputWithText vertexZInput, Vector3f[] vertexNormals, Vector3f[] vertices, ref Face face)
        {
            if (vertices != null && vertexNormals != null)
            {
                Vector3f vertexX = vertices[face.vertIndices.x];
                Vector3f vertexY = vertices[face.vertIndices.y];
                Vector3f vertexZ = vertices[face.vertIndices.z];
                vertexXInput.position = vertexX;
                vertexXInput.normal = vertexNormals[face.normalIndices.x];
                vertexYInput.position = vertexY;
                vertexYInput.normal = vertexNormals[face.normalIndices.y];
                vertexZInput.position = vertexZ;
                vertexZInput.normal = vertexNormals[face.normalIndices.z];
            }
        }

        private void _ComposeVertexOutput(ref VertexShaderOutput vertexXOutput, ref VertexShaderOutput vertexYOutput, ref VertexShaderOutput vertexZOutput, Colorf[] vertexColors, Vector2f[] uvs, ref Face face)
        {
            if (DisplayEngine.displayType == DisplayEngine.displayTypeSimpleTriangle)
            {
                vertexXOutput.color = vertexColors[face.uvIndices.x];
                vertexYOutput.color = vertexColors[face.uvIndices.y];
                vertexZOutput.color = vertexColors[face.uvIndices.z];
            }
            if (uvs != null)
            {
                //Get uv
                vertexXOutput.uv = uvs[face.uvIndices.x];
                vertexYOutput.uv = uvs[face.uvIndices.y];
                vertexZOutput.uv = uvs[face.uvIndices.z];
            }
        }
        private void _SetRenderDisplayRecord()
        {
            MainWin.triangles = _renderTriangles;
            MainWin.cullingObjects = _countCullingObjects;
            MainWin.backfaceCulling = _countBackCullingTriangles;
        }

        private void _DrawTriangles(ref VertexShaderOutput vertexXOutput, ref VertexShaderOutput vertexYOutput, ref VertexShaderOutput vertexZOutput, Texture texture)
        {
            _renderTriangles++;
            //Project view space to projection space
            Matrix4x4 cameraProjection = _cachedCamera.Projection;
            _cachedCamera.ProjectToProjectionSpace(ref vertexXOutput, ref cameraProjection);
            _cachedCamera.ProjectToProjectionSpace(ref vertexYOutput, ref cameraProjection);
            _cachedCamera.ProjectToProjectionSpace(ref vertexZOutput, ref cameraProjection);
            //Calculate uv
            vertexXOutput.uv *= vertexXOutput.reciprocalZ;
            vertexYOutput.uv *= vertexYOutput.reciprocalZ;
            vertexZOutput.uv *= vertexZOutput.reciprocalZ;
            //Pre multi-z
            _cachedCamera.TransformViewFrustumToScreen(ref vertexXOutput);
            _cachedCamera.TransformViewFrustumToScreen(ref vertexYOutput);
            _cachedCamera.TransformViewFrustumToScreen(ref vertexZOutput);
            Vector3f cameraPosition =  _cachedCamera.Trans.position;
            if (DisplayEngine.displayType == DisplayEngine.displayTypeSimpleTriangle)
            {
                _rasterization.DrawLine(ref vertexXOutput, ref vertexYOutput, cameraPosition);
                _rasterization.DrawLine(ref vertexYOutput, ref vertexZOutput, cameraPosition);
                _rasterization.DrawLine(ref vertexXOutput, ref vertexZOutput, cameraPosition);
            }
            else
            {
                if (MainWin.lineMode)
                {
                    vertexXOutput.color = _defaultLineColor;
                    vertexYOutput.color = _defaultLineColor;
                    vertexZOutput.color = _defaultLineColor;
                    _rasterization.DrawLine(ref vertexXOutput, ref vertexYOutput, cameraPosition);
                    _rasterization.DrawLine(ref vertexYOutput, ref vertexZOutput, cameraPosition);
                    _rasterization.DrawLine(ref vertexXOutput, ref vertexZOutput, cameraPosition);
                }
                else
                {
                    _rasterization.DrawTriangle(ref vertexXOutput, ref vertexYOutput, ref vertexZOutput, _cachedCamera.Trans.position, texture);
                }
            }
        }

        private bool _CheckBackface(ref VertexShaderOutput v1, ref VertexShaderOutput v2, ref VertexShaderOutput v3)
        {
            Vector3f p1 = v1.svPosition.ToXYZ();
            Vector3f p2 = v2.svPosition.ToXYZ();
            Vector3f p3 = v3.svPosition.ToXYZ();
            Vector3f d1 = p2 - p1;
            Vector3f d2 = p3 - p2;
            Vector3f normal = d1.Cross(d2);
            Vector3f viewDir = p1 - new Vector3f(0, 0, 0);
            if (normal.Dot(viewDir) > 0)
            {
                _countBackCullingTriangles++;
                return true;
            }
            return false;
        }

        private void _CaculateVertexLightsColor(ref VertexShaderOutput vertexXOutput, LightsContainer lightsContainer, Colorf specular, float gross)
        {
            int pointLightsLen = lightsContainer.GetLightsCount();
            Colorf color = new Colorf();
            for (int lightIndex = 0; lightIndex < pointLightsLen; lightIndex++)
            {
                float attenuation = 1;
                //Calculate Blinn-Phong
                PointLight pointLight = lightsContainer.GetLight(lightIndex);
                if (pointLight == null)
                {
                    continue;
                }
                Vector3f lightPosition = pointLight.Trans.position;
                Vector3f worldLightDir;
                if (pointLight.perPixel)
                {
                    continue;
                }
                if (pointLight.lightType == PointLight.Type.Point)
                {
                    Vector3f wl = vertexXOutput.worldPosition - lightPosition;
                    worldLightDir = wl.Normalized;
                }
                else
                {
                    worldLightDir = lightPosition.Normalized;
                }
                Vector3f reflectDir = LightsMath.ComputeReflect(-worldLightDir, vertexXOutput.worldNormal);
                Vector3f viewDir = (_cachedCamera.Trans.position - vertexXOutput.worldPosition).Normalized;
                Vector3f halfDir = (worldLightDir + viewDir).Normalized;
                Colorf specularColor = pointLight.color * specular * MathUtils.Pow(MathUtils.Clamp(reflectDir.Dot(viewDir)), LightsContainer.GetInstance().gross) * attenuation;
                color += specularColor;
            }
            vertexXOutput.fragColor += color;
        }
      
        private Queue<VertexShaderOutput> _cachedTriangles = new Queue<VertexShaderOutput>();

        private static readonly Colorf _defaultLineColor = Colorf.WHITE;

        private World _world;

        private Camera _cachedCamera;

        private bool _backCulling = true;

        private Rasterization _rasterization;

        private int _countBackCullingTriangles = 0;

        private int _countCullingObjects = 0;

        private int _renderTriangles = 0;

        private ClipTriangle _clipTriangle;

    }
}
