using System;
using System.Collections.Generic;
using System.Xml.Linq;  
using System.Text;
using System.Threading.Tasks;

namespace SQL
{   
    [Serializable]
    class Table
    {
        #region VAR
        
        /// <summary>
        /// Масив де буде зберігатися таблиця бд
        /// </summary>
        public List<string[]>  table;

        /// <summary>
        /// Ім'я таблиці
        /// </summary>
        public string name;
        
        /// <summary>
        /// Для зберігання імен атрибутів
        /// </summary>
        public List<Column> columns;

        #endregion

        #region CTOR

        /// <summary>
        /// Створює таблицю з кореневим елементом
        /// </summary>
        /// <param name="name"> Ім'я майбутньої таблиці</param>
        /// <param name="columns"> Список імен колонок </param>
        public Table(string name, List<Column> columns)
        {
            this.name = name;
            this.columns = columns;

    
            table = new List<string[]>();
        }

        /// <summary>
        /// Для серіалізації
        /// </summary>
        public Table()
        {
        }

        #endregion

        #region ADD ROW

        public void AddRow(List<Lexem> Values)
        {
            var row = new List<string>();
            int  flag = 0 ;//позначає чи вже відбулося присвоєння значення ключу
            for (int i = 0; i < columns.Count; i++)
            {
               // if (!columns[i].ValueIsType(Values[i - flag]))
                if (columns[i].primeryKey)
                {
                    row.Add( Convert.ToString(columns[i].LastPrimeryKeyValue()));
                    flag++;
                    continue;
                }
                if (!columns[i].ValueIsType(Values[i - flag]))
                {
                    Console.WriteLine("'" + Values[i - flag].getLexemCode() + "'");
                    var b = columns[i].ValueIsType(Values[i - flag]);
                    throw new Exception("Error type");
                } 
                   

                row.Add( Values[i - flag].code.get (Values[i - flag].pos_start, Values[i - flag].pos - 1));
            }
            table.Add(row.ToArray());
                
        }

        public void AddRow(string[] row)
        {
            table.Add(row);
        }

        #endregion

        #region CONSOLE WRITE

        /// <summary>
        /// Метод, який відображає таблицю в консолі
        /// </summary>
        public void WriteTable()
        {
            int[] maxSize = new int[columns.Count];
            
            for (int i = 0; i < maxSize.Length; i++)
                maxSize[i] = columns[i].name.Length;           

            for (int i = 0; i < columns.Count; i++)
            {            
                foreach(var cur in table)
                {
                    if (maxSize[i] < cur[i].Length)
                        maxSize[i] = cur[i].Length;
                }
            }

            int fullSize = 3;
            foreach (var cur in maxSize)
            {
                fullSize = fullSize + cur + 1;
            }

            string r = "";
            for (int i = 0; i < fullSize; i++)
            {
                r += "_";
            }
            Console.WriteLine(r);

            string s = "||";
            for (int i = 0; i < columns.Count; i++)
            {
                s = s + columns[i].name + getSpace(maxSize[i] - columns[i].name.Length) +"|";
            }
            Console.WriteLine(s + "|");
            writeSharp(fullSize);

            foreach(var cur in table)
            {
                s = "||";
                for (int i = 0; i < columns.Count; i++)
                {
                        if (cur[i] != null)
                            s = s + cur[i] + getSpace(maxSize[i] - cur[i].Length) + "|";
                        else
                            s = s + getSpace(maxSize[i]) + "|";
                }
                Console.WriteLine(s + "|");

            }
            writeSharp(fullSize);

        }
        
        /// <summary>
        /// Дає рядок зпробілами вказаної довжини
        /// застосовується для виведення таблиці
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        private string getSpace(int length)
        {
            string rez = "";
            for (int i = 0; i < length; i++)
            {
                rez += " "; 
            }
            return rez;
        }

        /// <summary>
        /// для розмежування коду
        /// </summary>
        /// <param name="length"></param>
        private void writeSharp(int length)
        {
            string rez = "";
            for (int i = 0; i < length; i++)
            {
                rez += "#";
            }
            Console.WriteLine(rez);
        }

        #endregion

        /// <summary>
        /// Повертає номер колонки з таким іменем
        /// </summary>
        /// <param name="nameColumn"></param>
        /// <returns></returns>
        public int getNumColumn(string nameColumn)
        {
            for (int i = 0; i < columns.Count; i++)
            {
                if (nameColumn == columns[i].name)
                    return i;
            }
            throw new Exception("Error name column");
        }

        /// <summary>
        /// повертає тип по імені колонки
        /// </summary>
        /// <param name="columnName"></param>
        /// <returns></returns>
        public string getTypeForColumn(string columnName)
        {
            foreach (var cur in columns)
                if (cur.name == columnName)
                    return cur.typeName;
            throw new Exception(columnName + " не присутній в цій таблиці");
        }

        /// <summary>
        /// сортує за певним параметром рядка таблиці
        /// </summary>
        /// <param name="nameColumn"></param>
        public void Sort(string nameColumn)
        {
            table.Sort(delegate (string[] row1, string[] row2)
            {
                 return row1[getNumColumn(nameColumn)].CompareTo(row2[getNumColumn(nameColumn)]);
            });
        }
    }
}
