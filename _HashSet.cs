using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Castom_Collection
{
    // ? indexer
    public class _HashSet<T>
    {
        public struct Slot
        {
            internal int hashCode;
            internal int next;
            internal T value;
        }

        private int[] _buckets;
        private Slot[] _slots;
        private int _count;
        private int _freeList;
        private int _lastIndex;
        private int _version;

        public int Count => _count;

        public _HashSet() : this(0) {}

        public _HashSet(int capacity)
        {
            if (capacity < 0)
                throw new ArgumentOutOfRangeException();
            Intilizie(capacity);
        }

        public bool Add(T value)
        {
            return AddIfNotPresent(value);
        }

        private bool AddIfNotPresent(T value)
        {
            if (_buckets == null)
                Intilizie(0);

            int hashCode = GetHashCode(value);
            int bucket = hashCode % _buckets.Length;

            for (int i = _buckets[hashCode % _buckets.Length] - 1; i >= 0; i = _slots[i].next)
            {
                if (_slots[i].hashCode == hashCode && _slots[i].value.Equals(value))
                {
                    return false;
                }
            }

            int index;
            if(_freeList >= 0)
            {
                index = _freeList;
                _freeList = _slots[index].next;
            }
            else
            {
                if(_lastIndex == _slots.Length)
                {
                    IncreaseCapacity();
                    bucket = hashCode % _slots.Length;
                }
                index = _lastIndex;
                _lastIndex++;   
            }

            _slots[index].hashCode = hashCode;
            _slots[index].value = value;
            _slots[index].next = _buckets[bucket] - 1;
            _buckets[bucket] = index + 1;
            _count++;
            _version++;

            return true;
        }

        public bool Contains(T item)
        {
            if (_buckets != null)
            {
                int hashCode = GetHashCode(item);                
                for (int i = _buckets[hashCode % _buckets.Length] - 1; i >= 0; i = _slots[i].next)
                {
                    if (_slots[i].hashCode == hashCode && _slots[i].value.Equals(item))
                    {
                        return true;
                    }
                }
            }            
            return false;
        }

        public void Clear()
        {
            if (_lastIndex > 0)
            {
                Array.Clear(_slots, 0, _lastIndex);
                Array.Clear(_buckets, 0, _buckets.Length);
                _lastIndex = 0;
                _count = 0;
                _freeList = -1;
            }
            _version++;
        }

        public bool TryGetValue(T equalValue, out T actualValue)
        {
            if (_buckets != null)
            {
                int i = IndexOf(equalValue);
                if (i >= 0)
                {
                    actualValue = _slots[i].value;
                    return true;
                }
            }
            actualValue = default(T);
            return false;
        }

        public int IndexOf(T item)
        {           
            int hashCode = GetHashCode(item);
            for (int i = _buckets[hashCode % _buckets.Length] - 1; i >= 0; i = _slots[i].next)
            {
                if ((_slots[i].hashCode) == hashCode && _slots[i].value.Equals(item))
                {
                    return i;
                }
            }            
            return -1;
        }

        private int GetHashCode(T item)
        {
            if (item == null)            
                return 0;            
            return item.GetHashCode() & 0x7FFFFFFF;
        }

        private void Intilizie(int capacity)
        {
            int size = HashHelper.GetPrime(capacity);
            _buckets = new int[size];            
            _slots = new Slot[size];
            _freeList = -1;
        }

        private void IncreaseCapacity()
        {
            int newSize = HashHelper.CurrentPrime;
            if (newSize <= _count)
                throw new ArgumentOutOfRangeException();
            SetCapacity(newSize);
        }

        private void SetCapacity(int size)
        {
            Slot[] newSlots = new Slot[size];
            if (_slots != null)
            {
                Array.Copy(_slots, 0, newSlots, 0, _lastIndex);
            }

            int[] newBuckets = new int[size];
            for (int i = 0; i < _lastIndex; i++)
            {
                int bucket = newSlots[i].hashCode % size;
                newSlots[i].next = newBuckets[bucket] - 1;
                newBuckets[bucket] = i + 1;
            }

            _slots = newSlots;
            _buckets = newBuckets;
        }
    }
}
