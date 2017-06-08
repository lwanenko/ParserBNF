using System;
using System.Collections.Generic;

namespace SQL
{
    class  Lexem:Const 
    {
        #region VAR

        /// <summary>
        /// це список всіх "Дітей" даної лексеми, 
        /// тобто список Лексем і констант,
        /// які утворюють цю лексему
        /// </summary>
        public List<Lexem> children = new List<Lexem>();

        /// <summary>
        /// Поточне правило БНФ, яке парситься // для економії часу
        /// </summary>
        private string curRule;

        /// <summary>
        /// Значення, яке відображає дана лексема
        /// </summary>
        public string value = null;

        #endregion

        #region CTOR

        public Lexem():base(-1, null,-1, null)
        {
                
        }

        public Lexem( string name, Lexem l , string value):base(l.level, name,l.pos_start,l.code)
        { 
            pos = l.pos;
            this.value = value;

        }

        public Lexem(int level, string name, int pos_start, int pos, Code code): base(level, name, pos_start, code)
        {
            this.pos = pos;
        }

        /// <summary>
        /// Стандартний констрктор, 
        /// який викликає батьківський конструктор Const
        /// </summary>
        /// <param name="level"> левел лексеми </param>
        /// <param name="name"> ім'я лексеми </param>
        /// <param name="pos_start"> стартова позиція лексеми в коді </param>
        public Lexem(int level, string name, int pos_start, Code code):base(level, name, pos_start, code)
        {

            isConst = false;
            curRule = BNF.GetRule(name);
            if (curRule == null)
                throw new System.Exception("BNF ERROR!!!");
        }
        #endregion

        /// <summary>
        /// Повертає код в програмі який відображає дану лексему
        /// або спрощене значення, яке утворюється при обробці програми
        /// </summary>
        /// <returns></returns>
        public string getLexemCode()
        {
            if (value != null)
                return value;
            if (pos >= pos_start && code != null)
            {
                value = code.get(pos_start, pos - 1);
                
            }
            if (value != null)
                   return value;
            throw new Exception("Error pos and posStart");
        }

        /// <summary>
        /// Відображає індексатор по дітям, даної лексеми, якщо вони присутні 
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public Lexem this[int index] 
        {
            get 
            {
                if (index < children.Count)
                   return children[index];
                throw new Exception("Error!!!");
            }
            set
            {
                this.children[index] = value;
            }
        }

        /// <summary>
        /// Парсить код по БНФ, створюючи при цьому дерево програми
        /// </summary>
        /// <returns>
        /// true  => Лесема виконується
        /// false => Лексема не представлена в коді
        /// </returns>
        public bool IsLexem() {
            System.Console.WriteLine(GetLevel() + "/" + this.name);
            
            int close;
            for (int i = 0; i < curRule.Length; i++)
            {
                switch(curRule[i])
                {
                    case '{':
                        close = CloseFig(i);
                        var l_fig = fig(i + 1 , close - 1,ref  pos);// на обробку блоку подається внутрішнє значення без дужок
                        i = close;
                        if (l_fig != null)
                            foreach(var cur in l_fig)
                                children.Add(cur);
                        break;
                    case '[':
                        close = CloseKvad(i);
                        var l_kvad = kvad(i + 1, close - 1,ref  pos); // на обробку блоку подається внутрішнє значення без дужок
                        i = close;
                        if (l_kvad != null)
                            foreach (var cur in l_kvad)
                                children.Add(cur);
                        break;
                    case '|':
                        close = CloseOr(i);                       
                        var l_or = or(i + 2, close - 2,ref pos);// на обробку блоку подається внутрішнє значення без дужок
                        if (l_or != null)
                        {
                            i = close;
                            foreach (var cur in l_or)
                                children.Add(cur);
                        }
                        else
                        {
                            System.Console.WriteLine(GetLevel() + @"\-" + this.name);
                            return false;
                        }
                        break;
                    case '<':
                        close = CloseLexem(i);                                  
                        var lexem = new Lexem(level + 1, curRule.Substring(i, close - i + 1), pos, this.code);
                        if (lexem.IsLexem())
                        {
                            i = close;
                            pos = lexem.pos;
                            children.Add(lexem);
                        }
                        else
                        {
                            System.Console.WriteLine(GetLevel() + @"\-" + this.name + " //" + curRule.Substring(i, curRule.Length - i));
                            return false;
                        }
                        break;
                    default://це для ' - інших випадків за правильного БНФ бути не може
                        close = CloseConst(i);
                        var constant = new Const(level + 1, curRule.Substring(i, close - i + 1), pos, this.code);
                        if (constant.IsConst())
                        {
                            i = close;
                            pos = constant.pos;
                            children.Add(constant.returnLexem() );
                        }
                        else
                        {
                            System.Console.WriteLine(GetLevel() + @"\-" + this.name + " //" + curRule.Substring(i,curRule.Length - i ));
                            return false;
                        }
                        break;
                }
            }
            System.Console.WriteLine(GetLevel() + @"\+" + this.name);

            for (int i = 0; i < children.Count; i++)
            {
                if (children[i] == null)
                {
                    children.RemoveAt(i);
                    i--;
                }
            }

            return true;
        }

        #region PARSER

        /// <summary>
        /// Комбінація з  n  повторів виразу (n є [0, +inf) n є N) 
        /// </summary>
        /// <param name="i"> це початкова позиція виразу в правилі бнф </param>
        /// <param name="ii"> це кінцева позиція виразу в правилі бнф </param>
        /// <param name="posFig"></param>
        /// <returns>
        /// Повертає 
        ///         * список з лексем, які потрібно додати до children
        ///         * null , коли список пустий, або жодного разу повтор не виконється 
        /// </returns>
        private List<Lexem> fig(int i, int ii, ref int  posFig)// дужки {} в бнф 
        {
            int close;
            var list_for_return = new List<Lexem>();
            var list_for_iteration = new List<Lexem>();
            int posFig_for_iteration = posFig;
            int new_i = i;// для ітерації while, щоб і завжди мала одне значення
            while (true)
            {
                list_for_iteration.Clear();
                
                for (i= new_i; i < ii + 1; i++)
                {
                    switch (curRule[i])
                    {
                        case '{':
                            close = CloseFig(i);
                            var l_fig = fig(i + 1, close - 1, ref posFig_for_iteration);// на обробку блоку подається внутрішнє значення без дужок
                            i = close;
                            if (l_fig != null)
                                foreach (var cur in l_fig)
                                    list_for_iteration.Add(cur);
                            break;
                        case '[':
                            close = CloseKvad(i);
                            var l_kvad = kvad(i + 1, close - 1, ref posFig_for_iteration); // на обробку блоку подається внутрішнє значення без дужок
                            i = close;
                            if (l_kvad != null)
                                foreach (var cur in l_kvad)
                                    list_for_iteration.Add(cur);
                            break;
                        case '|':
                            close = CloseOr(i);
                            var l_or = or(i + 2, close - 2, ref posFig_for_iteration);// на обробку блоку подається внутрішнє значення без дужок
                            if (l_or != null)
                            {
                                i = close;
                                foreach (var cur in l_or)
                                    list_for_iteration.Add(cur);
                            }
                            else
                                return list_for_return;
                            break;
                        case '<':
                            close = CloseLexem(i);                                  
                            var lexem = new Lexem(level + 1, curRule.Substring(i, close - i + 1), posFig_for_iteration, this.code);
                            if (lexem.IsLexem())
                            {
                                i = close;
                                posFig_for_iteration = lexem.pos;
                                list_for_iteration.Add(lexem);
                            }
                            else
                                return list_for_return;
                            break;
                        default://це для ' - інших випадків за правильного БНФ бути не може
                            close = CloseConst(i);
                            var constant = new Const(level + 1, curRule.Substring(i, close - i + 1), posFig_for_iteration, this.code);
                            if (constant.IsConst())
                            {
                                i = close;
                                posFig_for_iteration = constant.pos;
                                list_for_iteration.Add(constant.returnLexem());
                            }
                            else
                            {
                                return list_for_return;
                            }
                            break;
                    }
                }
                posFig = posFig_for_iteration;
                foreach (var cur in list_for_iteration)
                    list_for_return.Add(cur);
            }
           
        }

        private List<Lexem> kvad(int i, int ii, ref int posKvad)// дужки [] в бнф 
        {
            int curPosKvad = posKvad;//поточна позиція в коді даного блоку
            int close;
            var list_for_return = new List<Lexem>();
            for (; i < ii + 1; i++)
            {
                    switch (curRule[i])
                    {
                        case '{':
                            close = CloseFig(i);
                        var l_fig = fig(i + 1, close - 1, ref curPosKvad);// на обробку блоку подається внутрішнє значення без дужок
                        i = close;
                            if (l_fig != null)
                                foreach (var cur in l_fig)
                                    list_for_return.Add(cur);
                            break;
                        case '[':
                            close = CloseKvad(i);
                             var l_kvad = kvad(i + 1, close - 1,ref  curPosKvad); // на обробку блоку подається внутрішнє значення без дужок
                            i = close;
                            if (l_kvad != null)
                                foreach (var cur in l_kvad)
                                    list_for_return.Add(cur);
                            break;
                        case '|':
                            close = CloseOr(i);
                            var l_or = or(i + 2, close - 2, ref curPosKvad);// на обробку блоку подається внутрішнє значення без дужок
                        if (l_or != null)
                        {
                            i = close;
                            foreach (var cur in l_or)
                                list_for_return.Add(cur);
                        }
                        else
                            return null;
                            break;
                        case '<':
                            close = CloseLexem(i);                                  
                            var lexem = new Lexem(level + 1, curRule.Substring(i, close - i + 1), curPosKvad, this.code);
                            if (lexem.IsLexem())
                            {
                                i = close;
                                curPosKvad = lexem.pos;
                                list_for_return.Add(lexem);
                            }
                            else
                                return null;
                            break;
                        default://це для ' - інших випадків за правильного БНФ бути не може
                            close = CloseConst(i);
                            var constant = new Const(level + 1, curRule.Substring(i, close - i + 1), curPosKvad, this.code);
                            if (constant.IsConst())
                            {
                                i = close;
                                curPosKvad = constant.pos;
                                list_for_return.Add(constant.returnLexem());
                            }
                            else
                                return null;
                            break;
                    }
            }
            posKvad = curPosKvad;
            return list_for_return;
        }

        /// <summary>
        /// Блок вибору в правилі БНФ
        /// </summary>
        /// <param name="i"></param>
        /// <param name="ii"></param>
        /// /// <param name="posOr"></param>
        /// <returns></returns>
        private List<Lexem> or(int i, int ii, ref int posOr)// блок вибору ||<...>|'...'|... ...|| в бнф 
        {
            int close;
            int curPosOr = posOr;// поточна позиція, щоб мжна було повернутися до попередньої
            var list_for_return = new List<Lexem>();
            bool goToNext = false; //для зміни варіанту  
            for (; i < ii + 2; i++)
            {
                if (goToNext)
                {
                    i = nextOr(i, ii) + 1;
                    if (i >= ii)
                        return null;
                    curPosOr = posOr;
                    list_for_return = new List<Lexem>();
                    goToNext = false;
                }
                if (curRule[i] == '|')
                {
                    posOr = curPosOr;
                    return list_for_return;
                }

                switch (curRule[i])
                {
                    case '{':
                        close = CloseFig(i);
                        var l_fig = fig(i + 1, close - 1, ref curPosOr);// на обробку блоку подається внутрішнє значення без дужок
                        i = close;
                        if (l_fig != null)
                            foreach (var cur in l_fig)
                                list_for_return.Add(cur);
                        break;
                    case '[':
                        close = CloseKvad(i);
                        var l_kvad = kvad(i + 1, close - 1, ref curPosOr); // на обробку блоку подається внутрішнє значення без дужок
                        i = close;
                        if (l_kvad != null)
                            foreach (var cur in l_kvad)
                                list_for_return.Add(cur);
                        break;
                    case '<':
                        close = CloseLexem(i);                                  
                        var lexem = new Lexem(level + 1, curRule.Substring(i, close - i + 1), curPosOr, this.code);
                        if (lexem.IsLexem())
                        {
                            i = close;
                            curPosOr = lexem.pos;
                            list_for_return.Add(lexem);
                        }
                        else
                            goToNext = true;
                        break;
                    default://це для ' - інших випадків за правильного БНФ бути не може
                        close = CloseConst(i);
                        var constant = new Const(level + 1, curRule.Substring(i, close - i + 1), curPosOr, this.code);
                        if (constant.IsConst())
                        {
                            i = close;
                            curPosOr = constant.pos;
                            list_for_return.Add(constant.returnLexem());
                        }
                        else
                            goToNext = true;
                        break;
                }
            }
            return list_for_return;
        }

        /// <summary>
        /// Використовується для or(), 
        /// щоб знайти наступне значення, яке необхідно перевірити
        /// </summary>
        /// <param name="i"></param>
        /// <param name="ii"></param>
        /// <returns></returns>
        private int nextOr(int i, int ii)
        {
            for (i = i + 1; i < ii + 2; i++)
                if (curRule[i] == '|')
                    return i;
            throw new System.Exception("Помилка в блоці вибору!!!");
        }
        #endregion

        //Для методів регіону CLOSE, характерно:
        // (+) створюється помилка у випадку коли закриваючого елементу не знайдено
        // (+) повертається значення, яке відповідає позиції кінця блоку 
        #region CLOSE
        
        /// <summary>
        /// Визначає закриваючу фігурну дужку,
        /// для зовніжнього блоку фігуних дужок БНФ,
        /// починаючи з вказаної позиції
        /// </summary>
        /// <param name="posOpen">
        /// Позиція відкриваючої дужки в правилі БНФ
        /// </param>
        /// <returns>
        /// Позиція закриваючої дужки в правилі БНФ 
        /// </returns>
        private int CloseFig (int posOpen)
        {
            int OpenLevel = 1;
            for (int i = posOpen + 1; i < curRule.Length; i++)
            {
                if (curRule[i] == '}')
                {
                    OpenLevel--;
                    if (OpenLevel == 0)
                        return i;
                }
                if (curRule[i] == '{')
                    OpenLevel++;
            }
            throw new System.Exception("Помилка в БНФ "+this.name +"!!!");           
        }

        /// <summary>
        /// Визначає закриваючу квадратну дужку,
        /// для зовніжнього блоку квадратних дужок БНФ,
        /// починаючи з вказаної позиції
        /// </summary>
        /// <param name="posOpen">
        /// Позиція відкриваючої дужки в правилі БНФ
        /// </param>
        /// <returns>
        /// Позиція закриваючої дужки в правилі БНФ 
        /// </returns>
        private int CloseKvad (int posOpen)
        {
            int OpenLevel = 1;
            for (int i = posOpen + 1; i < curRule.Length; i++)
            {
                if (curRule[i] == ']')
                {
                    OpenLevel--;
                    if (OpenLevel == 0)
                        return i;
                }
                if (curRule[i] == '[')
                    OpenLevel++;
            }
            throw new System.Exception("Помилка в БНФ " + this.name + "!!!");
        }

        /// <summary>
        /// Визначає закриваючий елемент "блоку вибору",
        /// починаючи з вказаної позиції
        /// </summary>
        /// <param name="posOpen">
        /// Позиція відкриваючої дужки в правилі БНФ
        /// </param>
        /// <returns>
        /// Позиція закриваючої дужки в правилі БНФ 
        /// </returns>
        private int CloseOr(int posOpen)
        {
            if (curRule[posOpen + 1] == '|')
                for (int i = posOpen + 2; i < curRule.Length - 1; i++)
                    if (curRule[i] == '|' &&
                        curRule[i + 1] == '|')
                        return i + 1;  
                                         
            throw new System.Exception("Помилка в БНФ " + this.name + "!!!");
        }

        /// <summary>
        /// Визначає закриваючу дужку лексеми
        /// </summary>
        /// <param name="posOpen"> Позиція відкриваючої дужки</param>
        /// <returns> Позиція закриваючої дужки </returns>
        private int CloseLexem(int posOpen)
        {           
            for (int i = posOpen + 1; i < curRule.Length; i++)
                if (curRule[i] == '>')                       
                    return i;
            throw new System.Exception("Помилка в БНФ " + this.name + "!!!");
        }

        /// <summary>
        /// Визначає позицію закриваючих лапок константи
        /// </summary>
        /// <param name="posOpen"></param>
        /// <returns>
        /// Позиціюя закриваючих лапок константи
        /// </returns>
        private int CloseConst(int posOpen)
        {
            for (int i = posOpen + 1; i < curRule.Length; i++)
                if (curRule[i] == Convert.ToChar("'"))
                    return i;
           throw new System.Exception("Помилка в БНФ " + this.name + "!!!");
        }

        #endregion
    }
}