using System.Collections.Generic;
using PerfectWorldSurvivor.Draw;
using System;

namespace PerfectWorldSurvivor.Model
{
    public class ClipTriangle
    {
        public ClipTriangle()
        {
            _lightsOn = LightsContainer.GetInstance().lightOn;
            _perpixel = LightsContainer.GetInstance().hasPerpixelLight;
        }

        public void SetCamera(Camera camera)
        {
            _camera = camera;
        }

        public void Update()
        {
            if (_camera != null)
            {
                _zFactorY = _camera.HFOVTan;
                _zFactorX = _camera.Aspect * _zFactorY;
            }
            _lightsOn = LightsContainer.GetInstance().lightOn;
            _perpixel = LightsContainer.GetInstance().hasPerpixelLight;
        }

        public void ClipInViewSpace(ref VertexShaderOutput v1, ref VertexShaderOutput v2, ref VertexShaderOutput v3,Queue<VertexShaderOutput> result)
        {
            _ClearState();
            _TestX(ref v1, ref v2, ref v3, _zFactorX, ClipTriangle._clip_code_lx, ClipTriangle._clip_code_hx, ClipTriangle._clip_code_ix);
            //All vertices out of view frustum
            if (((_HasTag(_vertexCodes[0], ClipTriangle._clip_code_hx)) && (_HasTag(_vertexCodes[1], ClipTriangle._clip_code_hx)) && (_HasTag(_vertexCodes[2], ClipTriangle._clip_code_hx)))
                || ((_HasTag(_vertexCodes[0], ClipTriangle._clip_code_lx)) && (_HasTag(_vertexCodes[1], ClipTriangle._clip_code_lx)) && (_HasTag(_vertexCodes[2], ClipTriangle._clip_code_lx))))
            {
                _CopyQueue(_clippedVertices, result);
                return;
            }
            _TestY(ref v1, ref v2, ref v3, _zFactorY, ClipTriangle._clip_code_ly, ClipTriangle._clip_code_hy, ClipTriangle._clip_code_iy);
            //All vertices out of view frustum
            if (((_HasTag(_vertexCodes[0], ClipTriangle._clip_code_hy)) && (_HasTag(_vertexCodes[1], ClipTriangle._clip_code_hy)) && (_HasTag(_vertexCodes[2], ClipTriangle._clip_code_hy)))
                || ((_HasTag(_vertexCodes[0], ClipTriangle._clip_code_ly)) && (_HasTag(_vertexCodes[1], ClipTriangle._clip_code_ly)) && (_HasTag(_vertexCodes[2], ClipTriangle._clip_code_ly))))
            {
                _CopyQueue(_clippedVertices, result);
                return;
            }
            _clippedVertices.Enqueue(v1);
            _clippedVertices.Enqueue(v2);
            _clippedVertices.Enqueue(v3);

            _ClipLeftRight();
            _ClipBottomTop();
            _ClipNearFar();
            _CopyQueue(_clippedVertices, result);
        }

        private void _ClipLeftRight()
        {
            int count = _clippedVertices.Count;
            while (count > 0) //Left
            {
                VertexShaderOutput out1 = _clippedVertices.Dequeue();
                VertexShaderOutput out2 = _clippedVertices.Dequeue();
                VertexShaderOutput out3 = _clippedVertices.Dequeue();
                _ClipLeftRight(ref out1, ref out2, ref out3, ClipTriangle._clip_code_lx, ClipTriangle._clip_code_hx, ClipTriangle._clip_code_ix, _zFactorX, false);
                count -= 3;
            }
            count = _clippedVertices.Count;
            while (count > 0) //Right
            {
                VertexShaderOutput out1 = _clippedVertices.Dequeue();
                VertexShaderOutput out2 = _clippedVertices.Dequeue();
                VertexShaderOutput out3 = _clippedVertices.Dequeue();
                _ClipLeftRight(ref out1, ref out2, ref out3, ClipTriangle._clip_code_lx, ClipTriangle._clip_code_hx, ClipTriangle._clip_code_ix, -_zFactorX, true);
                count -= 3;
            }
        }

        private void _ClipBottomTop()
        {
            int count = _clippedVertices.Count;
            while (count > 0) //Top
            {
                VertexShaderOutput out1 = _clippedVertices.Dequeue();
                VertexShaderOutput out2 = _clippedVertices.Dequeue();
                VertexShaderOutput out3 = _clippedVertices.Dequeue();
                _ClipBottomTop(ref out1, ref out2, ref out3, ClipTriangle._clip_code_ly, ClipTriangle._clip_code_hy, ClipTriangle._clip_code_iy, -_zFactorY, true);
                count -= 3;
            }
            count = _clippedVertices.Count;
            while (count > 0) //Bottom
            {
                VertexShaderOutput out1 = _clippedVertices.Dequeue();
                VertexShaderOutput out2 = _clippedVertices.Dequeue();
                VertexShaderOutput out3 = _clippedVertices.Dequeue();
                _ClipBottomTop(ref out1, ref out2, ref out3, ClipTriangle._clip_code_ly, ClipTriangle._clip_code_hy, ClipTriangle._clip_code_iy, _zFactorY, false);
                count -= 3;
            }
        }

        private void _ClipNearFar()
        {
            int count = _clippedVertices.Count;
            while (count > 0) // far
            {
                VertexShaderOutput out1 = _clippedVertices.Dequeue();
                VertexShaderOutput out2 = _clippedVertices.Dequeue();
                VertexShaderOutput out3 = _clippedVertices.Dequeue();
                _ClipNearFar(ref out1, ref out2, ref out3, ClipTriangle._clip_code_lz, ClipTriangle._clip_code_hz, ClipTriangle._clip_code_iz, _camera.far, _camera.near, true);
                count -= 3;
            }
            count = _clippedVertices.Count;
            while (count > 0) //near
            {
                VertexShaderOutput out1 = _clippedVertices.Dequeue();
                VertexShaderOutput out2 = _clippedVertices.Dequeue();
                VertexShaderOutput out3 = _clippedVertices.Dequeue();
                _ClipNearFar(ref out1, ref out2, ref out3, ClipTriangle._clip_code_lz, ClipTriangle._clip_code_hz, ClipTriangle._clip_code_iz, _camera.far, _camera.near, false);
                count -= 3;
            }
        }

        private void _ClearState()
        {
            int _vertex_codesLen = _vertexCodes.Length;
            for (int i = 0; i < _vertex_codesLen; i++)
            {
                _vertexCodes[i] = 0;
            }
        }

        private void _ClipNearFar(ref VertexShaderOutput v1, ref VertexShaderOutput v2, ref VertexShaderOutput v3,int lowCode,int highCode,int inCode,float far,float near,bool high)
        {
            _ClearState();
            //Near and far clip face.
            int vertsInFrutum =  _TestZ(-(v1.svPosition.z), -(v2.svPosition.z), -(v3.svPosition.z), near, far, lowCode, highCode, inCode);
            //All vertices out of view frustum
            if (((_HasTag(_vertexCodes[0], highCode)) && (_HasTag(_vertexCodes[1], highCode)) && (_HasTag(_vertexCodes[2], highCode)))
                || ((_HasTag(_vertexCodes[0], lowCode)) && (_HasTag(_vertexCodes[1], lowCode)) && (_HasTag(_vertexCodes[2], lowCode))))
            {
                return;
            }
            float factorZ = high ? -far : - near;
            int realLowZ = high ? highCode : lowCode;
            int realHighZ = high ? lowCode : highCode;
            //Clip near
            if (_HasTag(_vertexCodes[0] | _vertexCodes[1] | _vertexCodes[2], realLowZ))
            {
                if (vertsInFrutum == 1)
                {
                    _SwapV1In(ref v1, ref v2, ref v3, inCode);
                    _ClipNewTriangleInNearFar(ref v1, ref v2, ref v3, factorZ);
                }
                else if (vertsInFrutum == 2)
                {
                    _SwapV1In(ref v1, ref v2, ref v3, realLowZ);
                    _CreateNewTriangleInNearFar(ref v1, ref v2, ref v3, factorZ);
                }
            }
            else
            {
                _clippedVertices.Enqueue(v1);
                _clippedVertices.Enqueue(v2);
                _clippedVertices.Enqueue(v3);
            }
        }

      
        private void _ClipLeftRight(ref VertexShaderOutput v1, ref VertexShaderOutput v2, ref VertexShaderOutput v3, int lowCode, int highCode, int inCode, float zFactor, bool high)
        {
            _ClearState();
            int vertsInFrutum = _TestX(ref v1, ref v2, ref v3, zFactor, lowCode, highCode, inCode);
            //All vertices out of view frustum
            bool allInLeft = _HasTag(_vertexCodes[0], highCode) && _HasTag(_vertexCodes[1], highCode) && _HasTag(_vertexCodes[2], highCode);
            bool allInRight = _HasTag(_vertexCodes[0], lowCode) && _HasTag(_vertexCodes[1], lowCode) && _HasTag(_vertexCodes[2], lowCode);
            if (allInLeft || allInRight)
            {
                return;
            }
            int realLowCode = high ? highCode : lowCode;
            int realHighCode = high ? lowCode : highCode;

            //Clip Left
            if (_HasTag(_vertexCodes[0] | _vertexCodes[1] | _vertexCodes[2], realLowCode))
            {
                if (vertsInFrutum == 0)
                {
                    _clippedVertices.Enqueue(v1);
                    _clippedVertices.Enqueue(v2);
                    _clippedVertices.Enqueue(v3);
                    return;
                }
                if (vertsInFrutum == 1)
                {
                    if (_HasTag(_vertexCodes[0] | _vertexCodes[1] | _vertexCodes[2], realHighCode))
                    {
                        _clippedVertices.Enqueue(v1);
                        _clippedVertices.Enqueue(v2);
                        _clippedVertices.Enqueue(v3);
                        return;
                    }

                    _SwapV1In(ref v1, ref v2, ref v3, inCode);
                    _ClipNewTriangleInLeftRight(ref v1, ref v2, ref v3, zFactor);
                  
                }
                else if (vertsInFrutum == 2)
                {
                    _SwapV1In(ref v1, ref v2, ref v3, realLowCode);
                    _CreateNewTriangleInLeftRight(ref v1, ref v2, ref v3, zFactor);
                }
            }
            else
            {
                _clippedVertices.Enqueue(v1);
                _clippedVertices.Enqueue(v2);
                _clippedVertices.Enqueue(v3);
            }
        }

       
        private void _ClipBottomTop(ref VertexShaderOutput v1, ref VertexShaderOutput v2, ref VertexShaderOutput v3,int lowCode,int highCode,int inCode,float zFactor,bool high)
        {
            _ClearState();
            int vertsInFrutum = _TestY(ref v1, ref v2, ref v3, zFactor, lowCode, highCode, inCode);
            //All vertices out of view frustum
            if (((_HasTag(_vertexCodes[0], highCode)) && (_HasTag(_vertexCodes[1], highCode)) && (_HasTag(_vertexCodes[2], highCode)))
                || ((_HasTag(_vertexCodes[0], lowCode)) && (_HasTag(_vertexCodes[1], lowCode)) && (_HasTag(_vertexCodes[2], lowCode))))
            {
                return;
            }
            int realLowCode = high ? highCode : lowCode;
            int realHighCode = high ? lowCode : highCode;
            if (_HasTag(_vertexCodes[0] | _vertexCodes[1] | _vertexCodes[2], realLowCode))
            {
                if (vertsInFrutum == 0)
                {
                    _clippedVertices.Enqueue(v1);
                    _clippedVertices.Enqueue(v2);
                    _clippedVertices.Enqueue(v3);
                    return;
                }
                if (vertsInFrutum == 1)
                {
                    if (_HasTag(_vertexCodes[0] | _vertexCodes[1] | _vertexCodes[2], realHighCode))
                    {
                        _clippedVertices.Enqueue(v1);
                        _clippedVertices.Enqueue(v2);
                        _clippedVertices.Enqueue(v3);
                        return;
                    }
                    _SwapV1In(ref v1, ref v2, ref v3, inCode);
                    _ClipTriangleInBottomTop(ref v1, ref v2, ref v3, zFactor);
                }
                else if (vertsInFrutum == 2)
                {
                    _SwapV1In(ref v1, ref v2, ref v3, realLowCode);
                    _CreateNewTriangleInBottomTop(ref v1, ref v2, ref v3, zFactor);
                }
            }
            else
            {
                _clippedVertices.Enqueue(v1);
                _clippedVertices.Enqueue(v2);
                _clippedVertices.Enqueue(v3);
            }
        }
        private void _SwapV1In(ref VertexShaderOutput v1, ref VertexShaderOutput v2, ref VertexShaderOutput v3, int inCode)
        {
            VertexShaderOutput tmp;
            if (_vertexCodes[0] == inCode)
            {
                //Do nothing
            }
            else if (_vertexCodes[1] == inCode)
            {
                //Swap
                tmp = v1;
                v1 = v2;
                v2 = v3;
                v3 = tmp;
            }
            else
            {
                //Swap
                tmp = v1;
                v1 = v3;
                v3 = v2;
                v2 = tmp;
            }
        }

        private int _TestZ(float v1, float v2, float v3, float min, float max, int lowCode, int highCode, int inCode)
        {
            int vertsInFrutum = 0;
            if (v1 > max)
            {
                _vertexCodes[0] = highCode;
            }
            else if (v1 < min)
            {
                _vertexCodes[0] = lowCode;
            }
            else
            {
                _vertexCodes[0] = inCode;
                vertsInFrutum++;
            }

            if (v2 > max)
            {
                _vertexCodes[1] = highCode;
            }
            else if (v2 < min)
            {
                _vertexCodes[1] = lowCode;
            }
            else
            {
                _vertexCodes[1] = inCode;
                vertsInFrutum++;
            }

            if (v3 > max)
            {
                _vertexCodes[2] = highCode;
            }
            else if (v3 < min)
            {
                _vertexCodes[2] = lowCode;
            }
            else
            {
                _vertexCodes[2] = inCode;
                vertsInFrutum++;
            }
            return vertsInFrutum;
        }

        private int _TestX(ref VertexShaderOutput v1, ref VertexShaderOutput v2, ref VertexShaderOutput v3, float zFactor, int lowCode, int highCode, int inCode)
        {
            int vertsInFrutum = 0;
            Vector4f v1SV = v1.svPosition;
            Vector4f v2SV = v2.svPosition;
            Vector4f v3SV = v3.svPosition;
            float v1z = -v1SV.z;
            float v2z = -v2SV.z;
            float v3z = -v3SV.z;
            float absZFactor = Math.Abs(zFactor);
            float zTest = absZFactor * v1z;
            if (v1SV.x > zTest)
            {
                _vertexCodes[0] |= highCode;
            }
            else if (v1SV.x < -zTest)
            {
                _vertexCodes[0] |= lowCode;
            }
            else
            {
                _vertexCodes[0] |= inCode;
                vertsInFrutum++;
            }
            zTest = absZFactor * v2z;
            if (v2SV.x > zTest)
            {
                _vertexCodes[1] |= highCode;
            }
            else if (v2SV.x < -zTest)
            {
                _vertexCodes[1] |= lowCode;
            }
            else
            {
                _vertexCodes[1] |= inCode;
                vertsInFrutum++;
            }
            zTest = absZFactor * v3z;
            if (v3SV.x > zTest)
            {
                _vertexCodes[2] |= highCode;
            }
            else if (v3SV.x < -zTest)
            {
                _vertexCodes[2] |= lowCode;
            }
            else
            {
                _vertexCodes[2] |= inCode;
                vertsInFrutum++;
            }
            return vertsInFrutum;
        }

        private int _TestY(ref VertexShaderOutput v1, ref VertexShaderOutput v2, ref VertexShaderOutput v3, float zFactor, int lowCode, int highCode, int inCode)
        {
            Vector4f v1SV = v1.svPosition;
            Vector4f v2SV = v2.svPosition;
            Vector4f v3SV = v3.svPosition;
            float v1z = -v1SV.z;
            float v2z = -v2SV.z;
            float v3z = -v3SV.z;
            int vertsInFrutum = 0;
            float absZFactor = Math.Abs(zFactor);
            float zTest = absZFactor * v1z;
            if (v1SV.y > zTest)
            {
                _vertexCodes[0] |= highCode;
            }
            else if (v1SV.y < -zTest)
            {
                _vertexCodes[0] |= lowCode;
            }
            else
            {
                _vertexCodes[0] |= inCode;
                vertsInFrutum++;
            }
            zTest = absZFactor * v2z;
            if (v2SV.y > zTest)
            {
                _vertexCodes[1] |= highCode;
            }
            else if (v2SV.y < -zTest)
            {
                _vertexCodes[1] |= lowCode;
            }
            else
            {
                _vertexCodes[1] |= inCode;
                vertsInFrutum++;
            }
            zTest = absZFactor * v3z;
            if (v3SV.y > zTest)
            {
                _vertexCodes[2] |= highCode;
            }
            else if (v3SV.y < -zTest)
            {
                _vertexCodes[2] |= lowCode;
            }
            else
            {
                _vertexCodes[2] |= inCode;
                vertsInFrutum++;
            }
            return vertsInFrutum;
        }

        private void _ClipNewTriangleInNearFar(ref VertexShaderOutput v1, ref VertexShaderOutput v2, ref VertexShaderOutput v3, float factorZ)
        {
            float xi, yi, ui, vi, t1, t2;
            //vt = v0 + t * v1;
            Vector4f v2v1 = v2.svPosition - v1.svPosition;
            t1 = (factorZ - v1.svPosition.z) / v2v1.z;
            xi = v1.svPosition.x + v2v1.x * t1;
            yi = v1.svPosition.y + v2v1.y * t1;
            //Rewrite v2
            v2.svPosition.x = xi;
            v2.svPosition.y = yi;
            v2.svPosition.z = factorZ;
            Vector4f v3v1 = v3.svPosition - v1.svPosition;
            t2 = (factorZ - v1.svPosition.z) / v3v1.z;
            xi = v1.svPosition.x + v3v1.x * t2;
            yi = v1.svPosition.y + v3v1.y * t2;
            //Rewrite v3
            v3.svPosition.x = xi;
            v3.svPosition.y = yi;
            v3.svPosition.z = factorZ;
            //Recaculate v2 uv
            ui = v1.uv.x + (v2.uv.x - v1.uv.x) * t1;
            vi = v1.uv.y + (v2.uv.y - v1.uv.y) * t1;
            //Recaculate v2 world 
            _InterpolateLightData(ref v1, ref v2, ref v3, t1, t2);

            v2.uv.x = ui;
            v2.uv.y = vi;
            //Recaculate v3 uv
            ui = v1.uv.x + (v3.uv.x - v1.uv.x) * t2;
            vi = v1.uv.y + (v3.uv.y - v1.uv.y) * t2;
            v3.uv.x = ui;
            v3.uv.y = vi;
            _clippedVertices.Enqueue(v1);
            _clippedVertices.Enqueue(v2);
            _clippedVertices.Enqueue(v3);
        }

        private void _ClipNewTriangleInLeftRight(ref VertexShaderOutput v1, ref VertexShaderOutput v2, ref VertexShaderOutput v3, float zFactor)
        {
            float t1, t2, xi, yi, ui, vi;
            //vt = v0 + t * v1;
            Vector4f v2v1 = v2.svPosition - v1.svPosition;
            t1 = (zFactor * v1.svPosition.z - v1.svPosition.x) / (-zFactor * v2v1.z + v2v1.x);

            xi = v1.svPosition.x + v2v1.x * t1;
            yi = v1.svPosition.y + v2v1.y * t1;

            //Rewrite v2
            v2.svPosition.x = xi;
            v2.svPosition.y = yi;
            v2.svPosition.z = v1.svPosition.z + v2v1.z * t1;

            Vector4f v3v1 = v3.svPosition - v1.svPosition;
            t2 = (zFactor * v1.svPosition.z - v1.svPosition.x) / (-zFactor * v3v1.z + v3v1.x);
            xi = v1.svPosition.x + v3v1.x * t2;
            yi = v1.svPosition.y + v3v1.y * t2;

            //Rewrite v3
            v3.svPosition.x = xi;
            v3.svPosition.y = yi;
            v3.svPosition.z = v1.svPosition.z + v3v1.z * t2;

            //Recaculate v2 uv
            ui = v1.uv.x + (v2.uv.x - v1.uv.x) * t1;
            vi = v1.uv.y + (v2.uv.y - v1.uv.y) * t1;

            //Recaculate v2 world 
            _InterpolateLightData(ref v1, ref v2, ref v3, t1, t2);


            v2.uv.x = ui;
            v2.uv.y = vi;

            //Recaculate v3 uv
            ui = v1.uv.x + (v3.uv.x - v1.uv.x) * t2;
            vi = v1.uv.y + (v3.uv.y - v1.uv.y) * t2;

            v3.uv.x = ui;
            v3.uv.y = vi;
            _clippedVertices.Enqueue(v1);
            _clippedVertices.Enqueue(v2);
            _clippedVertices.Enqueue(v3);
        }

        private void _CreateNewTriangleInLeftRight(ref VertexShaderOutput v1, ref VertexShaderOutput v2, ref VertexShaderOutput v3, float zFactor)
        {
            float t1, t2, x01i, y01i, x02i, y02i, z02i, u01i, v01i, u02i, v02i;
            VertexShaderOutput nv1 = v1;
            VertexShaderOutput nv2 = v2;
            VertexShaderOutput nv3 = v3;
            //vt = v0 + t * v1;
            Vector4f v2v1 = v2.svPosition - v1.svPosition;
            t1 = (zFactor * v1.svPosition.z - v1.svPosition.x) / (-zFactor * v2v1.z + v2v1.x);
            x01i = v1.svPosition.x + v2v1.x * t1;
            y01i = v1.svPosition.y + v2v1.y * t1;
            Vector4f v3v1 = v3.svPosition - v1.svPosition;
            t2 = (zFactor * v1.svPosition.z - v1.svPosition.x) / (-zFactor * v3v1.z + v3v1.x);
            x02i = v1.svPosition.x + v3v1.x * t2;
            y02i = v1.svPosition.y + v3v1.y * t2;
            z02i = v1.svPosition.z + v3v1.z * t2;
            v1.svPosition.x = x01i;
            v1.svPosition.y = y01i;
            v1.svPosition.z = v1.svPosition.z + v2v1.z * t1;
            nv2.svPosition.x = x01i;
            nv2.svPosition.y = y01i;
            nv2.svPosition.z = v1.svPosition.z;

            nv1.svPosition.x = x02i;
            nv1.svPosition.y = y02i;
            nv1.svPosition.z = z02i;

            u01i = v1.uv.x + (v2.uv.x - v1.uv.x) * t1;
            v01i = v1.uv.y + (v2.uv.y - v1.uv.y) * t1;


            u02i = v1.uv.x + (v3.uv.x - v1.uv.x) * t2;
            v02i = v1.uv.y + (v3.uv.y - v1.uv.y) * t2;

            v1.uv.x = u01i;
            v1.uv.y = v01i;

            nv1.uv.x = u02i;
            nv1.uv.y = v02i;

            nv2.uv.x = u01i;
            nv2.uv.y = v01i;

            _InterpolateLightData(ref v1, ref v2, ref v3, ref nv1, ref nv2, t1, t2);
            _clippedVertices.Enqueue(nv1);
            _clippedVertices.Enqueue(nv2);
            _clippedVertices.Enqueue(nv3);
            _clippedVertices.Enqueue(v1);
            _clippedVertices.Enqueue(v2);
            _clippedVertices.Enqueue(v3);
        }
       

        private void _CreateNewTriangleInNearFar(ref VertexShaderOutput v1, ref VertexShaderOutput v2, ref VertexShaderOutput v3, float factorZ)
        {
            float t1, t2, x01i, y01i, x02i, y02i, u01i, v01i, u02i, v02i;
            VertexShaderOutput nv1 = v1;
            VertexShaderOutput nv2 = v2;
            VertexShaderOutput nv3 = v3;
            //vt = v0 + t * v1;
            Vector4f v2v1 = v2.svPosition - v1.svPosition;
            t1 = (factorZ - v1.svPosition.z) / v2v1.z;
            x01i = v1.svPosition.x + v2v1.x * t1;
            y01i = v1.svPosition.y + v2v1.y * t1;
            Vector4f v3v1 = v3.svPosition - v1.svPosition;
            t2 = (factorZ - v1.svPosition.z) / v3v1.z;
            x02i = v1.svPosition.x + v3v1.x * t2;
            y02i = v1.svPosition.y + v3v1.y * t2;

            v1.svPosition.x = x01i;
            v1.svPosition.y = y01i;
            v1.svPosition.z = factorZ;

            nv2.svPosition.x = x01i;
            nv2.svPosition.y = y01i;
            nv2.svPosition.z = factorZ;

            nv1.svPosition.x = x02i;
            nv1.svPosition.y = y02i;
            nv1.svPosition.z = factorZ;

            u01i = v1.uv.x + (v2.uv.x - v1.uv.x) * t1;
            v01i = v1.uv.y + (v2.uv.y - v1.uv.y) * t1;
            u02i = v1.uv.x + (v3.uv.x - v1.uv.x) * t2;
            v02i = v1.uv.y + (v3.uv.y - v1.uv.y) * t2;
            v1.uv.x = u01i;
            v1.uv.y = v01i;
            nv1.uv.x = u02i;
            nv1.uv.y = v02i;
            nv2.uv.x = u01i;
            nv2.uv.y = v01i;
            _InterpolateLightData(ref v1, ref v2, ref v3, ref nv1, ref nv2, t1, t2);
            _clippedVertices.Enqueue(nv1);
            _clippedVertices.Enqueue(nv2);
            _clippedVertices.Enqueue(nv3);
            _clippedVertices.Enqueue(v1);
            _clippedVertices.Enqueue(v2);
            _clippedVertices.Enqueue(v3);
        }

        private void _ClipTriangleInBottomTop(ref VertexShaderOutput v1, ref VertexShaderOutput v2, ref VertexShaderOutput v3, float zFactor)
        {
            float xi, yi, zi, ui, vi, t1, t2;
            //vt = v0 + t * dv;
            Vector4f v2v1 = v2.svPosition - v1.svPosition;
            t1 = (zFactor * v1.svPosition.z - v1.svPosition.y) / (-zFactor * v2v1.z + v2v1.y);
            xi = v1.svPosition.x + v2v1.x * t1;
            yi = v1.svPosition.y + v2v1.y * t1;
            zi = v1.svPosition.z + v2v1.z * t1;
            //Rewrite v2
            v2.svPosition.x = xi;
            v2.svPosition.y = yi;
            v2.svPosition.z = zi;

            Vector4f v3v1 = v3.svPosition - v1.svPosition;
            t2 = (zFactor * v1.svPosition.z - v1.svPosition.y) / (-zFactor * v3v1.z + v3v1.y);
            xi = v1.svPosition.x + v3v1.x * t2;
            yi = v1.svPosition.y + v3v1.y * t2;
            zi = v1.svPosition.z + v3v1.z * t2;
            //Rewrite v3
            v3.svPosition.x = xi;
            v3.svPosition.y = yi;
            v3.svPosition.z = zi;

            //Recaculate v2 uv
            ui = v1.uv.x + (v2.uv.x - v1.uv.x) * t1;
            vi = v1.uv.y + (v2.uv.y - v1.uv.y) * t1;

            //Recaculate v2 world 
            _InterpolateLightData(ref v1, ref v2, ref v3, t1, t2);

            v2.uv.x = ui;
            v2.uv.y = vi;

            //Recaculate v3 uv
            ui = v1.uv.x + (v3.uv.x - v1.uv.x) * t2;
            vi = v1.uv.y + (v3.uv.y - v1.uv.y) * t2;

            v3.uv.x = ui;
            v3.uv.y = vi;

            _clippedVertices.Enqueue(v1);
            _clippedVertices.Enqueue(v2);
            _clippedVertices.Enqueue(v3);
        }

        private void _CreateNewTriangleInBottomTop(ref VertexShaderOutput v1, ref VertexShaderOutput v2, ref VertexShaderOutput v3, float zFactor)
        {
            float t1, t2, x01i, y01i, x02i, y02i, z02i, u01i, v01i, u02i, v02i;
            VertexShaderOutput nv1 = v1;
            VertexShaderOutput nv2 = v2;
            VertexShaderOutput nv3 = v3;
            Vector4f v2v1 = v2.svPosition - v1.svPosition;
            t1 = (zFactor * v1.svPosition.z - v1.svPosition.y) / (-zFactor * v2v1.z + v2v1.y);

            x01i = v1.svPosition.x + v2v1.x * t1;
            y01i = v1.svPosition.y + v2v1.y * t1;
            Vector4f v3v1 = v3.svPosition - v1.svPosition;
            t2 = (zFactor * v1.svPosition.z - v1.svPosition.y) / (-zFactor * v3v1.z + v3v1.y);
            x02i = v1.svPosition.x + v3v1.x * t2;
            y02i = v1.svPosition.y + v3v1.y * t2;
            z02i = v1.svPosition.z + v3v1.z * t2;
            v1.svPosition.x = x01i;
            v1.svPosition.y = y01i;
            v1.svPosition.z = v1.svPosition.z + v2v1.z * t1;
            nv2.svPosition.x = x01i;
            nv2.svPosition.y = y01i;
            nv2.svPosition.z = v1.svPosition.z;

            nv1.svPosition.x = x02i;
            nv1.svPosition.y = y02i;
            nv1.svPosition.z = z02i;

            u01i = v1.uv.x + (v2.uv.x - v1.uv.x) * t1;
            v01i = v1.uv.y + (v2.uv.y - v1.uv.y) * t1;

            u02i = v1.uv.x + (v3.uv.x - v1.uv.x) * t2;
            v02i = v1.uv.y + (v3.uv.y - v1.uv.y) * t2;

            v1.uv.x = u01i;
            v1.uv.y = v01i;

            nv1.uv.x = u02i;
            nv1.uv.y = v02i;

            nv2.uv.x = u01i;
            nv2.uv.y = v01i;
            _InterpolateLightData(ref v1, ref v2, ref v3, ref nv1, ref nv2, t1, t2);
            _clippedVertices.Enqueue(nv1);
            _clippedVertices.Enqueue(nv2);
            _clippedVertices.Enqueue(nv3);
            _clippedVertices.Enqueue(v1);
            _clippedVertices.Enqueue(v2);
            _clippedVertices.Enqueue(v3);

        }
        private void _InterpolateLightData(ref VertexShaderOutput v1, ref VertexShaderOutput v2, ref VertexShaderOutput v3, float t1, float t2)
        {
            if (_lightsOn)
            {
                if (_perpixel)
                {
                    v2.worldPosition = v1.worldPosition + (v2.worldPosition - v1.worldPosition) * t1;
                    v2.worldNormal = v1.worldNormal + (v2.worldNormal - v1.worldNormal) * t1;
                    v3.worldPosition = v1.worldPosition + (v3.worldPosition - v1.worldPosition) * t2;
                    v3.worldNormal = v1.worldNormal + (v3.worldNormal - v1.worldNormal) * t2;
                }
                else
                {
                    v2.fragColor = v1.fragColor + (v2.fragColor - v1.fragColor) * t1;
                    v3.fragColor = v1.fragColor + (v3.fragColor - v1.fragColor) * t2;
                }
            }
        }

        private void _InterpolateLightData(ref VertexShaderOutput v1, ref VertexShaderOutput v2, ref VertexShaderOutput v3, ref VertexShaderOutput nv1, ref VertexShaderOutput nv2, float t1, float t2)
        {
            if (_lightsOn)
            {
                if (_perpixel)
                {
                    v1.worldPosition = v1.worldPosition + (v2.worldPosition - v1.worldPosition) * t1;
                    v1.worldNormal = v1.worldNormal + (v2.worldNormal - v1.worldNormal) * t1;

                    nv1.worldPosition = v1.worldPosition + (v3.worldPosition - v1.worldPosition) * t2;
                    nv1.worldNormal = v1.worldNormal + (v3.worldNormal - v1.worldNormal) * t2;

                    nv2.worldPosition = v1.worldPosition;
                    nv2.worldPosition = v1.worldNormal;
                }
                else
                {
                    v1.fragColor = v1.fragColor + (v2.fragColor - v1.fragColor) * t1;
                    v1.fragColor = v1.fragColor + (v2.fragColor - v1.fragColor) * t1;

                    nv1.fragColor = v1.fragColor + (v3.fragColor - v1.fragColor) * t2;
                    nv1.fragColor = v1.fragColor + (v3.fragColor - v1.fragColor) * t2;

                    nv2.fragColor = v1.fragColor;
                    nv2.fragColor = v1.fragColor;
                }
            }
        }
        private void _CopyQueue(Queue<VertexShaderOutput> src, Queue<VertexShaderOutput> dst)
        {
            dst.Clear();
            while (src.Count > 0)
            {
                dst.Enqueue(src.Dequeue());
            }
        }
        private bool _HasTag(int value, int tag)
        {
            if ((value & tag) != 0)
            {
                return true;
            }
            return false;
        }

        private const int _clip_code_hz = 1;

        private const int _clip_code_lz = 2;

        private const int _clip_code_iz = 4;

        private const int _clip_code_hx = 8;

        private const int _clip_code_lx = 16;

        private const int _clip_code_ix = 32;

        private const int _clip_code_hy = 64;

        private const int _clip_code_ly = 128;

        private const int _clip_code_iy = 256;

        private const int _clip_code_null = 512;

        private int[] _vertexCodes = new int[3];

        private Queue<VertexShaderOutput> _clippedVertices = new Queue<VertexShaderOutput>();

        private Camera _camera;

        private float _zFactorY;

        private float _zFactorX;

        private bool _lightsOn;

        private bool _perpixel;
    }
}
