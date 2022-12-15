using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Piston.Vyrazy.Stringove
{
    public class HodnotaRetezce : IClenVyrazu<string>
    {
        public string Hodnota { get; set; }

        public HodnotaRetezce(string Hodnota)
        {
            this.Hodnota = Hodnota;
        }
    }
}
