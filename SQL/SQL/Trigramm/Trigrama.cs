using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQL
{
    class Trigrama
    {
        public string val;
       

        public Trigrama(string val)
        {
            this.val = val;
        }

        public bool equals(Trigrama t)
        {
            if (t.val == val)
                return true;
            else
                return false; 
        }
    }
}
