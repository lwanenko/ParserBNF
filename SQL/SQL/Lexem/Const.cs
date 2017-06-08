using System;

namespace SQL
{
    class Const
    {
        #region VAR
        
        /// <summary>
        /// Змінна для класу Lexem, яка вказує чи це константа
        /// </summary>
        public bool isConst = true;

        /// <summary>
        /// Код програми, який буде парситись
        /// </summary>
        public  Code code;

        /// <summary>
        /// Левел - це показник кількості зв'язків до mainLexem
        /// </summary>
        public int level;

        /// <summary>
        /// Позиція старту даної лексеми в коді програми та позиція кінця, або поточна позиція
        /// </summary>
        public int pos, pos_start;

        /// <summary>
        /// Ім'я лексеми, або вираз константи
        /// </summary>
        public string name;
        
        #endregion

        public Const(int level, string name, int pos_start, Code code)
        {
            this.code = code;
            this.name = name;
            this.level = level;
            
            this.pos_start = pos_start;
            pos = pos_start;
        }

        /// <summary>
        /// Видає кількість пробілів, яка відповідає левелу 
        /// </summary>
        /// <returns></returns>
        protected string GetLevel()
        {
            string cur = "";
            for (int i = 0; i < level; i++)
                cur = cur + "| ";
            return cur;
        }

        
        /// <summary>
        /// Для спрощення коду введемо додаткові штучні правила 
        ///в символах та позначеннях, що суттєво спростить алгоритм програми
        ///список спрощень:
        ///   { <-> (+
        ///   } <-> +)
        ///   [ <-> (-
        ///   ] <-> -)
        /// """ <-> ""
        /// </summary>
        private void OwerrideName()
        {
            switch (name)
            {
                case @"''":
                    name = "'" + '"' + "'";
                    break;
                case @"'!!'":
                    name = "'||'";
                    break;
                case "'(+'":
                    name = "'{'" ;
                    break;
                case "+)":
                    name = "'}'";
                    break;
                case "'(-'":
                    name = "'['";
                    break;
                case "'-)'":
                    name = "']'";
                    break;
            }
        }
        
        /// <summary>
        /// Перевіряє чи виконується константа 
        /// </summary>
        /// <returns></returns>
        public bool IsConst()
        {
            
            OwerrideName();
            if (pos >= code.length() - 1)
                return false;
          //  if (name == "'INSERT'")
          //     System.Console.WriteLine(code.get(pos, code.length() - pos ));
            for (int i = pos; i < code.length() && i < name.Length + pos - 2; i++)
                if (code[i] != name[i - pos + 1])
                {
                    if (name.Length > 3)
                        System.Console.WriteLine(GetLevel() + "-" + '"' + name + '"');
                        //Console.WriteLine("'"+code[i]+"'");
                    return false;
                }
            pos = name.Length + pos_start - 2;// -2  для лапок
            if (name[1] == '\n'|| name[1] == '\r')
                System.Console.WriteLine(GetLevel() + "+" + '"' + @"/n" + '"' + " // pos== " + pos);
            else
                System.Console.WriteLine(GetLevel() + "+" + '"' + name + '"' + " // pos== " + pos);          
            return true;
        }

        public Lexem returnLexem()
        {
            return new Lexem(level, name, pos_start, pos, code);
        }

    }
}
