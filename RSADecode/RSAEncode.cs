using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace RSAExample
{
    /// <summary>
    /// Класс-шифровщик шифротекста RSA.
    /// </summary>
    public class RSAEncode
    {
        /// <summary>
        /// Экземпляр класса RSAEncode.
        /// </summary>
        private static RSAEncode _instance;

        /// <summary>
        /// Возвращает экземпляр синглтона класса RSAEncode.
        /// </summary>
        public static RSAEncode Instance => _instance ?? (_instance = new RSAEncode());

        /// <summary>
        /// Закрытый конструктор класса.
        /// </summary>
        private RSAEncode() { }

        /// <summary>
        /// Метод парсинга числа в ulong.
        /// </summary>
        /// <param name="txt">Текстовое представление числа.</param>
        /// <returns>Возвращает ulong.</returns>
        private ulong ParseNum(string txt)
        {
            if (!ulong.TryParse(txt, out ulong u))
                throw new FormatException("Неверный формат числа.");
            return u;
        }

        /// <summary>
        /// Шифрует сообщение по RSA.
        /// </summary>
        /// <param name="sN">Открытый ключ.</param>
        /// <param name="sE">Открытая экспонента.</param>
        /// <param name="sS">Строка текста для шифрования.</param>
        /// <returns>Возвращает зашифрованный текст.</returns>
        public string EncodeRSA(string sN, string sE, string sS)
        {
            StringBuilder sb = new StringBuilder();
            ulong n = ParseNum(sN);
            ulong e = ParseNum(sE);

            BigInteger m = 0;
            foreach (char t in sS)
            {
                m *= 100;
                char ch = t;
                int ci = ch;
                m += ci;
            }

            string sc = m.ToString();


            for (int i = 0; i < sc.Length; i += sN.Length - 1)
            {
                int l = sN.Length - 1;
                if (i + l > sc.Length)
                {
                    l -= i + l - sc.Length;
                }

                string substr = sc.Substring(i, l);
                BigInteger mm = BigInteger.Parse(substr);
                BigInteger c = BigInteger.ModPow(mm, e, n);

                sb.Append(c.ToString());
            }


            return sb.ToString();
        }
    }
}
