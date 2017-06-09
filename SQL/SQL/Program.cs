using System;
using System.IO;
using System.Xml.Serialization;
using System;
using System.Threading.Tasks;
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

            //var lt = new LexemTree();
            //Program_Listener.Start(lt);

            AnalysisTrigram.Analysis(SQL.Properties.Resources.Code.ToString(), Properties.Resources.Avtor1.ToString(), Properties.Resources.Avtor2.ToString());

            var l = new LexemTree();
            var l1 = new LexemTree(Properties.Resources.Avtor1.ToString());
            var l2 = new LexemTree(Properties.Resources.Avtor2.ToString());
            AnalysisTrigram.AnalysisLexem(l.mainLexem,l1.mainLexem, l2.mainLexem );

            Console.ReadLine();

        }

    }
}
