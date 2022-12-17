using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Piston.Vyjimky
{
    public class ChybaKompilace : Exception
    {
        public ChybaKompilace(string zprava) : base("Chyba kompilace: "+zprava)
        { 
        }
    }
}
