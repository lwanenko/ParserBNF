using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQL
{
    static class Where_Listener
    {
        public static void Start(ref Table t, string tableName, Lexem w)
        {
            for (int i1 = 0; i1 < w.children.Count; i1++)
                if (w[i1].name == "<ListExpression>")
                {
                    for (int i2 = 0; i2 < w[i1].children.Count; i2++)
                    {
                        if (w[i1][i2].name == "<BoolExpression>")
                        {
                            for (int i3 = 0; i3 < w[i1][i2].children.Count; i3++)
                            {
                                if (w[i1][i2][i3].name == "<Expression>")
                                {
                                    for (int i = 0; i < w[i1][i2][i3].children.Count; i++)
                                    {
                                        if (w[i1][i2][i3][i].name == "<Function_for_SELECT>")
                                        {
                                            w[i1][i2][i3][i].value = Func_Listener.Start(w[i1][i2][i3][i], tableName);
                                        }
                                    }
                                }
                            }
                            bool firstExp = true; // перший вузол
                            bool getcompSign = false; // булевий знак присутній чи ні 
                            bool revers = false; // порядок входження 
                            Lexem column = new Lexem(); // колонка 
                            string value = null; // значення 
                            Lexem comp = new Lexem(); //
                            bool intValue = false;
                            foreach (var cur3 in w[i1][i2].children)
                            {

                                if (cur3.name == "<Expression>" && firstExp)
                                {
                                    foreach (var cur4 in cur3.children)
                                    {
                                        if (cur4 != null)
                                            switch (cur4.name)
                                            {
                                                case "<Function_for_SELECT>":
                                                    value = cur4.getLexemCode();
                                                    intValue = true;
                                                    break;
                                                case "<value>":
                                                    value = cur4.getLexemCode();
                                                    break;
                                                case "'NULL'":
                                                    value = null;
                                                    break;
                                                case "<ColumnName>":
                                                    column = cur4;
                                                    break;
                                                case "<SELECT>":
                                                    break;
                                            }
                                    }   
                                    firstExp = false;
                                }
                                if (cur3.name == "<comparisonSign>")
                                {
                                    getcompSign = true;
                                    comp = cur3;

                                }
                                if (getcompSign && cur3.name == "<Expression>")
                                {
                                    foreach (var cur4 in cur3.children)
                                    {
                                        if (cur4 != null)
                                            switch (cur4.name)
                                            {
                                                case "<Function_for_SELECT>":
                                                        value = cur4.getLexemCode();
                                                    intValue = true;
                                                    break;

                                                case "<value>":
                                                    value = cur4.getLexemCode();
                                                    break;

                                                case "'NULL'":
                                                    value = null;
                                                    break;
                                                case "<ColumnName>":
                                                    if (column.name == null)
                                                    {
                                                        revers = true;
                                                        column = cur4;
                                                    }
                                                    break;
                                                case "<SELECT>":
                                                    break;
                                            }
                                    }
                                }
                            }
                            //рівняння
                            switch (comp.getLexemCode())
                            {
                                case "=":                                   
                                    for (int i = 0; i < DB.getTable(tableName).columns.Count; i++)
                                    {
                                        if (column.getLexemCode() == DB.getTable(tableName).columns[i].name)
                                        {
                                            var tab = DB.getTable(tableName);
                                            foreach (var cur in tab.table)
                                            {
                                                if (!intValue && cur[tab.getNumColumn( column.getLexemCode())] == value)
                                                    SELECT_Listener.getRowForThisTable(cur, tab.columns, ref t);
                                            }
                                        }
                                    }
                                    break;
                                case "<>":
                                    for (int i = 0; i < DB.getTable(tableName).columns.Count; i++)
                                    {
                                        if (column.getLexemCode() == DB.getTable(tableName).columns[i].name)
                                        {
                                            var tab = DB.getTable(tableName);
                                            foreach (var cur in tab.table)
                                            {
                                                if (!intValue && cur[tab.getNumColumn(column.getLexemCode())]
                                                                            != value)
                                                    SELECT_Listener.getRowForThisTable(cur, tab.columns, ref t);
                                            }
                                        }
                                    }
                                    break;
                                case ">":
                                    if (revers)
                                    {
                                        for (int i = 0; i < DB.getTable(tableName).columns.Count; i++)
                                        {
                                            if (column.getLexemCode() == DB.getTable(tableName).columns[i].name)
                                            {
                                                var tab = DB.getTable(tableName);
                                                foreach (var cur in tab.table)
                                                {
                                                    if (!intValue && comparer(cur[tab.getNumColumn(column.getLexemCode())], value) < 0)
                                                         SELECT_Listener.getRowForThisTable(cur, tab.columns, ref t);
                                                }
                                            }
                                        }
                                        break;
                                    }
                                    for (int i = 0; i < DB.getTable(tableName).columns.Count; i++)
                                    {
                                        if (column.getLexemCode() == DB.getTable(tableName).columns[i].name)
                                        {
                                            var tab = DB.getTable(tableName);
                                            foreach (var cur in tab.table)
                                            {
                                                if (!intValue && comparer(cur[tab.getNumColumn(column.getLexemCode())], value) > 0)
                                                        SELECT_Listener.getRowForThisTable(cur, tab.columns, ref t);
                                            }
                                        }
                                    }
                                    break;
                                case "<":
                                    if (revers)
                                    {
                                        for (int i = 0; i < DB.getTable(tableName).columns.Count; i++)
                                        {
                                            if (column.getLexemCode() == DB.getTable(tableName).columns[i].name)
                                            {
                                                var tab = DB.getTable(tableName);
                                                foreach (var cur in tab.table)
                                                {
                                                    if (!intValue && comparer(cur[tab.getNumColumn(column.getLexemCode())], value) > 0)
                                                            SELECT_Listener.getRowForThisTable(cur, tab.columns, ref t);
                                                }
                                            }
                                        }
                                        break;
                                    }
                                    for (int i = 0; i < DB.getTable(tableName).columns.Count; i++)
                                    {
                                        if (column.getLexemCode() == DB.getTable(tableName).columns[i].name)
                                        {
                                            var tab = DB.getTable(tableName);
                                            foreach (var cur in tab.table)
                                            {
                                                if (!intValue && comparer(cur[tab.getNumColumn(column.getLexemCode())], value) < 0)
                                                        SELECT_Listener.getRowForThisTable(cur, tab.columns, ref t);
                                            }
                                        }
                                    }
                                    break;
                                case "<=":                                 
                                    if (revers)
                                    {
                                        for (int i = 0; i < DB.getTable(tableName).columns.Count; i++)
                                        {
                                            if (column.getLexemCode() == DB.getTable(tableName).columns[i].name)
                                            {
                                                var tab = DB.getTable(tableName);
                                                foreach (var cur in tab.table)
                                                {
                                                    if (!intValue && comparer(cur[tab.getNumColumn(column.getLexemCode())], value) >= 0)
                                                        SELECT_Listener.getRowForThisTable(cur, tab.columns, ref t);
                                                }
                                            }
                                        }
                                        break;
                                    }
                                    for (int i = 0; i < DB.getTable(tableName).columns.Count; i++)//!!!
                                    {
                                        if (column.getLexemCode() == DB.getTable(tableName).columns[i].name)
                                        {
                                            var tab = DB.getTable(tableName);
                                            foreach (var cur in tab.table)
                                            {
                                                if (!intValue && comparer(cur[tab.getNumColumn(column.getLexemCode())], value) <= 0)
                                                    SELECT_Listener.getRowForThisTable(cur, tab.columns, ref t);
                                            }
                                        }
                                    }
                                    break;
                                case ">=":
                                    if (revers)
                                    {
                                        for (int i = 0; i < DB.getTable(tableName).columns.Count; i++)
                                        {
                                            if (column.getLexemCode() == DB.getTable(tableName).columns[i].name)
                                            {
                                                var tab = DB.getTable(tableName);
                                                foreach (var cur in tab.table)
                                                {
                                                    if (!intValue && comparer(cur[tab.getNumColumn(column.getLexemCode())], value) <= 0)
                                                        SELECT_Listener.getRowForThisTable(cur, tab.columns, ref t);
                                                }
                                            }
                                        }
                                        break;
                                    }
                                    for (int i = 0; i < DB.getTable(tableName).columns.Count; i++)
                                    {
                                        if (column.getLexemCode() == DB.getTable(tableName).columns[i].name)
                                        {
                                            var tab = DB.getTable(tableName);
                                            foreach (var cur in tab.table)
                                            {
                                                if (!intValue && comparer(cur[tab.getNumColumn(column.getLexemCode())], value) >= 0)
                                                    SELECT_Listener.getRowForThisTable(cur, tab.columns, ref t);
                                                    
                                            }
                                        }
                                    }
                                    break;
                            }
                        }
                    }
                }
        }

       public  static int comparer(string s1, string s2)
       {
            for (int i = 0; i < Math.Min(s1.Length, s2.Length); i++)
            {
                if (s1[i] == '0'|| s2[i] == '0')
                    ;
                if (s1[i] > s2[i]) return 1;
                if (s1[i] < s2[i])
                    return -1;                   
            }
            if (s1.Length ==  s2.Length)
                return 0;
            if (s1.Length > s2.Length)
                return 1;
            else
                return -1;
       }

        public static int reversComparer(string s1, string s2)
        {
            if (s1.Length == s2.Length)
                for (int i = 0; i < Math.Min(s1.Length, s2.Length); i++)
                {
                    if (s1[i] == '0' || s2[i] == '0')
                        ;
                    if (s1[i] > s2[i]) return 1;
                    if (s1[i] < s2[i])
                        return -1;
                }

            if (s1.Length > s2.Length)
                return 1;
            else
                return -1;
        }
    }
}

