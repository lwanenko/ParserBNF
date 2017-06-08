using System;
using System.IO;
using System.Xml.Serialization;
using System;
using System.Threading.Tasks;
using MPI;
using System.Collections.Generic;

namespace SQL
{
    class Program
    {
        static void Main(string[] args)
        {
            XmlSerializer formatter = new XmlSerializer(typeof(BNF));
            using (FileStream fs = new FileStream("BNF.xml", FileMode.OpenOrCreate))
            {
                //BNF b = (BNF)formatter.Deserialize(fs);
                
                BNF.AddRule(true,
                #region STANDART BNF RULES
                "<Space>",  "||' '|'"+'\r'+"'|'" + '\n' + "'|'"+'\t'+"'||",// пробіл або ентер
                "<spaces>", "{<Space>}", //можлива послідовність пробілів
                "<Spaces>", "<Space>{<Space>}", //обов'язкова послідовність пробілів
                "<name>",   "||<ABC>|<abc>||{||<ABC>|<figure>|<zero>|<abc>||}",
                "<abc>",    "||'q'|'w'|'e'|'r'|'t'|'y'|'u'|'i'|'o'|'p'|'a'|'s'|'d'|'f'|'g'|'h'|'j'|'k'|'l'|'z'|'x'|'c'|'v'|'b'|'n'|'m'||", // великі та малі
                "<ABC>",    "||'Q'|'W'|'E'|'R'|'T'|'Y'|'U'|'I'|'O'|'P'|'A'|'S'|'D'|'F'|'G'|'H'|'J'|'K'|'L'|'Z'|'X'|'C'|'V'|'B'|'N'|'M'||",// літери
                "<figure>", "||'1'|'2'|'3'|'4'|'5'|'6'|'7'|'8'|'9'||",//цифри без 0
                "<zero>",   "'0'",
                #endregion       

                "<Program>",     "<Transaction>{';'<Transaction>}[';']<spaces>",
                "<Transaction>", "<spaces>||<SELECT>|<CREATE>|<INSERT>||<spaces>",  //Транзакція

                #region SELECT
                "<SELECT>", "<spaces>||'SELECT'|'select'||||<SELECT_ListNameCollumn>|<Function_for_SELECT>||||'FROM'|'from'||" +
                                     "<Spaces><TableName>[<spaces><WHERE>][<spaces><GROUP_BY>][<spaces><HAVING>][<spaces><ORDER_BY>]<spaces>",

                "<Function_for_SELECT>",      "<spaces>||<MIN>|<MAX>|<SUM>|<SQRT>|<AVG>|<UPPER>|<LOWER>||" +
                                              "<spaces>'('<spaces><ColumnName><spaces>')'<spaces>",

                "<SELECT_ListNameCollumn>", "<Spaces>[||'DISTINCT'|'distinct'|'ALL'|'all'||<Spaces>]" +
                                            "||[<TableName>'.']<ColumnName>{<spaces>','" +
                                            "<Spaces>[<TableName>'.']<ColumnName>}|'*'||<Spaces>",

                "<MIN>",   "||'MIN'|'min'||",
                "<MAX>",   "||'MAX'|'max'||",
                "<SUM>",   "||'SUM'|'sum'||",
                "<AVG>",   "||'AVG'|'avg'||",
                "<SQRT>",  "||'SQRT'|'sqrt'||",
                "<UPPER>", "'dewfewq'",// !!!
                "<LOWER>", "'dedeewd'",// !!!

                    #region WHERE
                    "<WHERE>",        "||'WHERE'|'where'||<ListExpression>",
                    "<BoolExpression>", "<Spaces><Expression>{<operator><Expression><spaces>}<spaces>" +
                                      "<comparisonSign><spaces><Expression>{<operator><Expression><spaces>}",
                    "<ListExpression>", "[||'NOT'|'not'||]<BoolExpression>{||<Spaces>'AND'|<Spaces>'OR'||[<Spaces>||'NOT'|'not'||]<BoolExpression>}",
                    "<Expression>", "||'NULL'|<Function_for_SELECT>|[<TableName>'.']<ColumnName>|<value>|'('<SELECT>')'||",//вираз
                    "<comparisonSign>","||'<>'|'<='|'>='|'<'|'>'|'='||",//знак порівняння
                    "<operator>",     "<spaces>||'+'|'-'|'*'|'/'||<spaces>",
                    #endregion 

                "<GROUP_BY>",  "'sdfsdaf'",//потім потрібно реалізувати!!!
                "<HAVING>",    "'sdfsdaf'",//!!!
                "<ORDER_BY>",  "<spaces>||'ORDER'|'order'||<Spaces>||'BY'|'by'||<Spaces><ColumnName><spaces>",
                #endregion

                #region CREATE
                "<CREATE>", "<spaces>||'CREATE'|'create'||<Spaces>||'Table'|'TABLE'|'table'||" +
                            "<Spaces><TableName><spaces>'('<spaces><ColumnName><Spaces><DataType>[<size>]<spaces>" +
                            "{','<spaces><ColumnName><Spaces><DataType>[<size>]<spaces>}" +
                            "[<spaces>','<spaces>'PRIMERY'<Spaces>'KEY'<spaces>'('<PrimeryColumnName>')'<spaces>]')'<spaces>",
                "<DataType>","||'int'|'INT'|'VARCHAR'|'varchar'|'float'|'FLOAT'|'DOUBLE'|'double'|'DATE'|'date'|'TIME'|'time'|'char'|'CHAR'||",
                "<size>"    ,"<spaces>'('<spaces><int_value><spaces>')'<spaces>",
                #endregion

                #region INSERT
                "<INSERT>", "<spaces>'INSERT'<Spaces>'INTO'<Spaces><TableName>[<spaces>" +
                            "'('<spaces><ColumnName>{<spaces>','<spaces><ColumnName>}<spaces>')']<VALUES>",
                "<VALUES>", "<Spaces>'VALUES'<spaces>'('<spaces><value>{<spaces>','<spaces><value>}<spaces>')'<spaces>",
                #endregion
               
                #region VALUE
                "<value>", "||<int_value>|<double_value>|<string_value>||",
                    "<int_value>", "||<figure>{<num>}|<zero>||",
                    "<num>","||<zero>|<figure>||",
                    "<double_value>", "||<int_value>|<zero>||'.'{||<zero>|<figure>||",
                    "<string_value>", "''{||<figure>|<ABC>|<abc>|<zero>||}''",
                #endregion

                #region NAMES                 
                "<PrimeryColumnName>","<name>",
                "<ColumnName>",  "<name>",
                "<TableName>",   "<name>"
                    
                #endregion
             
                );
            }

            var lt = new LexemTree();
            Program_Listener.Start(lt);

            string codeForLab = SQL.Properties.Resources.Code.ToString();
            codeForLab = codeForLab + " FsROM ";


            Lab1(SQL.Properties.Resources.Code.ToString(),  codeForLab);

            Console.ReadLine();

        }

        /// <summary>
        /// Послідовний код
        /// Завдання
        /// 1)  з наявного коду формую, набір триграм (символьних)
        /// 2)  для заданого коду, формую новий, відредагований, з набору триграм, які  в мене є
        /// 2.1) Дивлюсь які триграми по статистиці не використовуються в прикладі взагалі а тут використовуються 
        /// 2.2) визначаю проміжки в коді які не використовуються в прикладі
        /// 2.3) пробую їх замінити
        /// 2.4) підставляю в код
        /// 3) перевіряю утворений код на інтерпритаторі
        /// 4) вивожу час роботи 
        /// </summary>
        static void Lab1 (string code, string Fcode)
        {
            var startTime = DateTime.Now;

            var TD = TrigramsParcer.TrigramsDictionary(code);
            var TL = TrigramsParcer.TrigramsList(code);
            var FTL = TrigramsParcer.TrigramsList(Fcode);
            var FTD = TrigramsParcer.TrigramsDictionary(Fcode);
            //2.1
            var listForConvert = new List<Trigrama>();
            foreach (var cur1 in FTD)
            {
                bool flag = false;
                foreach(var cur2 in TD)
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
            for (int i = 1; i < FTL.Count - 1; i++)
            {
                if (last < i - 1 && first != -1)
                {
                    ConvertList.Add(new Pair(first, last));
                    first = -1;
                    last = -1;
                }
                if (listForConvert.Contains(FTL[i]))
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
                Trigrama cur_sub = FTL[p.i - 1];
                while (cur_sub.equals(FTL[p.ii + 1]))
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

            for (int i = 0; i < FTL.Count; i++)
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
                    rez.Add(FTL[i]);
            }

            Console.WriteLine();
            foreach (var cur in rez)
            {
                Console.Write(cur.val[0]);
            }
            Console.Write(rez[rez.Count-1].val[1]+ rez[rez.Count - 1].val[2]);

            Console.WriteLine("Lab1 Time: "+ (DateTime.Now- startTime));
        }

    }
}
