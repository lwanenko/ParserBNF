using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQL
{
    static class INSERT_Listener
    {

        /// <summary>
        /// Запускає обробник для INSERT
        /// </summary>
        /// <param name="insert"></param>
        public static void Start(Lexem insert)
        {
            string tableName = null;
            foreach (var cur in insert.children)
            {
                if (cur == null) continue;
                if (cur.name == "<TableName>")
                {
                    tableName = insert.code.get(cur.pos_start, cur.pos - 1);
                    break;
                }
            } 

            List<Lexem> values = new List<Lexem>();
            Lexem val = new Lexem();
            foreach (var cur in insert.children)
            {
                if (cur == null) continue;
                if (cur.name == "<VALUES>")
                {
                    val = cur;
                    break;
                }
            }

            foreach (var cur in val.children)
            {
                if (cur == null) continue;
                if (cur.name == "<value>")
                {
                    values.Add(cur);
                    continue;
                }
            }

            DB.Insert(tableName, values);
            Console.WriteLine("Insert +");
        }
    }
}
