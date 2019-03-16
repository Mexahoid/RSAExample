using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSAExample
{
    /// <summary>
    /// Синглтон Debugger
    /// </summary>
    public class Debugger
    {
        /// <summary>
        /// Экземпляр синглтона Debugger
        /// </summary>
        private static Debugger _instance;

        /// <summary>
        /// Список логов.
        /// </summary>
        private IList<string> _logs;

        /// <summary>
        /// Очищает список логов.
        /// </summary>
        public void Clear()
        {
            _logs.Clear();
        }

        /// <summary>
        /// Возвращает экземпляр синглтона класса Debugger.
        /// </summary>
        public static Debugger Instance => _instance ?? (_instance = new Debugger());

        /// <summary>
        /// Флаг для сохранения логов в файл.
        /// </summary>
        public bool IsLogging { get; set; }

        /// <summary>
        /// Закрытый конструктор класса.
        /// </summary>
        private Debugger()
        {
            _logs = new List<string>();
        }

        public void Log(object o)
        {
            _logs.Add(o.ToString());
        }

        public void GenerateLog()
        {
            if (!IsLogging)
                return;
            using (StreamWriter sw = new StreamWriter("logs.txt"))
            {
                foreach (string log in _logs)
                {
                    sw.WriteLine(log);
                }
            }
        }
    }
}
