using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQL
{
    class Sub_Pair
    {
        public Pair p;
        public List<Trigrama> list;

        public Sub_Pair(Pair pair, List<Trigrama> l)
        {
            p = pair;
            list = l;
        }

    }
}
