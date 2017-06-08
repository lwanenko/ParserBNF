using System.Xml.Serialization;
using System.IO;
using System;

namespace SQL
{
    /// <summary>
    /// Клас, який зберігає інформацію про правило БНФ
    /// </summary>
    [Serializable]
    [XmlInclude(typeof(BNF))]
    public class Rule
    {
        /// <summary>
        /// назва лексеми, завжди в <...>
        /// </summary>
        public string name;
        /// <summary>
        /// Правило, яке описує лексему
        /// </summary>
        public string rule;

        public Rule()
        {
                    
        }//умова серіалізації

        /// <summary>
        /// Звичайний конструктор
        /// </summary>
        public Rule(string name, string rule)
        {
            this.name = name;
            this.rule = rule;
        }

    }
}
