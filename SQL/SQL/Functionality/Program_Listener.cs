
namespace SQL
{   
    /// <summary>
    /// Клас для інтерпритації коду SQL
    /// </summary>
    class Program_Listener
    {
        #region VAR
        public static LexemTree tree = new LexemTree();


        #endregion

        public static void Start(LexemTree tree)
        {
            Program_Listener.tree = tree;
            //try
            {
                foreach (var cur_transaction in tree.mainLexem.children)//вибираємо транзакцію
                {
                    if (cur_transaction == null) continue;
                    if (cur_transaction.name == "<Transaction>")
                        foreach (var cur_command in cur_transaction.children)
                        {
                            if (cur_command == null) continue;
                            if (cur_command.name == "<SELECT>")
                                SELECT_Listener.Start(cur_command);
                            if (cur_command.name == "<CREATE>")
                                CREATE_Listener.Start(cur_command);
                            if (cur_command.name == "<INSERT>")
                                INSERT_Listener.Start(cur_command);
                        }

                }
            }
            //catch { }

            DB.WriteDB();
        }
    }
}
