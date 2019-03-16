using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

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
        private RSADecipher(){}

        /// <summary>
        /// Метод парсинга числа в ulong.
        /// </summary>
        /// <param name="txt">Текстовое представление числа.</param>
        /// <returns>Возвращает ulong.</returns>
        private ulong ParseNum(string txt)
        {
            if(!ulong.TryParse(txt, out ulong u))
                throw new FormatException("Неверный формат числа.");
            return u;
        }

        /// <summary>
        /// Метод парсинга шифротекста в число BigInteger;
        /// </summary>
        /// <param name="sC">Текстовое представление шифротекста.</param>
        /// <returns>Возвращает BigInteger.</returns>
        private BigInteger ParseCipher(string sC)
        {
            if (!BigInteger.TryParse(sC, out BigInteger a))
                throw new FormatException("Неверный формат шифротекста.");
            return a;
        }

        /// <summary>
        /// Превращает шифротекст в набор символов.
        /// </summary>
        /// <param name="sC">Строка шифротекста.</param>
        /// <returns>Возвращает char[]</returns>
        private char[] ConvertToChars(string sC)
        {
            if(sC.Length % 2 != 0)
                throw new FormatException("Шифротекст неверно преобразован.");

            char[] arr = new char[sC.Length / 2];

            for (int i = 0; i < sC.Length / 2; i++)
            {
                char first = sC[i * 2];
                char second = sC[i * 2+ 1];
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
            for (int k = 1;;k++)
            {
                BigInteger tphi = k * phi + 1;
                d = BigInteger.Divide(tphi, e);
                if (BigInteger.Multiply(d, e) == tphi)
                    break;
            }
            return new []{(ulong)d, p + 1, q + 1};
        }

        /// <summary>
        /// Факторизует число.
        /// </summary>
        /// <param name="num">Число, подвергаемое факторизации.</param>
        /// <returns>Возвращает List(ulong).</returns>
        private IList<ulong> Factorize(ulong num)
        {
            List<ulong> factors = new List<ulong>();
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

            var dbg = Debugger.Instance;
            dbg.Log("Дешифровка");
            dbg.Log("n = " + sN);
            dbg.Log("e = " + sE);
            dbg.Log("c = " + sC);

            ulong n = ParseNum(sN);
            ulong e = ParseNum(sE);
            ulong[] dpq = GetD(n, e);

            dbg.Log("d = " + dpq[0]);
            dbg.Log("p = " + dpq[1]);
            dbg.Log("q = " + dpq[2]);

            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < sC.Length; i += sN.Length)
            {
                int l = sN.Length;
                if (i + l > sC.Length)
                {
                    l -= i + l - sC.Length;
                }
                BigInteger c = ParseCipher(sC.Substring(i, l));
                BigInteger m = BigInteger.ModPow(c, dpq[0], n);
                dbg.Log($"Подстрока от {i} до {i + l}");
                dbg.Log($"m = {m}");
                dbg.Log($"c = {c}");
                BigInteger bc = BigInteger.ModPow(m, e, n);
                dbg.Log($"bc = {bc}");
                sb.Append(m);
            }

            dbg.Log("Интовая форма");
            dbg.Log(sb);
            try
            {
                var arr = ConvertToChars(sb.ToString());
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
