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

        private ulong CheckParseBlock(StringBuilder input, string sN)
        {
            // Попробовать взять sNl символов.
            ulong snulong = ParseNum(sN);

            StringBuilder sb = new StringBuilder();
            int snl = sN.Length;
            for (int i = 0; i < snl; i++)
            {
                sb.Append(input[i]);
            }
            ulong sbl = ParseNum(sb.ToString());

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

            


            StringBuilder sb = new StringBuilder();
            ulong n = ParseNum(sN);
            ulong e = ParseNum(sE);

            StringBuilder m = new StringBuilder();
            foreach (char t in sS)
            {
                int ci = t;
                m.Append(ci.ToString());
            }

            string sc = m.ToString();
            StringBuilder ssb = new StringBuilder(sc);
            ulong sss = CheckParseBlock(ssb, sN);

            dbg.Log("Интовая форма");
            dbg.Log(sc);

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
                dbg.Log($"Подстрока от {i} до {i + l}");
                dbg.Log($"m = {mm}");
                dbg.Log($"c = {c}");

                sb.Append(c.ToString());
            }
            dbg.Log("Зашифрованное сообщение");
            dbg.Log(sb);
            dbg.Log('\n');

            dbg.GenerateLog();

            return sb.ToString();
        }
    }
}
