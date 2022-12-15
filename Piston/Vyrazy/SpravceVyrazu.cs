using Piston.Vyjimky;
using Piston.Vyrazy.Aritmenticke;
using Piston.Vyrazy.Logicke;
using Piston.Vyrazy.Stringove;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Piston.Vyrazy
{
    public abstract class SpravceVyrazu
    {
        public abstract IClenVyrazu VytvorVyraz(string vyraz);
    }
}

