using System;

namespace SQL
{   
    [Serializable]
    class  LexemTree
    {
        #region VAR

        /// <summary>
        /// Головна лексема (зазвичай <Program>)
        /// </summary>
        public Lexem mainLexem;

        #endregion
        
        #region CTOR

        public LexemTree()
        {
            var code= new Code();
            code.set(SQL.Properties.Resources.Code.ToString());
            mainLexem = new Lexem(0,"<Program>",0, code);
            var flag = mainLexem.IsLexem();

            //якщо код повністю не входить в дерево 
            if (flag == true && mainLexem.pos != mainLexem.code.length() -1)
                Console.WriteLine("ERROR CODE!!!" + (mainLexem.pos - mainLexem.code.length() + 1));    
        }        
        
        #endregion
    }
}
