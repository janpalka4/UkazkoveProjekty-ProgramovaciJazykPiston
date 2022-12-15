using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Piston.Vyjimky
{
    public class ChybaInicializace : Exception
    {
        public ChybaInicializace(string zprava) : base("Chyba inicializace: " + zprava)
        { 
        
        }
    }
}
