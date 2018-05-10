using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PerfectWorldSurvivor.utils
{
    public class PerfectArray<T>
    {
        public int Count { get { return _count; } }

        public PerfectArray()
        {
            _data = new T[16];
            _count = 0;
        }

        public PerfectArray(int capacity)
        {
            _data = new T[capacity];
            _count = 0;
        }

        public T this[int index]
        {
            get { return _data[index]; }
        }

        public void Add(T value)
        {
           
            if (_count >= _data.Length)
            {
                
                T[] newArra = new T[_count * 2];
                Array.Copy(_data, newArra, _count);
                _data = newArra;
            }
            _data[_count] = value;
            _count++;
        }

        public T[] ToArray()
        {
            T[] newArra = new T[_count];
            Array.Copy(_data, newArra, _count);
            return newArra;
        }

        private int _count;

        private T[] _data;
    }
}
