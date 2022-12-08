using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Piston.Balicky
{
    public class Balicek : Attribute
    {
        public bool JeStaticky { get; set; }
        public string Jmeno { get; set; }

        public Balicek(string jmeno)
        {
            Jmeno = jmeno;
        }
    }

    public class Metoda : Attribute
    {
        public string Jmeno { get; set; }

        public Metoda(string jmeno)
        {
            Jmeno = jmeno;
        }
    }
}
