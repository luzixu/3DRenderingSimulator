using System;
using System.Collections.Generic;
using PerfectWorldSurvivor.Model;
using PerfectWorldSurvivor.UI;
using PerfectWorldSurvivor.Common;
using PerfectWorldSurvivor.Sources;
using PerfectWorldSurvivor.Utils;

namespace PerfectWorldSurvivor.Draw
{
    public class WorldEventController : IDisposable
    {
        private enum CameraMotionDirection
        {
            Left,
            Right,
            Forward,
            Back,
            Up,
            Down
        }

        public WorldEventController(World world)
        {
            _world = world;
            _SetDefaultGameObject();
            KeyMessageController.GetInstance().AddKeyDownHanlder(_HandleCameraKeyDownEvent);
        }

        public void Dispose()
        {
            _DestroyGameObject(_cachedBoat);
            _DestroyGameObject(_cachedChicken);
            _DestroyGameObject(_cachedCube);
            _DestroyGameObject(_cachedLineCube);
        }

        private void _DestroyGameObject(GameObject obj)
        {
            if (obj != null)
            {
                obj.Dispose();
            }
        }

        private void _SetDefaultGameObject()
        {
            if (DisplayEngine.displayType == DisplayEngine.displayTypeRasterizedTriangle)
            {
                _AddCube();
            }
            else if (DisplayEngine.displayType == DisplayEngine.displayTypeRasterizedTriangleWithLight)
            {
                _AddBoat();
            }
            else if (DisplayEngine.displayType == DisplayEngine.displayTypeSimpleTriangle)
            {
                _AddLineCube();
            }
            else if (DisplayEngine.displayType == DisplayEngine.displayTypeModeChicken)
            {
                _AddChicken();
            }
            _SetCameraDefault();
        }

        private void _RotateCamera(CameraMotionDirection roundDirection)
        {
            Camera mainCamera = _world.GetMainCamera();
            if (mainCamera == null)
            {
                return;
            }
            Vector3f axis = Vector3f.Forward;
            switch (roundDirection)
            {
                case CameraMotionDirection.Left:
                    axis = -mainCamera.Trans.Up;
                    break;
                case CameraMotionDirection.Right:
                    axis = mainCamera.Trans.Up;
                    break;
                case CameraMotionDirection.Up:
                    axis = mainCamera.Trans.Right;
                    break;
                case CameraMotionDirection.Down:
                    axis = -mainCamera.Trans.Right;
                    break;
            }
            mainCamera.Trans.Rotate(axis, _cameraRotationAngle);
        }

        private void _CameraTranslate(CameraMotionDirection direction,bool fast)
        {
            Camera mainCamera = _world.GetMainCamera();
            if (mainCamera == null)
            {
                return;
            }
            Vector3f translation = Vector3f.Zero;
            switch (direction)
            {
                case CameraMotionDirection.Left:
                    translation = -mainCamera.Trans.Right;
                    break;
                case CameraMotionDirection.Right:
                    translation = mainCamera.Trans.Right;
                    break;
                case CameraMotionDirection.Forward:
                    translation = mainCamera.Trans.Forward;
                    break;
                case CameraMotionDirection.Back:
                    translation = -mainCamera.Trans.Forward;
                    break;
                case CameraMotionDirection.Up:
                    translation = mainCamera.Trans.Up;
                    break;
                case CameraMotionDirection.Down:
                    translation = -mainCamera.Trans.Up;
                    break;
            }

            if (fast)
            {
                translation *= _cameraTranslationFastSpeed;
            }
            else
            {
                translation *= _cameraTranslationSlowSpeed;
            }

            mainCamera.Trans.Translate(translation);
        }

        private bool _HandleCameraKeyDownEvent(char key)
        {
            switch (key)
            {
                case InputKeys.cameraLeft:
                    _CameraTranslate(CameraMotionDirection.Left, true);
                    break;
                case InputKeys.cameraRight:
                    _CameraTranslate(CameraMotionDirection.Right, true);
                    break;
                case InputKeys.cameraLeftSlow:
                    _CameraTranslate(CameraMotionDirection.Left, false);
                    break;
                case InputKeys.cameraRightSlow:
                    _CameraTranslate(CameraMotionDirection.Right, false);
                    break;
                case InputKeys.cameraBack:
                    _CameraTranslate(CameraMotionDirection.Back, true);
                    break;
                case InputKeys.cameraForward:
                    _CameraTranslate(CameraMotionDirection.Forward, true);
                    break;
                case InputKeys.cameraDown:
                    _CameraTranslate(CameraMotionDirection.Down, true);
                    break;
                case InputKeys.cameraUp:
                    _CameraTranslate(CameraMotionDirection.Up, true);
                    break;
                case InputKeys.cameraRotateLeft:
                    _RotateCamera(CameraMotionDirection.Left);
                    break;
                case InputKeys.cameraRotateRight:
                    _RotateCamera(CameraMotionDirection.Right);
                    break;
                case InputKeys.cameraRotateUp:
                    _RotateCamera(CameraMotionDirection.Up);
                    break;
                case InputKeys.cameraRotateDown:
                    _RotateCamera(CameraMotionDirection.Down);
                    break;
                case InputKeys.planeTexture:
                    _AddBoat();
                    _SetLightFar();
                    _SetCameraDefault();
                    break;
                case InputKeys.chicken:
                    _AddChicken();
                    _SetLightFar();
                    _SetCameraDefault();
                    break;
                case InputKeys.box:
                    _AddCube();
                    _SetLightNear();
                    _SetCameraDefault();
                    break;
                case InputKeys.justLine:
                    _AddLineCube();
                    _SetLightNear();
                    _SetCameraDefault();
                    break;
                case InputKeys.reverseLightsOnState:
                    _ReverseLightState();
                    break;
                case InputKeys.objectRotateLeft:
                    _RotateObjects(_objectRotationAxisVertical, _objectRotationAngleVertical);
                    break;
                case InputKeys.objectRotateRight:
                    _RotateObjects(_objectRotationAxisVertical, -_objectRotationAngleVertical);
                    break;
                case InputKeys.objectRotateHorizontal:
                    _RotateObjects(_objectRotationAxisHorizantal, -_objectRotationAngleHorizantal);
                    break;
            }
            return true;
        }

        private void _ReverseLightState()
        {
            LightsContainer lightsContainer = LightsContainer.GetInstance();
            lightsContainer.lightOn = !lightsContainer.lightOn;
        }

        private void _SetLightNear()
        {
            _SetFirstLightPosition(_lightNearPosition);
        }
        private void _SetLightFar()
        {
            _SetFirstLightPosition(_lightFarPosition);
        }

        private void _SetFirstLightPosition(Vector3f position) 
        {
            PointLight light = LightsContainer.GetInstance().GetLight(0);
            if (light != null)
            {
                light.Trans.position = position;
            }
        }       

        private void _SetCameraDefault()
        {
            Camera mainCamera = _world.GetMainCamera();
            if (mainCamera == null)
            {
                return;
            }
            mainCamera.near = _cameraDefaultNear;
            mainCamera.far = _cameraDefaultFar;
            mainCamera.InitProjectionMatrix();

            mainCamera.Trans.position = _cameraDefaultPosition;
            mainCamera.Trans.LookAt(0, 0, 0);
        }
     
        private void _RotateObjects(Vector3f axis, float angle)
        {
            int gameObjectsCount = _world.GameObjectCount;
            for (int i = 0; i < gameObjectsCount; i++)
            {
                GameObject obj = _world.GetGameObject(i);
                obj.Trans.Rotate(axis, angle);
            }
        }
        private void _AddCube()
        {
            _world.Clear();
            if (_cachedCube == null)
            {
                _cachedCube = new GameObject();
                _cachedCube.SetMesh(MeshGenerator.CreateFaceCube());
            }
            _world.AddGameObject(_cachedCube);
        }

        private void _AddLineCube()
        {
            _world.Clear();
            if (_cachedLineCube == null)
            {
                _cachedLineCube = new GameObject();
                _cachedLineCube.SetMesh(MeshGenerator.CreateLineCube());
            }
            _world.AddGameObject(_cachedLineCube);
        }

        private void _AddBoat()
        {
            _world.Clear();
            if (_cachedBoat == null)
            {
                _cachedBoat = _CreateGameObject(ObjSrc.PlaneObjPath, TextureSrc.PlaneTexturePath);
            }
            _world.AddGameObject(_cachedBoat);
        }

        private void _AddChicken()
        {
            _world.Clear();
            if (_cachedChicken == null)
            {
                _cachedChicken = _CreateGameObject(ObjSrc.ChickenObjPath, TextureSrc.ChickenTexturePath); ;
            }
            _world.AddGameObject(_cachedChicken);
        }

        private GameObject _CreateGameObject(string objPath, string texturePath)
        {
            Mesh mesh = ObjLoader.LoadMesh(objPath);
            if (mesh == null)
            {
                return null;
            }
            Texture texture = Texture.Create(texturePath);

            if (texture == null)
            {
                return null;
            }
            GameObject result = new GameObject();
            Vector3f[] vertex = mesh.vertices;
            result.SetMesh(mesh);
            result.SetTexture(texture);
            return result;
        }

        private static readonly Vector3f _objectRotationAxisVertical = new Vector3f(0,1,0);

        private static readonly float _objectRotationAngleVertical = 20;

        private static readonly Vector3f _objectRotationAxisHorizantal = new Vector3f(1,0,0);

        private static readonly Vector3f _lightNearPosition = new Vector3f(0,4,0);

        private static readonly Vector3f _lightFarPosition = new Vector3f(0,150,0);

        private static readonly float _objectRotationAngleHorizantal = 20;

        private static readonly float _cameraDefaultNear = 1;

        private static readonly float _cameraDefaultFar = 100;

        private static readonly Vector3f _cameraDefaultPosition = new Vector3f(5, 5, 5);

        private static readonly float _cameraTranslationFastSpeed = 0.3f;

        private static readonly float _cameraTranslationSlowSpeed = 0.03f;

        private static readonly float _cameraRotationAngle = 10;

        private GameObject _cachedBoat;

        private GameObject _cachedCube;

        private GameObject _cachedLineCube;

        private GameObject _cachedChicken;

        private World _world;
    }
}
