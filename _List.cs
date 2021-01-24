using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Castom_Collection
{
    [Serializable]
    public class _List<T> : IList<T>
    {
        private const int _defaultCapacity = 4;
        private T[] _items;
        private int _size;
        private int _version;

        [NonSerialized]
        private T[] _tempArray;
        private int _capacity;              

        public _List(int capacity = _defaultCapacity)
        {
            if (capacity <= 0)
                throw new ArgumentOutOfRangeException();
            _capacity = capacity;
            _items = new T[_capacity];            
        }

        public T this[int index]
        {
            get
            {
                if (index >= _size || index < 0)
                    throw new ArgumentOutOfRangeException();
                return _items[index];
            }
            set
            {
                if (index >= _size || index < 0)
                    throw new ArgumentOutOfRangeException();
                _items[index] = value;
                _version++;
            }
        }

        public int Capacity => _capacity;
        public int Count => _size;
        public bool IsReadOnly => false;

        public void Add(T item)
        {
            if(_size == _items.Length) EnsureCapacity();            
            _items[_size++] = item;            
            _version++;
        }

        public void Clear()
        {
            if (_size > 0)
            {
                Array.Clear(_items, 0, _size);
                _size = 0;
            }
            _version++;
        }

        public bool Contains(T item)
        {
            for (int i = 0; i < _size; i++)
                if (item.Equals(_items[i]))
                    return true;
            return false;
        }

        public int IndexOf(T item)
        {
            for (int i = 0; i < _size; i++)
            {
                if (item.Equals(_items[i]))
                    return i;
            }
            return -1;
        }

        public void Insert(int index, T item)
        {
            if (index >= 0 && index <= _size)
            {
                _tempArray = _items;
                for (int i = 0; i < index; i++)
                    _items[i] = _tempArray[i];
                _items[index] = item;
                for (int i = index + 1; i < _capacity; i++)
                    _items[i] = _tempArray[i];
                _version++;
            }
            else
                throw new ArgumentOutOfRangeException();
        }

        public bool Remove(T item)
        {
            int index = IndexOf(item);
            if(index >= 0)
            {
                RemoveAt(index);
                return true;
            }
            return false;
        }

        public void RemoveAt(int index)
        {
            if(index >= 0 && index < _size)
            {
                _tempArray = new T[_capacity];
                for (int i = 0, j = 0; i < _capacity; i++)
                    if (i != index)
                        _tempArray[j++] = _items[i];
                _items = _tempArray;
                _tempArray = null;
                _version++;
            }
            else
                throw new ArgumentOutOfRangeException();
        }

        public IEnumerator<T> GetEnumerator()
        {
            return new Enumerator(this);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return new Enumerator(this);
        }

        private void EnsureCapacity()
        {
            _tempArray = _items;
            _capacity *= 2;
            _items = new T[_capacity];

            for (int i = 0; i < _tempArray.Length; i++)
                _items[i] = _tempArray[i];
            _tempArray = null;
        }

        public class Enumerator : IEnumerator<T>
        {
            private _List<T> _list;
            private int _version;
            private int _index;
            private T _current;

            public T Current => _current;

            object IEnumerator.Current
            {
                get
                {
                    if (_index == 0 || _index == _list._size + 1)
                        throw new InvalidOperationException();
                    return Current;
                }
            }

            public Enumerator(_List<T> list)
            {
                _list = list;
                _index = 0;
                _version = list._version;
                _current = default(T);
            }

            public void Dispose()
            {
                Reset();
            }

            public bool MoveNext()
            {
                _List<T> localList = _list;

                if (_version == _list._version && _index < localList._size)
                {
                    _current = localList._items[_index];
                    _index++;
                    return true;
                }

                return MoveNextRare();
            }

            private bool MoveNextRare()
            {
                if (_version != _list._version)
                    throw new InvalidOperationException();

                _index = _list._size + 1;
                _current = default(T);
                return false;
            }

            public void Reset()
            {
                _index = 0;
            }
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            //
        }
    }
}
