using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Castom_Collection
{
    public class SuperNode<T>
    {
        public SuperNode(T data)
        {
            Data = data;
        }

        public T Data { get; set; }
        public SuperNode<T> Next { get; set; }
        public SuperNode<T> Previous { get; set; }
    }
}
