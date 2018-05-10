using System.Collections.Generic;
using PerfectWorldSurvivor.Model;

namespace PerfectWorldSurvivor.Draw
{
    public class World
    {
        public World(float fieldOfView)
        {
            _objects = new List<GameObject>();
            _CreateCamera(fieldOfView);
            _CreateLights();
        }
        public int GameObjectCount
        {
            get { return _objects.Count; }
        }
        public GameObject GetGameObject(int index)
        {
            return _objects[index];
        }
        public void AddGameObject(GameObject obj)
        {
            if (obj != null)
            {
                _objects.Add(obj);
            }
        }
        public Camera GetMainCamera()
        {
            return _camera;
        }
        public void Update()
        {
            _camera.Update();
        }
        private void _CreateCamera(float fieldOfView)
        {
            _camera = new Camera(fieldOfView,RenderEngine.ViewportWidth,RenderEngine.ViewportHeight);
        }
        private void _CreateLights()
        {
            PointLight pointLight0 = new PointLight();
            pointLight0.Trans.position = new Vector3f(0, 150, 0);
            pointLight0.color = Colorf.RED;
            pointLight0.lightType = PointLight.Type.Point;
            pointLight0.perPixel = false;

            LightsContainer lightsContainer = LightsContainer.GetInstance();
            lightsContainer.ambient = new Colorf(0.1f, 0, 0, 1);
            lightsContainer.specular = Colorf.WHITE;
            lightsContainer.gross = 10;
            lightsContainer.AddLight(pointLight0);

            //PointLight directionalLight = new PointLight();
            //directionalLight.transform.position = new Vector3f(1, 3, 1);
            //directionalLight.color = new Colorf(0.3f, 0.4f, 0.4f, 1);
            //directionalLight.type = PointLight.Type.Directional;
        }

        public void Clear()
        {
            _objects.Clear();
        }

        private List<GameObject> _objects;

        private Camera _camera;

    }
}
