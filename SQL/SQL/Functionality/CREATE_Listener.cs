using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQL
{
    class CREATE_Listener
    {

        /// <summary>
        /// Запускає обробник, операції CREATE
        /// </summary>
        /// <param name="create"></param>
        public static void Start(Lexem create)
        {
            string tableName = null;
            foreach (var cur in create.children)
            {
                if (cur == null) continue;
                if (cur.name == "<TableName>")
                {
                    tableName = create.code.get(cur.pos_start, cur.pos - 1);
                    break;
                }      
            }

            List<Column> columns = new List<Column>();
            string curCulumn = null;
            int i = 0;

            foreach (var cur in create.children)
            {
                if (cur == null) continue;

                if (cur.name == "<ColumnName>")
                {
                    curCulumn = create.code.get(cur.pos_start, cur.pos - 1);
                    i++;
                    continue;
                }
                if (cur.name == "<DataType>")
                {
                    columns.Add( new Column(curCulumn, create.code.get(cur.pos_start, cur.pos - 1)) );
                    continue;
                }
                if (cur.name == "<size>")
                {
                    foreach (var curSize in cur.children)
                        if (cur.name == "<int_value>")
                        {
                            columns[i].AddSize(create.code.get(curSize.pos_start, curSize.pos - 1) );
                            break;
                        }                  
                    continue;
                }
                if (cur.name == "<PrimeryColumnName>")
                {
                    foreach (var column in columns)
                        column.IsPrimeryKey(create.code.get(cur.pos_start, cur.pos - 1));
                    break;
                }
            }
            DB.CreateTable(tableName, columns);
            Console.WriteLine("Create +");
        }
    }
}
