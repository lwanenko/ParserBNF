using System.Collections.Generic;
using System;

namespace SQL
{
    /// <summary>
    /// Клас, який реалізує представлення і зв'язок таблиць бази данних
    /// </summary>
    static class DB
    {
        #region VAR

        /// <summary>
        /// Список наявних таблиць
        /// </summary>
        public static List<Table> DataBase = new List<Table>();

        #endregion

        /// <summary>
        /// Створює ще одну таблицю
        /// </summary>
        /// <param name="name"></param>
        /// <param name="columns"></param>
        public static void CreateTable(string name, List<Column> columns)
        {
            foreach (var cur in DataBase)
            {
                if (cur.name == name)
                    throw new Exception( "Таке ім'я для таблиці вже використовується!!!" );
            }

            DataBase.Add(new Table(name, columns));
        }

        /// <summary>
        /// Перевіряє чи ім'я не належить до стандартних
        /// </summary>
        /// <param name="name"></param>
        private static  void standartName(string name)
        {
            if (name == "WHERE"  || name == "where"  ||
                name == "SELECT" || name == "select" ||
                name == "INSERT" || name == "insert" ||
                name == "CREATE" || name == "create" ||
                name == "STRING" || name == "string" ||
                name == "MAX"    ||  name == "ORDER" ||
                name == "MIN"    || name == "ON"     ||
                name == "VARCHAR"|| name == "varchar"||
                name == "FROM"   || name == "from"   ||
                name == "double" || name == "DOUBLE" ||
                name == "INT"    || name == "int"
                )
                throw new Exception("Таке ім'я не можна використовувати!!!");
        }



        /// <summary>
        /// Повертає таблицю з вказаним ім'ям
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static Table getTable(string name)
        {
            foreach (var cur in DataBase)
                if (name == cur.name)
                    return cur;
            throw new Exception("Немає такого імені таблиці в БД");
            
        }

        /// <summary>
        /// Додає рядок в таблицю
        /// </summary>
        /// <param name="TableName"></param>
        /// <param name="values"></param>
        public static void Insert(string TableName, List<Lexem> values)
        {
            foreach (var cur in DataBase)
            {
                if (cur.name == TableName)
                {
                    cur.AddRow(values);
                    break;
                }
            }
        }

        /// <summary>
        /// Виводить всі таблиці з БД
        /// </summary>
        public static void WriteDB()
        {
            Console.WriteLine();
            foreach (var cur in DataBase)
            {
                Console.WriteLine(cur.name);
                cur.WriteTable();
                System.Console.WriteLine();
            }
        }
    }
}
