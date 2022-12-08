using Piston.Vyjimky;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Piston.Vyrazy.Logicke
{
    public class LogickaHodnota : IClenVyrazu<bool>
    {
        public bool Hodnota { get; set; }

        public LogickaHodnota(string hodnota)
        {
            bool _hodnota;

            if (bool.TryParse(hodnota, out _hodnota))
            {
                Hodnota = _hodnota;
            }
            else
                throw new ChybaValidace($"Očekáván boolean {hodnota}");
        }
        public LogickaHodnota(bool hodnota) => Hodnota = hodnota;
        public LogickaHodnota()
        {

        }
    }
}
