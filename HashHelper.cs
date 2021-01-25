using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Castom_Collection
{
    public static class HashHelper
    {
        // ? bac toxac parzer
        private static readonly int[] _primes = {
            3, 7, 11, 17, 23, 29, 37, 47, 59, 71, 89, 107, 131, 163, 197, 239, 293, 353, 431, 521, 631, 761, 919,
            1103, 1327, 1597, 1931, 2333, 2801, 3371, 4049, 4861, 5839, 7013, 8419, 10103, 12143, 14591,
            17519, 21023, 25229, 30293, 36353, 43627, 52361, 62851, 75431, 90523, 108631, 130363, 156437,
            187751, 225307, 270371, 324449, 389357, 467237, 560689, 672827, 807403, 968897, 1162687, 1395263,
            1674319, 2009191, 2411033, 2893249, 3471899, 4166287, 4999559, 5999471, 7199369
        };

        private static int _current;

        static HashHelper()
        {
            _current = 0;
        }

        public static int MinPrime => _primes[0];              
        public static int CurrentPrime => _primes[_current++];

        public static int GetPrime(int number)
        {
            if (IsPrime(number))
                return number;
            int nearPrime = GetNearPrime(number);
            _current = GetIndex(nearPrime);
            return _primes[_current++];            
        }

        private static bool IsPrime(int number)
        {
            if (number < 3)
                return false;
            for (int i = 2; i <= number / 2; i++)
                if (number % i == 0)
                    return false;
            return true;
        }

        private static int GetNearPrime(int number)
        {
            if (IsPrime(number))
                return number;
            do
            {
                number++;
            } while (!IsPrime(number));
            return number;
        }

        private static int GetIndex(int prime)
        {
            for (int i = 0; i < _primes.Length; i++)
                if (_primes[i] == prime)
                    return i;
            return -1;
        }
    }
}
