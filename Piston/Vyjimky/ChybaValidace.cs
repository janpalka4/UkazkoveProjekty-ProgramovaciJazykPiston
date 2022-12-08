using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Piston.Vyjimky
{
    public class ChybaValidace : Exception
    {
        public ChybaValidace(string duvod) : base($"Výraz není validní: {duvod}")
        {

        }
    }
}
