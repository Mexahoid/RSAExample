using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSAExample
{
    /// <summary>
    /// Синглтон EratosthenesSieve
    /// </summary>
    public class EratosthenesSieve
    {
        /// <summary>
        /// Экземпляр синглтона EratosthenesSieve
        /// </summary>
        private static EratosthenesSieve _instance;

        private IList<int> _last;
        private int _lastUpperLimit, _lastLowerLimit;

        /// <summary>
        /// Возвращает экземпляр синглтона класса EratosthenesSieve.
        /// </summary>
        public static EratosthenesSieve Instance => _instance ?? (_instance = new EratosthenesSieve());

        /// <summary>
        /// Закрытый конструктор класса.
        /// </summary>
        private EratosthenesSieve(){}

        private int ApproximateNthPrime(int nn)
        {
            double n = (double)nn;
            double p;
            if (nn >= 7022)
            {
                p = n * Math.Log(n) + n * (Math.Log(Math.Log(n)) - 0.9385);
            }
            else if (nn >= 6)
            {
                p = n * Math.Log(n) + n * Math.Log(Math.Log(n));
            }
            else if (nn > 0)
            {
                p = new int[] { 2, 3, 5, 7, 11 }[nn - 1];
            }
            else
            {
                p = 0;
            }
            return (int)p;
        }
        
        private BitArray SieveOfEratosthenes(int limit)
        {
            BitArray bits = new BitArray(limit + 1, true)
            {
                [0] = false,
                [1] = false
            };
            for (int i = 0; i * i <= limit; i++)
            {
                if (!bits[i])
                    continue;
                for (int j = i * i; j <= limit; j += i)
                {
                    bits[j] = false;
                }
            }
            return bits;
        }

        /// <summary>
        /// Возвращает список простых чисел.
        /// </summary>
        /// <param name="upperLimit">Верхний предел.</param>
        /// <param name="lowerLimit">Нижний предел.</param>
        /// <returns>Возвращает список простых чисел.</returns>
        public IList<int> GeneratePrimesSieveOfEratosthenes(int upperLimit, int lowerLimit = -1)
        {


            if (_lastUpperLimit == upperLimit && _lastLowerLimit == lowerLimit)
                return _last;

            _lastLowerLimit = lowerLimit;
            _lastUpperLimit = upperLimit;

            //int limit = ApproximateNthPrime(n);
            BitArray bits = SieveOfEratosthenes(upperLimit);
            var primes = new List<int>();

            for (int i = 0, found = 0; i < upperLimit ; i++)
            {
                if (!bits[i])
                    continue;
                if (i > lowerLimit)
                    primes.Add(i);
                found++;
            }

            _last = primes;

            return primes;
        }

        /// <summary>
        /// Возвращает одное простое число из списка.
        /// </summary>
        /// <param name="input">Список простых чисел.</param>
        /// <returns>Возвращает int.</returns>
        public int GetRandomPrimeInList(IList<int> input)
        {
            Random rnd = new Random(DateTime.UtcNow.Millisecond);
            int rand = rnd.Next(0, input.Count);
            return input[rand];
        }
    }
}
