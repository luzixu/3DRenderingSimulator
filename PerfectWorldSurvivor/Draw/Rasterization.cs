using PerfectWorldSurvivor.Common;
using PerfectWorldSurvivor.Model;
using PerfectWorldSurvivor.Utils;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;

namespace PerfectWorldSurvivor.Draw
{
    public class Rasterization : IDisposable
    {
        public Rasterization(Bitmap canvas)
        {
            if (canvas == null)
            {
                throw new ArgumentException("canvas should not be null in class Rasterization");
            }

            _bitmap = canvas;
            _width = canvas.Width;
            _height = canvas.Height;
            _bufferSize = _width * _height;
            _depthBuffer = new float[_bufferSize];
            _beginCompleted = false;
        }
        public void Begin()
        {
            LightsContainer lightsContainer = LightsContainer.GetInstance();
            _ambient = lightsContainer.ambient;
            _specular = lightsContainer.specular;
            _gross = lightsContainer.gross;
            _lightsOn = lightsContainer.lightOn;
            try
            {
                _canvasData = _bitmap.LockBits(new Rectangle(0, 0, _width, _height), ImageLockMode.WriteOnly, PixelFormat.Format32bppRgb);
                _stride = _canvasData.Stride;
                _beginCompleted = true;
            }
            catch (Exception exception)
            {
                Logger.Log("Failed to lock bits in Rasterization.Exception: " + exception.ToString());
                _beginCompleted = false;
                _canvasData = null;
            }
        }

        public void End()
        {
            if (_beginCompleted && null != _bitmap)
            {
                _bitmap.UnlockBits(_canvasData);
            }
            _endCompleted = true;
        }

        public void Clear(Colorf color)
        {
            //Clear color-buffer
            unsafe
            {
                byte* start = (byte*)_canvasData.Scan0;
                for (int i = 0; i < _width; i++)
                {
                    for (int j = 0; j < _height; j++)
                    {
                        byte* b = start + j * _stride + i * 4;
                        _SetColorbyte(b, color);
                    }
                }
            }
            //Clear z-buffer
            for (int index = 0; index < _depthBuffer.Length; index++)
            {
                _depthBuffer[index] = float.MinValue;
            }
        }
       
        public void DrawTriangle(ref VertexShaderOutput v1, ref VertexShaderOutput v2, ref VertexShaderOutput v3, Vector3f cameraWorldPos, Texture texture)
        {
            _texture = texture;
            v1.svPosition.z = v1.reciprocalZ;
            v2.svPosition.z = v2.reciprocalZ;
            v3.svPosition.z = v3.reciprocalZ;
            _SwapTriangle(ref v1, ref v2, ref v3);
            if (_IsTriangleOutofView(ref v1, ref v2, ref v3))
            {
                return;
            }
            float dv1v2;
            float dv1v3;
            if (v2.svPosition.y - v1.svPosition.y > 0)
            {
                dv1v2 = (v2.svPosition.x - v1.svPosition.x) / (v2.svPosition.y - v1.svPosition.y);
            }
            else
            {
                dv1v2 = 0;
            }
            if (v3.svPosition.y - v1.svPosition.y > 0)
            {
                dv1v3 = (v3.svPosition.x - v1.svPosition.x) / (v3.svPosition.y - v1.svPosition.y);
            }
            else
            {
                dv1v3 = 0;
            }
            if (dv1v2 == 0)
            {
                if (_DrawV1V2(ref v1, ref v2, ref v3, cameraWorldPos))
                {
                    return;
                }
            }
            if (dv1v2 > dv1v3)
            {
                _DrawV2Right(ref v1, ref v2, ref v3, cameraWorldPos);
            }
            else
            {
                _DrawV2Left(ref v1, ref v2, ref v3, cameraWorldPos);
            }
        }
        public void DrawLine(ref VertexShaderOutput v1, ref VertexShaderOutput v2, Vector3f cameraPos)
        {
            int x0 = (int)v1.svPosition.x;
            int x1 = (int)v2.svPosition.x;
            int y0 = (int)v1.svPosition.y;
            int y1 = (int)v2.svPosition.y;
            int dx = Math.Abs(x1 - x0);
            int dy = Math.Abs(y1 - y0);
            int sx = (x0 < x1) ? 1 : -1;
            int sy = (y0 < y1) ? 1 : -1;
            int err = dx - dy;
            int startX = x0,startY = y0,endX = x1;
            float startZ = v1.svPosition.z;
            float endZ = v2.svPosition.z;
            float dz = endZ - startZ;
            bool useX = dx > dy;
            Colorf startColor = v1.color, endColor = v2.color;
            Colorf startLightFragColor = v1.fragColor, endLightFragColor = v2.fragColor;
            float dxf = x1 - x0;
            float dyf = y1 - y0;
            if (MathUtils.IsZero(dxf))
            {
                dxf = 1;
            }
            if (MathUtils.IsZero(dyf))
            {
                dyf = 1;
            }
            while (true)
            {
                float t = 1;
                if (useX)
                {
                    t = (x0 - startX) / dxf;
                }
                else
                {
                    t = (y0 - startY) / dyf;
                }
                Colorf color = MathUtils.Interpolate(t, startColor, endColor);
                if (_lightsOn)
                {
                    color +=_CalculateLineLights(t, ref v1, ref v2, ref startLightFragColor, ref endLightFragColor, ref cameraPos);
                }
                color = _ClampColor(color);
                float z = dz * t + startZ;
                if (x0 >= 0 && x0 < _width && y0 >= 0 && y0 < _height)
                {
                    _Draw(x0, y0, z, ref color);
                }
                if ((x0 == x1) && (y0 == y1))
                {
                    break;
                }
                int e2 = 2 * err;
                if (e2 > -dy)
                {
                    err -= dy;
                    x0 += sx;
                }
                if (e2 < dx)
                {
                    err += dx;
                    y0 += sy;
                }
            }
        }

        public void Dispose()
        {
            if (_beginCompleted && !_endCompleted)
            {
                End();
            }

            if (_bitmap != null)
            {
                _bitmap.Dispose();
            }
        }

        //V1- - - - V2        V1 -
        //    -  -               - -
        //    - -       or       - - -
        //     -              V2 - - - -V3
        //     V3                 
        private bool _DrawV1V2(ref VertexShaderOutput v1, ref VertexShaderOutput v2, ref VertexShaderOutput v3, Vector3f cameraWorldPos)
        {
            float v1x = v1.svPosition.x;
            float v2x = v2.svPosition.x;
            float v1y = v1.svPosition.y;
            float v2y = v2.svPosition.y;
            int startY = (int)v1.svPosition.y;
            int endY = (int)v3.svPosition.y;
            startY = startY > 0 ? startY : 0;
            endY = Math.Min(endY, _height - 1);
            if (v1y == v2y)
            {
                if (v2x < v1x)
                {
                    VertexShaderOutput tmp = v1;
                    v1 = v2;
                    v2 = tmp;
                }
                for (int y = startY; y <= endY; y++)
                {
                    if (y < v2.svPosition.y)
                    {
                        _FillLine(y, ref v1, ref v3, ref v1, ref v2, ref cameraWorldPos);
                    }
                    else
                    {
                        _FillLine(y, ref v1, ref v3, ref v2, ref v3, ref cameraWorldPos);
                    }
                }
                return true;
            }
            else if ((v1x == v2x) && (v2y > v1y))
            {
                if (v3.svPosition.x > v1x)
                {
                    for (int y = startY; y <= endY; y++)
                    {
                        if (y < v2.svPosition.y)
                        {
                          
                            _FillLine(y, ref v1, ref v2, ref v1, ref v3, ref cameraWorldPos);
                        }
                        else
                        {
                           
                            _FillLine(y, ref v2, ref v3, ref v1, ref v3, ref cameraWorldPos);
                        }
                    }
                    return true;
                }
                else
                {
                    for (int y = startY; y <= endY; y++)
                    {
                        if (y < v2.svPosition.y)
                        {
                            _FillLine(y, ref v1, ref v3, ref v1, ref v2, ref cameraWorldPos);
                        }
                        else
                        {
                            _FillLine(y, ref v1, ref v3, ref v2, ref v3, ref cameraWorldPos);
                        }
                    }
                    return true;
                }
            }
            return false;
        }
        //       V1
        //        -
        //       -- 
        //      - -
        //     -  -
        // V2 -   - 
        //     -  -
        //      - -
        //        -
        //       V3
        private void _DrawV2Left(ref VertexShaderOutput v1, ref VertexShaderOutput v2, ref VertexShaderOutput v3, Vector3f cameraWorldPos)
        {
            int startY = (int)v1.svPosition.y;
            int endY = (int)v3.svPosition.y;
            startY = startY > 0 ? startY : 0;
            endY = Math.Min(endY, _height - 1);
            for (int y = startY; y <= endY; y++)
            {
                if (y < v2.svPosition.y)
                {
                    _FillLine(y, ref v1, ref v2, ref v1, ref v3, ref cameraWorldPos);
                }
                else
                {
                    _FillLine(y, ref v2, ref v3, ref v1, ref v3, ref cameraWorldPos);
                }
            }
        }

        // V1
        // -
        // -- 
        // - -
        // -  -
        // -   - V2
        // -  -
        // - -
        // -
        // V3
        private void _DrawV2Right(ref VertexShaderOutput v1, ref VertexShaderOutput v2, ref VertexShaderOutput v3, Vector3f cameraWorldPos)
        {
            int startY = (int)v1.svPosition.y;
            int endY = (int)v3.svPosition.y;
            startY = startY > 0 ? startY : 0;
            endY = Math.Min(endY, _height - 1);
            for (int y = startY; y <= endY; y++)
            {
                if (y < v2.svPosition.y)
                {
                    _FillLine(y, ref v1, ref v3, ref v1, ref v2, ref cameraWorldPos);
                }
                else
                {
                    _FillLine(y, ref v1, ref v3, ref v2, ref v3, ref cameraWorldPos);
                }
            }
        }

        private void _FillLine(int y, ref VertexShaderOutput v1, ref VertexShaderOutput v2, ref VertexShaderOutput v3, ref VertexShaderOutput v4, ref Vector3f cameraWorldPos)
        {
            Vector4f pa = v1.svPosition;
            Vector4f pb = v2.svPosition;
            Vector4f pc = v3.svPosition;
            Vector4f pd = v4.svPosition;
            float gradient1 = pa.y != pb.y ? (y - pa.y) / (pb.y - pa.y) : 1;
            float gradient2 = pc.y != pd.y ? (y - pc.y) / (pd.y - pc.y) : 1;
            int sx = (int)MathUtils.Interpolate(gradient1, pa.x, pb.x);
            int ex = (int)MathUtils.Interpolate(gradient2, pc.x, pd.x);
            float z1 = MathUtils.Interpolate(gradient1, v1.reciprocalZ, v2.reciprocalZ);
            float z2 = MathUtils.Interpolate(gradient2, v3.reciprocalZ, v4.reciprocalZ);
            Vector3f sWorldNormal = MathUtils.Interpolate(gradient1, v1.worldNormal, v2.worldNormal);
            Vector3f eWorldNormal = MathUtils.Interpolate(gradient2, v3.worldNormal, v4.worldNormal);
            Vector3f sWorldPosition = MathUtils.Interpolate(gradient1, v1.worldPosition, v2.worldPosition);
            Vector3f eWorldPosition = MathUtils.Interpolate(gradient2, v3.worldPosition, v4.worldPosition);
            float su = MathUtils.Interpolate(gradient1, v1.uv.x, v2.uv.x);
            float eu = MathUtils.Interpolate(gradient2, v3.uv.x, v4.uv.x);
            float sv = MathUtils.Interpolate(gradient1, v1.uv.y, v2.uv.y);
            float ev = MathUtils.Interpolate(gradient2, v3.uv.y, v4.uv.y);
            int des = ex - sx;
            if(des == 0) 
            {
                des = 1;
            }
            float sxexReciprocal = 1 / ((float)des);
            for (int x = Math.Max(0, sx); x < ex && x < _width; x++)
            {
                float gradient = (x - sx) * sxexReciprocal;
                float z = MathUtils.Interpolate(gradient, z1, z2);
                Colorf color = Colorf.WHITE;
                if (_texture != null)
                {
                    float u = MathUtils.Interpolate(gradient, su, eu);
                    float v = MathUtils.Interpolate(gradient, sv, ev);
                    if (MathUtils.IsZero(z))
                    {
                        z = 1;
                    }
                    float w = 1 / z;
                    color = _texture.Map(u * w, v * w);
                }
                if (_lightsOn)
                {
                    color +=_CaculateLights(gradient, ref sWorldNormal, ref eWorldNormal, ref sWorldPosition, ref eWorldPosition, ref v1, ref v2, ref cameraWorldPos);
                }
                color = _ClampColor(color);
                _Draw(x, y, z, ref color);
            }
        }

        private void _SwapTriangle(ref VertexShaderOutput v1, ref VertexShaderOutput v2, ref VertexShaderOutput v3)
        {
            if (v1.svPosition.y > v2.svPosition.y)
            {
                VertexShaderOutput tmp = v1;
                v1 = v2;
                v2 = tmp;
            }

            if (v2.svPosition.y > v3.svPosition.y)
            {
                VertexShaderOutput tmp = v2;
                v2 = v3;
                v3 = tmp;
            }

            if (v1.svPosition.y > v2.svPosition.y)
            {
                VertexShaderOutput tmp = v1;
                v1 = v2;
                v2 = tmp;
            }
        }

        private Colorf _CalculateLineLights(float t, ref VertexShaderOutput v1, ref VertexShaderOutput v2, ref Colorf startLightFragColor, ref Colorf endLightFragColor, ref Vector3f cameraPos)
        {
            Vector3f worldPosition = MathUtils.Interpolate(t, v1.worldPosition, v2.worldPosition);
            Vector3f worldNormal = MathUtils.Interpolate(t, v1.worldNormal, v2.worldNormal).Normalized;
            Colorf lightColor = _ambient;
            LightsContainer lightsContainer = LightsContainer.GetInstance();
            int pointLightsLen = lightsContainer.GetLightsCount();
            for (int i = 0; i < pointLightsLen; i++)
            {
                float attenuation = 1;
                PointLight pointLight = lightsContainer.GetLight(i); //Calculate Blinn-Phong
                if (pointLight == null)
                {
                    continue;
                }
                if (!pointLight.perPixel)
                {
                    Colorf fragColor = MathUtils.Interpolate(t, startLightFragColor, endLightFragColor);
                    lightColor += fragColor;
                    continue;
                }
                Vector3f lightPosition = pointLight.Trans.position;
                Vector3f worldLightDir;
                if (pointLight.lightType == PointLight.Type.Point)
                {
                    Vector3f wl = worldPosition - lightPosition;
                    worldLightDir = wl.Normalized;
                }
                else
                {
                    worldLightDir = lightPosition.Normalized;
                }
                Vector3f reflectDir = LightsMath.ComputeReflect(-worldLightDir, worldNormal);
                Vector3f viewDir = (cameraPos - worldPosition).Normalized;
                Vector3f halfDir = (worldLightDir + viewDir).Normalized;
                Colorf specularColor = pointLight.color * _specular * MathUtils.Pow(MathUtils.Clamp(reflectDir.Dot(viewDir)), _gross) * attenuation;
                lightColor += specularColor;
            }
            return lightColor;
        }

        private Colorf _CaculateLights(float gradient, ref Vector3f sWorldNormal, ref Vector3f eWorldNormal, ref Vector3f sWorldPosition, ref Vector3f eWorldPosition, ref VertexShaderOutput v1, ref VertexShaderOutput v2, ref Vector3f cameraWorldPos)
        {
            Vector3f worldNormal = MathUtils.Interpolate(gradient, sWorldNormal, eWorldNormal).Normalized;
            Vector3f worldPosition = MathUtils.Interpolate(gradient, sWorldPosition, eWorldPosition);
            Colorf lightColor = _ambient;
            LightsContainer lightsContainer = LightsContainer.GetInstance();
            int pointLightsLen = lightsContainer.GetLightsCount();
            for (int i = 0; i < pointLightsLen; i++)
            {
                float attenuation = 1;
                //Calculate Blinn-Phong
                PointLight pointLight = lightsContainer.GetLight(i);
                Vector3f lightPosition = pointLight.Trans.position;
                Vector3f worldLightDir;
                if (!pointLight.perPixel)
                {
                    Colorf startLightFragColor = v1.fragColor;
                    Colorf endLightFragColor = v2.fragColor;
                    Colorf fragColor = MathUtils.Interpolate(gradient, startLightFragColor, endLightFragColor);
                    lightColor += fragColor;
                    continue;
                }
                if (pointLight.perPixel)
                {
                    if (pointLight.lightType == PointLight.Type.Point)
                    {
                        Vector3f wl = worldPosition - lightPosition;
                        worldLightDir = wl.Normalized;
                    }
                    else
                    {
                        worldLightDir = lightPosition.Normalized;
                    }
                    Vector3f reflectDir = LightsMath.ComputeReflect(-worldLightDir, worldNormal);
                    Vector3f viewDir = (cameraWorldPos - worldPosition).Normalized;
                    Vector3f halfDir = (worldLightDir + viewDir).Normalized;
                    Colorf specularColor = pointLight.color * _specular * MathUtils.Pow(MathUtils.Clamp(reflectDir.Dot(viewDir)), _gross) * attenuation;
                    lightColor += specularColor;
                }
            }
            return lightColor;
        }
        private bool _IsTriangleOutofView(ref VertexShaderOutput v1, ref VertexShaderOutput v2, ref VertexShaderOutput v3)
        {
            float minX = v1.svPosition.x;
            float maxX = v2.svPosition.x;
            if (minX > maxX)
            {
                maxX = minX;
                minX = v2.svPosition.x;
            }
            float x3 = v3.svPosition.x;
            if (x3 > maxX)
            {
                maxX = x3;
            }
            else if (x3 < minX)
            {
                minX = x3;
            }
            if (minX >= _width || maxX <= 0 || v3.svPosition.y < 0 || v1.svPosition.y > _height) //Cull triangles out of view port.
            {
                return true;
            }
            return false;
        }

        private Colorf _ClampColor(Colorf colorf)
        {
            colorf.r = MathUtils.Clamp(colorf.r);
            colorf.g = MathUtils.Clamp(colorf.g);
            colorf.b = MathUtils.Clamp(colorf.b);
            colorf.a = MathUtils.Clamp(colorf.a);
            return colorf;
        }

        private void _Draw(int x, int y, float z, ref Colorf color)
        {
            int index = (x + y * _width);
            if (_depthBuffer[index] > z)
            {
                return;
            }
            _depthBuffer[index] = z;
            unsafe
            {
                byte* b = (byte*)_canvasData.Scan0 + y * _stride + x * 4;
                _SetColorbyte(b, color);
            }
        }
        unsafe private void _SetColorbyte(byte* b, Colorf color)
        {
            b[0] = (byte)(color.b * _colorfToByte);
            b[1] = (byte)(color.g * _colorfToByte);
            b[2] = (byte)(color.r * _colorfToByte);
            b[3] = (byte)(color.a * _colorfToByte);
        }

        private const int _colorfToByte = 255;

        private BitmapData _canvasData = null;

        private readonly float[] _depthBuffer; // 1/z buffer

        private readonly int _bufferSize;

        private int _width;

        private int _height;

        private Bitmap _bitmap;

        private int _stride;

        private bool _beginCompleted = false;

        private bool _endCompleted = true;

        private Texture _texture;

        //Lights data
        private Colorf _ambient;

        private Colorf _specular;

        private float _gross;

        private bool _lightsOn;
        //End
    }
}
