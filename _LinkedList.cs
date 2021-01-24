using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Castom_Collection
{
    public class _LinkedList<T> : IEnumerable<T>
    {
        Node<T> _head;
        Node<T> _tail;
        int _count;

        public int Count => _count;
        public bool IsEmpty => (_count == 0);

        public void Add(T data)
        {
            Node<T> node = new Node<T>(data);
            if (_head == null)
                _head = node;
            else
                _tail.Next = node;
            _tail = node;
            _count++;
        }        

        public bool Remove(T data)
        {
            Node<T> current = _head;
            Node<T> previous = null;

            while (current != null)
            {
                if (current.Data.Equals(data))
                {
                    if (previous != null)
                    {
                        previous.Next = current.Next;
                        if (current.Next == null)
                            _tail = previous;
                    }
                    else
                    {
                        _head = _head.Next;
                        if (_head == null)
                            _tail = null;
                    }
                    _count--;
                    return true;
                }
                previous = current;
                current = current.Next;
            }

            return false;
        }

        public void Clear()
        {
            _head = null;
            _tail = null;
            _count = 0;
        }

        public void AppendFirst(T data)
        {
            Node<T> node = new Node<T>(data);
            node.Next = _head;
            _head = node;
            if (_count == 0)
                _tail = _head;
            _count++;
        }

        public bool Contains(T data)
        {
            Node<T> current = _head;
            while (current != null)
            {
                if (current.Data.Equals(data))
                    return true;
                current = current.Next;
            }
            return false;
        }

        public IEnumerator<T> GetEnumerator()
        {
            Node<T> current = _head;
            while (current != null)
            {
                yield return current.Data;
                current = current.Next;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)this).GetEnumerator();
        }
    }
}
