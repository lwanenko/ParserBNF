using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQL
{

    class LexemTrigrama
    {
        #region VAR

        public string Node1;

        public string Node2;

        public string Node3;

        #endregion

        #region CTOR

        /// <summary>
        /// звичайний  конструктор тригера
        /// </summary>
        /// <param name="l1"></param>
        /// <param name="l2"></param>
        /// <param name="l3"></param>
        public LexemTrigrama(Lexem l1, Lexem l2, Lexem l3)
        {
            Node1 = l1.name;
            Node2 = l2.name;
            Node3 = l3.name;
        }

        #endregion


        public bool Equals(LexemTrigrama t)
        {
            if (   (Node1 != t.Node1)
                || (Node1 != t.Node1)
                || (Node1 != t.Node1) )
                        return false;
            return true;
        }

    }
}

