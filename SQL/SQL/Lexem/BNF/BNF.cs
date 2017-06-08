using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;


namespace SQL
{
    [Serializable]
    [XmlInclude(typeof(BNF))]
    public class BNF
    {
        #region VAR
        /// <summary>
        /// Ім'я БНФ (ім'я мови програмування)
        /// </summary>
        public static string name = "SQL";

        /// <summary>
        /// Ліст в якому зберігаються правила
        /// </summary>
        private static  List<Rule> rules = new List<Rule>();
        #endregion

        public BNF()
        {
            Console.WriteLine("<<<Конструктор БНФ запущений>>>");
            WriteBNF();
        }

        ~BNF() {
            var knownTypes = new Type[] { typeof(BNF), typeof(Rule) };
            XmlSerializer formatter = new XmlSerializer(typeof(BNF),knownTypes);
            using (Stream fStream = new FileStream("BNF.xml",
                FileMode.Create, FileAccess.Write, FileShare.None))
            {
                formatter.Serialize(fStream, this);
            }

        }

        /// <summary>
        /// Виводить в консоль всі правила БНФ
        /// </summary>
        public static void WriteBNF() {
            foreach (var item in rules)
                Console.WriteLine(@"{0} ::= {1}", item.name, item.rule);
        }

        /// <summary>
        /// Додає правило в список
        /// </summary>
        /// <param name="r"></param>
        public void AddRule(Rule r)
        {
            bool curFlag = true;
            foreach (var cur in rules)
                if (cur.name == r.name)
                {
                    curFlag = false;
                    break;
                }
            if (curFlag)
                rules.Add(r);
        }

        /// <summary>
        /// Додає декілька правил в БНФ
        /// </summary>
        /// <param name="flag"> Вказує що правил буде декілька в масиві r </param>
        /// <param name="r"> Масив з іменами та означеннями правил БНФ </param>
        public static void AddRule(bool flag, params string[] r)
        {
            for (int i = 0; i < r.Length; i = i + 2)
            {
                bool curFlag = true;
                foreach (var cur in rules)
                    if (cur.name == r[i])
                    {
                        curFlag = false;
                        break;
                    }
                if (curFlag || flag == true)
                    rules.Add(new Rule(r[i], r[i + 1]));
            }
        }

        /// <summary>
        /// Додає правило в список
        /// </summary>
        /// <param name="title"> Назва правила    </param>
        /// <param name="rule"> Означення правила </param>
        public void AddRule(string title, string rule)
        {
            AddRule(new Rule (title,rule));
        }

        /// <summary>
        /// Видає означення правила по імені
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static string GetRule(string name) {
            foreach (var cur in rules)
                if (name.Equals(cur.name))
                    return cur.rule;
            return null;
     
        }
        

        public Rule this[int index]
        {
            get {
                return rules[index];
            }
        }
    }
}
