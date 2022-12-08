using Piston.Vyjimky;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Piston.Vyrazy.Aritmenticke
{
    public class Cislo : IClenVyrazu<double>
    {
        public double Hodnota { get; set; }

        public Cislo(double hodnota = 0)
        {
            Hodnota = hodnota;
        }

        public Cislo(string hodnota)
        {
            CultureInfo culture = CultureInfo.CreateSpecificCulture("en-US");
            double _hodnota = 0;
            if (double.TryParse(hodnota, NumberStyles.Float, culture, out _hodnota))
            {
                Hodnota = _hodnota;
            }
            else
            {
                throw new ChybaValidace($"Neplatný formát čísla {hodnota}");
            }
        }
    }
}
