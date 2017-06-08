using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQL
{
    static class OrderBy_Listener
    {
        /// <summary>
        /// Сортує таблицб за вказаним параметром
        /// </summary>
        /// <param name="t"></param>
        /// <param name="l"></param>
        public static void Start(ref Table t, Lexem l)
        {
            try
            {
                foreach (var cur in l.children)
                {
                    if (cur.name == "<ColumnName>")
                    {
                        t.Sort(cur.getLexemCode());
                        break;
                    }
                }
            }
            catch
            {
                throw new Exception("Error Order By!!!");
            }
        }
    }
}
