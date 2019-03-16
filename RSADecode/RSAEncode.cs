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
        /// Шифрует сообщение по RSA.
        /// </summary>
        /// <param name="sN">Открытый ключ.</param>
        /// <param name="sE">Открытая экспонента.</param>
        /// <param name="sS">Строка текста для шифрования.</param>
        /// <returns>Возвращает зашифрованный текст.</returns>
        public string EncodeRSA(string sN, string sE, string sS)
        {
            var dbg = Debugger.Instance;
            dbg.Clear();
            dbg.Log("Зашифровка");
            dbg.Log("n = " + sN);
            dbg.Log("e = " + sE);
            dbg.Log("text = " + sS);

            foreach (char c in sS)
            {
                if (c <= 99)
                    continue;
                dbg.Log("В тексте символы нижнего регистра.");
                dbg.Log('\n');
                dbg.GenerateLog();
                throw new ArgumentException("Текст содержит символы нижнего регистра.");
            }


            var nl = NumericLogics.Instance;

            StringBuilder sb = new StringBuilder();
            ulong n = nl.ParseNum(sN);
            ulong e = nl.ParseNum(sE);

            StringBuilder mm = new StringBuilder();
            foreach (char t in sS)
            {
                int ci = t;
                mm.Append(ci.ToString());
            }

            string sc = mm.ToString();

            dbg.Log("Интовая форма");
            dbg.Log(sc);

            StringBuilder ssb = new StringBuilder(sc);
            while (ssb.Length > 0)
            {
                ulong m = nl.CheckParseBlock(ssb, sN);

                BigInteger c = BigInteger.ModPow(m, e, n);
                dbg.Log($"Подстрока m: {m}");
                dbg.Log($"c = {c}");

                sb.Append(c.ToString());
            }

            /*for (int i = 0; i < sc.Length; i += sN.Length - 1)
            {
                int l = sN.Length - 1;
                if (i + l > sc.Length)
                {
                    l -= i + l - sc.Length;
                }

                string substr = sc.Substring(i, l);
                BigInteger mm = BigInteger.Parse(substr);
                BigInteger c = BigInteger.ModPow(mm, e, n);
                dbg.Log($"Подстрока от {i} до {i + l}");
                dbg.Log($"m = {mm}");
                dbg.Log($"c = {c}");

                sb.Append(c.ToString());
            }*/
            dbg.Log("Зашифрованное сообщение");
            dbg.Log(sb);
            dbg.Log('\n');

            dbg.GenerateLog();

            return sb.ToString();
        }
    }
}
