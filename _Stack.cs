using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Castom_Collection
{
    public class _Stack<T>
    {
        private T[] _array;
        private int _size;
        private int _version;
        private const int _defaultCapacity = 4;

        public _Stack(int capacity = _defaultCapacity)
        {
            if (capacity <= 0)
                throw new ArgumentOutOfRangeException();

            _array = new T[capacity];
            _size = 0;
            _version = 0;
        }

        public int Count => _size;

        public void Push(T item)
        {
            if(_size == _array.Length)
            {
                T[] newArray = new T[(_array.Length == 0) ? _defaultCapacity : 2 * _array.Length];
                Array.Copy(_array, 0, newArray, 0, _size);
                _array = newArray;
            }
            _array[_size++] = item;
            _version++;
        }

        public T Peek()
        {
            if (_size == 0)
                throw new InvalidOperationException();
            return _array[_size - 1];
        }

        public T Pop()
        {
            if (_size == 0)
                throw new InvalidOperationException();
            _version++;
            T removed = _array[--_size];
            _array[_size] = default(T);
            return removed;
        }

        public T[] ToArray()
        {
            T[] objArray = new T[_size];
            int i = 0;
            while (i < _size)
            {
                objArray[i] = _array[_size - i - 1];
                i++;
            }
            return objArray;
        }

        public void Clear()
        {
            Array.Clear(_array, 0, _size);
            _size = 0;
            _version++;
        }
    }
}
