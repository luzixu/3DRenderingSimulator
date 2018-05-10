using PerfectWorldSurvivor.Draw;


namespace PerfectWorldSurvivor.Model
{
    public class PointLight
    {
        public enum Type
        {
            Point,
            Directional
        }
      
        public Transform Trans
        {
            get { return _transform; }
        }

        public PointLight()
        {
            _transform = new Transform();
        }

        public Type lightType;

        public Colorf color;

        public bool perPixel;

        private Transform _transform;
    }
}
