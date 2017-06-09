using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MPI;
using System.Threading.Tasks;

namespace SQL
{
    class TrigramsParcer
    {
        #region LexemTrigram
        /// <summary>
        /// Парсить дерево програми, 
        /// </summary>
        /// <param name="lexem">Вхідна лексема</param>
        /// <returns></returns>
        public static List<Lexem> parceTree(Lexem lexem)
        {           
            List<Lexem> l = new List<Lexem>();
           
                l.Add(lexem);
                foreach (var cur in lexem.children)
                {
                    l.AddRange(parceTree(cur));
                }
            
            return l;
        }
        
        /// <summary>
        /// Створює список триграм з даної послідовності лексем
        /// </summary>
        /// <param name="l"></param>
        /// <returns></returns>
        public static List<LexemTrigrama> LexemTrigramsList(Lexem lexem)
        {
            var l = parceTree(lexem);
            Console.WriteLine();
            foreach(var cur in l )
                Console.Write(cur.name+ " ");

            var rez = new List<LexemTrigrama>();

            for (int i = 0; i < (l.Count - 2); i++)
            {
                rez.Add(new LexemTrigrama(l[i], l[i + 1], l[i + 2]));
            }
            return rez;
        }

        public static Dictionary<LexemTrigrama, int> LexemTrigramsDictionary(Lexem lexem)
        {
            var list =LexemTrigramsList(lexem);
            var rez = new Dictionary<LexemTrigrama, int>();

            foreach (var cur1 in list)
            {
                bool flag = false;
                foreach (var cur2 in rez)
                {
                    if (cur1.equals(cur2.Key))
                    {
                         rez[cur2.Key]++;
                        flag = true;
                        break;
                    }                   
                }
                if (!flag) rez.Add(cur1, 1); 
            }
            return rez;
        }
        #endregion

        #region Trigram
        public static List<Trigrama> TrigramsList(string code)
        {
           
            var rez = new List<Trigrama>();

            for (int i = 0; i < (code.Length - 2); i++)
            {
                rez.Add(new Trigrama(code.Substring(i,3)));
            }
            return rez;
        }

        public static Dictionary<Trigrama, int> TrigramsDictionary(string code)
        {
            var list = TrigramsList(code);
            var rez = new Dictionary<Trigrama, int>();

            foreach (var cur1 in list)
            {
                bool flag = false;
                foreach (var cur2 in rez)
                {
                    if (cur1.equals(cur2.Key))
                    {
                        rez[cur2.Key]++;
                        flag = true;
                        break;
                    }
                }
                if (!flag) rez.Add(cur1, 1);
            }
            return rez;
        }

        
        #endregion

    }
}
