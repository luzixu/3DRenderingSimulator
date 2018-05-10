using System.Collections.Generic;

namespace PerfectWorldSurvivor.UI
{
    public class KeyMessageController
    {
        public static KeyMessageController GetInstance()
        {
            if (_instance == null)
            {
                _instance = new KeyMessageController();
            }
            return _instance;
        }

        public void AddKeyEvent(char key)
        {
            if (!_keys.Contains(key))
            {
                _keys.Add(key);
            }
        }

        public void AddKeyDownHanlder(KeyDownHanlder handler)
        {
            _keyDownHandler += handler;
        }

        public void HandleKeyEvent()
        {
            if (_keyDownHandler == null)
            {
                return;
            }
            int keysLen = _keys.Count;
            for(int i = 0; i < keysLen; i++) 
            {
                 _keyDownHandler(_keys[i]);
            }
            _keys.Clear();
        }
        private KeyMessageController()
        {
            _keys = new List<char>();
        }

        public delegate bool KeyDownHanlder(char keyChar);

        private static KeyMessageController _instance;

        private List<char> _keys;

        private KeyDownHanlder _keyDownHandler;

    }
}
