using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Piston.Vyjimky
{
    public class ChybaOperace : Exception
    {
        public ChybaOperace(string duvod) : base($"Nelze provést operaci: {duvod}")
        {

        }
    }
}
