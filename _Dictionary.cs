using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Castom_Collection
{
    public class _Dictionary<TKey,TValue>
    {
        public struct Entry
        {
            public int hashCode;
            public int next;
            public TKey key;
            public TValue value;
        }

        private int[] _buckets;
        private Entry[] _entries;
        private int _count;
        private int _version;
        private int _freeList;
        private int _freeCount;

        public _Dictionary(): this(0) {}
        public _Dictionary(int capacity)
        {
            if (capacity > 0)
                Initialize(capacity);
        }

        public int Count => _count - _freeCount;

        public TValue this[TKey key]
        {
            get
            {
                int i = FindEntry(key);
                if (i >= 0) 
                    return _entries[i].value;
                throw new ArgumentOutOfRangeException();
            }
            set
            {
                Insert(key, value, false);
            }
        }

        private void Initialize(int capacity)
        {
            int size = GetNearPrime(capacity);
            _buckets = new int[size];
            for (int i = 0; i < _buckets.Length; i++)            
                _buckets[i] = -1;
            _entries = new Entry[size];
            _freeList = -1;
        }

        public void Add(TKey key,TValue value)
        {
            Insert(key, value, true);
        }

        private int FindEntry(TKey key)
        {
            if (key == null)
                throw new ArgumentNullException();

            if(_buckets != null)
            {
                int hashCode = key.GetHashCode() & 0x7FFFFFFF;
                for(int i = _buckets[hashCode % _buckets.Length]; i>= 0;i = _entries[i].next)
                {
                    if (_entries[i].hashCode == hashCode && _entries[i].key.Equals(key))
                    {
                        return i;
                    }
                }
            }
            return -1;
        }

        private void Insert(TKey key, TValue value, bool add)
        {
            if (key == null)
                throw new ArgumentNullException();

            if (_buckets == null) Initialize(0);
            int hashCode = key.GetHashCode() & 0x7FFFFFFF;
            int targetBucket = hashCode % _buckets.Length;

            for (int i = _buckets[targetBucket]; i >= 0; i = _entries[i].next)
            {
                if (_entries[i].hashCode == hashCode && _entries[i].key.Equals(key))
                {
                    if (add)
                    {
                        throw new ArgumentException();
                    }
                    _entries[i].value = value;
                    _version++;
                    return;
                }
            }

            int index;
            if (_freeCount > 0)
            {
                index = _freeList;
                _freeList = _entries[index].next;
                _freeCount--;
            }
            else
            {
                if (_count == _entries.Length)
                {
                    Resize();
                    targetBucket = hashCode % _buckets.Length;
                }
                index = _count;
                _count++;
            }

            _entries[index].hashCode = hashCode;
            _entries[index].next = _buckets[targetBucket];
            _entries[index].key = key;
            _entries[index].value = value;
            _buckets[targetBucket] = index;
            _version++;
        }           

        private void Resize()
        {
            //mb
        }

        private int GetNearPrime(int capacity)
        {
            if (IsPrime(capacity))
                return capacity;
            do
            {
                capacity++;
            } while (!IsPrime(capacity));            
            return capacity;
        }

        private bool IsPrime(int number)
        {
            if (number > 2)
            {
                for (int i = 2; i <= number / 2; i++)
                    if (number % i == 0)
                        return false;
                return true;
            }
            return false;
        }
    }
}
