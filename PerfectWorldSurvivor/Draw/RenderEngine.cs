using System.Drawing;
using System;

namespace PerfectWorldSurvivor.Draw
{
    public class RenderEngine : IDisposable
    {
        public RenderEngine(Bitmap bitmap)
        {
            if (bitmap == null)
            {
                throw new ArgumentException("bitmap should not be null in class RenderEngine");
            }
            _viewportWidth = bitmap.Width;
            _viewportHeight = bitmap.Height;
            float defaultFieldOfView = 67;
            _world = new World(defaultFieldOfView);
            _batch = new Batch(bitmap,_world);
            _worldEventController = new WorldEventController(_world);
        }
      
        public void Render()
        {
            _batch.Render();
        }

        public static float ViewportWidth
        {
            get { return _viewportWidth; }
        }

        public static float ViewportHeight
        {
            get { return _viewportHeight; }
        }

        public void Dispose()
        {
            _batch.Dispose();
        }

        private static float _viewportWidth;

        private static float _viewportHeight;

        private Batch _batch;

        private World _world;

        private WorldEventController _worldEventController;
    }
}
