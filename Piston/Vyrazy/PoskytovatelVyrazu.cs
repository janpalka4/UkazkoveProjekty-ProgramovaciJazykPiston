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
    public class PoskytovatelVyrazu
    {
        private readonly IServiceProvider provider;

        public PoskytovatelVyrazu(IServiceProvider provider) 
        { 
            this.provider = provider;
        }

        public SpravceVyrazu ZiskejSpravce(string vyraz)
        {
            if (SpravceAritmetickychVyrazu.JeVyrazValidni(vyraz))
                return (SpravceAritmetickychVyrazu)provider.GetService(typeof(SpravceAritmetickychVyrazu))!;

            if (SpravceLogickychVyrazu.JeVyrazValidni(vyraz))
                return (SpravceLogickychVyrazu)provider.GetService(typeof(SpravceLogickychVyrazu))!;

            if (SpravceRetezcovychVyrazu.JeVyrazValidni(vyraz))
                return (SpravceRetezcovychVyrazu)provider.GetService(typeof(SpravceRetezcovychVyrazu))!;

            throw new ChybaValidace("Výraz se nepodařilo zvalidovat");
        }
    }
}
