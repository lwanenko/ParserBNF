using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQL
{ 
    /// <summary>
    /// Клас, який відповідає за відповідність типів
    /// І використовується, для збереження колонки в таблиці
    /// </summary>
    class Column
    {
        #region VAR

        public string name;

        public string typeName;

        public bool primeryKey = false;

        private int lastPrimeryKey = -1;

        public int typeSize = 0;
        #endregion

        #region CTOR
        /// <summary>
        /// Конструктор для стандартного типу
        /// </summary>
        /// <param name="typeName"></param>
        public Column(string name,string typeName)
        {
            this.name = name;
            this.typeName = typeName;
        }

        /// <summary>
        /// Конструктор для спец типу
        /// </summary>
        /// <param name="typeName"></param>
        /// <param name="typeSize"></param>
        public Column(string name,string typeName, string typeSize)
        {
            this.name = name;
            this.typeName = typeName;
            this.typeSize = Convert.ToInt32(typeSize);
        }
        #endregion 

        /// <summary>
        /// додоє обмеження на розмір
        /// </summary>
        /// <param name="typeSize"></param>
        public void AddSize(string typeSize)
        {
            this.typeSize = Convert.ToInt32(typeSize);
        }

        /// <summary>
        /// перевіряє чи необхідно бану колонку зробити ключем
        /// </summary>
        /// <param name="name"></param>
        public void IsPrimeryKey(string name)
        {
            if (this.name == name)
            {
                primeryKey = true;
            }
        }


        public int LastPrimeryKeyValue()
        {          
            return ++lastPrimeryKey;
        }

        /// <summary>
        /// Перевіряє чи належить значення даному типу даних
        /// </summary>
        /// <param name="type">  тип </param>
        /// <param name="Value"> значення</param>
        /// <returns>
        ///     true => значення відповідає типу
        ///     false => значення не відповідає типу
        /// </returns>
        public bool ValueIsType(Lexem l)
        {
            if(typeName == "int" || typeName == "INT")
                return intChek(l);
            if (typeName == "varchar" || typeName == "VARCHAR")
                return varcharChek(l);
            if (typeName == "double" || typeName == "DOUBLE")
                return doubleChek(l);
            return false;
        }

        #region CHEK
        /// <summary>
        /// Перевіряє чи це значення String
        /// </summary>
        /// <param name="l"></param>
        /// <returns></returns>
        private bool varcharChek(Lexem l)
        {
            if (l.name != "<value>") return false;

            if (l.children[0].name != "<string_value>") return false;
            var s = l.getLexemCode();

            //size chek 
            if (typeSize > 0 &&  s.Length > typeSize)
                return false;
            return true;
        }

        /// <summary>
        /// Перевіряє чи це значення double
        /// </summary>
        /// <param name="l"></param>
        /// <returns></returns>
        private bool doubleChek(Lexem l)
        {
            try
            {
                if (l.name != "<value>")
                    return false;
                if (l.children[0].name != "<double_value>")
                    return false;
                var i = Convert.ToDouble(l.code.get(l.children[0].pos_start, l.children[0].pos));
            }
            catch
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// Перевіряє чи це інтове значення
        /// </summary>
        /// <param name="l"></param>
        /// <returns></returns>
        private bool intChek(Lexem l)
        {
            try
            {
                if (l.name != "<value>") return false;
                if (l.children[0].name != "<int_value>") return false;
                var i = Convert.ToInt32(l.getLexemCode());
            }
            catch
            {
                return false;
            }
            return true;
        }
        #endregion

    }
}
