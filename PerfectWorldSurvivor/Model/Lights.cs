using System;
using System.Collections.Generic;

namespace PerfectWorldSurvivor.Model
{
    public class LightsContainer
    {
        public static LightsContainer GetInstance()
        {
            return _Instance;
        }

        public void Clear()
        {
            _PointLights.Clear();
        }

        public void AddLight(PointLight pointLight)
        {
            _PointLights.Add(pointLight);
            if (pointLight.perPixel)
            {
                hasPerpixelLight = true;
            }
        }
        public int GetLightsCount()
        {
            return _PointLights.Count;
        }

        public PointLight GetLight(int index)
        {
            if (index < 0 || index >= _PointLights.Count)
            {
                return null;
            }
            return _PointLights[index];
        }

        private LightsContainer()
        {
            _PointLights = new List<PointLight>();
        }

        public Colorf ambient;

        public Colorf specular;

        public float gross;

        public bool lightOn;

        public bool hasPerpixelLight;

        private readonly List<PointLight> _PointLights;

        private static readonly LightsContainer _Instance = new LightsContainer();
    }

}
