﻿using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace RSAExample
{
    /// <summary>
    /// Класс-дешифровщик шифротекста RSA.
    /// </summary>
    public class RSADecipher
    {
        /// <summary>
        /// Экземпляр класса RSADecipher.
        /// </summary>
        private static RSADecipher _instance;

        /// <summary>
        /// Возвращает экземпляр синглтона класса RSADecipher.
        /// </summary>
        public static RSADecipher Instance => _instance ?? (_instance = new RSADecipher());

        /// <summary>
        /// Закрытый конструктор класса.
        /// </summary>
        private RSADecipher() { }


        /// <summary>
        /// Превращает шифротекст в набор символов.
        /// </summary>
        /// <param name="sC">Строка шифротекста.</param>
        /// <returns>Возвращает char[]</returns>
        private char[] ConvertToChars(string sC)
        {
            if (sC.Length % 2 != 0)
            {
                throw new FormatException("Шифротекст неверно преобразован.");
            }

            char[] arr = new char[sC.Length / 2];

            for (int i = 0; i < sC.Length / 2; i++)
            {
                char first = sC[i * 2];
                char second = sC[i * 2 + 1];
                arr[i] = (char)int.Parse($"{first}{second}");
            }

            return arr;
        }

        /// <summary>
        /// Пытается получить d.
        /// </summary>
        /// <param name="n">Открытый ключ.</param>
        /// <param name="e">Открытая экспонента.</param>
        /// <returns>Возвращает массив из d, p и q.</returns>
        private ulong[] GetD(ulong n, ulong e)
        {
            IList<ulong> factors = Factorize(n);
            ulong p, q;

            if (factors.Count == 2)
            {
                p = factors[0] - 1;
                q = factors[1] - 1;
            }
            else
            {
                throw new Exception("n состоит не из двух простых чисел.");
            }

            BigInteger exp = e;
            BigInteger phi = p * q;
            BigInteger d;
            for (int k = 1; ; k++)
            {
                BigInteger tphi = k * phi + 1;
                d = BigInteger.Divide(tphi, e);
                if (BigInteger.Multiply(d, e) == tphi)
                {
                    break;
                }
            }
            return new[] { (ulong)d, p + 1, q + 1 };
        }

        /// <summary>
        /// Факторизует число.
        /// </summary>
        /// <param name="num">Число, подвергаемое факторизации.</param>
        /// <returns>Возвращает List(ulong).</returns>
        private IList<ulong> Factorize(ulong num)
        {
            //IList<int> lst = EratosthenesSieve.Instance.GeneratePrimesSieveOfEratosthenes((int)num);

            List<ulong> factors = new List<ulong>();
            //foreach (int i in lst)
            //{
            //    if (num % (ulong) i != 0)
            //        continue;
            //    factors.Add((ulong)i);
            //    num /= (ulong)i;
            //}

            for (ulong i = 2; i <= num; i++)
            {
                while (num % i == 0)
                {
                    factors.Add(i);
                    num /= i;
                }
            }
            return factors;
        }

        /// <summary>
        /// Расшифровывает RSA шифротекст.
        /// </summary>
        /// <param name="sN">Открытый ключ.</param>
        /// <param name="sE">Открытая экспонента.</param>
        /// <param name="sC">Шифротекст.</param>
        /// <returns>Возвращает расшифрованный текст.</returns>
        public string DecipherRSA(string sN, string sE, string sC)
        {
            StringBuilder deciphered = new StringBuilder();

            Debugger dbg = Debugger.Instance;
            dbg.Log("Дешифровка");
            dbg.Log("n = " + sN);
            dbg.Log("e = " + sE);
            dbg.Log("c = " + sC);

            NumericLogics nl = NumericLogics.Instance;

            ulong n = nl.ParseNum(sN);
            ulong e = nl.ParseNum(sE);
            ulong[] dpq = GetD(n, e);

            dbg.Log("d = " + dpq[0]);
            dbg.Log("p = " + dpq[1]);
            dbg.Log("q = " + dpq[2]);

            StringBuilder sb = new StringBuilder();


            StringBuilder ssb = new StringBuilder(sC);
            while (ssb.Length > 0)
            {
                ulong c = nl.CheckParseBlock(ssb, sN);

                BigInteger m = BigInteger.ModPow(c, dpq[0], n);
                dbg.Log($"Подстрока c: {c}");
                dbg.Log($"m = {m}");
                BigInteger bc = BigInteger.ModPow(m, e, n);
                dbg.Log($"bc = {bc}");

                sb.Append(m);
            }

            dbg.Log("Интовая форма");
            dbg.Log(sb);
            try
            {
                char[] arr = ConvertToChars(sb.ToString());
                foreach (char c in arr)
                {
                    deciphered.Append(c);
                }
            }
            catch
            {
                dbg.Log("Шифротекст неверно преобразован");
                dbg.Log('\n');
                dbg.GenerateLog();
                throw;
            }

            dbg.Log("Сообщение");
            dbg.Log(deciphered);
            dbg.Log('\n');

            dbg.GenerateLog();

            deciphered.Append($"\nd = {dpq[0]}\np = {dpq[1]}\nq = {dpq[2]}");

            return deciphered.ToString();
        }

    }
}
