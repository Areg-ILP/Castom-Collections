using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Castom_Collection
{
    public class SuperLinkedList<T>
    {
        private SuperNode<T> _head;
        private SuperNode<T> _tail;
        private int _count;

        public int Count => _count;
        public bool IsEmpty => (_count == 0);

        public void Add(T data)
        {
            SuperNode<T> node = new SuperNode<T>(data);

            if (_head == null)
                _head = node;
            else
            {
                _tail.Next = node;
                node.Previous = _tail;
            }
            _tail = node;
            _count++;            
        }

        public void AddFirst(T data)
        {
            SuperNode<T> node = new SuperNode<T>(data);
            SuperNode<T> temp = _head;
            node.Next = temp;
            _head = node;

            if (_count == 0)
                _tail = _head;
            else
                temp.Previous = node;
            _count++;
        }

        public bool Remove(T data)
        {
            SuperNode<T> current = _head;

            while(current != null)
            {
                if (current.Data.Equals(data))                
                    break;
                current = current.Next;
            }

            if (current != null)
            {
                if (current.Next != null)                
                    current.Next.Previous = current.Previous;                
                else                
                    _tail = current.Previous;                                
                if (current.Previous != null)                
                    current.Previous.Next = current.Next;                
                else                                   
                    _head = current.Next;                
                _count--;
                return true;
            }
            return false;
        }

        public bool Contains(T data)
        {
            SuperNode<T> current = _head;
            while (current != null)
            {
                if (current.Data.Equals(data))
                    return true;
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
    }
}
