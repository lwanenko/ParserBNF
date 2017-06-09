using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQL
{
    static class AnalysisTrigram
    {
        /// <summary>
        /// Завдання
        /// 1)  з наявного коду формую, набір триграм (символьних)
        /// 2)  для заданого коду, формую новий, відредагований, з набору триграм, які  в мене є
        /// 2.1) Дивлюсь які триграми по статистиці не використовуються в прикладі взагалі а тут використовуються 
        /// 2.2) визначаю проміжки в коді які не використовуються в прикладі
        /// 2.3) пробую їх замінити
        /// 2.4) підставляю в код
        /// </summary>
        public static void AnalysisMistake(string codeExample, string AnalizCode)
        {           

            var TD = TrigramsParcer.TrigramsDictionary(codeExample);
            var TL = TrigramsParcer.TrigramsList(codeExample);
            var AnCodeTL = TrigramsParcer.TrigramsList(AnalizCode);
            var AnCodeTD = TrigramsParcer.TrigramsDictionary(AnalizCode);
            
            //2.1
            var listForConvert = new List<Trigrama>();
            foreach (var cur1 in AnCodeTD)
            {
                bool flag = false;
                foreach (var cur2 in TD)
                {
                    if (cur1.Key == cur2.Key)
                    {
                        flag = true;
                        break;
                    }
                }
                if (!flag)
                {
                    listForConvert.Add(cur1.Key);
                }
            }

            //2.2
            int first = -1;
            int last = -1;
            var ConvertList = new List<Pair>();
            for (int i = 1; i < AnCodeTL.Count - 1; i++)
            {
                if (last < i - 1 && first != -1)
                {
                    ConvertList.Add(new Pair(first, last));
                    first = -1;
                    last = -1;
                }
                if (listForConvert.Contains(AnCodeTL[i]))
                {
                    if (first == -1)
                        first = i;
                    last = i;
                }

            }

            //2.3

            var listSub = new List<Sub_Pair>();

            foreach (var p in ConvertList)
            {
                var substitute = new List<Trigrama>();
                Trigrama cur_sub = AnCodeTL[p.i - 1];
                while (cur_sub.equals(AnCodeTL[p.ii + 2]))
                {
                    bool next = false;
                    foreach (var cur in TL)
                    {
                        if (next)
                        {
                            cur_sub = cur;
                            break;
                        }
                        if (cur.equals(cur_sub))
                        {
                            next = true;
                        }
                    }
                    if (!next) break;
                    substitute.Add(cur_sub);
                }
                listSub.Add(new Sub_Pair(p, substitute));
            }

            //2.4

            var rez = new List<Trigrama>();

            for (int i = 0; i < AnCodeTL.Count; i++)
            {
                bool flag = true;
                foreach (var cur in listSub)
                {
                    if (cur.p.i == i)
                    {
                        flag = false;
                        foreach (var c in cur.list)
                        {
                            rez.Add(c);
                        }
                        i = cur.p.ii;
                    }
                }
                if (flag)
                    rez.Add(AnCodeTL[i]);
            }

            Console.WriteLine();
            foreach (var cur in rez)
            {
                Console.Write(cur.val[0]);
            }
            Console.Write(rez[rez.Count - 1].val[1] + rez[rez.Count - 1].val[2]);


        }

        /// <summary>
        /// Аналізує код 
        /// </summary>
        /// <return>
        /// Виводить номер найймовірнішого автора
        /// </return>
        public static void Analysis(string AnalizCode, params string[] codeExample)
        {
            var AnCodeTL = TrigramsParcer.TrigramsList(AnalizCode);

            List<int> rez = new List<int>();

            foreach (var code in codeExample)
            {
               
                var TL = TrigramsParcer.TrigramsList(code);

                var listForConvert = new List<Trigrama>();
                foreach (var cur1 in AnCodeTL)
                {
                    bool flag = false;
                    foreach (var cur2 in TL)
                    {
                        if (cur1.equals(cur2))
                        {
                            flag = true;
                            break;
                        }
                    }
                    if (!flag)
                    {
                        listForConvert.Add(cur1);
                    }
                }

                TL.Clear();
                rez.Add(listForConvert.Count);
            }

            #region return rez
            int min = rez[0];
            int avtor = 0;
            for (int i = 0; i < rez.Count; i++)
            {
                if (min > rez[i])
                {
                    avtor = i;
                    min = rez[i];
                }
            }

            Console.WriteLine("Автор тексту(звичайнi триграмми): " + (avtor + 1));
            #endregion
        }

        /// <summary>
        /// Аналізує код 
        /// </summary>
        /// <return>
        /// Виводить номер найймовірнішого автора
        /// </return>
        public static void AnalysisLexem(Lexem tree, params Lexem[] treeExample)
        {
            var AnCodeTL = TrigramsParcer.LexemTrigramsList(tree);

            List<int> rez = new List<int>();

            foreach (var code in treeExample)
            {
               
                var TL = TrigramsParcer.LexemTrigramsList(code);

                var listForConvert = new List<LexemTrigrama>();
                foreach (var cur1 in AnCodeTL)
                {
                    bool flag = false;
                    foreach (var cur2 in TL)
                    {
                        if (cur1.equals(cur2))
                        {
                            flag = true;
                            break;
                        }
                    }
                    if (!flag)
                    {
                        listForConvert.Add(cur1);
                    }
                }

                TL.Clear();
                rez.Add(listForConvert.Count);
            }

            #region return rez
            int min = rez[0];
            int avtor = 0;
            for (int i = 0; i < rez.Count; i++)
            {
                if (min > rez[i])
                {
                    avtor = i;
                    min = rez[i];
                }
            }

            Console.WriteLine("Автор тексту(звичайнi триграмми): " + (avtor + 1));
            #endregion
        }



    }
}
