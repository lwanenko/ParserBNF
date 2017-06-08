using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQL
{
    /// <summary>
    /// Зберігає код програми,яку інтерпретують, та необхідні(допоміжні) функції  
    /// </summary>
    
    class Code
    {
        #region VAR
        /// <summary>
        /// код який досліджується
        /// </summary>
        public string code;
        #endregion


        /// <summary>
        /// Визначає довжину коду
        /// </summary>
        /// <returns>
        /// Повертає довжину коду
        /// </returns>
        public int length()
        {
            return code.Length;
        }

        /// <summary>
        /// Отримання потрібного по порядку символу code
        /// </summary>
        /// <param name="i"> індекс </param>
        /// <returns>символ вказаного індексу або помилка "CODE INDEX ERROR!!!" </returns>
        private char get(int i)
        {
            if (i > -1 && i < code.Length)
                return code[i];
            else
                throw new Exception("CODE INDEX ERROR!!!");
        }
        
        /// <summary>
        ///  Змінюю значення code
        /// </summary>
        /// <param name="s"> майбутнє значення </param>
        public  void set(string s)
        {
            code = s + " ";
        }

        public  string get(int pos_start, int pos)
        {
            return code.Substring(pos_start, pos - pos_start + 1);
        }

        #region INDEXATOR

        /// <summary>
        /// Отримання потрібного по порядку символу code
        /// </summary>
        /// <param name="i"> індекс </param>
        /// <returns>символ вказаного індексу або помилка "CODE INDEX ERROR!!!" </returns>
        public char this[int i] 
        {
            get 
            {
                return get(i);
            }
        }

        /// <summary>
        /// Перевіряє чи продовжується після певної позиції строка s
        /// </summary>
        /// <param name="index_start"> Індекс старту </param>
        /// <param name="s"> рядок який перевіряється</param>
        /// <returns>
        /// true  => продовжується
        /// false => не продовжується
        /// </returns>
        public bool this[int index_start, string s] 
        {
            get 
            {
                try
                {
                    for (int i = 0; i < s.Length; i++)
                        if (this[i + index_start] != s[i])
                            return false;
                }
                catch
                {
                    return false;
                }
                return true;
            }
        }
        
        #endregion
    }
}
