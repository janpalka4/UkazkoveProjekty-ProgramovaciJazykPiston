using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Piston.Vstup
{
    public class SpravceSouboru
    {
        public SpravceSouboru() { 
        
        }

        public string PrectiTextovySoubor(string cesta) {
            if (!File.Exists(cesta))
                throw new Exception("Soubor nenalezen");

            return File.ReadAllText(cesta);
        }
    }
}
