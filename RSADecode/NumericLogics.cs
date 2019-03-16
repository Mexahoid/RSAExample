using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace RSAExample
{
    /// <summary>
    /// Синглтон NumericLogics
    /// </summary>
    public class NumericLogics
    {
        /// <summary>
        /// Экземпляр синглтона NumericLogics
        /// </summary>
        private static NumericLogics _instance;

        /// <summary>
        /// Возвращает экземпляр синглтона класса NumericLogics.
        /// </summary>
        public static NumericLogics Instance => _instance ?? (_instance = new NumericLogics());

        /// <summary>
        /// Закрытый конструктор класса.
        /// </summary>
        private NumericLogics()
        {
        }

        /// <summary>
        /// Метод парсинга числа в ulong.
        /// </summary>
        /// <param name="txt">Текстовое представление числа.</param>
        /// <returns>Возвращает ulong.</returns>
        public ulong ParseNum(string txt)
        {
            if (!ulong.TryParse(txt, out ulong u))
                throw new FormatException("Неверный формат числа.");
            return u;
        }

        /// <summary>
        /// Метод парсинга шифротекста в число BigInteger;
        /// </summary>
        /// <param name="sC">Текстовое представление шифротекста.</param>
        /// <returns>Возвращает BigInteger.</returns>
        public BigInteger ParseCipher(string sC)
        {
            if (!BigInteger.TryParse(sC, out BigInteger a))
                throw new FormatException("Неверный формат шифротекста.");
            return a;
        }

        /// <summary>
        /// Метод выдергивания блока чисел, меньших n.
        /// </summary>
        /// <param name="input">Входной StringBuilder с кодом.</param>
        /// <param name="sN">n</param>
        /// <returns>Возвращает ulong.</returns>
        public ulong CheckParseBlock(StringBuilder input, string sN)
        {
            // Попробовать взять sNl символов.
            ulong snulong = ParseNum(sN);

            StringBuilder sb = new StringBuilder();
            int snl = sN.Length;
            ulong sbl = 0;
            if (input.Length < snl)
            {
                snl = input.Length;
            }
            for (int i = 0; i < snl; i++)
            {
                sb.Append(input[i]);
            }
            sbl = ParseNum(sb.ToString());

            while (sbl > snulong)
            {
                sb = new StringBuilder();
                snl--;
                for (int i = 0; i < snl; i++)
                {
                    sb.Append(input[i]);
                }
                sbl = ParseNum(sb.ToString());
            }

            input.Remove(0, snl);
            return sbl;
        }
    }
}
