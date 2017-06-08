using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQL
{
    static class Func_Listener
    {
        public static string Start(Lexem l, string tableName)
        {
            string column = null;
            string rez = null;
            foreach (var cur in l.children)
            {
                if (cur.name == "<ColumnName>")
                {
                    column = cur.getLexemCode();
                }
            }

            foreach (var cur in l.children)
            {
                switch (cur.name)
                {
                    
                    case "<MIN>":
                        string min = null;
                        int columnMIN = DB.getTable(tableName).getNumColumn(column);
                        bool firstMIN = true;
                        foreach (var cur2 in DB.getTable(tableName).table)
                        {
                            if (!firstMIN)
                            {
                                if (Where_Listener.comparer(min, cur2[columnMIN]) > 1)
                                    min = cur2[columnMIN];
                            }
                            else
                            {
                                firstMIN = false;
                                min = cur2[columnMIN];
                            }
                        }
                        return min;                        
                    case "<MAX>":
                        string max = null;
                        int columnMAX = DB.getTable(tableName).getNumColumn(column);
                        bool firstMAX = true;
                        foreach (var cur2 in DB.getTable(tableName).table)
                        {
                            if (!firstMAX)
                            {
                                if (Where_Listener.comparer(max, cur2[columnMAX]) < 1)
                                    max = cur2[columnMAX];
                            }
                            else
                            {
                                firstMAX = false;
                                max = cur2[columnMAX];
                            }
                        }
                        return max;
                    case "<SUM>":
                        double sum = 0;
                        int columnSUM = DB.getTable(tableName).getNumColumn(column);
                        foreach (var cur2 in DB.getTable(tableName).table)
                                sum += Convert.ToDouble(cur2[columnSUM]);
                        return sum+"";
                    case "<SQRT>":
                        double sqrt = 0;
                        int numSQRT = 0;
                        int columnSQRT = DB.getTable(tableName).getNumColumn(column);
                        bool firstSQRT = true;
                        foreach (var cur2 in DB.getTable(tableName).table)
                        {
                            if (!firstSQRT)
                            {
                                sqrt = Convert.ToDouble(cur2[columnSQRT]);
                            }
                            else
                            {
                                firstMAX = false;
                                sqrt+= Convert.ToDouble(cur2[numSQRT]);
                            }
                        }
                        return (Math.Sqrt(sqrt)) + "";
                    case "<AVG>":
                        double avg = 0;
                        int numAVG = 0;
                        int columnAVG = DB.getTable(tableName).getNumColumn(column);
                        bool firstAVG = true;
                        foreach (var cur2 in DB.getTable(tableName).table)
                        {
                            if (!firstAVG)
                            {
                                    avg = Convert.ToDouble (cur2[columnAVG]);
                            } 
                            else
                            {
                                firstMAX = false;
                                avg +=Convert.ToDouble( cur2[columnAVG]);
                            }
                        }
                        return (avg / numAVG) +"";

                    case "<UPPER>":
                        foreach (var cur2 in DB.getTable(tableName).table) { }
                        return rez;
                    case "<LOWER>":
                        foreach (var cur2 in DB.getTable(tableName).table) { }
                        return rez;
                    default:
                        break;
                }
            }
            throw new Exception("Error Func!!!");
        }

    }
}
