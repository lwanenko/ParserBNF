using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQL
{   
    /// <summary>
    /// Клас для обробки SELECT запитів
    /// </summary>
    class SELECT_Listener
    {
        static int selectCount = 0;


        public static void Start(Lexem select)
        {
            string tableName = "";
            bool distinct = false;

           // bool rezIsValue = false;

            var columns = new List<Column>();
            var forcolumns = new Lexem();
            foreach (var cur in select.children)
            {
                if (cur.name == "'DISTINCT'" || cur.name =="'distinct'")
                    distinct = true;
                if (cur.name == "<SELECT_ListNameCollumn>")
                    forcolumns = cur;
                if (cur.name == "<TableName>")
                {
                    tableName = cur.code.get(cur.pos_start , cur.pos -1);
                    columns = GetColumns(forcolumns, ref distinct , tableName);
                }
            }   
            var selectTable = new Table("select" + selectCount, columns);

            bool flag = false;
            foreach (var cur in select.children)
            {
                if (cur.name == "<WHERE>")
                {
                    flag = true;
                    Where_Listener.Start(ref selectTable, tableName, cur);
                    continue;
                }
                if (cur.name == "<ORDER_BY>")
                {
                    if (!flag)
                        GetAll(ref selectTable, tableName);
                    OrderBy_Listener.Start(ref selectTable, cur);
                    break;
                }
                    
            }

            if (distinct)
            {
                for (int i1 = 0; i1 < selectTable.table.Count - 1; i1++)
                {
                    for (int i2 = i1+1; i2 < selectTable.table.Count; i2++)
                    {
                        bool isFirst = true;
                        for (int ii = 0; ii < selectTable.table[i1].Length; ii++)
                        {
                            if (selectTable.table[i1][ii] != selectTable.table[i2][ii])
                            {
                                isFirst = false;
                                break;
                            }
                        }
                       if (isFirst)
                            selectTable.table.RemoveAt(i2);
                    }
                }
            }

            Console.WriteLine("SELECT +");

            Console.WriteLine(select.getLexemCode()+";");
            selectTable.WriteTable();
        }

        /// <summary>
        /// Додає всі можливі значення до таблиці відповіді
        /// </summary>
        /// <param name="selectTable"></param>
        /// <param name="tableName"></param>
        private static void GetAll(ref Table table, string tableName)
        {
            foreach (var cur in DB.DataBase)
            {
                if (cur.name == tableName)
                {   
                    foreach(var curRow in cur.table)
                        getRowForThisTable(curRow,cur.columns,ref table);
                }
            }
        }

        private static List<Column> GetColumns(Lexem list, ref bool distinct, string tableName)
        {
            var columns = new List<Column>();
            foreach (var cur in list.children)
            {
                if (cur.name == "'DISTINCT'" || cur.name == "'distinct'")
                    distinct = true;
                if (cur.name == "'*'")
                {
                    var t = DB.getTable(tableName);
                    columns = t.columns;
                }
                if (cur.name == "<ColumnName>")
                {
                    var t = DB.getTable(tableName);
                    var type = t.getTypeForColumn(cur.code.get(cur.pos_start, cur.pos - 1));
                    columns.Add( new Column(cur.getLexemCode(), type) );
                }
            }
            return columns;
        }

        
        /// <summary>
        /// повертає рядок інтерпритований під таблицю для SELECT
        /// </summary>
        /// <param name="row"> вхідний рядок</param>
        /// <param name="columns"> колонки вхідного рядку</param>
        /// <returns></returns>
        public static void getRowForThisTable(string[] row, List<Column> columns, ref Table thisTable)
        {
            string[] newRow = new string[thisTable.columns.Count];           
            for (int j = 0; j < thisTable.columns.Count; j++)
            {
                for (int i = 0; i< columns.Count; i++)
                {
               
                    if (columns[i].name == thisTable.columns[j].name)
                    {
                        newRow[j] = row[i];
                    }   
                }              
            }

            thisTable.AddRow(newRow);
        }
    }
}
